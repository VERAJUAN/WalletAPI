using Microsoft.AspNetCore.Mvc;
using WalletAPI.Models;
using WalletAPI.Services;

namespace WalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly WalletService _walletService;

        public WalletController(WalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wallet>>> GetWallets()
        {
            try
            {
                var wallets = await _walletService.GetAllWalletsAsync();
                return Ok(wallets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Wallet>> GetWallet(int id)
        {
            try
            {
                var wallet = await _walletService.GetWalletByIdAsync(id);
                return Ok(wallet);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] Wallet wallet)
        {
            try
            {
                var created = await _walletService.CreateWalletAsync(wallet);
                return created ? CreatedAtAction(nameof(GetWallet), new { id = wallet.Id }, wallet) :
                                 BadRequest(new { Error = "No se pudo crear la billetera." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWallet(int id, [FromBody] Wallet wallet)
        {
            try
            {
                if (id != wallet.Id) return BadRequest(new { Error = "El ID en la URL no coincide con el del cuerpo." });

                var updated = await _walletService.UpdateWalletAsync(wallet);
                return updated ? NoContent() : NotFound(new { Error = "No se pudo actualizar la billetera." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            try
            {
                var deleted = await _walletService.DeleteWalletAsync(id);
                return deleted ? NoContent() : NotFound(new { Error = "No se pudo eliminar la billetera." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
