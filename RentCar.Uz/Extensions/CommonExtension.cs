using RentCar.Uz.Models.Reservations;

namespace RentCar.Uz.Extensions;

public static class CommonExtension
{
    public static decimal CalculateTotalAmount(this decimal totalAmount, Reservation reservation, decimal dailyPrice)
    {
        TimeSpan rentalDuration = reservation.ReturnDate.Date.Add(reservation.ReturnTime.TimeOfDay) - reservation.ReservationDate.Date.Add(reservation.ReservationTime.TimeOfDay);

        totalAmount = dailyPrice * (decimal)rentalDuration.TotalDays;

        return totalAmount;
    }

    public static decimal CalculateTotalAmount(this decimal additionalPayment, Reservation reservation)
    {
        TimeSpan rentalDuration = DateTime.Now.Date.AddDays(1).Add(DateTime.Now.TimeOfDay) - reservation.ReturnDate.Date.Add(reservation.ReservationTime.TimeOfDay);

        additionalPayment = 100000 * (decimal)rentalDuration.TotalHours;

        if (additionalPayment <= 0)
            additionalPayment = 0;

        return additionalPayment;
    }
}
