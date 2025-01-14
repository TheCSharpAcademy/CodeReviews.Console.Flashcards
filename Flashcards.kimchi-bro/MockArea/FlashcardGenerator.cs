using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using GTranslate.Translators;
using CrypticWizard.RandomWordGenerator;
using static CrypticWizard.RandomWordGenerator.WordGenerator;

internal static class FlashcardGenerator
{
    private static readonly Dictionary<string, string> _languages = new()
    {
        { "Arabic" , "ar" },
        { "Chinese" , "zh" },
        { "German" , "de" },
        { "Italian" , "it"},
        { "Japanese" , "ja"},
        { "Korean" , "ko"},
        { "Portuguese" , "pt"},
        { "Russian" , "ru"},
        { "Spanish" , "es" },
        { "Turkish" , "tr"}
    };

    private static readonly Dictionary<string, PartOfSpeech> _partsOfSpeech = new()
    {
        { "Noun" , PartOfSpeech.noun },
        { "Verb" , PartOfSpeech.verb },
        { "Adjective" , PartOfSpeech.adj }
    };

    internal static async Task CreateNewFlashcardsAsync()
    {
        await GenerateNewFlashcards();
    }

    private static async Task GenerateNewFlashcards()
    {
        try
        {
            AnsiConsole.MarkupLine("Generating new flashcards.\n");
            var languageChoice = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
                "Choose language:", _languages.Keys);
            if (languageChoice == DisplayInfoHelpers.Back)
            {
                Console.Clear();
                return;
            }

            var language = _languages[languageChoice];

            var stack = GetStackIdFromDbByStackName(languageChoice);
            if (stack.StackName == null)
            {
                Console.Clear();
                return;
            }

            var partOfSpeechChoice = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
                "Choose a part of speech:", _partsOfSpeech.Keys);
            if (partOfSpeechChoice == DisplayInfoHelpers.Back) return;
            var partOfSpeech = _partsOfSpeech[partOfSpeechChoice];

            var numberOfFlashcards = InputHelpers.GetPositiveNumberInput(
                "Enter a number of flashcards to generate:");

            AnsiConsole.MarkupLine($"\nSelected language: [yellow]{languageChoice}[/]");
            AnsiConsole.MarkupLine($"Selected part of speech: [yellow]{partOfSpeechChoice}[/]");
            AnsiConsole.MarkupLine($"Number of flashcards: [yellow]{numberOfFlashcards}[/]\n");
            var confirm = DisplayInfoHelpers.GetYesNoAnswer("[yellow]Is everything correct?[/]");
            if (!confirm)
            {
                Console.Clear();
                return;
            }

            var randomWords = GenerateRandomWords(numberOfFlashcards, partOfSpeech);

            var flashcards = new List<Flashcard>();
            await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                {
                    var task = ctx.AddTask("[yellow]Generating flashcards:[/]", true, 100);
                    flashcards = await Translate(randomWords, language, (progress) =>
                    {
                        task.Increment(progress);
                    });
                    task.Increment(100 - task.Value);
                });

            FlashcardCreate.AddFlashcardsInBulk(flashcards, stack);

            AnsiConsole.MarkupLine("[green]New flashcards created and stored in database successfully![/]\n");
            AnsiConsole.MarkupLine("List of new flashcards:");

            foreach (var flashcard in flashcards)
            {
                AnsiConsole.MarkupLine($"[yellow]{flashcard.FlashcardFront}[/] => {flashcard.FlashcardBack}");
                await Task.Delay(300);
            }

            DisplayInfoHelpers.PressAnyKeyToContinue();
        }
        catch (Exception ex)
        {
            DisplayErrorHelpers.GeneralError(ex);
        }
    }

    private static Stack GetStackIdFromDbByStackName(string stackName)
    {
        try
        {
            using var connection = new SqlConnection(Config.ConnectionString);
            connection.Open();
            var parameters = new DynamicParameters();
            parameters.Add("@StackName", stackName);
            var existingStackId = connection.QueryFirstOrDefault<int>(@"
                SELECT StackId
                FROM Stack
                WHERE StackName = @StackName", parameters);

            if (existingStackId != 0)
            {
                return new Stack()
                {
                    StackId = existingStackId,
                    StackName = stackName
                };
            }
            else
            {
                var stackId = connection.QueryFirstOrDefault<int>(@"
                    INSERT INTO Stack (StackName)
                    VALUES (@StackName);
                    SELECT SCOPE_IDENTITY() AS StackId", parameters);

                return new Stack()
                {
                    StackId = stackId,
                    StackName = stackName
                };
            }
        }
        catch (SqlException ex)
        {
            DisplayErrorHelpers.SqlError(ex);
            return new Stack();
        }
        catch (Exception ex)
        {
            DisplayErrorHelpers.GeneralError(ex);
            return new Stack();
        }
    }

    private static List<string> GenerateRandomWords(int number, PartOfSpeech partOfSpeech)
    {
        var myWordGenerator = new WordGenerator();
        List<string> words = myWordGenerator.GetWords(partOfSpeech, number);
        return words;
    }

    private static async Task<List<Flashcard>> Translate(
        List<string> words, string language, Action<int> progressCallback)
    {
        var flashcards = new List<Flashcard>();

        var translator = new MicrosoftTranslator();
        //use other translators if reached API request limit
        //var translator = new GoogleTranslator();
        //var translator = new YandexTranslator();

        int progress = 0;
        foreach (var word in words)
        {
            var result = await translator.TranslateAsync(word, language);

            flashcards.Add(new Flashcard()
            {
                FlashcardFront = result.Translation,
                FlashcardBack = word
            });

            progress++;
            progressCallback(progress);
            await Task.Delay(300);
        }
        return flashcards;
    }
}
