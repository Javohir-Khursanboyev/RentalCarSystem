using RentCar.Uz.Services;
using Spectre.Console;

namespace RentCar.Uz.Display;

public class MainMenu
{
    private readonly CarCategoryMenu carCategoryMenu;
    private readonly CarMenu carMenu;
    private readonly CustomerMenu customerMenu;
    private readonly ReservationMenu reservationMenu;

    private readonly CategoryService categoryService;
    private readonly CarService carService;
    private readonly CustomerService customerService;
    private readonly ReservationService reservationService;
    public MainMenu()
    {
        categoryService = new CategoryService();
        carService = new CarService(categoryService);
        customerService = new CustomerService();
        reservationService = new ReservationService(customerService, carService);

        carCategoryMenu = new CarCategoryMenu(categoryService);
        carMenu = new CarMenu(carService, categoryService);
        customerMenu = new CustomerMenu(customerService);
        reservationMenu = new ReservationMenu(reservationService, categoryService, customerService, carService);
    }
    public async ValueTask Main()
    {
        bool circle = true;
        while (circle)
        {

            Console.Clear();
            var table = Selection.DataTable();
            AnsiConsole.Write(table);
            Console.Write(">>> :");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": await carCategoryMenu.DisplayAsync(); break;
                case "2": await carMenu.DisplayAsync(); break;
                case "3": await customerMenu.DisplayAsync(); break;
                case "4": await reservationMenu.DisplayAsync(); break;
                case "5": circle = false; break;
                default: Console.Clear(); Console.WriteLine("You have entered an invalid command !"); Thread.Sleep(800); break;
            }
        }
    }
}
