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
    [Route("api/ColorMovementStock")]
    public class ColorMovementStockController : Controller
    {
        #region PrivateMembers
        // Repository
        private IRepositoryPainting<ColorMovementStock> repository;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        #endregion PrivateMenbers

        #region Constructor
        public ColorMovementStockController(IRepositoryPainting<ColorMovementStock> repo, IMapper map)
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

        // GET: api/ColorMovementStock
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);

            //var Includes = new List<string> { "ColorMovementStock" };
            //return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastRoomViewModel, ColorMovementStock>
            //                     (await this.repository.GetAllWithInclude2Async(Includes)),
            //                      this.DefaultJsonSettings);
        }

        // GET: api/ColorMovementStock/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);

            var Includes = new List<string> { "MovementStockStatus","ColorItem" };
            return new JsonResult(this.mapper.Map<ColorMovementStock, ColorMovementStockViewModel>
                                 (await this.repository.GetAsynvWithIncludes(key, "ColortMovementStockId", Includes)),
                                  this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST
        // POST: api/ColorMovementStock/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                            .Include(x => x.MovementStockStatus)
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
                case "MovementStockDate":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.MovementStockDate);
                    else
                        QueryData = QueryData.OrderBy(e => e.MovementStockDate);
                    break;

                case "ColorNameString":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.ColorItem.ColorName);
                    else
                        QueryData = QueryData.OrderBy(e => e.ColorItem.ColorName);
                    break;

                case "StatusNameString":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.MovementStockStatus.StatusName);
                    else
                        QueryData = QueryData.OrderBy(e => e.MovementStockStatus.StatusName);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.MovementStockDate);
                    break;
            }

            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<ColorMovementStockViewModel>(Scroll,
                this.ConvertTable.ConverterTableToViewModel<ColorMovementStockViewModel,ColorMovementStock>
                (await QueryData.AsNoTracking().ToListAsync())), 
                this.DefaultJsonSettings);
        }

        // POST: api/ColorMovementStock
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ColorMovementStock nColorMovementStock)
        {
            if (nColorMovementStock != null)
            {
                nColorMovementStock.CreateDate = DateTime.Now;
                nColorMovementStock.Creator = nColorMovementStock.Creator ?? "Someone";
                // null for FK
                nColorMovementStock.ColorItem = null;
                nColorMovementStock.MovementStockStatus = null;

                return new JsonResult(await this.repository.AddAsync(nColorMovementStock), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found color movement stock data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/ColorMovementStock/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]ColorMovementStock uColorMovementStock)
        {
            var Message = "Color movement stock not been found.";

            if (uColorMovementStock != null)
            {
                // set modified
                uColorMovementStock.ModifyDate = DateTime.Now;
                uColorMovementStock.Modifyer = uColorMovementStock.Modifyer ?? "Someone";
                // null for FK
                uColorMovementStock.ColorItem = null;
                uColorMovementStock.MovementStockStatus = null;

                return new JsonResult(await this.repository.UpdateAsync(uColorMovementStock, key), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/ColorMovementStock/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
