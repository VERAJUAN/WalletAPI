using Moq;
using FluentAssertions;
using WalletAPI.Services;
using WalletAPI.Repositories.Interfaces;
using WalletAPI.Models;

namespace WalletAPI.Tests.UnitTests.Services
{
    public class TransactionServiceTests
    {
        private readonly TransactionService _transactionService;
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly Mock<IWalletRepository> _mockWalletRepository;

        public TransactionServiceTests()
        {
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockWalletRepository = new Mock<IWalletRepository>();

            _transactionService = new TransactionService(_mockTransactionRepository.Object, _mockWalletRepository.Object);
        }


        [Fact]
        public async Task CreateTransactionAsync_ShouldReturnTrue_WhenWalletExists()
        {
            // Arrange
            var transaction = new Transaction
            {
                Id = 1,
                WalletId = 1,
                Amount = 100,
                Type = new TypeTransaction { Id = 1, Name = "Crédito" }
            };

            _mockWalletRepository
                .Setup(repo => repo.ExistsAsync(transaction.WalletId))
                .ReturnsAsync(true);

            _mockTransactionRepository
                .Setup(repo => repo.AddAsync(transaction))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _transactionService.CreateTransactionAsync(transaction);

            // Assert
            result.Should().BeTrue();

            _mockWalletRepository.Verify(repo => repo.ExistsAsync(transaction.WalletId), Times.Once);
            _mockTransactionRepository.Verify(repo => repo.AddAsync(transaction), Times.Once);
        }

        [Fact]
        public async Task CreateTransactionAsync_ShouldThrowKeyNotFoundException_WhenWalletDoesNotExist()
        {
            // Arrange
            var transaction = new Transaction
            {
                Id = 2,
                WalletId = 99,
                Amount = 50,
                Type = new TypeTransaction { Id = 2, Name = "Débito" }
            };

            _mockWalletRepository
                .Setup(repo => repo.ExistsAsync(transaction.WalletId))
                .ReturnsAsync(false);

            // Act
            var act = async () => await _transactionService.CreateTransactionAsync(transaction);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("No se encontró la billetera asociada.");

            _mockWalletRepository.Verify(repo => repo.ExistsAsync(transaction.WalletId), Times.Once);
            _mockTransactionRepository.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Never);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_ShouldReturnTransaction_WhenExists()
        {
            // Arrange
            var fakeTransaction = new Transaction
            {
                Id = 3,
                WalletId = 1,
                Amount = 75,
                Type = new TypeTransaction { Id = 1, Name = "Crédito" }
            };

            _mockTransactionRepository
                .Setup(repo => repo.GetByIdAsync(3))
                .ReturnsAsync(fakeTransaction);

            // Act
            var result = await _transactionService.GetTransactionByIdAsync(3);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(3);
            result.Amount.Should().Be(75);
            result.Type.Name.Should().Be("Crédito");

            _mockTransactionRepository.Verify(repo => repo.GetByIdAsync(3), Times.Once);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_ShouldThrowKeyNotFoundException_WhenNotFound()
        {
            // Arrange
            _mockTransactionRepository
                .Setup(repo => repo.GetByIdAsync(99))
                .ReturnsAsync((Transaction?)null);

            // Act
            var act = async () => await _transactionService.GetTransactionByIdAsync(99);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("No se encontró la transacción.");

            _mockTransactionRepository.Verify(repo => repo.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task GetAllTransactionsAsync_ShouldReturnList_WhenTransactionsExist()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { Id = 1, WalletId = 1, Amount = 200, Type = new TypeTransaction { Id = 1, Name = "Crédito" } },
                new Transaction { Id = 2, WalletId = 1, Amount = 50, Type = new TypeTransaction { Id = 2, Name = "Débito" } }
            };

            _mockTransactionRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(transactions);

            // Act
            var result = await _transactionService.GetAllTransactionsAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
        }
    }
}
