using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace API.Helpers
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response,
            int currentPage, int pageSize, int totalCount, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, pageSize, totalCount, totalPages);
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, jsonOptions));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}