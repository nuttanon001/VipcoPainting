using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using VipcoPainting.Helpers;
using VipcoPainting.Models;
using VipcoPainting.ViewModels;
using VipcoPainting.Services.Interfaces;

namespace VipcoPainting.Controllers
{
    [Produces("application/json")]
    [Route("api/PaymentCostHistory")]
    public class PaymentCostHistoryController : Controller
    {
        // GET: api/PaymentCostHistory
        #region PrivateMenbers

        private IRepositoryPainting<PaymentCostHistory> repository;
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<PaymentCostHistory> helpers;

        #endregion PrivateMenbers

        #region Constructor

        public PaymentCostHistoryController(IRepositoryPainting<PaymentCostHistory> repo,IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
            this.helpers = new HelpersClass<PaymentCostHistory>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/PaymentCostHistory
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            //var Includes = new List<string> { "ProjectCodeMaster" };
            //return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),
            //                            this.DefaultJsonSettings);
        }

        // GET: api/PaymentCostHistory/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            //var Includes = new List<string> { "ProjectCodeMaster" };
            //return new JsonResult(await this.repository.GetAsynvWithIncludes(key, "ProjectCodeSubId", Includes),
            //                            this.DefaultJsonSettings);
        }

        // GET: api/PaymentCostHistory/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = await this.repository.GetAllAsQueryable()
                                      .Where(x => x.PaymentDetailId == MasterId).ToListAsync();

            return new JsonResult(QueryData, this.DefaultJsonSettings);
        }
     
        #endregion GET

        #region POST

        // POST: api/PaymentCostHistory
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PaymentCostHistory nPaymentCostHistory)
        {
            if (nPaymentCostHistory != null)
            {
                nPaymentCostHistory = helpers.AddHourMethod(nPaymentCostHistory);

                nPaymentCostHistory.CreateDate = DateTime.Now;
                nPaymentCostHistory.Creator = nPaymentCostHistory.Creator ?? "Someone";

                return new JsonResult(await this.repository.AddAsync(nPaymentCostHistory), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found PaymentCostHistory data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/PaymentCostHistory/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]PaymentCostHistory uPaymentCostHistory)
        {
            var Message = "Not found PaymentCostHistory data.";
            try
            {
                if (uPaymentCostHistory != null)
                {
                    // add hour to DateTime to set Asia/Bangkok
                    uPaymentCostHistory = helpers.AddHourMethod(uPaymentCostHistory);

                    uPaymentCostHistory.ModifyDate = DateTime.Now;
                    uPaymentCostHistory.Modifyer = uPaymentCostHistory.Modifyer ?? "Someone";

                    return new JsonResult(await this.repository.UpdateAsync(uPaymentCostHistory, key), this.DefaultJsonSettings);
                }
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/PaymentCostHistory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
