using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Service;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listkey = "hasnnames";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(2);
        }
        public async Task<IActionResult> Index()
        {
            HashSet<string> keys = new HashSet<string>();
            if (db.KeyExists(listkey))
            {
                db.SetMembers(listkey).ToList().ForEach(x => keys.Add(x.ToString()));
            }

            return View(keys);
        }
        [HttpPost]
        public async Task<IActionResult> Add(string name)
        {
            db.KeyExpire(listkey, DateTime.Now.AddMinutes(5));
            await db.SetAddAsync(listkey, name);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string name)
        {
            await db.SetRemoveAsync(listkey, name);
            return RedirectToAction("Index");
        }
    }
}
