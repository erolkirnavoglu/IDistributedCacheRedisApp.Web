using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Service;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly RedisService _redisService;
        protected readonly IDatabase db;
       
        public BaseController(RedisService redisService)
        {

            _redisService = redisService;
            db = _redisService.GetDb(4);

        }
    }
}
