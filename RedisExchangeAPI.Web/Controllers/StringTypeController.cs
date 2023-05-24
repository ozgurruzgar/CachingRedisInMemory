using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            var database = _redisService.GetDb(0);

            database.StringSet("name", "Özgür Rüzgar");
            database.StringSet("ziyaretci", 100);

            return View();
        }

        public IActionResult Show()
        {
            var value = _database.StringLength("name");

            //_database.StringIncrement("ziyaretci",10);

            //var count = _database.StringDecrementAsync("ziyaretci", 1).Result;

            return View();
        }
    }
}
