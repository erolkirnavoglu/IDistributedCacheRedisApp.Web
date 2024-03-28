using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedisExchangeApi.Web.Service;
using StackExchange.Redis;


namespace RedisExchangeApi.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listkey = "names";
        public ListTypeController(RedisService redisService)
        {

            _redisService = redisService;
            db = _redisService.GetDb(1);

        }
        public IActionResult Index()
        {
            List<string> list = new List<string>();
            if (db.KeyExists(listkey))
            {
                db.ListRange(listkey).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }

            return View(list);
        }
        public IActionResult Add(string name)
        {
            Product pro = new();
            pro.ProductName = name;
            pro.Price = (decimal)4.5;
            pro.Id = count;

            var json = JsonConvert.SerializeObject(pro);

            db.ListRightPush(listkey, json);

            return RedirectToAction("Index");
        }
        static int count = 1;
        public async Task<IActionResult> Delete(string name)
        {
           
          

            await db.ListRemoveAsync(listkey, name);
            count++;

            return RedirectToAction("Index");
        }
        public class Product
        {
            public int Id { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
        }
    }
}
