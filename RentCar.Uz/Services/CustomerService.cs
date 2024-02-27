using RentCar.Uz.Configurations;
using RentCar.Uz.Extensions;
using RentCar.Uz.Helpers;
using RentCar.Uz.Interfaces;
using RentCar.Uz.Models.Customers;

namespace RentCar.Uz.Services;

public class CustomerService : ICustomerService
{
    private List<Customer> customers;
    public async ValueTask<CustomerViewModel> CreateAsync(CustomerCreationModel customer)
    {
        customers = await FileIO.ReadAsync<Customer>(Constants.CUSTOMERS_PATH);
        var existCustomer = customers.FirstOrDefault(c => c.Email == customer.Email);
        if (existCustomer != null)
        {
            if (existCustomer.IsDeleted)
                return await UpdateAsync(existCustomer.Id, customer.MapTo<CustomerUpdatedModel>(), true);

            throw new Exception($"This customer is already exist With this email: {customer.Email}");
        }

        var createdCustomer = customers.Create<Customer>(customer.MapTo<Customer>());
        await FileIO.WriteAsync(Constants.CUSTOMERS_PATH, customers);
        return createdCustomer.MapTo<CustomerViewModel>();
    }

    public async ValueTask<bool> DeleteAsync(long id)
    {
        customers = await FileIO.ReadAsync<Customer>(Constants.CUSTOMERS_PATH);
        var existCustomer = customers.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This customer is not found With this id: {id}");

        existCustomer.IsDeleted = true;
        existCustomer.DeletedAt = DateTime.UtcNow;
        await FileIO.WriteAsync(Constants.CUSTOMERS_PATH, customers);

        return true;
    }

    public async ValueTask<IEnumerable<CustomerViewModel>> GetAllAsync()
    {
        customers = await FileIO.ReadAsync<Customer>(Constants.CUSTOMERS_PATH);
        return customers.Where(c => !c.IsDeleted).MapTo<CustomerViewModel>();
    }

    public async ValueTask<CustomerViewModel> GetByIdAsync(long id)
    {
        customers = await FileIO.ReadAsync<Customer>(Constants.CUSTOMERS_PATH);
        var existCustomer = customers.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This customer is not found With this id: {id}");

        return existCustomer.MapTo<CustomerViewModel>();
    }

    public async ValueTask<CustomerViewModel> UpdateAsync(long id, CustomerUpdatedModel customer, bool isUsesDeleted = false)
    {
        customers = await FileIO.ReadAsync<Customer>(Constants.CUSTOMERS_PATH);
        var existCustomer = new Customer();
        if (isUsesDeleted)
            existCustomer = customers.FirstOrDefault(c => c.Id == id);
        else
            existCustomer = customers.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
                ?? throw new Exception($"This customer is not found With this id: {id}");

        existCustomer.IsDeleted = false;
        existCustomer.Email = customer.Email;
        existCustomer.Phone = customer.Phone;
        existCustomer.UpdatedAt = DateTime.UtcNow;
        existCustomer.Password = customer.Password;
        existCustomer.LastName = customer.LastName;
        existCustomer.FirstName = customer.FirstName;
        existCustomer.DateOfBirth = customer.DateOfBirth;

        await FileIO.WriteAsync(Constants.CUSTOMERS_PATH, customers);
        return existCustomer.MapTo<CustomerViewModel>();
    }

    public async ValueTask<CustomerViewModel> SecurityCheckAsync(long id, string password)
    {
        customers = await FileIO.ReadAsync<Customer>(Constants.CUSTOMERS_PATH);
        var existCustomer = customers.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This customer is not found With this id: {id}");
        if (existCustomer.Password != password)
            throw new Exception($"This password: {password} is incorrect");

        return existCustomer.MapTo<CustomerViewModel>();
    }

    public async ValueTask<CustomerViewModel> DepositAsync(long id, decimal amount)
    {
        customers = await FileIO.ReadAsync<Customer>(Constants.CUSTOMERS_PATH);
        var existCustomer = customers.FirstOrDefault(c => c.Id == id && !c.IsDeleted)
            ?? throw new Exception($"This customer is not found With this id: {id}");

        existCustomer.Balance += amount;
        await FileIO.WriteAsync(Constants.CUSTOMERS_PATH, customers);
        return existCustomer.MapTo<CustomerViewModel>();
    }

    public async ValueTask ToPayAsync(long id, decimal amount, decimal deposit)
    {
        customers = await FileIO.ReadAsync<Customer>(Constants.CUSTOMERS_PATH);
        var existCustomer = customers.FirstOrDefault(c => c.Id == id);
        if (existCustomer.Balance < amount + deposit)
            throw new Exception($"Customer balance is not enough this balance {existCustomer.Balance}");

        existCustomer.Balance -= (amount + deposit);
        await FileIO.WriteAsync(Constants.CUSTOMERS_PATH, customers);
    }

    public async ValueTask ReturnOfPaymentAsync(long id, decimal amount, decimal deposit)
    {
        customers = await FileIO.ReadAsync<Customer>(Constants.CUSTOMERS_PATH);
        var existCustomer = customers.FirstOrDefault(c => c.Id == id);
        existCustomer.Balance += (deposit - amount);
        await FileIO.WriteAsync(Constants.CUSTOMERS_PATH, customers);
    }
}