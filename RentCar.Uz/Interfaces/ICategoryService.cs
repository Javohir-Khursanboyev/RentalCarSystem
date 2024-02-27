using RentCar.Uz.Models.CarCategories;

namespace RentCar.Uz.Interfaces;

public interface ICategoryService
{
    ValueTask<CategoryViewModel> CreateAsync(CategoryCreationModel category);
    ValueTask<CategoryViewModel> UpdateAsync(long id, CategoryUpdatedModel category, bool isUsesDeleted);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<CategoryViewModel> GetByIdAsync(long id);
    ValueTask<IEnumerable<CategoryViewModel>> GetAllAsync();
}
