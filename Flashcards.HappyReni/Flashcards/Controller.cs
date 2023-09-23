namespace Flashcards
{
    internal class Controller
    {
        private readonly UI ui;
        private readonly Database db;
        private SELECTOR selector;
        private Dictionary<string,Stack> Stacks { get; set; } = new();
        private List<Session> Sessions { get; set; } = new();

        public Controller()
        {
            ui = new UI();
            db = new Database();

            CheckDatabaseConnection();
            GetStacksFromDatabase();
            SetFlashcardsInStack();
            GetSessionsFromDatabase();

            selector = ui.MainMenu();
            while (true)
            {
                Action();
            }
        }

        private void CheckDatabaseConnection()
        {
            if (!db.isConnected)
            {
                ui.Write("Can't connect to DB. Check your connection again.");
                ui.WaitForInput();
                Environment.Exit(0);
            }
        }

        private void GetStacksFromDatabase()
        {
            Stacks = db.GetStacksFromDatabase();
            if (Stacks == null) Stacks = new();
        }

        private void SetFlashcardsInStack()
        {
            foreach(var stack in Stacks.Values)
            {
                var cards = db.SetFlashcardsInStack(stack.Id,"Load");
                stack.SetFlashcards(cards);
            }
        }

        private void GetSessionsFromDatabase()
        {
            Sessions = db.GetSessions();
            if (Sessions == null) Sessions = new();
        }

        private void Action()
        {
            switch (selector)
            {
                case SELECTOR.CREATE:
                    CreateStack();
                    break;
                case SELECTOR.MANAGE:
                    ViewAllStacks();
                    string name = ui.GetInput("Choose a name of stack.").str;
                    ManageStack(name);
                    break;
                case SELECTOR.STUDY:
                    ViewAllSessions();
                    break;
                case SELECTOR.EXIT:
                    Environment.Exit(0);
                    break;
                default:
                    ui.Write("Invalid Input");
                    break;
            }
            selector = ui.GoToMainMenu("Type any keys to continue.");
        }
        private void CreateStack()
        {
            var name = ui.CreateStack();
            try
            {
                if (!Validation.IsUniqueStackName(name, Stacks.Keys.ToList<string>()))
                {
                    var id = db.Insert(name);
                    var stack = new Stack(id, name);
                    Stacks[name] = stack;
                    ui.Write($"{name} is created.");
                }
            }
            catch(Exception e)
            {
                var message = "Not an unique name.";
                if (e.Message == message) ui.Write($"{message} Try other names.");
                else ui.Write($"failed to create.");
            }
        }

        private void ManageStack(string name)
        {
            try
            {
                if (Validation.IsValidStackName(name, Stacks))
                {
                    int action = ui.ManageStack(name);
                    var _name = name;
                    switch ((MANAGE_SELECTOR)action)
                    {
                        case MANAGE_SELECTOR.BACK:
                            return;
                        case MANAGE_SELECTOR.VIEW:
                            ViewAllFlashcards(_name);
                            break;
                        case MANAGE_SELECTOR.CREATE:
                            CreateFlashcard(_name);
                            break;
                        case MANAGE_SELECTOR.EDIT:
                            EditFlashcard(_name);
                            break;
                        case MANAGE_SELECTOR.DELETE:
                            DeleteFlashcard(_name);
                            break;
                        case MANAGE_SELECTOR.STUDY:
                            Study(_name);
                            return;
                        case MANAGE_SELECTOR.CHANGE:
                            _name = ChangeStack();
                            break;
                        case MANAGE_SELECTOR.DELETESTACK:
                            if (DeleteStack(_name)) return;
                            else break;
                        default:
                            ui.Write("Invalid Input");
                            break;
                    }
                    ui.WaitForInput("Press any key to continue..");
                    ManageStack(_name);
                }
            }
            catch(Exception e)
            {
                ui.Write(e.Message);
            }
        }

        private void CreateFlashcard(string name)
        {
            var front = ui.GetInput("Type a front word.").str;
            var back = ui.GetInput("Type a back word.").str;
            var card = new Flashcard(Stacks[name].Id, front, back);

            if (db.Insert(card))
            {
                Stacks[name].InsertFlashCard(card);
                ui.Write($"Successfully created.");
            }
            else ui.Write($"failed to create.");
        }

        private void EditFlashcard(string name)
        {
            ViewAllFlashcards(name);
            try
            {
                var front = ui.GetInput("Type a front word.").str;
                if (Validation.IsValidFlashcard(front, Stacks[name].Flashcards))
                {
                    var back = ui.GetInput("Type a new back word.").str;
                    var index = Stacks[name].FindFlashcard(front);
                    Flashcard card = Stacks[name].GetFlashcard(index);
                    if (db.Update(card))
                    {
                        Stacks[name].EditFlashcard(index, back);
                        ui.Write($"Successfully updated.");
                    }
                }
            }
            catch(Exception e)
            {
                ui.Write(e.Message);
            }
        }
        private void DeleteFlashcard(string name)
        {
            ViewAllFlashcards(name);
            try
            {
                var front = ui.GetInput("Type a front word to delete.").str;
                if (Validation.IsValidFlashcard(front, Stacks[name].Flashcards))
                {
                    var index = Stacks[name].FindFlashcard(front);
                    var id = Stacks[name].Flashcards[index].Id;

                    if (db.Delete(id))
                    {
                        Stacks[name].DeleteFlashCard(index);
                        foreach (var stack in Stacks.Values)
                        {
                            stack.UpdateFlashcardID(id);
                        }
                        Flashcard.DownCount();
                        ui.Write($"Successfully deleted.");
                    }
                }
            }
            catch(Exception e)
            {
                ui.Write(e.Message);
            }
        }

        private string ChangeStack()
        {
            Console.Clear();
            ViewAllStacks();

            var name = ui.GetInput("Type an ID of Stack to change.").str;
            if (Validation.IsValidStackName(name, Stacks))
            {
                ui.Write("Successfully changed.");
                return name;
            }
            else throw new Exception("no such a stack.");
        }

        private bool DeleteStack(string name)
        {
            Console.Clear();
            var res = ui.GetInput("Are you sure to delete this stack? (Y)").str;
            if (res == "Y")
            {
                if (db.Delete(name))
                {
                    Stacks[name].DeleteFlashCard();
                    Stacks.Remove(name);
                    Stack.DownCount();
                    db.UpdateID();
                    ui.Write("Successfully deleted.");
                    return true;
                }
                else
                {
                    ui.Write("Failed to delete.");
                    return false;
                }
            }
            else
            {
                ui.Write("Failed to delete.");
                return false;
            }
        }

        private void ViewAllStacks()
        {
            List<List<object>> stackList = new();
            var sorted = from item in Stacks
                         orderby item.Value.Id ascending
                         select item.Value;

            foreach (var stack in sorted)
            {
                stackList.Add(stack.GetField());
            }
            ui.MakeTable(stackList, "stack");
        }

        private void ViewAllFlashcards(string name)
        {
            var cards = db.SetFlashcardsInStack(Stacks[name].Id,"View");
            List<List<object>> cardList = new();

            try
            {
                foreach (var card in cards)
                {
                    cardList.Add(new List<object> { card.Dto.Front, card.Dto.Back });
                }
                ui.MakeTable(cardList, "Flashcards");
            }
            catch (Exception e)
            {
                ui.Write(e.Message);
            }
        }

        private void Study(string name)
        {
            Console.Clear();
            try
            {
                var cards = Stacks[name].Flashcards;
                var questionCount = cards.Count();
                var score = 0;
                var startTime = DateTime.Now;

                ui.Write("Guess the back words.");
                score = StartStudySessions(cards);
                var endTime = DateTime.Now;
                var format = "yyyy-MM-dd HH:mm:ss";
                var session = new Session(Stacks[name].Id,
                                           startTime.ToString(format),
                                           endTime.ToString(format),
                                           score,
                                           questionCount);
                StudyReport(session);
                db.Insert(session);
            }
            catch(Exception e)
            {
                ui.Write(e.Message);
            }
        }

        private int StartStudySessions(List<Flashcard> cards)
        {
            var score = 0;
            foreach (var card in cards)
            {
                var front = card.QuestionDto.Front;
                var answer = ui.GetInput(front).str;

                if (answer == card.Back)
                {
                    score++;
                    ui.WaitForInput("Correct!");
                }
                else
                {
                    ui.WaitForInput("Wrong answer!");
                }
                Console.Clear();
            }

            return score;
        }

        private void StudyReport(Session session)
        {
            Console.Clear();
            ui.Write("Study Finished!");
            ui.Write($"Your score is {session.Score} out of {session.QuestionCount} questions");
            Sessions.Add(session);
        }

        private void ViewAllSessions()
        {
            Console.Clear();
            List<List<object>> tableData = new();

            try
            {
                foreach (var session in Sessions)
                {
                    var sessionData = session.GetField();
                    sessionData[0] = db.SearchStackName(session.StackId, "Stack");
                    if (sessionData[0] != null) tableData.Add(sessionData);
                }
                ui.MakeTable(tableData, "Sessions");
            }
            catch (Exception e)
            {
                ui.Write(e.Message);
            }

        }
    }
}
