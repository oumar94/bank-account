using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyBankAccount.Core.Interfaces;
using MyBankAccount.Core.Models;
using MyBankAccount.WebApiAdapter.Controllers;
using MyBankAccount.WebApiAdapter.Dtos;
using System.Net;

namespace MyBankAccount.WebApiAdapter.Tests
{
    public class BankAccountControllerTest
    {
        private Mock<IBankAccountService> _bankAccountService;
        private Mock<ILogger<BankAccountsController>> _logger;
        public BankAccountControllerTest()
        {
            _bankAccountService = new Mock<IBankAccountService>();
            _logger = new Mock<ILogger<BankAccountsController>>();
        }

        [Test]
        public async Task DepositAsyncTest()
        {
            //Arrange
            int bankAccountId = 2;
            int amount = 300;
            var request = CreateRequest(bankAccountId, amount);
            var bankAccountController = new BankAccountsController(_logger.Object, _bankAccountService.Object);
            _bankAccountService.Setup(c => c.DepositMoneyAsync(amount, bankAccountId)).ReturnsAsync(true);

            //Act
            var result = await bankAccountController.DepositAsync(request);

            //Assert
            var resultObject = result as OkObjectResult;
            AssertResult(resultObject);
            _bankAccountService.Verify(c => c.DepositMoneyAsync(amount, bankAccountId), Times.Once);
        }

        [Test]
        public async Task WithdrawAsyncTest()
        {
            //Arrange
            int bankAccountId = 2;
            int amount = 300;
            var request = CreateRequest(bankAccountId, amount);
            var bankAccountController = new BankAccountsController(_logger.Object, _bankAccountService.Object);
            _bankAccountService.Setup(c => c.WithDrawMoneyAsync(amount, bankAccountId)).ReturnsAsync(true);

            //Act
            var result = await bankAccountController.WithdrawAsync(request);

            //Assert
            var resultObject = result as OkObjectResult;
            AssertResult(resultObject);
            _bankAccountService.Verify(c => c.WithDrawMoneyAsync(amount, bankAccountId), Times.Once);
        }

        [Test]
        public async Task GetCurrentBalanceTest()
        {
            //Arrange
            int bankAccountId = 2;
            int initialBalance = 150;
            var bankAccount = CreateBankAccount(bankAccountId, initialBalance);
            var bankAccountController = new BankAccountsController(_logger.Object, _bankAccountService.Object);
            _bankAccountService.Setup(c => c.GetCurrentBalanceAsync(bankAccountId)).ReturnsAsync(bankAccount);

            //Act
            var result = await bankAccountController.GetByIdAsync(bankAccountId);
            var resultObject = result as OkObjectResult;
            var actualResult = resultObject?.Value as BankAccount;

            //Assert
            Assert.NotNull(resultObject);
            Assert.NotNull(actualResult);
            Assert.That(resultObject.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(actualResult.Balance, Is.EqualTo(150));
            _bankAccountService.Verify(c => c.GetCurrentBalanceAsync(bankAccountId), Times.Once);
        }

        #region
        private OperationRequest CreateRequest(int bankAccountId, int amount)
        {
            return new OperationRequest
            {
                Amount = amount,
                BankAccountId = bankAccountId,
            };
        }
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
        private void AssertResult(OkObjectResult resultObject)
        {
            Assert.That(resultObject, Is.Not.Null);
            Assert.That(resultObject.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(resultObject.Value, Is.EqualTo("success"));
        }
        #endregion
    }
}