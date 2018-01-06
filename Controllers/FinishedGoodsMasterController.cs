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
    [Route("api/FinishedGoodsMaster")]
    public class FinishedGoodsMasterController : Controller
    {
        #region PrivateMembers

        // Repository
        private IRepositoryPainting<FinishedGoodsMaster> repository;
        private IRepositoryPainting<MovementStockStatus> repositoryMoveStatus;
        private IRepositoryPainting<ColorMovementStock> repositoryMovement;
        private IRepositoryMachine<Employee> repositoryEmp;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        // Helper
        private HelpersClass<FinishedGoodsMaster> helpers;
        #endregion PrivateMembers

        #region Constructor
        public FinishedGoodsMasterController(
            IRepositoryPainting<FinishedGoodsMaster> repo,
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
            this.helpers = new HelpersClass<FinishedGoodsMaster>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }
        #endregion Constructor

        #region GET

        // GET: api/FinishedGoodsMaster
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);

            //var Includes = new List<string> { "FinishedGoodsMaster" };
            //return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastRoomViewModel, FinishedGoodsMaster>
            //                     (await this.repository.GetAllWithInclude2Async(Includes)),
            //                      this.DefaultJsonSettings);
        }

        // GET: api/FinishedGoodsMaster/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);

            var Includes = new List<string> { "ColorItem" };
            var finishedGoodsMaster = this.mapper.Map<FinishedGoodsMaster, FinishedGoodsMasterViewModel>
                            (await this.repository.GetAsynvWithIncludes(key, "FinishedGoodsMasterId", Includes));

            if (finishedGoodsMaster != null)
                finishedGoodsMaster.ReceiveByString = (await this.repositoryEmp.GetAsync(finishedGoodsMaster.ReceiveBy))?.NameThai ?? "-";

            return new JsonResult(finishedGoodsMaster, this.DefaultJsonSettings);
        }
        #endregion GET

        #region POST
        // POST: api/FinishedGoodsMaster/GetScroll
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
                case "FinishedGoodsDate":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.FinishedGoodsDate);
                    else
                        QueryData = QueryData.OrderBy(e => e.FinishedGoodsDate);
                    break;
                case "Quantity":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Quantity);
                    else
                        QueryData = QueryData.OrderBy(e => e.Quantity);
                    break;
                case "ColorNameString":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.ColorItem.ColorName);
                    else
                        QueryData = QueryData.OrderBy(e => e.ColorItem.ColorName);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.FinishedGoodsDate);
                    break;
            }

            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<FinishedGoodsMasterViewModel>(Scroll,
                this.ConvertTable.ConverterTableToViewModel<FinishedGoodsMasterViewModel, FinishedGoodsMaster>
                (await QueryData.AsNoTracking().ToListAsync())),
                this.DefaultJsonSettings);
        }

        // POST: api/FinishedGoodsMaster
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]FinishedGoodsMaster nFinishedGoodsMaster)
        {
            if (nFinishedGoodsMaster != null)
            {
                nFinishedGoodsMaster = this.helpers.AddHourMethod(nFinishedGoodsMaster);

                nFinishedGoodsMaster.CreateDate = DateTime.Now;
                nFinishedGoodsMaster.Creator = nFinishedGoodsMaster.Creator ?? "Someone";
                // null for FK
                nFinishedGoodsMaster.ColorItem = null;

                if (nFinishedGoodsMaster.ColorMovementStock == null)
                {
                    var StockStatus = await this.repositoryMoveStatus.GetAllAsQueryable()
                                          .FirstOrDefaultAsync(x => x.StatusMovement == StatusMovement.Stock);
                    if (StockStatus != null)
                    {
                        nFinishedGoodsMaster.ColorMovementStock = new ColorMovementStock()
                        {
                            MovementStockDate = nFinishedGoodsMaster.FinishedGoodsDate,
                            Quantity = nFinishedGoodsMaster.Quantity.Value,
                            ColorItemId = nFinishedGoodsMaster.ColorItemId,
                            CreateDate = nFinishedGoodsMaster.CreateDate,
                            Creator = nFinishedGoodsMaster.Creator,
                            MovementStockStatusId = StockStatus.MovementStockStatusId,
                        };
                    }
                }

                return new JsonResult(await this.repository.AddAsync(nFinishedGoodsMaster), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found finished goods master data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/FinishedGoodsMaster/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]FinishedGoodsMaster uFinishedGoodsMaster)
        {
            var Message = "Finished goods master not been found.";

            if (uFinishedGoodsMaster != null)
            {
                uFinishedGoodsMaster = this.helpers.AddHourMethod(uFinishedGoodsMaster);
                // set modified
                uFinishedGoodsMaster.ModifyDate = DateTime.Now;
                uFinishedGoodsMaster.Modifyer = uFinishedGoodsMaster.Modifyer ?? "Someone";
                // null for FK
                uFinishedGoodsMaster.ColorItem = null;
                uFinishedGoodsMaster.ColorMovementStock = null;

                var updateFG = await this.repository.UpdateAsync(uFinishedGoodsMaster, key);

                if (updateFG != null && updateFG.ColorMovementStockId.HasValue)
                {
                    var updateMovementStock = await this.repositoryMovement.GetAllAsQueryable()
                                                        .FirstOrDefaultAsync(x => x.ColortMovementStockId == updateFG.ColorMovementStockId);

                    updateMovementStock.Quantity = updateFG.Quantity ?? updateMovementStock.Quantity;
                    updateMovementStock.MovementStockDate = updateFG.FinishedGoodsDate;
                    updateMovementStock.ModifyDate = updateFG.ModifyDate;
                    updateMovementStock.Modifyer = updateFG.Modifyer;

                    await this.repositoryMovement.UpdateAsync(updateMovementStock, updateMovementStock.ColortMovementStockId);
                }

                return new JsonResult(updateFG, this.DefaultJsonSettings);
            }
            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/FinishedGoodsMaster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
