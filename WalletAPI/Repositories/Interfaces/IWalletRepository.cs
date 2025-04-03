using WalletAPI.Models;

namespace WalletAPI.Repositories.Interfaces
{
    public interface IWalletRepository
    {
        Task<IEnumerable<Wallet>> GetAllAsync();
        Task<Wallet?> GetByIdAsync(int id);
        Task AddAsync(Wallet wallet);
        Task UpdateAsync(Wallet wallet);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
