using RentCar.Uz.Configurations;
using RentCar.Uz.Extensions;
using RentCar.Uz.Helpers;
using RentCar.Uz.Interfaces;
using RentCar.Uz.Models.Cars;

namespace RentCar.Uz.Services;

public class CarService : ICarService
{
    private List<Car> cars;
    private CategoryService categoryService;
    public CarService(CategoryService categoryService)
    {
        this.categoryService = categoryService;
    }
    public async ValueTask<CarViewModel> CreateAsync(CarCreationModel car)
    {
        var existCategory = await categoryService.GetByIdAsync(car.CategoryId);
        cars = await FileIO.ReadAsync<Car>(Constants.CARS_PATH);
        var existCar = cars.FirstOrDefault(c => c.Model.ToLower() == car.Model.ToLower() && c.Brand.ToLower() == car.Brand.ToLower());
        if (existCar != null)
        {
            if (existCar.IsDeleted)
                return await UpdateAsync(existCar.Id, car.MapTo<CarUpdatedModel>(), true);

            throw new Exception($"This car is already exist with this brand model: {car.Brand} {car.Model}");
        }

        if (!File.Exists(car.CarPng))
            throw new Exception($"This car path file is not exist with this path {car.CarPng}");

        var createdCar = cars.Create<Car>(car.MapTo<Car>());
        await FileIO.WriteAsync(Constants.CARS_PATH, cars);
        return createdCar.MapTo<CarViewModel>();
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        cars = await FileIO.ReadAsync<Car>(Constants.CARS_PATH);
        var existCar = cars.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This car is not found with this id: {id}");

        existCar.IsDeleted = true;
        existCar.DeletedAt = DateTime.UtcNow;
        await FileIO.WriteAsync(Constants.CARS_PATH, cars);

        return true;
    }

    public async ValueTask<IEnumerable<CarViewModel>> GetAllAsync()
    {
        cars = await FileIO.ReadAsync<Car>(Constants.CARS_PATH);
        return cars.Where(c => !c.IsDeleted).MapTo<CarViewModel>();
    }

    public async ValueTask<CarViewModel> GetByIdAsync(long id)
    {
        cars = await FileIO.ReadAsync<Car>(Constants.CARS_PATH);
        var existCar = cars.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This car is not found with this id: {id}");

        return existCar.MapTo<CarViewModel>();
    }

    public async ValueTask<CarViewModel> UpdateAsync(long id, CarUpdatedModel car, bool isUsesDeleted = false)
    {
        var existCategory = await categoryService.GetByIdAsync(car.CategoryId);
        cars = await FileIO.ReadAsync<Car>(Constants.CARS_PATH);
        var existCar = new Car();
        if (isUsesDeleted)
            existCar = cars.FirstOrDefault(c => c.Id == id);
        else
            existCar = cars.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new Exception($"This car is not found with this id: {id}");

        if (!File.Exists(car.CarPng))
            throw new Exception($"This car path file is not exist with this path {car.CarPng}");

        existCar.Brand = car.Brand;
        existCar.Model = car.Model;
        existCar.IsDeleted = false;
        existCar.CarPng = car.CarPng;
        existCar.Deposit = car.Deposit;
        existCar.CategoryId = car.CategoryId;
        existCar.UpdatedAt = DateTime.UtcNow;
        existCar.DailyPrice = car.DailyPrice;
        existCar.Description = car.Description;

        await FileIO.WriteAsync(Constants.CARS_PATH, cars);
        return existCar.MapTo<CarViewModel>();
    }

    public async ValueTask<IEnumerable<CarViewModel>> GetByCategoryIdAsync(long categoryId)
    {
        cars = await FileIO.ReadAsync<Car>(Constants.CARS_PATH);
        return cars.Where(c => !c.IsDeleted && c.IsAvailable).Where(c => c.CategoryId == categoryId).MapTo<CarViewModel>();
    }

    public async ValueTask<IEnumerable<CarViewModel>> GetAllNotBookedAsync()
    {
        cars = await FileIO.ReadAsync<Car>(Constants.CARS_PATH);
        return cars.Where(c => !c.IsDeleted && c.IsAvailable).MapTo<CarViewModel>();
    }

    public async ValueTask CarIsAvailableAsync(long id, bool available)
    {
        cars = await FileIO.ReadAsync<Car>(Constants.CARS_PATH);
        var existCar = cars.FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        existCar.IsAvailable = available;

        await FileIO.WriteAsync(Constants.CARS_PATH, cars);
    }
}