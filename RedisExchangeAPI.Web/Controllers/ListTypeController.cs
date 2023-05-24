using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string listKey = "names";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            List<string> namesList = new List<string>();
            if(_database.KeyExists(listKey))
            {
                _database.ListRange(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            _database.ListRightPush(listKey,name);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteItem(string name)
        {
            _database.ListRemoveAsync(listKey,name).Wait();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem()
        {
            _database.ListLeftPop(listKey);

            return RedirectToAction("Index");
        }

    }
}
