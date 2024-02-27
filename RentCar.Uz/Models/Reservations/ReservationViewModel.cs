using RentCar.Uz.Enums;
using RentCar.Uz.Models.Cars;
using RentCar.Uz.Models.Customers;

namespace RentCar.Uz.Models.Reservations;

public class ReservationViewModel
{
    public long Id { get; set; }
    public CustomerViewModel Customer { get; set; }
    public CarViewModel Car { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime ReservationTime { get; set; }
    public DateTime ReturnDate { get; set; }
    public DateTime ReturnTime { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AdditionalPayment { get; set; }
    public ReservationStatus Status { get; set; }
}