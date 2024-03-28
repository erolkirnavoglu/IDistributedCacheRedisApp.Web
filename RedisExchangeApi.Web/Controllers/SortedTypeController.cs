using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Service;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class SortedTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listkey = "sorted";

        public SortedTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(3);
        }
        public IActionResult Index()
        {
            HashSet<string> keys = new HashSet<string>();
            if (db.KeyExists(listkey))
            {
                db.SortedSetScan(listkey).ToList().ForEach(p =>
                {
                    keys.Add(p.ToString());
                });
            }
            return View(keys);
        }
        public IActionResult Add(string name,int score)
        {
            db.KeyExpire(listkey,DateTime.Now.AddMinutes(5));
            db.SortedSetAdd(listkey, name, score);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(string name)
        {
            db.SortedSetRemove(listkey, name);
            return RedirectToAction("Index");
        }
    }
}
