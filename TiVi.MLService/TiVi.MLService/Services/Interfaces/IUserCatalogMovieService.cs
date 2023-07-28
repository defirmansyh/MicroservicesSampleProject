using TiVi.MLService.Models.Response;

namespace TiVi.MLService.Services.Interfaces
{
    public interface IUserCatalogMovieService
    {
        List<UserCatalogResponse> GetTrendingMovies();
    }
}
