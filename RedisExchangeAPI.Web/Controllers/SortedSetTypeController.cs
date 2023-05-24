﻿using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _database;
        private string listKey = "sortedsetnames";
        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _database = _redisService.GetDb(3);
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();

            if(_database.KeyExists(listKey))
            {
                //_database.SortedSetScan(listKey).ToList().ForEach(x =>
                //{
                //    list.Add(x.ToString());
                //});

                _database.SortedSetRangeByRank(listKey, order: Order.Descending).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }


            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name,int score) 
        {
            _database.SortedSetAdd(listKey, name, score);

            _database.KeyExpire(listKey, DateTime.Now.AddMinutes(1));

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            _database.SortedSetRemove(listKey, name);

            return RedirectToAction("Index");
        }
    }
}
