using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Service;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        private string hashKey = "hashKey";
        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string, object> list = new();
            if (db.KeyExists(hashKey))
            {
                db.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name!, x.Value);
                });
            }

            return View(list);
        }
        public IActionResult Add(string name,string val) 
        {
            db.HashSet(hashKey, name,val);

            return RedirectToAction("Index");
        }
        public IActionResult Delete(string name)
        {
            db.HashDelete(hashKey, name);
            return RedirectToAction("Index");
        }
    }
}
