using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaLib_Staff_Management_Lib.Common;
using NaLib_Staff_Management_Lib.Data;
using NaLib_Staff_Management_Lib.Dtos;
using NaLib_Staff_Management_service.Controllers.Extensions;
using System.Data;
using System.Security;

namespace NaLib_Staff_Management_service.Controllers
{
    [Route(KnownUrls.Role)]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly NaLib_context _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration instance.</param>
        /// <param name="context">Database context instance.</param>
        public RoleController(IConfiguration configuration, NaLib_context context)
        {
            _configuration = configuration;
            _context = context;
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="userRoleRequest">Details of the new role.</param>
        /// <returns>
        /// The created role details.
        /// </returns>
        /// <response code="201">Role created successfully.</response>
        /// <response code="400">Validation error.</response>
        /// <response code="500">An error occurred while creating the role.</response>
        [HttpPost(KnownUrls.UserRole)]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserRoleDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRole([FromBody] UserRoleRequest userRoleRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var newRole = new Role
                {
                    RoleId = Guid.NewGuid(),
                    Name = userRoleRequest.RoleName,
                    Description = userRoleRequest.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _context.Roles.AddAsync(newRole);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var userRoleDto = new UserRoleDto
                {
                    RoleName = newRole.Name,
                    RoleId = Guid.NewGuid(),
                };

                return this.SendApiResponse(userRoleDto, "User Role created successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }



        /// <summary>
        /// Retrieves all user roles.
        /// </summary>
        /// <returns>A list of user roles.</returns>
        [HttpGet]
        [Route("all")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserRoleDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _context.Roles
                    .Select(r => new UserRoleDto
                    {
                        RoleName = r.Name,
                        Description = r.Description
                    })
                    .ToListAsync();

                return this.SendApiResponse(roles, "Roles retrieved successfully.");
            }
            catch (Exception ex)
            {
                return this.SendApiError("DatabaseError", "An error occurred while fetching roles.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// creates permissions
        /// </summary>
        /// <param name="permissionRequest"></param>
        /// <returns></returns>
        [HttpPost(KnownUrls.Permission)]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PermissionDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionRequest permissionRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var newPermission = new Permission
                {
                    PermissionId = Guid.NewGuid(),
                    Resource = permissionRequest.Resource,
                    Action = permissionRequest.Action,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow)
                };
                await _context.Permissions.AddAsync(newPermission);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                var permissionResponse = new PermissionDto
                {
                    PermissionId = newPermission.PermissionId,
                };

                return this.SendApiResponse(permissionResponse, "Permission created successfully.");
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();

                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }



        /// <summary>
        /// maps the roles to the associated permission
        /// </summary>
        /// <param name="rolePermissionRequest"></param>
        /// <returns></returns>

        [HttpPost(KnownUrls.RolePermission)]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<RolePermissionDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRolePermission([FromBody] RolePermissionRequest rolePermissionRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var newRolePermission = new RolePermmsion
                {
                    RoleId = rolePermissionRequest.RoleId,
                    PermissionId = rolePermissionRequest.PermissionId
                };

                await _context.RolePermmsions.AddAsync(newRolePermission);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var rolePermissionResponse = new RolePermissionDto
                {
                    RoleId = newRolePermission.RoleId,
                    PermissionId = newRolePermission.PermissionId
                };
                return this.SendApiResponse(rolePermissionResponse, "Role-Permission maped successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }





    }
}
