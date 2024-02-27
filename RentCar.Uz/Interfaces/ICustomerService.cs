using RentCar.Uz.Models.Customers;

namespace RentCar.Uz.Interfaces;

public interface ICustomerService
{
    ValueTask<CustomerViewModel> CreateAsync(CustomerCreationModel customer);
    ValueTask<CustomerViewModel> UpdateAsync(long id, CustomerUpdatedModel customer, bool isUsesDeleted);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<CustomerViewModel> GetByIdAsync(long id);
    ValueTask<IEnumerable<CustomerViewModel>> GetAllAsync();
    ValueTask<CustomerViewModel> DepositAsync(long id, decimal amount);
    ValueTask<CustomerViewModel> SecurityCheckAsync(long id, string password);
    ValueTask ToPayAsync(long id, decimal amount, decimal deposit);
    ValueTask ReturnOfPaymentAsync(long id, decimal amount, decimal deposit);
}
