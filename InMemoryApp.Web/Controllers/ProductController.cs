using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    { 
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache; 
        }
        public IActionResult Index()
        {
            //I.Way is check for Key value's whether in memory
            //if(String.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            //{
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString());
            //}
            //II.Way
            //if(!_memoryCache.TryGetValue("time", out string timecache))
            //{}
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            options.AbsoluteExpiration = DateTime.Now.AddSeconds(20);

            options.SlidingExpiration = TimeSpan.FromSeconds(10);

            options.Priority = CacheItemPriority.High;

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value}=> reason: {reason}");
            });

                _memoryCache.Set<string>("time", DateTime.Now.ToString(),options);

            Product product = new Product { Id=1,Name="Kalem",Price=200};

            _memoryCache.Set<Product>("product1", product);
           

            return View();
        }

        public IActionResult Show()
        {
            _memoryCache.TryGetValue("time", out string timecache);
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.callback = callback;
            ViewBag.time = timecache;

            ViewBag.product = _memoryCache.Get("product1");
            return View();
        }
    }
}
