using RentCar.Uz.Display;

namespace RentCar.Uz;

internal class Program
{
    static async Task Main(string[] args)
    {
        var mainMenu = new MainMenu();
        await mainMenu.Main();
    }
}