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
    [Route("api/RequisitionMaster")]
    public class RequisitionMasterController : Controller
    {
        #region PrivateMembers
        // Repository
        private IRepositoryPainting<RequisitionMaster> repository;
        private IRepositoryPainting<MovementStockStatus> repositoryMoveStatus;
        private IRepositoryPainting<ColorMovementStock> repositoryMovement;
        private IRepositoryMachine<Employee> repositoryEmp;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        // Helper
        private HelpersClass<RequisitionMaster> helpers;

        private Func<DateTime, DateTime> ChangeTimeZone = d => d.AddHours(+7);
        #endregion PrivateMenbers

        #region Constructor
        public RequisitionMasterController(
            IRepositoryPainting<RequisitionMaster> repo, 
            IRepositoryPainting<MovementStockStatus> repoMoveStatus,
            IRepositoryPainting<ColorMovementStock> repoMoveStock,
            IRepositoryMachine<Employee> repoEmp,
            IMapper map)
        {
            // Repository
            this.repository = repo;
            this.repositoryMoveStatus = repoMoveStatus;
            this.repositoryMovement = repoMoveStock;
            this.repositoryEmp = repoEmp;
            // Mapper
            this.mapper = map;
            // Helper
            this.helpers = new HelpersClass<RequisitionMaster>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }
        #endregion Constructor

        #region GET

        // GET: api/RequisitionMaster
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);

            //var Includes = new List<string> { "RequisitionMaster" };
            //return new JsonResult(this.ConvertTable.ConverterTableToViewModel<RequisitionMasterViewModel, RequisitionMaster>
            //                     (await this.repository.GetAllWithInclude2Async(Includes)),
            //                      this.DefaultJsonSettings);
        }

        // GET: api/RequisitionMaster/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);

            var Includes = new List<string> { "ColorItem" };

            var requisitionMaster = this.mapper.Map<RequisitionMaster, RequisitionMasterViewModel>
                            (await this.repository.GetAsynvWithIncludes(key, "RequisitionMasterId", Includes));

            if (requisitionMaster != null)
                requisitionMaster.RequisitionByString = (await this.repositoryEmp.GetAsync(requisitionMaster.RequisitionBy))?.NameThai ?? "-";

            return new JsonResult(requisitionMaster, this.DefaultJsonSettings);
        }

        // GET: api/PaintTaskDetail/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.PaintTaskDetailId == MasterId)
                                .Include(x => x.ColorItem);

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<RequisitionMasterViewModel, RequisitionMaster>
                                    (await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST
        // POST: api/RequisitionMaster/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                            .Include(x => x.ColorItem)
                                            .AsQueryable();

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.ColorItem.ColorName.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "RequisitionDate":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.RequisitionDate);
                    else
                        QueryData = QueryData.OrderBy(e => e.RequisitionDate);
                    break;

                case "ColorNameString":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.ColorItem.ColorName);
                    else
                        QueryData = QueryData.OrderBy(e => e.ColorItem.ColorName);
                    break;

                case "Quantity":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Quantity);
                    else
                        QueryData = QueryData.OrderBy(e => e.Quantity);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.RequisitionDate);
                    break;
            }

            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<RequisitionMasterViewModel>(Scroll,
                this.ConvertTable.ConverterTableToViewModel<RequisitionMasterViewModel, RequisitionMaster>
                (await QueryData.AsNoTracking().ToListAsync())),
                this.DefaultJsonSettings);
        }

        // POST: api/RequisitionMaster
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RequisitionMaster nRequisitionMaster)
        {
            if (nRequisitionMaster != null)
            {
                nRequisitionMaster = this.helpers.AddHourMethod(nRequisitionMaster);

                nRequisitionMaster.CreateDate = DateTime.Now;
                nRequisitionMaster.Creator = nRequisitionMaster.Creator ?? "Someone";
                // null for FK
                nRequisitionMaster.ColorItem = null;

                if (nRequisitionMaster.ColorMovementStock == null)
                {
                    var StockStatus = await this.repositoryMoveStatus.GetAllAsQueryable()
                                          .FirstOrDefaultAsync(x => x.StatusMovement == StatusMovement.Requsition);
                    if (StockStatus != null)
                    {
                        nRequisitionMaster.ColorMovementStock = new ColorMovementStock()
                        {
                            MovementStockDate = nRequisitionMaster.RequisitionDate.Value,
                            Quantity = nRequisitionMaster.Quantity.Value,
                            ColorItemId = nRequisitionMaster.ColorItemId,
                            CreateDate = nRequisitionMaster.CreateDate,
                            Creator = nRequisitionMaster.Creator,
                            MovementStockStatusId = StockStatus.MovementStockStatusId,
                        };
                    }
                }

                return new JsonResult(await this.repository.AddAsync(nRequisitionMaster), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found requisition master data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/RequisitionMaster/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]RequisitionMaster uRequisitionMaster)
        {
            var Message = "Requisition master not been found.";

            if (uRequisitionMaster != null)
            {
                uRequisitionMaster = this.helpers.AddHourMethod(uRequisitionMaster);
                // set modified
                uRequisitionMaster.ModifyDate = DateTime.Now;
                uRequisitionMaster.Modifyer = uRequisitionMaster.Modifyer ?? "Someone";
                // null for FK
                uRequisitionMaster.ColorItem = null;
                uRequisitionMaster.PaintTaskDetail = null;
                uRequisitionMaster.ColorMovementStock = null;

                var updateRequisition = await this.repository.UpdateAsync(uRequisitionMaster, key);

                if (updateRequisition != null && updateRequisition.ColorMovementStockId.HasValue)
                {
                    var updateMovementStock = await this.repositoryMovement.GetAllAsQueryable()
                                                        .FirstOrDefaultAsync(x => x.ColortMovementStockId == updateRequisition.ColorMovementStockId);

                    updateMovementStock.Quantity = updateRequisition.Quantity ?? updateMovementStock.Quantity;
                    updateMovementStock.MovementStockDate = updateRequisition.RequisitionDate ?? updateMovementStock.MovementStockDate;
                    updateMovementStock.ModifyDate = updateRequisition.ModifyDate;
                    updateMovementStock.Modifyer = updateRequisition.Modifyer;

                    await this.repositoryMovement.UpdateAsync(updateMovementStock,updateMovementStock.ColortMovementStockId);
                }

                return new JsonResult(updateRequisition, this.DefaultJsonSettings);
            }
            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/RequisitionMaster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
