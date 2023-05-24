using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string listKey = "hashnames";
        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(2);
        }
        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();

            if(_database.KeyExists(listKey))
            {
                _database.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return View();
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            if (!_database.KeyExists(listKey))
            {
                _database.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
            }
            _database.SetAdd(listKey, name);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
           await _database.SetRemoveAsync(listKey, name);

            return RedirectToAction("Index");
        } 
    }
}
