using RentCar.Uz.Models.CarCategories;
using RentCar.Uz.Models.Cars;
using RentCar.Uz.Models.Customers;
using RentCar.Uz.Models.Reservations;
using Spectre.Console;

namespace RentCar.Uz.Display;

public static partial class Selection
{
    public static string SelectionMenu(string title, string[] options)
    {
        var selection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title($"[darkorange3_1]{title}[/]")
        .PageSize(5)
        .AddChoices(options)
        .HighlightStyle(new Style(foreground: Color.White, background: Color.Blue))
        );
        return selection;
    }

    public static string SelectionMenu(string title)
    {
        string[] options = { "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00",
            "21:00", "22:00", "23:00", "00:00", "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", };
        var selection = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title($"[darkorange3_1]{title}[/]")
        .PageSize(3)
        .AddChoices(options)
        .HighlightStyle(new Style(foreground: Color.White, background: Color.Blue))
        );
        return selection;
    }

    public static Table DataTable()
    {
        var table = new Table();

        table.AddColumn("[blue]1.CarCategory[/]");
        table.AddColumn("[blue]2.Car[/]");
        table.AddColumn("[blue]3.Customer[/]");
        table.AddColumn("[blue]4.Reservation[/]");
        table.AddColumn("[red]5.Exit[/]");

        return table;
    }
}

public static partial class Selection
{
    public static Table DataTable(string title, CategoryViewModel category)
    {
        var table = new Table();
        table.Title(title);
        table.AddColumn("[slateblue1]Id[/]");
        table.AddColumn("[slateblue1]Name[/]");

        table.AddRow(category.Id.ToString(), category.Name);
        return table;
    }

    public static Table DataTable(string title, List<CategoryViewModel> categories)
    {
        var table = new Table();
        table.Title(title);
        table.AddColumn("[slateblue1]Id[/]");
        table.AddColumn("[slateblue1]Name[/]");

        foreach (var category in categories)
        {
            table.AddRow(category.Id.ToString(), category.Name);
        }
        return table;
    }
}

public static partial class Selection
{
    public static Table DataTable(string title, CustomerViewModel customer)
    {
        var table = new Table();
        table.Title(title);
        table.AddColumn("[slateblue1]Id[/]");
        table.AddColumn("[slateblue1]FirstName[/]");
        table.AddColumn("[slateblue1]LastName[/]");
        table.AddColumn("[slateblue1]DateOfBirth[/]");
        table.AddColumn("[slateblue1]Email[/]");
        table.AddColumn("[slateblue1]Phone[/]");
        table.AddColumn("[slateblue1]PassportNumber[/]");
        table.AddColumn("[slateblue1]Balance[/]");

        table.AddRow(customer.Id.ToString(), customer.FirstName, customer.LastName, customer.DateOfBirth.ToString(),
            customer.Email, customer.Phone, customer.PassportNumber, customer.Balance.ToString());
        return table;
    }

    public static Table DataTable(string title, List<CustomerViewModel> customers)
    {
        var table = new Table();
        table.Title(title);
        table.AddColumn("[slateblue1]Id[/]");
        table.AddColumn("[slateblue1]FirstName[/]");
        table.AddColumn("[slateblue1]LastName[/]");
        table.AddColumn("[slateblue1]DateOfBirth[/]");
        table.AddColumn("[slateblue1]Email[/]");
        table.AddColumn("[slateblue1]Phone[/]");
        table.AddColumn("[slateblue1]PassportNumber[/]");
        table.AddColumn("[slateblue1]Balance[/]");

        foreach (var customer in customers)
        {
            table.AddRow(customer.Id.ToString(), customer.FirstName, customer.LastName, customer.DateOfBirth.ToString(), customer.Email, customer.Phone, customer.PassportNumber, customer.Balance.ToString());
        }
        return table;
    }
}

public static partial class Selection
{
    public static Table DataTable(string title, CarViewModel car)
    {
        var table = new Table();
        table.Title(title);
        table.AddColumn("");

        var image = new CanvasImage(car.CarPng);
        image.MaxWidth(16);
        table.AddRow(image);
        var panel = new Panel($"Id : {car.Id} \nCategoryId : {car.CategoryId} \nCar : {car.Brand} {car.Model} \nDescription : {car.Description}" +
            $" \nDailyPrice : {car.DailyPrice} \nDeposit : {car.Deposit}");
        panel.Padding = new Padding(1, 1, 1, 1);
        panel.Border = BoxBorder.None;
        table.AddRow(panel);

        table.Border(TableBorder.None);
        return table;
    }

    public static Table DataTable(string title, List<CarViewModel> cars)
    {
        if (cars.Count == 0)
            Console.WriteLine("Cars is not already please first create car");

        var table = new Table();
        table.Title(title);
        table.AddColumn("");

        foreach (var car in cars)
        {
            var image = new CanvasImage(car.CarPng);
            image.MaxWidth(16);
            table.AddRow(image);
            var panel = new Panel($"Id : {car.Id} \nCategoryId : {car.CategoryId} \nCar : {car.Brand} {car.Model} \nDescription : {car.Description} " +
                $"\nDailyPrice : {car.DailyPrice} \nDeposit : {car.Deposit}");
            panel.Padding = new Padding(1, 1, 1, 1);
            panel.Border = BoxBorder.None;
            table.AddRow(panel);

        }
        table.Border(TableBorder.None);
        return table;
    }
}

public static partial class Selection
{
    public static Table DataTable(string title, ReservationViewModel model)
    {
        var table = new Table();
        table.Title(title);
        table.AddColumn("[slateblue1]Id[/]");
        table.AddColumn("[slateblue1]ReservationDate[/]");
        table.AddColumn("[slateblue1]ReservationTime[/]");
        table.AddColumn("[slateblue1]ReturnDate[/]");
        table.AddColumn("[slateblue1]ReturnTime[/]");
        table.AddColumn("[slateblue1]TotalAmount[/]");
        table.AddColumn("[slateblue1]AdditionalPayment[/]");
        table.AddColumn("[slateblue1]Statust[/]");

        table.AddRow(model.Id.ToString(), model.ReservationDate.ToString(), model.ReservationTime.TimeOfDay.ToString(), model.ReturnDate.ToString(),
            model.ReturnTime.TimeOfDay.ToString(), model.TotalAmount.ToString(), model.AdditionalPayment.ToString(), model.Status.ToString());
        return table;
    }

    public static Table DataTable(string title, List<ReservationViewModel> models)
    {
        var table = new Table();
        table.Title(title);
        table.AddColumn("[slateblue1]Id[/]");
        table.AddColumn("[slateblue1]ReservationDate[/]");
        table.AddColumn("[slateblue1]ReservationTime[/]");
        table.AddColumn("[slateblue1]ReturnDate[/]");
        table.AddColumn("[slateblue1]ReturnTime[/]");
        table.AddColumn("[slateblue1]TotalAmount[/]");
        table.AddColumn("[slateblue1]AdditionalPayment[/]");
        table.AddColumn("[slateblue1]Status[/]");

        foreach (var model in models)
        {
            table.AddRow(model.Id.ToString(), model.ReservationDate.ToString(), model.ReservationTime.TimeOfDay.ToString(), model.ReturnDate.ToString(),
            model.ReturnTime.TimeOfDay.ToString(), model.TotalAmount.ToString(), model.AdditionalPayment.ToString(), model.Status.ToString());
        }
        return table;
    }
}