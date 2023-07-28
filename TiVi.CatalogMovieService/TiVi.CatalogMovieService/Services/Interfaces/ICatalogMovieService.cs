using TiVi.UserCatalogService.Models.Base;
using TiVi.UserCatalogService.Models.Request;
using TiVi.UserCatalogService.Models.Response;

namespace TiVi.UserCatalogService.Services.Interfaces
{
    public interface ICatalogMovieService
    {
        BasePagingResponse<UserCatalogResponse> GetAllCatalogMovie(PagingParams param);
        BasePagingResponse<UserCatalogResponse> GetCatalogMovie(PagingParams param);
        BasePagingResponse<UserCatalogResponse> GetTrendingMovie(PagingParams param);

        bool InsertUserPrfile(UserProfileRequest userProfileRequest);
        bool IsMovieExist(int movieId);
    }
}
