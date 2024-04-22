using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using MyBankAccount.Core.Models;

namespace MyBankAccount.SQLiteAdapter.Tests
{
    [TestFixture]
    public class BankAccountSQLiteRepositoryTest
    {
        private Mock<BankAccountDBContext> _dbContext;
        public BankAccountSQLiteRepositoryTest()
        {
            var config = new Mock<IConfiguration>();
            _dbContext = new Mock<BankAccountDBContext>(config.Object);
        }

        [Test]
        public async Task DepositMoneyAsyncTest()
        {
            //Arrange
            int newBalance = 1200;
            int bankAccountId = 3;
            var repository = new BankAccountSQLiteRepository(_dbContext.Object);
            _dbContext.Setup(c => c.Transactions.Add(It.IsAny<Transaction>())).Verifiable();
            _dbContext.Setup(c => c.BankAccounts.Update(It.IsAny<BankAccount>())).Verifiable();
            _dbContext.Setup(c => c.BankAccounts).ReturnsDbSet(GetBanKAccounts());
            _dbContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            //Act
            var result = await repository.DepositMoneyAsync(newBalance, bankAccountId);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task WithdrawMoneyAsyncTest()
        {
            //Arrange
            int newBalance = 400;
            int bankAccountId = 4;
            var repository = new BankAccountSQLiteRepository(_dbContext.Object);
            _dbContext.Setup(c => c.Transactions.Add(It.IsAny<Transaction>())).Verifiable();
            _dbContext.Setup(c => c.BankAccounts.Update(It.IsAny<BankAccount>())).Verifiable();
            _dbContext.Setup(c => c.BankAccounts).ReturnsDbSet(GetBanKAccounts());
            _dbContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            //Act
            var result = await repository.WithdrawMoneyAsync(newBalance, bankAccountId);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task GetPreviousTransactionsAsyncTest()
        {
            //Arrange
            int bankAccountId = 5;
            var repository = new BankAccountSQLiteRepository(_dbContext.Object);
            _dbContext.Setup(c => c.Transactions).ReturnsDbSet(GetTransactions());

            //Act
            var result = await repository.GetPreviousTransactionsAsync(bankAccountId);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        #region private methods
        private IEnumerable<Transaction> GetTransactions()
        {
            return new List<Transaction>
            {
                new Transaction {BankAccountId=5, OperationDate=new DateTime(2022,06,08),OperationType=Domain.Enums.OperationType.Deposit.ToString(),Amount=200},
                new Transaction {BankAccountId=5, OperationDate=new DateTime(2023,06,28),OperationType=Domain.Enums.OperationType.Withdraw.ToString(),Amount=50},
                new Transaction {BankAccountId=7, OperationDate=new DateTime(2021,03,08),OperationType=Domain.Enums.OperationType.Deposit.ToString(),Amount=700},
                new Transaction {BankAccountId=5, OperationDate=new DateTime(2022,06,10),OperationType=Domain.Enums.OperationType.Withdraw.ToString(),Amount=100}
            };
        }

        private IEnumerable<BankAccount> GetBanKAccounts()
        {
            return new List<BankAccount>
           {
               new BankAccount{Id=3, Balance=150, LastOperationDate=new DateTime(2022,11,21)},
               new BankAccount{Id=4, Balance=120, LastOperationDate=new DateTime(2023,10,04)},
           };
        }
        #endregion
    }
}