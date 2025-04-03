using WalletAPI.Models;
using WalletAPI.Repositories.Interfaces;

namespace WalletAPI.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;

        public TransactionService(ITransactionRepository transactionRepository, IWalletRepository walletRepository)
        {
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            try
            {
                return await _transactionRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las transacciones", ex);
            }
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            try
            {
                return await _transactionRepository.GetByIdAsync(id)
                    ?? throw new KeyNotFoundException("No se encontró la transacción.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la transacción", ex);
            }
        }

        public async Task<bool> CreateTransactionAsync(Transaction transaction)
        {
            try
            {
                // Validar si la Wallet asociada existe
                if (!await _walletRepository.ExistsAsync(transaction.WalletId))
                    throw new KeyNotFoundException("No se encontró la billetera asociada.");

                await _transactionRepository.AddAsync(transaction);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la transacción", ex);
            }
        }
    }
}
