using RentCar.Uz.Models.Reservations;

namespace RentCar.Uz.Interfaces;

public interface IReservationService
{
    ValueTask<ReservationViewModel> RentalCarAsync(ReservationCreationModel model);
    ValueTask<ReservationViewModel> ReturnCarAsync(long id);
    ValueTask<IEnumerable<ReservationViewModel>> CarRentalHistoryAsync(long customerId);
}