using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisService _redisService;
        protected readonly IDatabase _database;
        protected string listKey = "sortedsetnames";
        public BaseController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(1);
        }
    }
}
