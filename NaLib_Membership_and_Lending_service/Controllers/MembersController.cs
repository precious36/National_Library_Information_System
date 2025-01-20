
using NaLib_Membership_and_Lending_service.Controllers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NaLib_Membership_and_Lending_Lib.Common;
using NaLib_Membership_and_Lending_Lib.Data;
using NaLib_Membership_and_Lending_Lib.Dto;

namespace NaLib_Membership_and_Lending_service.Controllers
{

    /// <summary>
    /// for members
    /// </summary>
    [Route(KnownUrls.Member)]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly Member_and_LendingBDContext _context;
        private readonly IConfiguration _configuration;

        public MembersController(Member_and_LendingBDContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// creates member
        /// </summary>
        /// <param name="memberRequest"></param>
        /// <returns></returns>
        [HttpPost(KnownUrls.CreateMember)]
        [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMember([FromBody] MemberRequest memberRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newMember = new Member
                {

                    Name = memberRequest.Name,
                    Email = memberRequest.Email,
                    Phone = memberRequest.Phone,
                    PostalAddress = memberRequest.PostalAddress,
                    PhysicalAddress = memberRequest.PhysicalAddress,
                    DateEnrolled = DateOnly.FromDateTime(memberRequest.DateEnrolled),
                    Status = memberRequest.Status
                };
                await _context.Members.AddAsync(newMember);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                var memberResponse = new MemberDto
                {
                    MemberId = newMember.MemberId,
                    Name = newMember.Name,
                    Email = newMember.Email,
                    Phone = newMember.Phone,
                    PostalAddress = newMember.PostalAddress,
                    PhysicalAddress = newMember.PhysicalAddress,
                    DateEnrolled = newMember.DateEnrolled,
                    Status = newMember.Status
                };
                return this.SendApiResponse(memberResponse, "Member created successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// gets all the members
        /// </summary>
        /// <returns></returns>
        [HttpGet(KnownUrls.GetAllMembers)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MemberDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMembers()
        {
            try
            {
                var members = await _context.Members.ToListAsync();
                var memberResponses = members.Select(m => new MemberDto
                {
                    MemberId = m.MemberId,
                    Name = m.Name,
                    Email = m.Email,
                    Phone = m.Phone,
                    PostalAddress = m.PostalAddress,
                    PhysicalAddress = m.PhysicalAddress,
                    DateEnrolled = m.DateEnrolled,
                    Status = m.Status
                });
                return this.SendApiResponse(memberResponses, "Members retrieved successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// gets the member by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(KnownUrls.GetMemberById)]
        [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMemberById(int id)
        {
            try
            {
                var member = await _context.Members.FindAsync(id);
                if (member == null)
                {
                    return this.SendApiError("MemberNotFound", "Member not found.", StatusCodes.Status404NotFound);
                }
                var memberResponse = new MemberDto
                {
                    MemberId = member.MemberId,
                    Name = member.Name,
                    Email = member.Email,
                    Phone = member.Phone,
                    PostalAddress = member.PostalAddress,
                    PhysicalAddress = member.PhysicalAddress,
                    DateEnrolled = member.DateEnrolled,
                    Status = member.Status
                };
                return this.SendApiResponse(memberResponse, "Member retrieved successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// updates the member
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberRequest"></param>
        /// <returns></returns>
        [HttpPut(KnownUrls.UpdateMember)]
        [ProducesResponseType(typeof(ApiResponse<MemberDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] MemberRequest memberRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var existingMember = await _context.Members.FindAsync(id);
            if (existingMember == null)
            {
                return this.SendApiError("MemberNotFound", "Member not found.", StatusCodes.Status404NotFound);
            }

            existingMember.Name = memberRequest.Name;
            existingMember.Email = memberRequest.Email;
            existingMember.Phone = memberRequest.Phone;
            existingMember.PostalAddress = memberRequest.PostalAddress;
            existingMember.PhysicalAddress = memberRequest.PhysicalAddress;
            existingMember.DateEnrolled = DateOnly.FromDateTime(memberRequest.DateEnrolled);
            existingMember.Status = memberRequest.Status;

            await _context.SaveChangesAsync();

            var memberResponse = new MemberDto
            {
                MemberId = existingMember.MemberId,
                Name = existingMember.Name,
                Email = existingMember.Email,
                Phone = existingMember.Phone,
                PostalAddress = existingMember.PostalAddress,
                PhysicalAddress = existingMember.PhysicalAddress,
                DateEnrolled = existingMember.DateEnrolled,
                Status = existingMember.Status
            };

            return this.SendApiResponse(memberResponse, "Member updated successfully.");
        }

    }
}