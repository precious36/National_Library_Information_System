
using NaLib_Membership_and_Lending_service.Controllers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaLib_Membership_and_Lending_Lib.Common;
using NaLib_Membership_and_Lending_Lib.Data;
using NaLib_Membership_and_Lending_Lib.Dto;

namespace NaLib_Membership_and_Lending_service.Controllers
{
    /// <summary>
    /// for lending transactions
    /// </summary>
    [Route(KnownUrls.LendingTransactionBaseUrl)]
    [ApiController]
    public class LendingTransactionsController : ControllerBase
    {
        private readonly Member_and_LendingBDContext _context;
        private readonly IConfiguration _configuration;

        public LendingTransactionsController(Member_and_LendingBDContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        /// <summary>
        /// gets all lending transactions
        /// </summary>
        /// <returns></returns>
        [HttpGet(KnownUrls.GetAllLendingTransactions)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LendingTransactionDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllLendingTransactions()
        {
            try
            {
                var lendingTransactions = await _context.LendingTransactions
                    .Include(lt => lt.Member)
                    .Include(lt => lt.Resource)
                    .ToListAsync();
                var lendingTransactionResponses = lendingTransactions.Select(lt => new LendingTransactionDto
                {
                    TransactionId = lt.TransactionId,
                    MemberId = lt.MemberId,
                    ResourceId = lt.ResourceId,
                    CheckoutDate = lt.CheckoutDate,
                    DueDate = lt.DueDate,
                    ReturnDate = lt.ReturnDate,
                    Status = lt.Status
                });
                return this.SendApiResponse(lendingTransactionResponses, "Lending transactions retrieved successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// get the the transaction by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(KnownUrls.GetLendingTransactionById)]
        [ProducesResponseType(typeof(ApiResponse<LendingTransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLendingTransactionById(int id)
        {
            try
            {
                var lendingTransaction = await _context.LendingTransactions
                    .Include(lt => lt.Member)
                    .Include(lt => lt.Resource)
                    .FirstOrDefaultAsync(lt => lt.TransactionId == id);
                if (lendingTransaction == null)
                {
                    return this.SendApiError("LendingTransactionNotFound", "Lending transaction not found.", StatusCodes.Status404NotFound);
                }
                var lendingTransactionResponse = new LendingTransactionDto
                {
                    TransactionId = lendingTransaction.TransactionId,
                    MemberId = lendingTransaction.MemberId,
                    ResourceId = lendingTransaction.ResourceId,
                    CheckoutDate = lendingTransaction.CheckoutDate,
                    DueDate = lendingTransaction.DueDate,
                    ReturnDate = lendingTransaction.ReturnDate,
                    Status = lendingTransaction.Status
                };
                return this.SendApiResponse(lendingTransactionResponse, "Lending transaction retrieved successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }



        /// <summary>
        /// records the transaction
        /// </summary>
        /// <param name="lendingTransactionRequest"></param>
        /// <returns></returns>
        [HttpPost(KnownUrls.CreateLendingTransaction)]
        [ProducesResponseType(typeof(ApiResponse<LendingTransactionDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLendingTransaction([FromBody] LendingTransactionRequest lendingTransactionRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var newLendingTransaction = new LendingTransaction
            {
                MemberId = lendingTransactionRequest.MemberId,
                ResourceId = lendingTransactionRequest.ResourceId,
                CheckoutDate = lendingTransactionRequest.CheckoutDate,
                DueDate = lendingTransactionRequest.DueDate,
                ReturnDate = lendingTransactionRequest.ReturnDate,
                Status = lendingTransactionRequest.Status
            };
            await _context.LendingTransactions.AddAsync(newLendingTransaction);
            await _context.SaveChangesAsync();

            var lendingTransactionResponse = new LendingTransactionDto
            {
                TransactionId = newLendingTransaction.TransactionId,
                MemberId = newLendingTransaction.MemberId,
                ResourceId = newLendingTransaction.ResourceId,
                CheckoutDate = newLendingTransaction.CheckoutDate,
                DueDate = newLendingTransaction.DueDate,
                ReturnDate = newLendingTransaction.ReturnDate,
                Status = newLendingTransaction.Status
            };
            return this.SendApiResponse(lendingTransactionResponse, "Lending transaction created successfully.");
        }


        /// <summary>
        /// updates the transaction
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lendingTransactionRequest"></param>
        /// <returns></returns>
        [HttpPut(KnownUrls.UpdateLendingTransaction)]
        [ProducesResponseType(typeof(ApiResponse<LendingTransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLendingTransaction(int id, [FromBody] LendingTransactionRequest lendingTransactionRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var existingLendingTransaction = await _context.LendingTransactions.FindAsync(id);
            if (existingLendingTransaction == null)
            {
                return this.SendApiError("LendingTransactionNotFound", "Lending transaction not found.", StatusCodes.Status404NotFound);
            }

            existingLendingTransaction.MemberId = lendingTransactionRequest.MemberId;
            existingLendingTransaction.ResourceId = lendingTransactionRequest.ResourceId;
            existingLendingTransaction.CheckoutDate = lendingTransactionRequest.CheckoutDate;
            existingLendingTransaction.DueDate = lendingTransactionRequest.DueDate;
            existingLendingTransaction.ReturnDate = lendingTransactionRequest.ReturnDate;
            existingLendingTransaction.Status = lendingTransactionRequest.Status;

            await _context.SaveChangesAsync();

            var lendingTransactionResponse = new LendingTransactionDto
            {
                TransactionId = existingLendingTransaction.TransactionId,
                MemberId = existingLendingTransaction.MemberId,
                ResourceId = existingLendingTransaction.ResourceId,
                CheckoutDate = existingLendingTransaction.CheckoutDate,
                DueDate = existingLendingTransaction.DueDate,
                ReturnDate = existingLendingTransaction.ReturnDate,
                Status = existingLendingTransaction.Status
            };
            return this.SendApiResponse(lendingTransactionResponse, "Lending transaction updated successfully.");
        }

    }
}