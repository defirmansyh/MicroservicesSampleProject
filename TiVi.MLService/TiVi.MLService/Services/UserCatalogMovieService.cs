using Newtonsoft.Json;
using RestSharp;
using TiVi.MLService.Models.Base;
using TiVi.MLService.Models.Response;
using TiVi.MLService.Services.Interfaces;
using TiVi.MLService.Utilities;

namespace TiVi.MLService.Services
{
    public class UserCatalogMovieService : IUserCatalogMovieService
    {
        public List<UserCatalogResponse> GetTrendingMovies()
        {
            var response = new List<UserCatalogResponse>();
            var token = ClaimIdentity.ClaimHttpRequest.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var body = new[] { new { page_number = 1, page_size = 10 } };
            var jsonBody = JsonConvert.SerializeObject(body.FirstOrDefault());
            var dataTrending = RestHelper<BaseJsonResponse<BasePagingResponse<UserCatalogResponse>>>.Submit(
                Method.POST,
                SettingHelper.GenerateEndpoint("UserCatalog", "GetTrendingMovies"),
                token,
                jsonBody
                );

            if (!dataTrending.is_success)
            {
                throw new Exception("Movie Trending Not Found");
            }

            foreach (UserCatalogResponse item in dataTrending.data.data)
            {
                response.Add(item);
            }

            return response;

        }
    }
}
