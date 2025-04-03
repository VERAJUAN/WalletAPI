using WalletAPI.Models;
using WalletAPI.Repositories.Interfaces;

namespace WalletAPI.Services
{
    public class WalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<IEnumerable<Wallet>> GetAllWalletsAsync()
        {
            try
            {
                return await _walletRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las billeteras", ex);
            }
        }

        public async Task<Wallet?> GetWalletByIdAsync(int id)
        {
            try
            {
                return await _walletRepository.GetByIdAsync(id) ??
                       throw new KeyNotFoundException("No se encontró la billetera.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la billetera", ex);
            }
        }

        public async Task<bool> CreateWalletAsync(Wallet wallet)
        {
            try
            {
                if (await _walletRepository.ExistsAsync(wallet.Id))
                    throw new InvalidOperationException("La billetera ya existe.");

                await _walletRepository.AddAsync(wallet);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la billetera", ex);
            }
        }

        public async Task<bool> UpdateWalletAsync(Wallet wallet)
        {
            try
            {
                if (!await _walletRepository.ExistsAsync(wallet.Id))
                    throw new KeyNotFoundException("No se encontró la billetera para actualizar.");

                await _walletRepository.UpdateAsync(wallet);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la billetera", ex);
            }
        }

        public async Task<bool> DeleteWalletAsync(int id)
        {
            try
            {
                if (!await _walletRepository.ExistsAsync(id))
                    throw new KeyNotFoundException("No se encontró la billetera para eliminar.");

                await _walletRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la billetera", ex);
            }
        }
    }
}
