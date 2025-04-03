using Microsoft.EntityFrameworkCore;
using WalletAPI.Data;
using WalletAPI.Models;
using WalletAPI.Repositories.Interfaces;

namespace WalletAPI.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly WalletDbContext _context;

        public TransactionRepository(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            try
            {
                return await _context.Transactions.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las transacciones", ex);
            }
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Transactions.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la transacción", ex);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.Transactions.AnyAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar si la transacción existe", ex);
            }
        }

        public async Task AddAsync(Transaction transaction)
        {
            try
            {
                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la transacción", ex);
            }
        }
    }
}
