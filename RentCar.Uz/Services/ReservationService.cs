using RentCar.Uz.Configurations;
using RentCar.Uz.Extensions;
using RentCar.Uz.Helpers;
using RentCar.Uz.Interfaces;
using RentCar.Uz.Models.Reservations;

namespace RentCar.Uz.Services;

public class ReservationService : IReservationService
{
    private readonly CustomerService customerService;
    private readonly CarService carService;
    private List<Reservation> reservations;
    public ReservationService(CustomerService customerService, CarService carService)
    {
        this.customerService = customerService;
        this.carService = carService;
    }

    public async ValueTask<IEnumerable<ReservationViewModel>> CarRentalHistoryAsync(long customerId)
    {
        reservations = await FileIO.ReadAsync<Reservation>(Constants.RESERVATIONS_PATH);
        return reservations.Where(r => r.CustomerId == customerId).MapTo<ReservationViewModel>();
    }

    public async ValueTask<ReservationViewModel> RentalCarAsync(ReservationCreationModel model)
    {
        var existCustomer = await customerService.GetByIdAsync(model.CustomerId);
        var existCar = await carService.GetByIdAsync(model.CarId);

        if (!existCar.IsAvailable)
            throw new Exception($"This car already rental");

        reservations = await FileIO.ReadAsync<Reservation>(Constants.RESERVATIONS_PATH);
        model.TotalAmount = model.TotalAmount.CalculateTotalAmount(model.MapTo<Reservation>(), existCar.DailyPrice);
        await customerService.ToPayAsync(model.CustomerId, model.TotalAmount, existCar.Deposit);
        var createdReservation = reservations.Create<Reservation>(model.MapTo<Reservation>());
        createdReservation.Status = Enums.ReservationStatus.Active;
        await carService.CarIsAvailableAsync(model.CarId, false);

        await FileIO.WriteAsync(Constants.RESERVATIONS_PATH, reservations);

        var viewModel = createdReservation.MapTo<ReservationViewModel>();
        viewModel.Customer = existCustomer;
        viewModel.Car = existCar;
        return viewModel;
    }

    public async ValueTask<ReservationViewModel> ReturnCarAsync(long id)
    {
        reservations = await FileIO.ReadAsync<Reservation>(Constants.RESERVATIONS_PATH);
        var existReservation = reservations.FirstOrDefault(r => r.Id == id)
            ?? throw new Exception($"This reservation is not found with this id {id}");

        var existCustomer = await customerService.GetByIdAsync(existReservation.CustomerId);
        var existCar = await carService.GetByIdAsync(existReservation.CarId);

        if (existCar.IsAvailable)
            throw new Exception($"This car already return ");

        existReservation.AdditionalPayment = existReservation.AdditionalPayment.CalculateTotalAmount(existReservation);
        existReservation.DeletedAt = DateTime.UtcNow;
        existReservation.Status = Enums.ReservationStatus.Completed;
        await carService.CarIsAvailableAsync(existReservation.CarId, true);
        await customerService.ReturnOfPaymentAsync(existReservation.CustomerId, existReservation.AdditionalPayment, existCar.Deposit);
        await FileIO.WriteAsync(Constants.RESERVATIONS_PATH, reservations);

        var viewModel = existReservation.MapTo<ReservationViewModel>();
        viewModel.Customer = existCustomer;
        viewModel.Car = existCar;
        return viewModel;
    }
}