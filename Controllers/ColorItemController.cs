using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using VipcoPainting.Helpers;
using VipcoPainting.Models;
using VipcoPainting.Services.Interfaces;
using VipcoPainting.ViewModels;

namespace VipcoPainting.Controllers
{
    [Produces("application/json")]
    [Route("api/ColorItem")]
    public class ColorItemController : Controller
    {
        #region PrivateMenbers

        private IRepositoryPainting<ColorItem> repository;
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<ColorItem> helpers;

        #endregion PrivateMenbers

        #region Constructor

        public ColorItemController(IRepositoryPainting<ColorItem> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
            this.helpers = new HelpersClass<ColorItem>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/ColorItem
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            // var Includes = new List<string> { "" };
            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<ColorItemViewModel,ColorItem>(await this.repository.GetAllAsync()),
                                        this.DefaultJsonSettings);
        }

        // GET: api/ColorItem/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            // var Includes = new List<string> { "" };
            return new JsonResult(this.mapper.Map<ColorItem, ColorItemViewModel>(await this.repository.GetAsynvWithIncludes(key, "ColorItemId")),
                                        this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST

        // POST: api/ColorItem/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable().AsQueryable();
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.ColorName.ToLower().Contains(keyword) ||
                                                 x.ColorCode.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "ColorName":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.ColorName);
                    else
                        QueryData = QueryData.OrderBy(e => e.ColorName);
                    break;

                case "ColorCode":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.ColorCode);
                    else
                        QueryData = QueryData.OrderBy(e => e.ColorCode);
                    break;

                case "VolumeSolids":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.VolumeSolids);
                    else
                        QueryData = QueryData.OrderBy(e => e.VolumeSolids);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.CreateDate);
                    break;
            }

            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<ColorItem>(Scroll,
                this.ConvertTable.ConverterTableToViewModel<ColorItemViewModel, ColorItem>( await QueryData.AsNoTracking().ToListAsync())),
                this.DefaultJsonSettings);
        }

        // POST: api/ColorItem
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ColorItem nColorItem)
        {
            if (nColorItem != null)
            {
                nColorItem = helpers.AddHourMethod(nColorItem);

                nColorItem.CreateDate = DateTime.Now;
                nColorItem.Creator = nColorItem.Creator ?? "Someone";

                var Runing = await this.repository.GetAllAsQueryable().CountAsync(x => x.CreateDate.Value.Year == nColorItem.CreateDate.Value.Year) + 1;
                nColorItem.ColorCode = $"{nColorItem.CreateDate.Value.ToString("yy")}/{Runing.ToString("0000")}";

                return new JsonResult(await this.repository.AddAsync(nColorItem), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found ColorItem data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/ColorItem/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]ColorItem uColorItem)
        {
            var Message = "Not found ColorItem data.";
            try
            {
                if (uColorItem != null)
                {
                    // add hour to DateTime to set Asia/Bangkok
                    uColorItem = helpers.AddHourMethod(uColorItem);

                    uColorItem.ModifyDate = DateTime.Now;
                    uColorItem.Modifyer = uColorItem.Modifyer ?? "Someone";

                    var UpdateData = await this.repository.UpdateAsync(uColorItem, key);
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

        // DELETE: api/ColorItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}