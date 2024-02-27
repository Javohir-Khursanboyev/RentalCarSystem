using RentCar.Uz.Configurations;
using RentCar.Uz.Extensions;
using RentCar.Uz.Helpers;
using RentCar.Uz.Interfaces;
using RentCar.Uz.Models.CarCategories;

namespace RentCar.Uz.Services;

public class CategoryService : ICategoryService
{
    private List<Category> categories;
    public async ValueTask<CategoryViewModel> CreateAsync(CategoryCreationModel category)
    {
        categories = await FileIO.ReadAsync<Category>(Constants.CAR_CATEGORIES_PATH);
        var existCategory = categories.FirstOrDefault(c => c.Name.ToLower() == category.Name.ToLower());
        if (existCategory != null)
        {
            if (existCategory.IsDeleted)
                return await UpdateAsync(existCategory.Id, category.MapTo<CategoryUpdatedModel>(), true);

            throw new Exception($"This category is already exist with this name: {category.Name}");
        }

        var createdCategory = categories.Create<Category>(category.MapTo<Category>());
        await FileIO.WriteAsync(Constants.CAR_CATEGORIES_PATH, categories);
        return createdCategory.MapTo<CategoryViewModel>();
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        categories = await FileIO.ReadAsync<Category>(Constants.CAR_CATEGORIES_PATH);
        var existCategory = categories.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This category is not found with this id: {id}");

        existCategory.IsDeleted = true;
        existCategory.DeletedAt = DateTime.UtcNow;
        await FileIO.WriteAsync(Constants.CAR_CATEGORIES_PATH, categories);

        return true;
    }

    public async ValueTask<IEnumerable<CategoryViewModel>> GetAllAsync()
    {
        categories = await FileIO.ReadAsync<Category>(Constants.CAR_CATEGORIES_PATH);
        return categories.Where(c => !c.IsDeleted).MapTo<CategoryViewModel>();
    }

    public async ValueTask<CategoryViewModel> GetByIdAsync(long id)
    {
        categories = await FileIO.ReadAsync<Category>(Constants.CAR_CATEGORIES_PATH);
        var existCategory = categories.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This category is not found with this id: {id}");

        return existCategory.MapTo<CategoryViewModel>();
    }

    public async ValueTask<CategoryViewModel> UpdateAsync(long id, CategoryUpdatedModel category, bool isUsesDeleted = false)
    {
        categories = await FileIO.ReadAsync<Category>(Constants.CAR_CATEGORIES_PATH);
        var existCategory = new Category();
        if (isUsesDeleted)
            existCategory = categories.FirstOrDefault(c => c.Id == id);
        else
            existCategory = categories.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This category is not found with this id: {id}");

        existCategory.IsDeleted = false;
        existCategory.Name = category.Name;
        existCategory.UpdatedAt = DateTime.UtcNow;
        await FileIO.WriteAsync(Constants.CAR_CATEGORIES_PATH, categories);

        return existCategory.MapTo<CategoryViewModel>();
    }
}