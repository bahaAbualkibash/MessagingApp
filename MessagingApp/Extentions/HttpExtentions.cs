using MessagingApp.Helpers;
using System.Text.Json;

namespace MessagingApp.Extentions
{
    public static class HttpExtentions
    {
        public static void AddPaginationHeader(this HttpResponse httpResponse,int currentPage,int itemsPerPage,int totalItems,int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage,itemsPerPage,totalItems,totalPages);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            httpResponse.Headers.Add("Pagination",JsonSerializer.Serialize(paginationHeader,options));
            httpResponse.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
