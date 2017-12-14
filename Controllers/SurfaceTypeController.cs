using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using VipcoPainting.Helpers;
using VipcoPainting.Models;
using VipcoPainting.ViewModels;
using VipcoPainting.Services.Interfaces;

namespace VipcoPainting.Controllers
{
    [Produces("application/json")]
    [Route("api/SurfaceType")]
    public class SurfaceTypeController : Controller
    {
        #region PrivateMenbers

        private IRepositoryPainting<SurfaceType> repository;
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<SurfaceType> helpers;

        #endregion PrivateMenbers

        #region Constructor

        public SurfaceTypeController(IRepositoryPainting<SurfaceType> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
            this.helpers = new HelpersClass<SurfaceType>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/SurfaceType
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "" };
            return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),
                                        this.DefaultJsonSettings);
        }

        // GET: api/SurfaceType/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "" };
            return new JsonResult(await this.repository.GetAsynvWithIncludes(key, "SurfaceTypeId", Includes),
                                        this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST

        // POST: api/SurfaceType/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable().AsQueryable();
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.SurfaceCode.ToLower().Contains(keyword) ||
                                                 x.SurfaceName.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "SurfaceCode":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.SurfaceCode);
                    else
                        QueryData = QueryData.OrderBy(e => e.SurfaceCode);
                    break;

                case "SurfaceName":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.SurfaceName);
                    else
                        QueryData = QueryData.OrderBy(e => e.SurfaceName);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.CreateDate);
                    break;
            }

            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<SurfaceType>(Scroll, await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }

        // POST: api/SurfaceType
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SurfaceType nSurfaceType)
        {
            if (nSurfaceType != null)
            {
                nSurfaceType = helpers.AddHourMethod(nSurfaceType);

                nSurfaceType.CreateDate = DateTime.Now;
                nSurfaceType.Creator = nSurfaceType.Creator ?? "Someone";

                return new JsonResult(await this.repository.AddAsync(nSurfaceType), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found SurfaceType data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/SurfaceType/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]SurfaceType uSurfaceType)
        {
            var Message = "Not found SurfaceType data.";
            try
            {
                if (uSurfaceType != null)
                {
                    // add hour to DateTime to set Asia/Bangkok
                    uSurfaceType = helpers.AddHourMethod(uSurfaceType);

                    uSurfaceType.ModifyDate = DateTime.Now;
                    uSurfaceType.Modifyer = uSurfaceType.Modifyer ?? "Someone";

                    var UpdateData = await this.repository.UpdateAsync(uSurfaceType, key);
                    if (UpdateData != null)
                        return new JsonResult(UpdateData, this.DefaultJsonSettings);
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

        // DELETE: api/SurfaceType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
