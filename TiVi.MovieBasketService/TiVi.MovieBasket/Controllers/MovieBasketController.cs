using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using TiVi.MovieBasketService.Models.Base;
using TiVi.MovieBasketService.Models.Request;
using TiVi.MovieBasketService.Models.Response;
using TiVi.MovieBasketService.Utilities;

namespace TiVi.MovieBasketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieBasketController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IMapper _mapper;

        public MovieBasketController(IDistributedCache distributedCache, IMapper mapper)
        {
            _distributedCache = distributedCache;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [Route("GetMovie")]
        public BaseJsonResponse<List<MovieBasketResponse>> GetMovieBasket()
        {
            var result = new BaseJsonResponse<List<MovieBasketResponse>>();

            result.is_success = true;
            result.data = _distributedCache.GetRecord<List<MovieBasketResponse>>(ClaimIdentity.CurrentUserName);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("InsertMovie")]
        public BaseJsonResponse<string> InsertMovie([FromBody]MovieBasketRequest movieBasketRequest)
        {
            var result = new BaseJsonResponse<string>();

            var dataBasket = _distributedCache.GetRecord<List<MovieBasketResponse>>(ClaimIdentity.CurrentUserName);
            var dataBasketMap = _mapper.Map<List<MovieBasketRequest>>(dataBasket);
            dataBasketMap.Add(movieBasketRequest);
            _distributedCache.SetRecord<List<MovieBasketRequest>>(ClaimIdentity.CurrentUserName, dataBasketMap);

            result.is_success = true;
            result.data = "Success";
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("RemoveMovie")]
        public BaseJsonResponse<string> RemoveMovie(int[] movieId)
        {
            var result = new BaseJsonResponse<string>();

            var dataBasket = _distributedCache.GetRecord<List<MovieBasketResponse>>(ClaimIdentity.CurrentUserName);
            if (dataBasket != null)
            {
                Array.ForEach(movieId, movie =>
                {
                    foreach (var item in dataBasket.ToList())
                    {
                        if (item.MovieId == movie)
                        {
                            dataBasket.Remove(item);
                        }
                    }
                });
            }
            else
            {
                result.is_success = true;
                result.data = "Basket is empty";
                return result;
            }
            
            if (dataBasket != null)
            {
                _distributedCache.SetRecord<List<MovieBasketResponse>>(ClaimIdentity.CurrentUserName, dataBasket);
            }

            result.is_success = true;
            result.data = "Success";
            return result;
        }
    }
}
