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
    [Route("api/MovementStockStatus")]
    public class MovementStockStatusController : Controller
    {
        #region PrivateMembers

        // Repository
        private IRepositoryPainting<MovementStockStatus> repository;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;

        #endregion PrivateMembers

        #region Constructor
        public MovementStockStatusController(IRepositoryPainting<MovementStockStatus> repo, IMapper map)
        {
            // Repository
            this.repository = repo;
            // Mapper
            this.mapper = map;
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }
        #endregion Constructor

        #region GET

        // GET: api/MovementStockStatus
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);

            //var Includes = new List<string> { "MovementStockStatus" };
            //return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastRoomViewModel, MovementStockStatus>
            //                     (await this.repository.GetAllWithInclude2Async(Includes)),
            //                      this.DefaultJsonSettings);
        }

        // GET: api/MovementStockStatus/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);

            //var Includes = new List<string> { "ColorItem" };
            //return new JsonResult(this.mapper.Map<MovementStockStatus, FinishedGoodsMasterViewModel>
            //                     (await this.repository.GetAsynvWithIncludes(key, "FinishedGoodsMasterId", Includes)),
            //                      this.DefaultJsonSettings);
        }
        #endregion GET

        #region POST
        // POST: api/MovementStockStatus/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable().AsQueryable();
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.StatusName.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "StatusName":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.StatusName);
                    else
                        QueryData = QueryData.OrderBy(e => e.StatusName);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.CreateDate);
                    break;
            }

            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<MovementStockStatus>
                (Scroll,await QueryData.AsNoTracking().ToListAsync()),
                this.DefaultJsonSettings);
        }

        // POST: api/MovementStockStatus
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MovementStockStatus nMovementStockStatus)
        {
            if (nMovementStockStatus != null)
            {
                nMovementStockStatus.CreateDate = DateTime.Now;
                nMovementStockStatus.Creator = nMovementStockStatus.Creator ?? "Someone";
                // null for FK

                return new JsonResult(await this.repository.AddAsync(nMovementStockStatus), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found movement stock status data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/MovementStockStatus/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]MovementStockStatus uMovementStockStatus)
        {
            var Message = "Movement stock status not been found.";

            if (uMovementStockStatus != null)
            {
                // set modified
                uMovementStockStatus.ModifyDate = DateTime.Now;
                uMovementStockStatus.Modifyer = uMovementStockStatus.Modifyer ?? "Someone";
                // null for FK

                return new JsonResult(await this.repository.UpdateAsync(uMovementStockStatus, key), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/MovementStockStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
