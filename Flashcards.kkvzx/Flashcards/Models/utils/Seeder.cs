using Npgsql;

namespace Flashcards.Models.utils;

public class Seeder()
{
    public void Seed()
    {
        try
        {
            using (NpgsqlConnection connection = new(Context.FlashcardsDbConnectionString))
            {
                connection.Open();
                NpgsqlCommand tableCommand =
                    new NpgsqlCommand(
                        @$"INSERT INTO {Context.StacksTable} (name)
                            VALUES ('Portuguese'), ('German'), ('French'), ('Italian'), ('Spanish');
                           INSERT INTO {Context.FlashcardsTable} (stack_id, front_text, back_text)
                            VALUES 
                                (1, 'To learn', 'Aprender'),
                                (1, 'To study', 'Estudar'),
                                (1, 'To remember', 'Lembrar'),
                                (1, 'To forget', 'Esquecer'),
                                (1, 'Word', 'Palavra'),
                                (1, 'Book', 'Livro'),
                                (1, 'Card', 'Carta'),
                                (1, 'Sentence', 'Frase'),
                                (1, 'Translation', 'Tradução'),
                                (1, 'Knowledge', 'Conhecimento'),
                                (2, 'To learn', 'Lernen'),
                                (2, 'To study', 'Studieren'),
                                (2, 'To remember', 'Erinnern'),
                                (2, 'To forget', 'Vergessen'),
                                (2, 'Word', 'Wort'),
                                (2, 'Book', 'Buch'),
                                (2, 'Card', 'Karte'),
                                (2, 'Sentence', 'Satz'),
                                (2, 'Translation', 'Übersetzung'),
                                (3, 'To learn', 'Apprendre'),
                                (3, 'To study', 'Étudier'),
                                (3, 'To remember', 'Se souvenir'),
                                (3, 'To forget', 'Oublier'),
                                (3, 'Word', 'Mot'),
                                (3, 'Book', 'Livre'),
                                (3, 'Card', 'Carte'),
                                (3, 'Knowledge', 'Connaissance'),
                                (4, 'To learn', 'Imparare'),
                                (4, 'To study', 'Studiare'),
                                (4, 'Translation', 'Traduzione'),
                                (4, 'Knowledge', 'Conoscenza'),
                                (5, 'To learn', 'Aprender'),
                                (5, 'To study', 'Estudiar'),
                                (5, 'To remember', 'Recordar'),
                                (5, 'To forget', 'Olvidar'),
                                (5, 'Word', 'Palabra'),
                                (5, 'Book', 'Libro'),
                                (5, 'Card', 'Tarjeta'),
                                (5, 'Sentence', 'Frase'),
                                (5, 'Translation', 'Traducción'),
                                (5, 'Knowledge', 'Conocimiento');
                        INSERT INTO {Context.SessionsTable} (stack_id, occurence_date, score)
                            VALUES 
                                (1, '2025-01-26', 25),
                                (1, '2025-01-27', 30),
                                (1, '2025-01-30', 26),
                                (1, '2025-02-13', 44),
                                (2, '2025-03-26', 5),
                                (2, '2025-04-27', 0),
                                (2, '2025-04-30', 1),
                                (2, '2025-05-13', 5),
                                (3, '2024-06-30', 15),
                                (3, '2024-07-14', 23);
                           ", connection);

                tableCommand.ExecuteNonQuery();
            }
        }
        catch (Exception err)
        {
            Console.WriteLine(err.Message);
            Console.ReadKey();
        }
    }
}