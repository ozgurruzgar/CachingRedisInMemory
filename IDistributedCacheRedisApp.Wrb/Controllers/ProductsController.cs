using IDistributedCacheRedisApp.Wrb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Wrb.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();

            cacheOptions.AbsoluteExpiration=DateTime.Now.AddMinutes(1);

            //_distributedCache.SetString("name","Özgür", cacheOptions);

            Product product = new Product { Id=1,Name="Kalem",Price=100};

            string jsonProduct = JsonConvert.SerializeObject(product);

            //_distributedCache.SetString("product:1", jsonProduct,cacheOptions);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            _distributedCache.Set("product", byteProduct);


            return View();
        }

        public IActionResult Show()
        {
            //string name = _distributedCache.GetString("name");

            Byte[] byteProduct = _distributedCache.Get("product:1");

            string jsonProduct = Encoding.UTF8.GetString(byteProduct);

            Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);

            ViewBag.Product = product;

            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.Remove("name");

            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/mclaren.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("araba", imageByte);

            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedCache.Get("araba");

            return File(imageByte, "image/jpg");
        }
    }
}
