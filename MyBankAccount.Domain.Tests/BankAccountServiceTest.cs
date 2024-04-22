using Moq;
using MyBankAccount.Core.Interfaces;
using MyBankAccount.Core.Models;
using MyBankAccount.Domain.Services;

namespace MyBankAccount.Domain.Tests
{
    [TestFixture]
    public class BankAccountServiceTest
    {
        private Mock<IBankAccountRepository> _repository;
        public BankAccountServiceTest()
        {
            _repository = new Mock<IBankAccountRepository>();
        }

        [Test]
        public async Task DepositMoneyTest()
        {
            //Arrange
            int bankAccountId = 2;
            int initialBalance = 100;
            int depositAmount = 500;
            var bankAccount = CreateBankAccount(bankAccountId, initialBalance);
            var updatedBalance = depositAmount + bankAccount.Balance;
            var bankAccountService = new BankAccountService(_repository.Object);
            _repository.Setup(c => c.GetCurrentBalanceAsync(bankAccountId)).ReturnsAsync(bankAccount);
            _repository.Setup(c => c.DepositMoneyAsync(updatedBalance, bankAccountId)).ReturnsAsync(true);

            //Act
            var result = await bankAccountService.DepositMoneyAsync(depositAmount, bankAccountId);

            //Assert
            Assert.That(result, Is.True);
            _repository.Verify(c => c.GetCurrentBalanceAsync(bankAccountId), Times.Once);
            _repository.Verify(c => c.DepositMoneyAsync(updatedBalance, bankAccountId), Times.Once);
        }

        [Test]
        public async Task WithdrawMoneyTest()
        {
            //Arrange
            int bankAccountId = 3;
            int initialBalance = 600;
            int withdrawAmount = 200;
            var bankAccount = CreateBankAccount(bankAccountId, initialBalance);
            var updatedBalance = bankAccount.Balance - withdrawAmount;
            var bankAccountService = new BankAccountService(_repository.Object);
            _repository.Setup(c => c.GetCurrentBalanceAsync(bankAccountId)).ReturnsAsync(bankAccount);
            _repository.Setup(c => c.WithdrawMoneyAsync(updatedBalance, bankAccountId)).ReturnsAsync(true);

            //Act
            var result = await bankAccountService.WithDrawMoneyAsync(withdrawAmount, bankAccountId);

            //Assert
            Assert.That(result, Is.True);
            _repository.Verify(c => c.GetCurrentBalanceAsync(bankAccountId), Times.Once);
            _repository.Verify(c => c.WithdrawMoneyAsync(updatedBalance, bankAccountId), Times.Once);
        }

        [Test]
        public async Task GetCurrentBalanceTest()
        {
            //Arrange
            int bankAccountId = 4;
            int initialBalance = 600;
            var bankAccount = CreateBankAccount(bankAccountId, initialBalance);
            var bankAccountService = new BankAccountService(_repository.Object);
            _repository.Setup(c => c.GetCurrentBalanceAsync(bankAccountId)).ReturnsAsync(bankAccount);

            //Act
            var result = await bankAccountService.GetCurrentBalanceAsync(bankAccountId);

            //Assert
            Assert.That(result.Balance, Is.EqualTo(600));
            _repository.Verify(c => c.GetCurrentBalanceAsync(bankAccountId), Times.Once);
        }

        [Test]
        public async Task GetPreviousTransactionsTest()
        {
            //Arrange
            int bankAccountId = 5;
            int initialBalance = 600;
            var bankAccount = CreateBankAccount(bankAccountId, initialBalance);
            var transactions = GetTransactions().Where(t => t.BankAccountId == bankAccountId);

            var bankAccountService = new BankAccountService(_repository.Object);
            _repository.Setup(c => c.GetPreviousTransactionsAsync(bankAccountId)).ReturnsAsync(transactions);

            //Act
            var result = await bankAccountService.GetPreviousTransactionsAsync(bankAccountId);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(3));
            _repository.Verify(c => c.GetPreviousTransactionsAsync(bankAccountId), Times.Once);
        }

        #region private methods
        private BankAccount CreateBankAccount(int bankAccountId, int initialBalance)
        {
            return new BankAccount
            {
                Id = bankAccountId,
                CreationDate = DateTime.Now,
                LastOperationDate = DateTime.Now,
                Balance = initialBalance
            };
        }
        private IEnumerable<Transaction> GetTransactions()
        {
            return new List<Transaction>
            {
                new Transaction {BankAccountId=5, OperationDate=new DateTime(2022,06,08),OperationType=Enums.OperationType.Deposit.ToString(),Amount=200},
                new Transaction {BankAccountId=5, OperationDate=new DateTime(2023,06,28),OperationType=Enums.OperationType.Withdraw.ToString(),Amount=50},
                new Transaction {BankAccountId=7, OperationDate=new DateTime(2021,03,08),OperationType=Enums.OperationType.Deposit.ToString(),Amount=700},
                new Transaction {BankAccountId=5, OperationDate=new DateTime(2022,06,10),OperationType=Enums.OperationType.Withdraw.ToString(),Amount=100}
            };
        }
        #endregion
    }
}