using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PriorMoney.Storage.Interface;
using PriorMoney.WebApp.Controllers.Razor.Models;

namespace PriorMoney.WebApi.Controllers.Razor
{
    [Route("Index")]
    public class IndexController : Controller
    {
        private readonly IDbLogicManager _dbLogicManager;

        public IndexController(IDbLogicManager dbLogicManager)
        {
            _dbLogicManager = dbLogicManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int numberOfLastOperationsToGet = 10;
            var lastOperations = await _dbLogicManager.GetLastOperations(numberOfLastOperationsToGet);

            var model = new IndexViewModel(lastOperations);

            return View("Index", model);
        }

    }
}