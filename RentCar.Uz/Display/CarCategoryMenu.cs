using RentCar.Uz.Configurations;
using RentCar.Uz.Models.CarCategories;
using RentCar.Uz.Services;
using Spectre.Console;

namespace RentCar.Uz.Display;

public class CarCategoryMenu
{
    private readonly CategoryService categoryService;
    public CarCategoryMenu(CategoryService categoryService)
    {
        this.categoryService = categoryService;
    }
    public async ValueTask DisplayAsync()
    {
        bool circle = true;

        if (!Security())
            circle = false;

        var options = new string[] { "Create", "GetById", "Update", "Delete", "GetAll", "[red]Back[/]" };
        var title = "-- CarCategoryMenu --";

        while (circle)
        {
            AnsiConsole.Clear();
            var selection = Selection.SelectionMenu(title, options);
            switch (selection)
            {
                case "Create":
                    await CreateAsync();
                    break;
                case "GetById":
                    await GetByIdAsync();
                    break;
                case "Update":
                    await UpdateAsync();
                    break;
                case "Delete":
                    await DeleteAsync();
                    break;
                case "GetAll":
                    await GetAllAsync();
                    break;
                case "[red]Back[/]":
                    circle = false;
                    break;
            }
        }
    }

    async ValueTask CreateAsync()
    {
        Console.Clear();
        string name = AnsiConsole.Ask<string>("Car category name:");
        CategoryCreationModel category = new()
        {
            Name = name
        };
        try
        {
            var addedCategory = await categoryService.CreateAsync(category);
            AnsiConsole.Markup("[orange3]Succesful created[/]\n");

            var table = Selection.DataTable("Category", addedCategory);
            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]{ex.Message}[/]\n");
        }
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask UpdateAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter category Id to update: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter category Id to update: ");
        }
        string name = AnsiConsole.Ask<string>("Car category name:");
        CategoryUpdatedModel category = new()
        {
            Name = name
        };

        try
        {
            var updatedCategory = await categoryService.UpdateAsync(id, category);
            AnsiConsole.Markup("[orange3]Succesful updated[/]\n");

            var table = Selection.DataTable("Category", updatedCategory);
            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]{ex.Message}[/]\n");
        }
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask GetByIdAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter category Id: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter category Id: ");
        }

        try
        {
            var category = await categoryService.GetByIdAsync(id);
            var table = Selection.DataTable("Category", category);
            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]{ex.Message}[/]\n");
        }
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask DeleteAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter category Id to delete: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter category Id to delete: ");
        }

        try
        {
            await categoryService.DeleteAsync(id);
            AnsiConsole.Markup("[orange3]Succesful deleted[/]\n");
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]{ex.Message}[/]\n");
        }
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask GetAllAsync()
    {
        Console.Clear();
        var categories = await categoryService.GetAllAsync();
        var table = Selection.DataTable("Categories", categories.ToList());
        AnsiConsole.Write(table);
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    private bool Security()
    {
        Console.Clear();
        string password = AnsiConsole.Prompt(
        new TextPrompt<string>("Enter password :")
         .PromptStyle("red")
         .Secret());
        if (password == Constants.ADMIN_PAROL)
            return true;

        return false;
    }
}
