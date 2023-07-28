using AutoMapper;
using TiVi.UserCatalogService.DataAccess;
using TiVi.UserCatalogService.Models.Request;
using TiVi.UserCatalogService.Models.Response;

namespace TiVi.UserCatalogService.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CatalogMovie,UserCatalogResponse>();
            CreateMap<UserProfileRequest, UserProfileMovie>();
        }
    }
}
