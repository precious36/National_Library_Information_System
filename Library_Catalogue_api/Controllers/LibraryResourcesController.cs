using Library_Catalogue_api.Controllers.Extensions;
using Library_Catalogue_api.Controllers.Utilss;
using Library_Catalogue_Lib.Common;
using Library_Catalogue_Lib.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using NaLib_Staff_Management_Lib.Common;


[ApiController]
[Route(KnownlUrls.LibraryCatalogue)]
public class LibraryResourcesController : ControllerBase
{
    private readonly IMongoCollection<LibraryResource> _libraryResourcesCollection;
    private readonly IMongoCollection<Publisher> _publishersCollection;
    private readonly IMongoCollection<Author> _authorsCollection;

    /// <summary>
    /// Initializes a new instance of the <see cref="LibraryResourcesController"/> class.
    /// </summary>
    /// <param name="context"></param>
    public LibraryResourcesController(CatalogueDbContext context)
    {
        _libraryResourcesCollection = context.GetCollection<LibraryResource>("LibraryResources");
        _publishersCollection = context.GetCollection<Publisher>("Publishers");
        _authorsCollection = context.GetCollection<Author>("Author");   
    }

    /// <summary>
    /// Creates a new library resource.
    /// </summary>
    /// <param name="resource">Details of the new library resource.</param>
    /// <returns>
    /// The created library resource details.
    /// </returns>
    /// <response code="201">Library resource created successfully.</response>
    /// <response code="400">Validation error or bad request.</response>
    /// <response code="500">An error occurred while creating the library resource.</response>
    [HttpPost(KnownlUrls.CreateResource)]
    [ProducesResponseType(typeof(LibraryResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateLibraryResource([FromBody] LibraryResource resource)
    {
        if (resource == null)
        {
            return BadRequest("Resource is null.");
        }

        try
        {
            await _libraryResourcesCollection.InsertOneAsync(resource);
            return CreatedAtAction(nameof(GetLibraryResource), new { id = resource.Id }, resource);
        }
        catch (Exception ex)
        {
            return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
        }
    }

    



    /// <summary>
    /// Creates a new publisher.
    /// </summary>
    /// <param name="publisher">Details of the new publisher.</param>
    /// <returns>
    /// The created publisher details.
    /// </returns>
    /// <response code="201">Publisher created successfully.</response>
    /// <response code="400">Validation error or bad request.</response>
    /// <response code="500">An error occurred while creating the publisher.</response>
    [HttpPost(KnownlUrls.CreatePublisher)]
    [ProducesResponseType(typeof(Publisher), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePublisher([FromBody] Publisher publisher)
    {
        if (publisher == null)
        {
            return BadRequest("Publisher is null.");
        }

        try
        {
            publisher.CreatedAt = DateTime.UtcNow;
            publisher.UpdatedAt = DateTime.UtcNow;
            return CreatedAtAction(nameof(GetPublisherById), new { id = publisher.Id }, publisher);
        }
        catch (Exception ex)
        {

            return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Retrieves a library resource by its ID.
    /// </summary>
    /// <param name="id">The ID of the library resource to retrieve.</param>
    /// <returns>
    /// The requested library resource details.
    /// </returns>
    /// <response code="200">Library resource retrieved successfully.</response>
    /// <response code="400">Invalid ID format.</response>
    /// <response code="404">Library resource not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LibraryResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLibraryResource(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        try
        {
            var resource = await _libraryResourcesCollection
                .Find(x => x.Id == objectId)
                .FirstOrDefaultAsync();

            if (resource == null)
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status400BadRequest,

                });

            return Ok(resource);
        }
        catch (Exception ex)
        {
            return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
        }
    }


    /// <summary>
    /// Retrieves all library resources with pagination and sorting.
    /// </summary>
    /// <param name="page">The page number (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="sortBy">The field to sort by (default is "type").</param>
    /// <param name="sortOrder">The sort order ("asc" or "desc", default is "asc").</param>
    /// <returns>A paginated list of library resources.</returns>
    /// <response code="200">Library resources retrieved successfully.</response>
    /// <response code="400">Invalid pagination or sorting parameters.</response>
    /// <response code="500">An error occurred while retrieving resources.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<LibraryResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllLibraryResources(
        int page = 1,
        int pageSize = 10,
        string sortBy = "type",
        string sortOrder = "asc")
    {
        try
        {
            
            var filter = Builders<LibraryResource>.Filter.Empty;

            // Call the pagination helper
            var result = await NaLibCatalogueHrlpers.PaginateAndSortAsync(
                _libraryResourcesCollection,
                filter,
                page,
                pageSize,
                sortBy,
                sortOrder
            );

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest("Invalid format.");
        }
        catch (Exception ex)
        {
            return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
        }
    }



    /// <summary>
    /// Gets all publishers.
    /// </summary>
    /// <returns>List of publishers.</returns>
    /// <response code="200">Publishers retrieved successfully.</response>
    /// <response code="500">An error occurred while retrieving the publishers.</response>
    [HttpGet("get_all_publishers")]
    [ProducesResponseType(typeof(IEnumerable<Publisher>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPublishers()
    {
        try
        {
            var publishers = await _publishersCollection.Find(_ => true).ToListAsync();
            return Ok(publishers);
        }
        catch (Exception ex)
        {
            return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Gets a publisher by ID.
    /// </summary>
    /// <param name="id">Publisher ID.</param>
    /// <returns>Publisher details.</returns>
    /// <response code="200">Publisher retrieved successfully.</response>
    /// <response code="404">Publisher not found.</response>
    /// <response code="500">An error occurred while retrieving the publisher.</response>
    [HttpGet("get_by_id")]
    [ProducesResponseType(typeof(Publisher), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPublisherById(string id)
    {
        try
        {
            var publisher = await _publishersCollection.Find(p => p.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
            if (publisher == null)
            {
                return NotFound($"Publisher with ID {id} not found.");
            }

            return Ok(publisher);
        }
        catch (Exception ex)
        {
            return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Updates a publisher.
    /// </summary>
    /// <param name="id">Publisher ID.</param>
    /// <param name="updatedPublisher">Updated publisher details.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Publisher updated successfully.</response>
    /// <response code="404">Publisher not found.</response>
    /// <response code="500">An error occurred while updating the publisher.</response>
    [HttpPut("publisher{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePublisher(string id, [FromBody] Publisher updatedPublisher)
    {
        try
        {
            var filter = Builders<Publisher>.Filter.Eq(p => p.Id, ObjectId.Parse(id));
            var update = Builders<Publisher>.Update
                .Set(p => p.Name, updatedPublisher.Name)
                .Set(p => p.Address, updatedPublisher.Address)
                .Set(p => p.ContactNumber, updatedPublisher.ContactNumber)
                .Set(p => p.Email, updatedPublisher.Email)
                .Set(p => p.Website, updatedPublisher.Website)
                .Set(p => p.EstablishedDate, updatedPublisher.EstablishedDate)
                .Set(p => p.UpdatedAt, DateTime.UtcNow);

            var result = await _publishersCollection.UpdateOneAsync(filter, update);

            if (result.MatchedCount == 0)
            {
                return NotFound($"Publisher with ID {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Deletes a publisher by ID.
    /// </summary>
    /// <param name="id">Publisher ID.</param>
    /// <returns>No content.</returns>
    /// <response code="204">Publisher deleted successfully.</response>
    /// <response code="404">Publisher not found.</response>
    /// <response code="500">An error occurred while deleting the publisher.</response>
    [HttpDelete("delete_publisher{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePublisher(string id)
    {
        try
        {
            var result = await _publishersCollection.DeleteOneAsync(p => p.Id == ObjectId.Parse(id));

            if (result.DeletedCount == 0)
            {
                return NotFound($"Publisher with ID {id} not found.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Creates a new author.
    /// </summary>
    /// <param name="author">Details of the new author.</param>
    /// <returns>
    /// The created author details.
    /// </returns>
    /// <response code="201">Author created successfully.</response>
    /// <response code="400">Validation error or bad request.</response>
    /// <response code="500">An error occurred while creating the author.</response>
    [HttpPost("create_auther")]
    [ProducesResponseType(typeof(Author), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAuthor([FromBody] Author author)
    {
        if (author == null)
        {
            return BadRequest("Author is null.");
        }

        try
        {
            author.CreatedAt = DateTime.UtcNow;
            author.UpdatedAt = DateTime.UtcNow;
            await _authorsCollection.InsertOneAsync(author);
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id.ToString() }, author);
        }
        catch (Exception ex)
        {
            return this.SendApiError("DatabaseError", "An error occurred while processing your request.", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Fetches the details of an author by ID.
    /// </summary>
    /// <param name="id">The ID of the author to fetch.</param>
    /// <returns>
    /// The details of the author.
    /// </returns>
    /// <response code="200">Author details fetched successfully.</response>
    /// <response code="404">Author not found.</response>
    /// <response code="500">An error occurred while fetching the author details.</response>
    [HttpGet("get_auther{id}")]
    [ProducesResponseType(typeof(Author), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuthorById(string id)
    {
        try
        {
            var author = await _authorsCollection.Find(a => a.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
            if (author == null)
            {
                return NotFound($"Author with id {id} not found.");
            }
            return Ok(author);
        }
        catch (Exception ex)
        {
            return this.SendApiError("DatabaseError", "An error occurred while fetching the author.", StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Deletes an author by ID.
    /// </summary>
    /// <param name="id">The ID of the author to delete.</param>
    /// <returns>
    /// A response indicating the success or failure of the delete operation.
    /// </returns>
    /// <response code="204">Author deleted successfully.</response>
    /// <response code="404">Author not found.</response>
    /// <response code="500">An error occurred while deleting the author.</response>
    [HttpDelete("delete_auther{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAuthor(string id)
    {
        try
        {
            var result = await _authorsCollection.DeleteOneAsync(a => a.Id == ObjectId.Parse(id));
            if (result.DeletedCount == 0)
            {
                return NotFound($"Author with id {id} not found.");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return this.SendApiError("DatabaseError", "An error occurred while deleting the author.", StatusCodes.Status500InternalServerError);
        }
    }



}
