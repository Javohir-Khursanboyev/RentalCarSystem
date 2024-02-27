using RentCar.Uz.Configurations;
using RentCar.Uz.Models.Cars;
using RentCar.Uz.Services;
using Spectre.Console;

namespace RentCar.Uz.Display;

public class CarMenu
{
    private readonly CarService carService;
    private readonly CategoryService categoryService;
    public CarMenu(CarService carService, CategoryService categoryService)
    {
        this.carService = carService;
        this.categoryService = categoryService;

    }

    public async ValueTask DisplayAsync()
    {
        bool circle = true;
        if (!Security())
            circle = false;

        var options = new string[] { "Create", "GetById", "Update", "Delete", "GetAll", "[red]Back[/]" };
        var title = "-- CarMenu --";

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
        var categories = await categoryService.GetAllAsync();
        var table = Selection.DataTable("All categories", categories.ToList());
        AnsiConsole.Write(table);
        long id = AnsiConsole.Ask<long>("Enter carCategory Id : ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter carCategory Id : ");
        }

        string brand = AnsiConsole.Ask<string>("Enter car brand : ");
        string model = AnsiConsole.Ask<string>("Enter car model : ");
        string description = AnsiConsole.Ask<string>("Enter description : ");
        decimal dailyPrice = AnsiConsole.Ask<decimal>("Enter  DailyPrice : ");
        while (dailyPrice <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            dailyPrice = AnsiConsole.Ask<decimal>("Enter DailyPrice : ");
        }

        decimal deposit = AnsiConsole.Ask<decimal>("Enter  Deposit : ");
        while (deposit <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            deposit = AnsiConsole.Ask<decimal>("Enter Deposit : ");
        }

        string carPng = AnsiConsole.Ask<string>("Enter car image path []blue(.png or .jpg):[/] ");
        while (Path.GetExtension(carPng).ToLower() != ".png" && Path.GetExtension(carPng).ToLower() != ".jpg")
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            carPng = AnsiConsole.Ask<string>("Enter car image path [blue](.png or .jpg):[/] ");
        }

        var car = new CarCreationModel()
        {
            CategoryId = id,
            Brand = brand,
            Model = model,
            Description = description,
            DailyPrice = dailyPrice,
            Deposit = deposit,
            CarPng = carPng
        };
        try
        {
            var addedCar = await carService.CreateAsync(car);
            AnsiConsole.Markup("[orange3]Succesful created[/]\n");
            var carTable = Selection.DataTable("Car", addedCar);
            AnsiConsole.Write(carTable);
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
        long id = AnsiConsole.Ask<long>("Enter car Id to update: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter car Id to update : ");
        }
        var categories = await categoryService.GetAllAsync();
        var table = Selection.DataTable("All categories", categories.ToList());
        AnsiConsole.Write(table);

        long categoryId = AnsiConsole.Ask<long>("Enter carCategory Id : ");
        while (categoryId <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            categoryId = AnsiConsole.Ask<long>("Enter carCategory Id : ");
        }
        string brand = AnsiConsole.Ask<string>("Enter car brand : ");
        string model = AnsiConsole.Ask<string>("Enter car model : ");
        string description = AnsiConsole.Ask<string>("Enter description : ");
        decimal dailyPrice = AnsiConsole.Ask<decimal>("Enter  DailyPrice : ");
        while (dailyPrice <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            dailyPrice = AnsiConsole.Ask<decimal>("Enter DailyPrice : ");
        }

        decimal deposit = AnsiConsole.Ask<decimal>("Enter  Deposit : ");
        while (deposit <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            deposit = AnsiConsole.Ask<decimal>("Enter Deposit : ");
        }

        string carPng = AnsiConsole.Ask<string>("Enter car image path [blue](.png or .jpg):[/] ");
        while (Path.GetExtension(carPng).ToLower() != ".png" && Path.GetExtension(carPng).ToLower() != ".jpg")
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            carPng = AnsiConsole.Ask<string>("Enter car image path [blue](.png or .jpg): [/]");
        }
        var car = new CarUpdatedModel()
        {
            CategoryId = id,
            Brand = brand,
            Model = model,
            Description = description,
            DailyPrice = dailyPrice,
            Deposit = deposit,
            CarPng = carPng
        };
        try
        {
            var updatedCar = await carService.UpdateAsync(id, car);
            AnsiConsole.Markup("[orange3]Succesful updated[/]\n");

            var CarTable = Selection.DataTable("Car", updatedCar);
            AnsiConsole.Write(CarTable);
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
        long id = AnsiConsole.Ask<long>("Enter car Id: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter car Id: ");
        }

        try
        {
            var car = await carService.GetByIdAsync(id);
            var table = Selection.DataTable("Car", car);
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
        long id = AnsiConsole.Ask<long>("Enter car Id to delete: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter car Id to delete: ");
        }

        try
        {
            await carService.DeleteAsync(id);
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
        var cars = await carService.GetAllAsync();
        var table = Selection.DataTable("Cars", cars.ToList());
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