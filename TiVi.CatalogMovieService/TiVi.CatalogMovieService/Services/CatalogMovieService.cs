using AutoMapper;
using Azure;
using TiVi.UserCatalogService.DataAccess;
using TiVi.UserCatalogService.Models.Base;
using TiVi.UserCatalogService.Models.Request;
using TiVi.UserCatalogService.Models.Response;
using TiVi.UserCatalogService.Services.Interfaces;
using TiVi.UserCatalogService.Utilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TiVi.UserCatalogService.Services
{
    public class CatalogMovieService : BaseService, ICatalogMovieService
    {
        private readonly TiViContext _context;
        private readonly IMapper _mapper; 
        public CatalogMovieService(TiViContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public BasePagingResponse<UserCatalogResponse> GetAllCatalogMovie(PagingParams param)
        {
            int skip = (param.page_number - 1) * param.page_size;

            var dataResult = new BasePagingResponse<UserCatalogResponse>();


            var moviesMap = _mapper.Map<List<UserCatalogResponse>>(_context.CatalogMovies.Where(w => w.IsAcitve.Equals(true)));

            int total = moviesMap.Count();
            var data = moviesMap.Skip(skip).Take(param.page_size).ToList();

            dataResult.data = data;
            dataResult.total_record = moviesMap?.Count != 0 ? total : 0;
            return dataResult;
        }

        public BasePagingResponse<UserCatalogResponse> GetCatalogMovie(PagingParams param)
        {
            int skip = (param.page_number - 1) * param.page_size;

            List<UserCatalogResponse> response = new List<UserCatalogResponse>();
            var dataResult = new BasePagingResponse<UserCatalogResponse>();
            var user = _context.UserAccounts.SingleOrDefault(x => x.Username == CurrentUserName);
            if (user != null)
            {
                var userProfile = _context.UserProfileMovies.Where(w => w.UserAccountId == user.UserAccountId && w.IsActive.Equals(true)).ToList();
                foreach (var item in userProfile)
                {
                    var movie = _context.CatalogMovies.SingleOrDefault(s => s.MovieId == item.MovieId && s.IsAcitve.Equals(true));
                    response.Add(_mapper.Map<UserCatalogResponse>(movie));
                }

                int total = response.Count();
                var data = response.Skip(skip).Take(param.page_size).ToList();

                dataResult.data = data;
                dataResult.total_record = response?.Count != 0 ? total : 0;
            }
            return dataResult;
        }

        public BasePagingResponse<UserCatalogResponse> GetTrendingMovie(PagingParams param)
        {
            int skip = (param.page_number - 1) * param.page_size;

            var dataResult = new BasePagingResponse<UserCatalogResponse>();

            var trendingMovie = from mt in _context.MovieTrendings
                                join m in _context.CatalogMovies on new { mt.MovieId, IsActive = mt.IsActive } equals new { m.MovieId, IsActive = m.IsAcitve }
                                select new UserCatalogResponse
                                {
                                    MovieId = mt.MovieId,
                                    MovieName = m.MovieName,
                                    ReleaseYear = m.ReleaseYear
                                };

            int total = trendingMovie.Count();
            var data = trendingMovie.Skip(skip).Take(param.page_size).ToList();

            dataResult.data = data;
            dataResult.total_record = trendingMovie?.Count() != 0 ? total : 0;
            return dataResult;
        }

        public bool InsertUserPrfile(UserProfileRequest userProfileRequest)
        {
            var userProfile = _mapper.Map<UserProfileMovie>(userProfileRequest);
            userProfile.UserAccountId = CurrentUserId;
            SetAuditFieldsInsert(userProfile);
            _context.Add(userProfile);
            _context.SaveChanges();
            return true;
        }

        public bool IsMovieExist(int movieId)
        {
            return _context.CatalogMovies.Any(a => a.MovieId == movieId);
        }
    }
}
