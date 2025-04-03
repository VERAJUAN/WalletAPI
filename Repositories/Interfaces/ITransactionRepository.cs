using WalletAPI.Models;

namespace WalletAPI.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task AddAsync(Transaction transaction);
    }
}
