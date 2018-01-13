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
    [Route("api/PaymentDetail")]
    public class PaymentDetailController : Controller
    {
        #region PrivateMenbers
        // Repository
        private IRepositoryPainting<PaymentDetail> repository;
        private IRepositoryPainting<PaymentCostHistory> repositoryCostHis;
        // Mapper
        private IMapper mapper;
        // Helper
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<PaymentDetail> helpers;


        #endregion PrivateMenbers

        #region Constructor

        public PaymentDetailController(
            IRepositoryPainting<PaymentDetail> repo, 
            IRepositoryPainting<PaymentCostHistory> repoCostHis,
            IMapper map)
        {
            // Repository
            this.repository = repo;
            this.repositoryCostHis = repoCostHis;
            // Mapper
            this.mapper = map;
            // Helper
            this.helpers = new HelpersClass<PaymentDetail>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/PaymentDetail
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<PaymentDetailViewModel,PaymentDetail>(await this.repository.GetAllAsync()), this.DefaultJsonSettings);
            //var Includes = new List<string> { "StandradTimeInt", "StandradTimeExt", "SurfaceTypeInt", "SurfaceTypeExt" };

            //return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastWorkItemViewModel, PaymentDetail>
            //                     (await this.repository.GetAllWithInclude2Async(Includes)),
            //                      this.DefaultJsonSettings);
        }

        // GET: api/PaymentDetail/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            return new JsonResult(this.mapper.Map<PaymentDetail,PaymentDetailViewModel>(await this.repository.GetAsync(key)), this.DefaultJsonSettings);
            //var Includes = new List<string> { "StandradTimeInt", "StandradTimeExt", "SurfaceTypeInt", "SurfaceTypeExt" };

            //return new JsonResult(this.mapper.Map<PaymentDetail, BlastWorkItemViewModel>
            //                     (await this.repository.GetAsynvWithIncludes(key, "BlastWorkItemId", Includes)),
            //                      this.DefaultJsonSettings);
        }
        #endregion GET

        #region POST
        // POST: api/PaymentDetail/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable().AsQueryable();

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);
            foreach (var keyword in filters)
                QueryData = QueryData.Where(x => x.Description.ToLower().Contains(keyword));
            // Order
            switch (Scroll.SortField)
            {
                case "Description":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Description);
                    else
                        QueryData = QueryData.OrderBy(e => e.Description);
                    break;

                case "PaymentTypeString":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.PaymentType);
                    else
                        QueryData = QueryData.OrderBy(e => e.PaymentType);
                    break;

                case "LastCost":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.LastCost);
                    else
                        QueryData = QueryData.OrderBy(e => e.LastCost);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.CreateDate);
                    break;
            }
            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);
            // Return
            return new JsonResult(new ScrollDataViewModel<PaymentDetailViewModel>(Scroll,
                this.ConvertTable.ConverterTableToViewModel<PaymentDetailViewModel, PaymentDetail>
                (await QueryData.AsNoTracking().ToListAsync())), this.DefaultJsonSettings);
        }
        // POST: api/PaymentDetail
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PaymentDetail nPaymentDetail)
        {
            if (nPaymentDetail != null)
            {
                nPaymentDetail = this.helpers.AddHourMethod(nPaymentDetail);

                nPaymentDetail.CreateDate = DateTime.Now;
                nPaymentDetail.Creator = nPaymentDetail.Creator ?? "Someone";

                //new payment history
                nPaymentDetail.PaymentCostHistorys.Add(new PaymentCostHistory
                {
                    CreateDate = nPaymentDetail.CreateDate,
                    Creator = nPaymentDetail.Creator,
                    PaymentCost = nPaymentDetail.LastCost,
                    StartDate = nPaymentDetail?.CreateDate ?? DateTime.Now,
                });

                return new JsonResult(await this.repository.AddAsync(nPaymentDetail), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found BlastWork data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/PaymentDetail/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]PaymentDetail uPaymentDetail)
        {
            var Message = "Payment detail not been found.";

            if (uPaymentDetail != null)
            {
                uPaymentDetail = this.helpers.AddHourMethod(uPaymentDetail);

                // set modified
                uPaymentDetail.ModifyDate = DateTime.Now;
                uPaymentDetail.Modifyer = uPaymentDetail.Modifyer ?? "Someone";

                // get dbData
                var dbPaymentCostHistory = await this.repositoryCostHis.GetAllAsQueryable()
                                                                        .Where(x => x.PaymentDetailId == key)
                                                                        .OrderByDescending(x => x.StartDate)
                                                                        .FirstOrDefaultAsync();

                if (dbPaymentCostHistory == null)
                {
                    await this.repositoryCostHis.AddAsync(new PaymentCostHistory
                    {
                        CreateDate = uPaymentDetail.CreateDate,
                        Creator = uPaymentDetail.Creator,
                        PaymentCost = uPaymentDetail.LastCost,
                        StartDate = uPaymentDetail?.CreateDate ?? DateTime.Now,
                        PaymentDetailId = uPaymentDetail.PaymentDetailId
                    });
                }
                else
                {
                    if (dbPaymentCostHistory.StartDate != null && dbPaymentCostHistory.EndDate != null)
                    {
                        await this.repositoryCostHis.AddAsync(new PaymentCostHistory
                        {
                            CreateDate = uPaymentDetail.CreateDate,
                            Creator = uPaymentDetail.Creator,
                            PaymentCost = uPaymentDetail.LastCost,
                            StartDate = uPaymentDetail?.CreateDate ?? DateTime.Now,
                            PaymentDetailId = uPaymentDetail.PaymentDetailId
                        });
                    }
                    else if (dbPaymentCostHistory.StartDate.Date == uPaymentDetail.ModifyDate.Value.Date)
                    {
                        dbPaymentCostHistory.PaymentCost = uPaymentDetail.LastCost;
                        dbPaymentCostHistory.ModifyDate = uPaymentDetail.ModifyDate;
                        dbPaymentCostHistory.Modifyer = uPaymentDetail.Modifyer;

                        await this.repositoryCostHis.UpdateAsync(dbPaymentCostHistory, dbPaymentCostHistory.PaymentCostHistoryId);
                    }
                    else
                    {
                        dbPaymentCostHistory.EndDate = uPaymentDetail.ModifyDate;
                        dbPaymentCostHistory.ModifyDate = uPaymentDetail.ModifyDate;
                        dbPaymentCostHistory.Modifyer = uPaymentDetail.Modifyer;
                        // update LastCost
                        await this.repositoryCostHis.UpdateAsync(dbPaymentCostHistory, dbPaymentCostHistory.PaymentCostHistoryId);
                        // add NewCost
                        await this.repositoryCostHis.AddAsync(new PaymentCostHistory
                        {
                            CreateDate = uPaymentDetail.CreateDate,
                            Creator = uPaymentDetail.Creator,
                            PaymentCost = uPaymentDetail.LastCost,
                            StartDate = uPaymentDetail?.CreateDate ?? DateTime.Now,
                            PaymentDetailId = uPaymentDetail.PaymentDetailId
                        });
                    }
                }
                return new JsonResult(await this.repository.UpdateAsync(uPaymentDetail, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/PaymentDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
