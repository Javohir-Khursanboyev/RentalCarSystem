using RentCar.Uz.Configurations;
using RentCar.Uz.Models.Customers;
using RentCar.Uz.Models.Reservations;
using RentCar.Uz.Services;
using Spectre.Console;

namespace RentCar.Uz.Display;

public class ReservationMenu
{
    private readonly ReservationService reservationService;
    private readonly CategoryService categoryService;
    private readonly CustomerService customerService;
    private readonly CarService carService;
    private CustomerViewModel customer;
    public ReservationMenu(ReservationService reservationService, CategoryService categoryService, CustomerService customerService, CarService carService)
    {
        this.reservationService = reservationService;
        this.categoryService = categoryService;
        this.customerService = customerService;
        this.carService = carService;
    }
    public async ValueTask DisplayAsync()
    {
        bool circle = true;

        if (!await SecurityCheck())
            circle = false;

        var options = new string[] { "Cars", "RentalCar", "ReturnCar", "CarRentalHistory", "RentalConditions", "[red]Back[/]" };
        var title = "-- ReservationMenu --";

        while (circle)
        {
            AnsiConsole.Clear();
            var selection = Selection.SelectionMenu(title, options);
            switch (selection)
            {
                case "Cars":
                    await CarsAsync();
                    break;
                case "RentalCar":
                    await RentalCarAsync();
                    break;
                case "ReturnCar":
                    await ReturnCarAsync();
                    break;
                case "CarRentalHistory":
                    await CarRentalHistoryAsync();
                    break;
                case "RentalConditions":
                    RentalConditions();
                    break;
                case "[red]Back[/]":
                    circle = false;
                    break;
            }
        }
    }

    public async ValueTask CarsAsync()
    {
        var categories = await categoryService.GetAllAsync();
        string[] options = new string[categories.Count() + 1];
        int i = 0;
        foreach (var category in categories)
        {
            options[i] = $"{category.Id} {category.Name}";
            i++;
        }
        options[categories.Count()] = "AllCar";
        var selection = Selection.SelectionMenu("", options);
        if (selection == "AllCar")
        {
            var cars = await carService.GetAllNotBookedAsync();
            var table = Selection.DataTable("Cars", cars.ToList());
            AnsiConsole.Write(table);
        }
        else
        {
            long categoryId = long.Parse(selection.Split(' ')[0]);
            var cars = await carService.GetByCategoryIdAsync(categoryId);
            var table = Selection.DataTable("Cars", cars.ToList());
            AnsiConsole.Write(table);
        }
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    public async ValueTask RentalCarAsync()
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

        DateTime reservationDate = AnsiConsole.Ask<DateTime>("Enter  reservationDate (mm.dd.yyyy): ");
        while (reservationDate < DateTime.Now)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            reservationDate = AnsiConsole.Ask<DateTime>("Enter  reservationDate (mm.dd.yyyy): ");
        }

        var selection1 = Selection.SelectionMenu("ReservationDate");

        DateTime returnDate = AnsiConsole.Ask<DateTime>("Enter  returnDate (mm.dd.yyyy): ");
        while (returnDate < reservationDate)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            returnDate = AnsiConsole.Ask<DateTime>("Enter  returnDate (mm.dd.yyyy): ");
        }

        var selection2 = Selection.SelectionMenu("ReturnDate");
        var model = new ReservationCreationModel()
        {
            CarId = id,
            CustomerId = customer.Id,
            ReservationDate = reservationDate,
            ReservationTime = Convert.ToDateTime(selection1),
            ReturnDate = returnDate,
            ReturnTime = Convert.ToDateTime(selection2)
        };

        try
        {
            var reservation = await reservationService.RentalCarAsync(model);
            AnsiConsole.Markup("[orange3]Succesful created[/]\n");

            var carTable = Selection.DataTable("Car", reservation.Car);
            AnsiConsole.Write(carTable);
            var customerTable = Selection.DataTable("Customer", reservation.Customer);
            AnsiConsole.Write(customerTable);
            var reservationTable = Selection.DataTable("Reservation", reservation);
            AnsiConsole.Write(reservationTable);
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]{ex.Message}[/]\n");
        }
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    public async ValueTask ReturnCarAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter  reservation Id: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter reservation Id : ");
        }

        try
        {
            var reservation = await reservationService.ReturnCarAsync(id);
            AnsiConsole.Markup("[orange3]Succesful[/]\n");
            var reservationTable = Selection.DataTable("Reservation", reservation);
            AnsiConsole.Write(reservationTable);
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]{ex.Message}[/]\n");
        }
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    public void RentalConditions()
    {
        Console.Clear();
        Console.WriteLine(Constants.CONDITIONS);
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    public async ValueTask CarRentalHistoryAsync()
    {
        Console.Clear();
        var reservations = await reservationService.CarRentalHistoryAsync(customer.Id);
        var table = Selection.DataTable("Reservations", reservations.ToList());
        AnsiConsole.Write(table);
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }
    public async ValueTask<bool> SecurityCheck()
    {
        AnsiConsole.Clear();
        long id = AnsiConsole.Ask<long>("Enter user Id : ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter user Id : ");
        }

        string password = AnsiConsole.Prompt(
        new TextPrompt<string>("Enter password :")
         .PromptStyle("red")
         .Secret());

        try
        {
            customer = await customerService.SecurityCheckAsync(id, password);
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"[red]{ex.Message}[/]\n");
            Console.WriteLine("Enter any keyword to continue");
            Console.ReadKey();
            Console.Clear();
            return false;
        }
        return true;
    }
}