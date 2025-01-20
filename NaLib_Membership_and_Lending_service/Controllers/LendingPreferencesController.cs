
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
    /// for preferences
    /// </summary>
    [Route(KnownUrls.LendingPreferenceBaseUrl)]
    [ApiController]
    public class LendingPreferencesController : ControllerBase
    {
        private readonly Member_and_LendingBDContext _context;
        private readonly IConfiguration _configuration;

        public LendingPreferencesController(Member_and_LendingBDContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// gets all the preferences
        /// </summary>
        /// <returns></returns>
        [HttpGet(KnownUrls.GetAllLendingPreferences)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LendingPreferenceDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllLendingPreferences()
        {
            try
            {
                var lendingPreferences = await _context.LendingPreferences
                    .Include(lp => lp.Member)
                    .ToListAsync();
                var lendingPreferenceResponses = lendingPreferences.Select(lp => new LendingPreferenceDto
                {
                    PreferenceId = lp.PreferenceId,
                    MemberId = lp.MemberId,
                    ResourceType = lp.ResourceType,
                    Topic = lp.Topic
                });
                return this.SendApiResponse(lendingPreferenceResponses, "Lending preferences retrieved successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// gets the prefrence by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(KnownUrls.GetLendingPreferenceById)]
        [ProducesResponseType(typeof(ApiResponse<LendingPreferenceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLendingPreferenceById(int id)
        {
            try
            {
                var lendingPreference = await _context.LendingPreferences
                    .Include(lp => lp.Member)
                    .FirstOrDefaultAsync(lp => lp.PreferenceId == id);
                if (lendingPreference == null)
                {
                    return this.SendApiError("LendingPreferenceNotFound", "Lending preference not found.", StatusCodes.Status404NotFound);
                }
                var lendingPreferenceResponse = new LendingPreferenceDto
                {
                    PreferenceId = lendingPreference.PreferenceId,
                    MemberId = lendingPreference.MemberId,
                    ResourceType = lendingPreference.ResourceType,
                    Topic = lendingPreference.Topic
                };
                return this.SendApiResponse(lendingPreferenceResponse, "Lending preference retrieved successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// creates preferences
        /// </summary>
        /// <param name="lendingPreferenceRequest"></param>
        /// <returns></returns>
        [HttpPost(KnownUrls.CreateLendingPreference)]
        [ProducesResponseType(typeof(ApiResponse<LendingPreferenceDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLendingPreference([FromBody] LendingPreferenceRequest lendingPreferenceRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var newLendingPreference = new LendingPreference
            {
                MemberId = lendingPreferenceRequest.MemberId,
                ResourceType = lendingPreferenceRequest.ResourceType,
                Topic = lendingPreferenceRequest.Topic
            };
            await _context.LendingPreferences.AddAsync(newLendingPreference);
            await _context.SaveChangesAsync();

            var lendingPreferenceResponse = new LendingPreferenceDto
            {
                PreferenceId = newLendingPreference.PreferenceId,
                MemberId = newLendingPreference.MemberId,
                ResourceType = newLendingPreference.ResourceType,
                Topic = newLendingPreference.Topic
            };
            return this.SendApiResponse(lendingPreferenceResponse, "Lending preference created successfully.");
        }


        /// <summary>
        /// updates the preference
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lendingPreferenceRequest"></param>
        /// <returns></returns>
        [HttpPut(KnownUrls.UpdateLendingPreference)]
        [ProducesResponseType(typeof(ApiResponse<LendingPreferenceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLendingPreference(int id, [FromBody] LendingPreferenceRequest lendingPreferenceRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var existingLendingPreference = await _context.LendingPreferences.FindAsync(id);
            if (existingLendingPreference == null)
            {
                return this.SendApiError("LendingPreferenceNotFound", "Lending preference not found.", StatusCodes.Status404NotFound);
            }

            existingLendingPreference.ResourceType = lendingPreferenceRequest.ResourceType;
            existingLendingPreference.Topic = lendingPreferenceRequest.Topic;

            await _context.SaveChangesAsync();

            var lendingPreferenceResponse = new LendingPreferenceDto
            {
                PreferenceId = existingLendingPreference.PreferenceId,
                MemberId = existingLendingPreference.MemberId,
                ResourceType = existingLendingPreference.ResourceType,
                Topic = existingLendingPreference.Topic
            };
            return this.SendApiResponse(lendingPreferenceResponse, "Lending preference updated successfully.");
        }
    }
}