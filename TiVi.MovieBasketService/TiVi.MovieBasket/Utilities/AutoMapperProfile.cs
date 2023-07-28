using AutoMapper;
using TiVi.MovieBasketService.Models.Request;
using TiVi.MovieBasketService.Models.Response;

namespace TiVi.MovieBasketService.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MovieBasketResponse, MovieBasketRequest>();
        }
    }
}
