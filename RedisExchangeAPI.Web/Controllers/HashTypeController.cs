using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        public string hashKey { get; set; } = "sozlık";
        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string,string> list = new Dictionary<string,string>();

            if(_database.KeyExists(hashKey))
            {
                _database.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            _database.HashSet(hashKey, name, value);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            _database.HashDelete(hashKey, name);
            return RedirectToAction("Index");
        }
    }
}
