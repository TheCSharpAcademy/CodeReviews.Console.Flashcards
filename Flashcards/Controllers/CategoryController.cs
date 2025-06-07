using Flashcards.Data;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Controllers;

public class CategoryController
{
    internal static void AddCategory()
    {
        Category category = new Category();
        DataConnection dataConnection = new DataConnection();

        category.Name = AnsiConsole.Ask<string>("Insert Category Name:");

        while (string.IsNullOrWhiteSpace(category.Name))
        {
            category.Name = AnsiConsole.Ask<string>("Name cannot be empty. Insert Category Name:");
        }

        dataConnection.InsertCategory(category);
    }

    internal static void ViewAllCategories(IEnumerable<Category> categories)
    {
        var table = new Table();
        table.AddColumn(new TableColumn("Name"));

        foreach (var category in categories)
        {
            table.AddRow(category.Name);
        }

        AnsiConsole.Write(table);
    }

    internal static void UpdateCategory()
    {
        Category category = new Category();

        category.Id = ChooseCategory("Choose Category:");
        category.Name = AnsiConsole.Ask<string>("Insert Category Name:");

        DataConnection dataConnection = new DataConnection();
        dataConnection.UpdateCategory(category);
    }

    internal static void DeleteCategory()
    {
        var id = ChooseCategory("Choose Category to Delete:");

        if (!AnsiConsole.Confirm("Are you sure you want to delete this category?")) return;

        DataConnection dataConnection = new DataConnection();
        dataConnection.DeleteCategory(id);
    }

    internal static int ChooseCategory(string message)
    {
        DataConnection dataConnection = new DataConnection();
        var categories = dataConnection.GetAllCategories();
        var categoryArray = categories.Select(c => c.Name).ToArray();
        var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Please choose a category: ")
            .AddChoices(categoryArray));
        var categoryId = categories.Single(c => c.Name == option).Id;

        return categoryId;
    }
}