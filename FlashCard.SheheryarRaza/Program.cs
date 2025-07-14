using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FlashCard.SheheryarRaza.DataBase;
using FlashCard.SheheryarRaza.Entities;
using FlashCard.SheheryarRaza.Managers;
using System;
using System.IO;
using Spectre.Console;
using System.Linq;

namespace FlashCard.SheheryarRaza
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            IServiceCollection services = new ServiceCollection();

            services.AddDbContext<DBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<StackManager>();
            services.AddTransient<FlashcardManager>();
            services.AddTransient<StudySessionManager>();

            services.AddTransient<FlashcardApp>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                try
                {
                    context.Database.Migrate();
                    ConsoleHelper.DisplayMessage("Database migrations applied successfully.", ConsoleColor.Green);

                    SeedDatabase(context);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.DisplayMessage($"Error applying migrations or seeding database: {ex.Message}", ConsoleColor.Red);
                    ConsoleHelper.DisplayMessage("Please ensure SQL Server is running and the connection string is correct.", ConsoleColor.Red);
                    ConsoleHelper.PressAnyKeyToContinue();
                    return;
                }
            }

            var app = serviceProvider.GetRequiredService<FlashcardApp>();
            app.RunApplication();
        }

        private static void SeedDatabase(DBContext context)
        {
            if (!context.Stacks.Any())
            {
                ConsoleHelper.DisplayMessage("Seeding initial stacks...", ConsoleColor.Yellow);
                var stack1 = new Stacks { Name = "Programming Basics", Description = "Fundamental concepts of programming", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                var stack2 = new Stacks { Name = "SQL Fundamentals", Description = "Basic SQL commands and database concepts", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                var stack3 = new Stacks { Name = "Web Development Basics", Description = "Core concepts for building web applications", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
                var stack4 = new Stacks { Name = "Data Structures & Algorithms", Description = "Essential concepts for efficient programming", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

                context.Stacks.AddRange(stack1, stack2, stack3, stack4);
                context.SaveChanges();
                ConsoleHelper.DisplayMessage("Initial stacks seeded.", ConsoleColor.Green);

                if (!context.FlashCards.Any(fc => fc.StackId == stack1.Id))
                {
                    ConsoleHelper.DisplayMessage("Seeding flashcards for 'Programming Basics'...", ConsoleColor.Yellow);
                    context.FlashCards.AddRange(
                        new FlashCards { StackId = stack1.Id, Question = "What is a variable?", Answer = "A storage location paired with an associated symbolic name.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new FlashCards { StackId = stack1.Id, Question = "What is a loop?", Answer = "A control flow statement that allows code to be executed repeatedly based on a given Boolean condition.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new FlashCards { StackId = stack1.Id, Question = "What is an algorithm?", Answer = "A set of well-defined instructions to solve a particular problem.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    );
                    context.SaveChanges();
                    ConsoleHelper.DisplayMessage("Flashcards for 'Programming Basics' seeded.", ConsoleColor.Green);
                }

                if (!context.FlashCards.Any(fc => fc.StackId == stack2.Id))
                {
                    ConsoleHelper.DisplayMessage("Seeding flashcards for 'SQL Fundamentals'...", ConsoleColor.Yellow);
                    context.FlashCards.AddRange(
                        new FlashCards { StackId = stack2.Id, Question = "What is SQL?", Answer = "Structured Query Language, used to manage and manipulate relational databases.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new FlashCards { StackId = stack2.Id, Question = "What is a primary key?", Answer = "A unique identifier for each record in a table.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new FlashCards { StackId = stack2.Id, Question = "What is a foreign key?", Answer = "A field in one table that uniquely identifies a row of another table.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    );
                    context.SaveChanges();
                    ConsoleHelper.DisplayMessage("Flashcards for 'SQL Fundamentals' seeded.", ConsoleColor.Green);
                }

                if (!context.FlashCards.Any(fc => fc.StackId == stack3.Id))
                {
                    ConsoleHelper.DisplayMessage("Seeding flashcards for 'Web Development Basics'...", ConsoleColor.Yellow);
                    context.FlashCards.AddRange(
                        new FlashCards { StackId = stack3.Id, Question = "What is HTML?", Answer = "HyperText Markup Language, the standard markup language for documents designed to be displayed in a web browser.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new FlashCards { StackId = stack3.Id, Question = "What is CSS?", Answer = "Cascading Style Sheets, used for describing the presentation of a document written in a markup language.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new FlashCards { StackId = stack3.Id, Question = "What is JavaScript?", Answer = "A programming language that enables interactive web pages.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    );
                    context.SaveChanges();
                    ConsoleHelper.DisplayMessage("Flashcards for 'Web Development Basics' seeded.", ConsoleColor.Green);
                }

                if (!context.FlashCards.Any(fc => fc.StackId == stack4.Id))
                {
                    ConsoleHelper.DisplayMessage("Seeding flashcards for 'Data Structures & Algorithms'...", ConsoleColor.Yellow);
                    context.FlashCards.AddRange(
                        new FlashCards { StackId = stack4.Id, Question = "What is a stack?", Answer = "A linear data structure that follows the LIFO (Last In, First Out) principle.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new FlashCards { StackId = stack4.Id, Question = "What is a queue?", Answer = "A linear data structure that follows the FIFO (First In, First Out) principle.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                        new FlashCards { StackId = stack4.Id, Question = "What is Big O notation?", Answer = "A mathematical notation that describes the limiting behavior of a function when the argument tends towards a particular value or infinity.", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                    );
                    context.SaveChanges();
                    ConsoleHelper.DisplayMessage("Flashcards for 'Data Structures & Algorithms' seeded.", ConsoleColor.Green);
                }
            }
            else
            {
                ConsoleHelper.DisplayMessage("Database already contains data. Skipping seeding.", ConsoleColor.Yellow);
            }
        }
    }
}
