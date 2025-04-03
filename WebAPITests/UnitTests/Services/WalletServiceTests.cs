using Moq;
using FluentAssertions;
using WalletAPI.Services;
using WalletAPI.Models;
using WalletAPI.Repositories.Interfaces;

namespace WalletAPI.Tests.UnitTests.Services
{
    public class WalletServiceTests
    {
        private readonly WalletService _walletService;
        private readonly Mock<IWalletRepository> _mockWalletRepository;

        public WalletServiceTests()
        {
            _mockWalletRepository = new Mock<IWalletRepository>();
            _walletService = new WalletService(_mockWalletRepository.Object);
        }

        [Fact]
        public async Task GetWalletById_ReturnsWallet_WhenExists()
        {
            // Arrange
            var fakeWallet = new Wallet { Id = 1, Balance = 500 };
            _mockWalletRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(fakeWallet);

            // Act
            var result = await _walletService.GetWalletByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Balance.Should().Be(500);
        }

        [Fact]
        public async Task GetWalletById_ReturnsNull_WhenNotFound()
        {
            // Arrange
            _mockWalletRepository.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Wallet)null);

            // Act
            var result = await _walletService.GetWalletByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }
    }
}
