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
    [Route("api/EmployeeGroup")]
    public class EmployeeGroupController : Controller
    {
        #region PrivateMenbers

        private IRepositoryMachine<EmployeeGroup> repository;
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<EmployeeGroup> helpers;

        #endregion PrivateMenbers

        #region Constructor

        public EmployeeGroupController(IRepositoryMachine<EmployeeGroup> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
            this.helpers = new HelpersClass<EmployeeGroup>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/EmployeeGroup
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "" };
            return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),
                                        this.DefaultJsonSettings);
        }

        // GET: api/EmployeeGroup/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "" };
            return new JsonResult(await this.repository.GetAsync(key),this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST

        // POST: api/EmployeeGroup/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                           // .Where(x => GroupEmployee.Contains(x.GroupCode))
                                           .AsQueryable();

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);
            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.GroupCode.ToLower().Contains(keyword) ||
                                                 x.Description.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "GroupCode":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.GroupCode);
                    else
                        QueryData = QueryData.OrderBy(e => e.GroupCode);
                    break;

                case "Description":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Description);
                    else
                        QueryData = QueryData.OrderBy(e => e.Description);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.Description);
                    break;
            }

            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<EmployeeGroup>
                (Scroll, await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }

        // POST: api/EmployeeGroup
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EmployeeGroup nEmployeeGroup)
        {
            if (nEmployeeGroup != null)
            {
                return new JsonResult(await this.repository.AddAsync(nEmployeeGroup), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found EmployeeGroup data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/EmployeeGroup/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(string key, [FromBody]EmployeeGroup uEmployeeGroup)
        {
            var Message = "Not found EmployeeGroup data.";
            try
            {
                if (uEmployeeGroup != null)
                {
                    var UpdateData = await this.repository.UpdateAsync(uEmployeeGroup, key);
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

        // DELETE: api/EmployeeGroup/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
