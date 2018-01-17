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
    [Route("api/SubPaymentDetail")]
    public class SubPaymentDetailController : Controller
    {
        #region PrivateMembers
        // Repository
        private IRepositoryPainting<SubPaymentDetail> repository;
        // Mapper
        private IMapper mapper;
        // Helper
        private HelpersClass<SubPaymentDetail> helpers;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        #endregion PrivateMenbers

        #region Constructor
        public SubPaymentDetailController(IRepositoryPainting<SubPaymentDetail> repo, IMapper map)
        {
            // Repository
            this.repository = repo;
            // Mapper
            this.mapper = map;
            // Helper
            this.helpers = new HelpersClass<SubPaymentDetail>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/SubPaymentDetail
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            //var Includes = new List<string> { "PaintTeam" };

            //return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastRoomViewModel, SubPaymentDetail>
            //                     (await this.repository.GetAllWithInclude2Async(Includes)),
            //                      this.DefaultJsonSettings);
        }

        // GET: api/SubPaymentDetail/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "PaintTaskDetail", "PaymentCostHistory.PaymentDetail" };

            return new JsonResult(this.mapper.Map<SubPaymentDetail, SubPaymentDetailViewModel>
                                 (await this.repository.GetAsynvWithIncludes(key, "SubPaymentDetailId", Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/SubPaymentDetail/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.SubPaymentMasterId == MasterId)
                                .Include(x => x.PaymentDetail)
                                .Include(x => x.PaymentCostHistory)
                                .AsNoTracking();
           
            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<SubPaymentDetailViewModel, SubPaymentDetail>
                            (await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST

        // POST: api/SubPaymentDetail
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SubPaymentDetail nSubPaymenyDetail)
        {
            if (nSubPaymenyDetail != null)
            {
                nSubPaymenyDetail = this.helpers.AddHourMethod(nSubPaymenyDetail);

                nSubPaymenyDetail.CreateDate = DateTime.Now;
                nSubPaymenyDetail.Creator = nSubPaymenyDetail.Creator ?? "Someone";
                //Relation
                nSubPaymenyDetail.PaymentDetail = null;

                return new JsonResult(await this.repository.AddAsync(nSubPaymenyDetail), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found subpayment detail data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/SubPaymentDetail/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]SubPaymentDetail uSubPaymentDetail)
        {
            var Message = "SubPayment detail not been found.";

            if (uSubPaymentDetail != null)
            {
                uSubPaymentDetail = this.helpers.AddHourMethod(uSubPaymentDetail);

                // set modified
                uSubPaymentDetail.ModifyDate = DateTime.Now;
                uSubPaymentDetail.Modifyer = uSubPaymentDetail.Modifyer ?? "Someone";
                // Relation
                uSubPaymentDetail.PaymentDetail = null;

                return new JsonResult(await this.repository.UpdateAsync(uSubPaymentDetail, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/SubPaymentDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
