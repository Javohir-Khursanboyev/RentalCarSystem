using RentCar.Uz.Models.Cars;

namespace RentCar.Uz.Interfaces;

public interface ICarService
{
    ValueTask<CarViewModel> CreateAsync(CarCreationModel car);
    ValueTask<CarViewModel> UpdateAsync(long id, CarUpdatedModel car, bool isUsesDeleted);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<CarViewModel> GetByIdAsync(long id);
    ValueTask<IEnumerable<CarViewModel>> GetByCategoryIdAsync(long categoryId);
    ValueTask<IEnumerable<CarViewModel>> GetAllNotBookedAsync();
    ValueTask<IEnumerable<CarViewModel>> GetAllAsync();
    ValueTask CarIsAvailableAsync(long id, bool available);
}