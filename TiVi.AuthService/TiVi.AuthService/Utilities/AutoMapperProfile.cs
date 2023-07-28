using AutoMapper;
using TiVi.AuthService.DataAccess;
using TiVi.AuthService.Models.Request;
using TiVi.AuthService.Models.ViewModel;

namespace TiVi.AuthService.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserAccount, UserAccountVModel>();
            CreateMap<RegisterRequest, UserAccount>();
        }
    }
}
