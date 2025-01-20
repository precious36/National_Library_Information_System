using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaLib_Staff_Management_Lib.Common;
using NaLib_Staff_Management_Lib.Data;
using NaLib_Staff_Management_Lib.Dtos;
using NaLib_Staff_Management_service.Controllers.Extensions;
using NaLib_Staff_Management_service.Utils;

namespace NaLib_Staff_Management_service.Controllers
{
    /// <summary>
    /// User management controller
    /// </summary>
    [Route(KnownUrls.User)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NaLib_context _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration instance.</param>
        /// <param name="context">Database context instance.</param>
        public UserController(IConfiguration configuration, NaLib_context context)
        {
            _configuration = configuration;
            _context = context;
        }

        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        [HttpPost(KnownUrls.CreateUser)]
      
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var hashedPassword = NaLibHelpers.HashPassword(userRequest.Password);
                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    FirstName = userRequest.FirstName,
                    LastName = userRequest.LastName,
                    Email = userRequest.Email,
                    Password = hashedPassword,
                    PasswordLastUpdated = DateTime.UtcNow,
                    DepartmentId = userRequest.DepartmentId,
                    RoleId = userRequest.RoleId,
                    LibraryId = userRequest.LibraryId,
                    InitialPasswordExpired = false,
                    IsAccountLocked = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                var userResponse = new UserDto
                {
                    UserId = newUser.UserId,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    DepartmentId = newUser.DepartmentId,
                    RoleId = newUser.RoleId,
                    LibraryId = newUser.LibraryId,
                };

                return this.SendApiResponse(userResponse, "User created successfully.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return this.SendApiError("NotFound", "User not found.", StatusCodes.Status404NotFound);
            }

            var userResponse = new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DepartmentId = user.DepartmentId,
                RoleId = user.RoleId,
                LibraryId = user.LibraryId,
            };

            return this.SendApiResponse(userResponse, "User retrieved successfully.");
        }

        /// <summary>
        /// Updates a user's details.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return this.SendApiError("NotFound", "User not found.", StatusCodes.Status404NotFound);
            }

            user.FirstName = userRequest.FirstName;
            user.LastName = userRequest.LastName;
            user.Email = userRequest.Email;
            user.DepartmentId = userRequest.DepartmentId;
            user.RoleId = userRequest.RoleId;
            user.LibraryId = userRequest.LibraryId;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var userResponse = new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DepartmentId = user.DepartmentId,
                RoleId = user.RoleId,
                LibraryId = user.LibraryId,
            };

            return this.SendApiResponse(userResponse, "User updated successfully.");
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return this.SendApiError("Not found", "user not found", StatusCodes.Status404NotFound);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return this.SendApiResponse<object>(null, "User deleted successfully.");
        }

        /// <summary>
        /// Lists all users.
        /// </summary>
        [HttpGet("all")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<UserDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            var userResponses = users.Select(user => new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DepartmentId = user.DepartmentId,
                RoleId = user.RoleId,
                LibraryId = user.LibraryId,
            }).ToList();

            return this.SendApiResponse(userResponses, "Users retrieved successfully.");
        }

        /// <summary>
        /// Adds a new user detail to the system.
        /// </summary>
        [HttpPost(KnownUrls.AddUserDetail)]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserDetail>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUserDetail([FromBody] UserDetailRequest userDetailRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            var userDetail = new UserDetail
            {
                UserId = userDetailRequest.UserId,
                Qualification = userDetailRequest.Qualification,
                Eperience = userDetailRequest.Experience,
                Skills = userDetailRequest.Skills,
                EmploymentDate = userDetailRequest.EmploymentDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            try
            {
                await _context.UserDetails.AddAsync(userDetail);
                await _context.SaveChangesAsync();
             
                return this.SendApiResponse(userDetail, "User detail added successfully..");
            }
            catch (Exception)
            {
                return this.SendApiError("DatabaseError", "An error occurred while saving the user detail.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates an existing user detail.
        /// </summary>
        [HttpPut(KnownUrls.UpdateUserDetail)]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserDetail>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserDetail(int id, [FromBody] UserDetailRequest userDetailRequest)
        {
            var existingUserDetail = await _context.UserDetails.FindAsync(id);
            if (existingUserDetail == null)
            {
                return this.SendApiError("NotFound", "UserDeatails not found", StatusCodes.Status404NotFound);
            }

            existingUserDetail.Qualification = userDetailRequest.Qualification;
            existingUserDetail.Eperience = userDetailRequest.Experience;
            existingUserDetail.Skills = userDetailRequest.Skills;
            existingUserDetail.EmploymentDate = userDetailRequest.EmploymentDate;
            existingUserDetail.UpdatedAt = DateTime.UtcNow;

            try
            {
                _context.UserDetails.Update(existingUserDetail);
                await _context.SaveChangesAsync();
                return this.SendApiResponse(existingUserDetail, "User detail updated successfully.");
            }
            catch (Exception)
            {
                return this.SendApiError("DatabaseError", "An error occurred while updating the user detail.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets user detail by ID.
        /// </summary>
        [HttpGet(KnownUrls.GetUserDetail)]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserDetail>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserDetail(int id)
        {
            var userDetail = await _context.UserDetails.FindAsync(id);
            if (userDetail == null)
            {
                return this.SendApiResponse<object>(null, "User deatals not found.");
            }

            return this.SendApiResponse(userDetail, "User detail retrieved successfully.");
        }

    }
}
