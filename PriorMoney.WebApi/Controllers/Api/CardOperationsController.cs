using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Results;
using Microsoft.AspNetCore.Mvc;
using PriorMoney.Model;
using PriorMoney.Storage.Interface;

namespace PriorMoney.WebApp.Controllers
{
    public class CardOperationsController : ODataController
    {
        private readonly IStorage<CardOperation> _cardOperationStorage;

        public CardOperationsController(IStorage<CardOperation> cardOperationStorage)
        {
            _cardOperationStorage = cardOperationStorage;
        }

        // GET api/values
        [HttpGet]
        [EnableQuery]
        public IQueryable<CardOperation> Get()
        {
            return _cardOperationStorage.GetAllAsQueryable();
        }

        [HttpPost]
        [EnableQuery]
        public async Task<IActionResult> Post([FromBody]CardOperation cardOperation)
        {
            await _cardOperationStorage.Add(cardOperation);

            return Created(cardOperation);
        }
    }
}
