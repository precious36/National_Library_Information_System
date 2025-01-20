using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NaLib_Staff_Management_Lib.Common;
using NaLib_Staff_Management_Lib.Data;
using NaLib_Staff_Management_Lib.Dtos;
using NaLib_Staff_Management_service.Controllers.Extensions;

namespace NaLib_Staff_Management_service.Controllers
{
    [Route(KnownUrls.Library)]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly NaLib_context _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryController"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        public LibraryController(NaLib_context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// creates locations for the libraries
        /// </summary>
        /// <param name="locationRequest"></param>
        /// <returns></returns>
        [HttpPost(KnownUrls.Library_location)]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<LocationDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLibraryLocation([FromBody] LocationRequest locationRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newLocation = new Location
                {
                    LocationId = Guid.NewGuid(),
                    Name = System.Text.Encoding.UTF8.GetBytes(locationRequest.Name),
                    IsCity = locationRequest.IsCity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _context.Locations.AddAsync(newLocation);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                var locationResponse = new LocationDto
                {
                    LocationId = newLocation.LocationId,
                    Name = System.Text.Encoding.UTF8.GetString(newLocation.Name),
                    IsCity = newLocation.IsCity
                };
                return this.SendApiResponse(locationResponse, "Library Location created successfully.");
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }


        /// <summary>
        /// creates new library
        /// </summary>
        /// <param name="libraryRequest"></param>
        /// <returns></returns>
        [HttpPost(KnownUrls.Librarycreation)]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<LibraryDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLibrary([FromBody] LibraryRequest libraryRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var locationExists = await _context.Locations.AnyAsync(l => l.LocationId == libraryRequest.LocationId);
                if (!locationExists)
                {
                    return this.SendApiError("location", "Location not found.", StatusCodes.Status400BadRequest);
                }
                var newLibrary = new Library
                {
                    LibraryId = Guid.NewGuid(),
                    Name = libraryRequest.Name,
                    LocationId = libraryRequest.LocationId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _context.Libraries.AddAsync(newLibrary);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                var libraryResponse = new LibraryDto
                {
                    LibraryId = newLibrary.LibraryId,
                    Name = newLibrary.Name,
                    LocationId = newLibrary.LocationId
                };
                return this.SendApiResponse(libraryResponse, "Library created successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a new department
        /// </summary>
        /// <param name="departmentRequest"></param>
        /// <returns></returns>
        [HttpPost(KnownUrls.CreateDepartment)]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<DepartmentDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentRequest departmentRequest)
        {
            if (!ModelState.IsValid)
            {
                return this.SendApiError<object>(null, "Validation error", ModelState);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newDepartment = new Department
                {
                    DepartmentId = Guid.NewGuid(),
                    Name = departmentRequest.Name,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _context.Departments.AddAsync(newDepartment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var departmentResponse = new DepartmentDto
                {
                    DepartmentId = newDepartment.DepartmentId,
                    Name = newDepartment.Name
                };

                return this.SendApiResponse(departmentResponse, "Department created successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
            }
        }


    }
}
