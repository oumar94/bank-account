using Microsoft.AspNetCore.Mvc;
using MyBankAccount.Core.Interfaces;
using MyBankAccount.Core.Models;
using MyBankAccount.Domain;
using MyBankAccount.Domain.Exceptions;
using MyBankAccount.WebApiAdapter.Dtos;
using System.Net;

namespace MyBankAccount.WebApiAdapter.Controllers
{
    [ApiController]
    [Route("")]
    public class BankAccountsController : ControllerBase
    {
        private readonly ILogger<BankAccountsController> _logger;
        private readonly IBankAccountService _bankAccountService;

        public BankAccountsController(ILogger<BankAccountsController> logger, IBankAccountService bankAccountService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bankAccountService = bankAccountService ?? throw new ArgumentNullException(nameof(bankAccountService));
        }

        /// <summary>
        /// Permet de faire un depôt d'argent sur un compte bancaire
        /// </summary>
        /// <param name="request">Objet de requete contenant l'id du compte bancaire ainsi que le montant à deposer</param>
        /// <response code="200">depot effectué avec succès sur le compte bancaire specifié</response>
        /// <response code="404">compte bancaire introuvable pour l'id specifié</response>
        /// <response code="500">Erreur coté serveur</response>
        /// <returns></returns>
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(BankAccountResponse), 404)]
        [ProducesResponseType(typeof(void), 500)]
        [HttpPost("bank-accounts/deposits")]
        public async Task<IActionResult> DepositAsync([FromBody] OperationRequest request)
        {
            try
            {
                await _bankAccountService.DepositMoneyAsync(request.Amount, request.BankAccountId);
                return Ok("success");
            }
            catch (BankAccountNotFoundException ex)
            {
                return NotFound(new BankAccountResponse { StatusCode = (int)HttpStatusCode.NotFound, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Permet de faire un retrait d'argent sur un compte bancaire 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="request">Objet de requete contenant l'id du compte bancaire ainsi que le montant à deposer</param>
        /// <response code="200">retrait effectué avec succès sur le compte bancaire specifié</response>
        /// <response code="400">Montant de retrait superieur au solde disponible</response>
        /// <response code="404">compte bancaire introuvable pour l'id specifié</response>
        /// <response code="500">Erreur coté serveur</response>
        /// <returns></returns>
        [HttpPost("bank-accounts/withdraws")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(BankAccountResponse), 400)]
        [ProducesResponseType(typeof(BankAccountResponse), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> WithdrawAsync([FromBody] OperationRequest request)
        {
            try
            {
                await _bankAccountService.WithDrawMoneyAsync(request.Amount, request.BankAccountId);
                return Ok("success");
            }
            catch (BankAccountNotFoundException ex)
            {
                return NotFound(new BankAccountResponse { StatusCode = (int)HttpStatusCode.NotFound, Message = ex.Message });
            }
            catch (InsufficientBalanceException ex)
            {
                return BadRequest(new BankAccountResponse { StatusCode = (int)HttpStatusCode.BadRequest, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retourne le details liés à un compte bancaire spécifique
        /// </summary>
        /// <param name="id">id du compte bancaire</param>
        /// <response code="200">informations liés au compte bancaire specifié</response>
        /// <response code="404">compte bancaire introuvable pour l'id specifié</response>
        /// <response code="500">Erreur coté serveur</response>
        /// <returns></returns>
        [HttpGet("bank-accounts/{id}")]
        [ProducesResponseType(typeof(BankAccount), 200)]
        [ProducesResponseType(typeof(BankAccountResponse), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _bankAccountService.GetCurrentBalanceAsync(id);
                if (result == null) return NotFound(new BankAccountResponse { StatusCode = (int)HttpStatusCode.NotFound, Message = Constants.BankAccountNotFoundMessage });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// retourne l'historique des transactions qui ont eu lieu sur le compte bancaire spécifié
        /// </summary>
        /// <param name="id">id du compte bancaire</param>
        /// <response code="200">l'historique des transactions qui ont eu lieu sur le compte bancaire spécifié</response>
        /// <response code="500">Erreur coté serveur</response>
        /// <returns></returns>
        [HttpGet("bank-accounts/{id}/transactions")]
        [ProducesResponseType(typeof(IEnumerable<Transaction>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var result = await _bankAccountService.GetPreviousTransactionsAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}