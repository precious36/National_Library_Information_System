
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
    /// for damages and losses
    /// </summary>
    [Route(KnownUrls.DamageAndLossBaseUrl)]
    [ApiController]
    public class DamageAndLossController : ControllerBase
    {
        private readonly Member_and_LendingBDContext _context;
        private readonly IConfiguration _configuration;

        public DamageAndLossController(Member_and_LendingBDContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// get all damages
        /// </summary>
        /// <returns></returns>
        [HttpGet(KnownUrls.GetAllDamageAndLoss)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<DamageAndLossDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDamageAndLoss()
        {
            try
            {
                var damageAndLossRecords = await _context.DamageAndLosses
                    .Include(dal => dal.Transaction)
                    .ToListAsync();
                var damageAndLossResponses = damageAndLossRecords.Select(dal => new DamageAndLossDto
                {
                    DamageId = dal.DamageId,
                    TransactionId = dal.TransactionId,
                    DamageType = dal.DamageType,
                    ReplacementCost = (decimal)dal.ReplacementCost
                });
                return this.SendApiResponse(damageAndLossResponses, "Damage and loss records retrieved successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// get damage and loss by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet(KnownUrls.GetDamageAndLossById)]
        [ProducesResponseType(typeof(ApiResponse<DamageAndLossDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDamageAndLossById(int id)
        {
            try
            {
                var damageAndLossRecord = await _context.DamageAndLosses
                    .Include(dal => dal.Transaction)
                    .FirstOrDefaultAsync(dal => dal.DamageId == id);
                if (damageAndLossRecord == null)
                {
                    return this.SendApiError("DamageAndLossNotFound", "Damage and loss record not found.", StatusCodes.Status404NotFound);
                }
                var damageAndLossResponse = new DamageAndLossDto
                {
                    DamageId = damageAndLossRecord.DamageId,
                    TransactionId = damageAndLossRecord.TransactionId,
                    DamageType = damageAndLossRecord.DamageType,
                    ReplacementCost = (decimal)damageAndLossRecord.ReplacementCost
                };
                return this.SendApiResponse(damageAndLossResponse, "Damage and loss record retrieved successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// records damages and losses
        /// </summary>
        /// <param name="damageAndLossRequest"></param>
        /// <returns></returns>
        [HttpPost(KnownUrls.CreateDamageAndLoss)]
        [ProducesResponseType(typeof(ApiResponse<DamageAndLossDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDamageAndLoss([FromBody] DamageAndLossRequest damageAndLossRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var newDamageAndLoss = new DamageAndLoss
            {
                TransactionId = damageAndLossRequest.TransactionId,
                DamageType = damageAndLossRequest.DamageType,
                ReplacementCost = damageAndLossRequest.ReplacementCost
            };
            await _context.DamageAndLosses.AddAsync(newDamageAndLoss);
            await _context.SaveChangesAsync();

            var damageAndLossResponse = new DamageAndLossDto
            {
                DamageId = newDamageAndLoss.DamageId,
                TransactionId = newDamageAndLoss.TransactionId,
                DamageType = newDamageAndLoss.DamageType,
                ReplacementCost = (decimal)newDamageAndLoss.ReplacementCost
            };
            return this.SendApiResponse(damageAndLossResponse, "Damage and loss record created successfully.");
        }
       
        /// <summary>
        /// updates the damage and loss
        /// </summary>
        /// <param name="id"></param>
        /// <param name="damageAndLossRequest"></param>
        /// <returns></returns>
        [HttpPut(KnownUrls.UpdateDamageAndLoss)]
        [ProducesResponseType(typeof(ApiResponse<DamageAndLossDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDamageAndLoss(int id, [FromBody] DamageAndLossRequest damageAndLossRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var existingDamageAndLoss = await _context.DamageAndLosses.FindAsync(id);
            if (existingDamageAndLoss == null)
            {
                return this.SendApiError("DamageAndLossNotFound", "Damage and loss record not found.", StatusCodes.Status404NotFound);
            }

            existingDamageAndLoss.TransactionId = damageAndLossRequest.TransactionId;
            existingDamageAndLoss.DamageType = damageAndLossRequest.DamageType;
            existingDamageAndLoss.ReplacementCost = damageAndLossRequest.ReplacementCost;

            await _context.SaveChangesAsync();

            var damageAndLossResponse = new DamageAndLossDto
            {
                DamageId = existingDamageAndLoss.DamageId,
                TransactionId = existingDamageAndLoss.TransactionId,
                DamageType = existingDamageAndLoss.DamageType,
                ReplacementCost = (decimal)existingDamageAndLoss.ReplacementCost
            };
            return this.SendApiResponse(damageAndLossResponse, "Damage and loss record updated successfully.");
        }
       
    }
}