using Moq;
using MyBankAccount.PersistanceAdapter.Repositories;

namespace MyBankAccount.PersistanceAdapter.Tests
{
    [TestFixture]
    public class BankAccountRepositoryTest
    {
        private Mock<ICsvFileWriter> _writer;
        public BankAccountRepositoryTest()
        {
            _writer = new Mock<ICsvFileWriter>();
        }

        [Test]
        public async Task DepositMoneyAsyncTest()
        {
            //Arrange
            int newBalance = 1650;
            int bankAccountId = 3;
            var repository = new BankAccountRepository(_writer.Object);
            _writer.Setup(c => c.UpdateCsvFileBankAccountAsync(It.IsAny<string>(), newBalance, bankAccountId, It.IsAny<string>())).ReturnsAsync(true);

            //Act
            var result = await repository.DepositMoneyAsync(newBalance, bankAccountId);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task WithdrawMoneyAsyncTest()
        {
            //Arrange
            int newBalance = 500;
            int bankAccountId = 4;
            var repository = new BankAccountRepository(_writer.Object);
            _writer.Setup(c => c.UpdateCsvFileBankAccountAsync(It.IsAny<string>(), newBalance, bankAccountId, It.IsAny<string>())).ReturnsAsync(true);

            //Act
            var result = await repository.WithdrawMoneyAsync(newBalance, bankAccountId);

            //Assert
            Assert.That(result, Is.True);
        }
    }
}