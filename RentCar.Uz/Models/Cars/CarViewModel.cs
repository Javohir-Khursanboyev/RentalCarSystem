namespace RentCar.Uz.Models.Cars;

public class CarViewModel
{
    public long Id { get; set; }
    public long CategoryId { get; set; }
    public string Model { get; set; }
    public string Brand { get; set; }
    public string CarPng { get; set; }
    public string Description { get; set; }
    public decimal DailyPrice { get; set; }
    public decimal Deposit { get; set; }
    public bool IsAvailable { get; set; } = true;
}
