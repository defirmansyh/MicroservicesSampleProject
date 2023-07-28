using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TiVi.UserCatalogService.Models;
using TiVi.UserCatalogService.Models.Base;
using TiVi.UserCatalogService.Models.Request;
using TiVi.UserCatalogService.Models.Response;
using TiVi.UserCatalogService.Services.Interfaces;

namespace TiVi.UserCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCatalogController : ControllerBase
    {
        private readonly ICatalogMovieService _catalogMovieService;
        public UserCatalogController(ICatalogMovieService catalogMovieService)
        {
            _catalogMovieService = catalogMovieService;
        }

        [HttpPost]
        [Route("AllCatalog")]
        public BaseJsonResponse<BasePagingResponse<UserCatalogResponse>> GetAllCatalog([FromBody] PagingParams param)
        {
            var result = new BaseJsonResponse<BasePagingResponse<UserCatalogResponse>>();

            var data = _catalogMovieService.GetAllCatalogMovie(param);
            result.is_success = true;
            result.data = data;
            return result;
        }

        [HttpPost]
        [Route("TrendingMovie")]
        public BaseJsonResponse<BasePagingResponse<UserCatalogResponse>> GetTrendingMovie([FromBody] PagingParams param)
        {
            var result = new BaseJsonResponse<BasePagingResponse<UserCatalogResponse>>();

            var data = _catalogMovieService.GetTrendingMovie(param);
            result.is_success = true;
            result.data = data;
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("UserCatalog")]
        public BaseJsonResponse<BasePagingResponse<UserCatalogResponse>> GetUserCatalog([FromBody]PagingParams param)
        {
            var result = new BaseJsonResponse<BasePagingResponse<UserCatalogResponse>>();

            var data = _catalogMovieService.GetCatalogMovie(param);
            result.is_success = true;
            result.data = data;
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("GiveRating")]
        public BaseJsonResponse<string> GiveRatingMovie([FromBody]UserProfileRequest userProfile)
        {
            var result = new BaseJsonResponse<string>();

            if (userProfile.MovieRating > 5 || userProfile.MovieRating < 1)
            {
                result.is_success = false;
                result.data = "Rating must be greater than 0 and less than equal 5";
                return result;
            }

            if (!_catalogMovieService.IsMovieExist(userProfile.MovieId))
            {
                result.is_success = false;
                result.data = "Movie does not exists";
                return result;
            }

            result.is_success = _catalogMovieService.InsertUserPrfile(userProfile);
            result.data = "Success";
            return result;
        }
    }
}
