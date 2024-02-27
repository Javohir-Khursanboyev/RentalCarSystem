using RentCar.Uz.Models.Customers;
using RentCar.Uz.Services;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace RentCar.Uz.Display;

public class CustomerMenu
{
    private readonly CustomerService customerService;
    public CustomerMenu(CustomerService customerService)
    {
        this.customerService = customerService;
    }

    public async ValueTask DisplayAsync()
    {
        bool circle = true;
        var options = new string[] { "Create", "GetById", "Update", "Delete", "GetAll", "Deposit", "[red]Back[/]" };
        var title = "-- CustomerMenu --";

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
                case "Deposit":
                    await DepositAsync();
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
        string firstName = AnsiConsole.Ask<string>("FirstName:");
        string lastName = AnsiConsole.Ask<string>("LastName:");
        DateTime dateOfBirth = AnsiConsole.Ask<DateTime>("Enter dateOfBirth [blue]mm.dd.year:[/]");

        string email = AnsiConsole.Ask<string>("Email [blue](email@gmail.com):[/]");
        while (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{3,}$"))
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            email = AnsiConsole.Ask<string>("Email [blue](email@gmail.com):[/]");
        }

        string passportNumber = AnsiConsole.Ask<string>("Passport number [blue]AC XXX xx xx[/]:");
        while (!Regex.IsMatch(passportNumber, @"^((AC\s)?\w{2})?\s\d{3}\s\d{2}\s\d{2}$"))
        {
            AnsiConsole.MarkupLine("[red]Invalid input.[/]");
            passportNumber = AnsiConsole.Ask<string>("Passport number [blue]AC XXX xx xx[/]:");
        }

        string phone = AnsiConsole.Ask<string>("Phone [blue](+998XXxxxxxxx):[/]");
        while (!Regex.IsMatch(phone, @"^\+998\d{9}$"))
        {
            AnsiConsole.MarkupLine("[red]Invalid input.[/]");
            phone = AnsiConsole.Ask<string>("Phone [blue](+998XXxxxxxxx):[/]");
        }

        string password = AnsiConsole.Prompt(
        new TextPrompt<string>("Enter password :")
         .PromptStyle("red")
         .Secret());

        CustomerCreationModel customer = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            Phone = phone,
            PassportNumber = passportNumber,
            DateOfBirth = dateOfBirth
        };
        try
        {
            var addedCustomer = await customerService.CreateAsync(customer);
            AnsiConsole.Markup("[orange3]Succesful created[/]\n");

            var table = Selection.DataTable("Customer", addedCustomer);
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
        long id = AnsiConsole.Ask<long>("Enter customer Id to update: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter customer Id to update: ");
        }
        string firstName = AnsiConsole.Ask<string>("FirstName:");
        string lastName = AnsiConsole.Ask<string>("LastName:");
        DateTime dateOfBirth = AnsiConsole.Ask<DateTime>("Enter dateOfBirth [blue]mm.dd.year:[/]");

        string email = AnsiConsole.Ask<string>("Email [blue](email@gmail.com):[/]");
        while (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{3,}$"))
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            email = AnsiConsole.Ask<string>("Email [blue](email@gmail.com):[/]");
        }
        string passportNumber = AnsiConsole.Ask<string>("Passport number [blue]AC XXX xx xx[/]:");
        while (!Regex.IsMatch(passportNumber, @"^((AC\s)?\w{2})?\s\d{3}\s\d{2}\s\d{2}$"))
        {
            AnsiConsole.MarkupLine("[red]Invalid input.[/]");
            passportNumber = AnsiConsole.Ask<string>("Passport number [blue]AC XXX xx xx[/]:");
        }

        string phone = AnsiConsole.Ask<string>("Phone [blue](+998XXxxxxxxx):[/]");
        while (!Regex.IsMatch(phone, @"^\+998\d{9}$"))
        {
            AnsiConsole.MarkupLine("[red]Invalid input.[/]");
            phone = AnsiConsole.Ask<string>("Phone [blue](+998XXxxxxxxx):[/]");
        }

        string password = AnsiConsole.Prompt(
        new TextPrompt<string>("Enter password :")
         .PromptStyle("red")
         .Secret());

        CustomerUpdatedModel customer = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            PassportNumber = passportNumber,
            DateOfBirth = dateOfBirth,
            Password = password
        };
        try
        {
            var updatedCustomer = await customerService.UpdateAsync(id, customer);
            AnsiConsole.Markup("[orange3]Succesful updated[/]\n");

            var table = Selection.DataTable("Customer", updatedCustomer);
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

    async ValueTask GetAllAsync()
    {
        Console.Clear();
        var customers = await customerService.GetAllAsync();
        var table = Selection.DataTable("Customers", customers.ToList());
        AnsiConsole.Write(table);
        Console.WriteLine("Enter any keyword to continue");
        Console.ReadKey();
        Console.Clear();
    }

    async ValueTask DeleteAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter customer Id to delete: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter customer Id to delete: ");
        }

        try
        {
            await customerService.DeleteAsync(id);
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

    async ValueTask GetByIdAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter customer Id: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter customer Id: ");
        }

        try
        {
            var customer = await customerService.GetByIdAsync(id);
            var table = Selection.DataTable("Customer", customer);
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

    async ValueTask DepositAsync()
    {
        Console.Clear();
        long id = AnsiConsole.Ask<long>("Enter customer Id: ");
        while (id <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            id = AnsiConsole.Ask<long>("Enter customer Id: ");
        }

        decimal amount = AnsiConsole.Ask<decimal>("Enter amount : ");
        while (amount <= 0)
        {
            AnsiConsole.MarkupLine("[red]Was entered in the wrong format .Try again![/]");
            amount = AnsiConsole.Ask<decimal>("Enter amount : ");
        }

        try
        {
            var customer = await customerService.DepositAsync(id, amount);
            var table = Selection.DataTable("Customer", customer);
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
}
