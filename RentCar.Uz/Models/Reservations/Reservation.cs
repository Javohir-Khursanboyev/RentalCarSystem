using RentCar.Uz.Enums;
using RentCar.Uz.Models.Commons;

namespace RentCar.Uz.Models.Reservations;

public class Reservation:Auditable
{
    public long CustomerId { get; set; }
    public long CarId { get; set; } 
    public DateTime ReservationDate { get; set; }
    public DateTime ReservationTime { get; set; }
    public DateTime ReturnDate { get; set; }
    public DateTime ReturnTime { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal AdditionalPayment {  get; set; }
    public ReservationStatus Status { get; set; }
}