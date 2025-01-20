using MongoDB.Driver;

namespace Library_Catalogue_api.Controllers.Utilss
{
    public static class NaLibCatalogueHrlpers
    {

        /// <summary>
        /// sorting and pagination function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<PaginatedResponse<T>> PaginateAndSortAsync<T>(
            IMongoCollection<T> collection,
            FilterDefinition<T> filter,
            int page,
            int pageSize,
            string sortBy = "id",
            string sortOrder = "asc")
        {
            if (page <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page and pageSize must be greater than zero.");
            }

            var sortDefinition = sortOrder.ToLower() == "desc"
                ? Builders<T>.Sort.Descending(sortBy)
                : Builders<T>.Sort.Ascending(sortBy);

            var totalItems = await collection.CountDocumentsAsync(filter);

            var items = await collection.Find(filter)
                .Sort(sortDefinition)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PaginatedResponse<T>
            {
                Items = items,
                TotalItems = (int)totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
            };
        }
    }

    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
