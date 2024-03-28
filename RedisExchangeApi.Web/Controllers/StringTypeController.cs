using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Service;

namespace RedisExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
        }
        public IActionResult Index()
        {
          var db=  _redisService.GetDb(0);

            db.StringSet("name","Erol Kırnavoğlu");
            db.StringSet("ziyaretci", 1000);

            return View();
        }
        public IActionResult Show()
        {

            var db=_redisService.GetDb(0);

           var name= db.StringGet("name");

            ViewBag.Name = name;

            return View();
        }
    }
}
