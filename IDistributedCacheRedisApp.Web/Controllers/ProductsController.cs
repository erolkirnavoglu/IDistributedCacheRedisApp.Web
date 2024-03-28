using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();

            List<Product> list = new();
         

            for (int i = 0; i < 10; i++)
            {
                Product product = new() { Id = i, Name = "Erol", Price = (decimal)4.5 };

                //  cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(100);


                 string json = JsonConvert.SerializeObject(product);
             

                await _distributedCache.SetStringAsync($"product_{i}", json, cacheOptions);
                // list.Add(product);
            }
            return View();
        }
        public async Task<IActionResult> Show()
        {
            LinkedList<Product> list = new LinkedList<Product>();
            for (int i = 0; i < 10; i++)
            {
                string product = await _distributedCache.GetStringAsync($"product_{i}");
                Product pro = JsonConvert.DeserializeObject<Product>(product);
                list.AddLast(pro);

            }


            ViewBag.List = list;
            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.Remove("Name");
            return View();
        }

        public async Task<IActionResult> ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/erol.png");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            await _distributedCache.SetAsync("resim", imageByte);

            return View();
        }
        public async Task<IActionResult> ShowImage()
        {
            byte[] resimByte = await _distributedCache.GetAsync("resim");

            return File(resimByte, "image/png");
        }
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }
    }
}
