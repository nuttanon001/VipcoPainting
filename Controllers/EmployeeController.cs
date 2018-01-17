using AutoMapper;
using Newtonsoft.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using VipcoPainting.Models;
using VipcoPainting.Helpers;
using VipcoPainting.ViewModels;
using VipcoPainting.Services.Interfaces;

namespace VipcoPainting.Controllers
{
    [Produces("application/json")]
    [Route("api/Employee")]
    public class EmployeeController : Controller
    {
        #region PrivateMenbers
        private IRepositoryMachine<Employee> repository;
        private IMapper mapper;
        private ConverterTableToVM ConvertTable;
        private JsonSerializerSettings DefaultJsonSettings;
        #endregion PrivateMenbers

        #region Constructor

        public EmployeeController(IRepositoryMachine<Employee> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion

        #region GET

        // GET: api/Employee/
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
        }

        // GET: api/Employee/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            return new JsonResult(
                this.mapper.Map<Employee, EmployeeViewModel>(await this.repository.GetAsync(key)),
                this.DefaultJsonSettings);
        }

        // GET: api/Employee/GetByMaster/5
        [HttpGet("GetByMaster/{MasterCode}")]
        public async Task<IActionResult> GetByMaster(string MasterCode)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                           .Where(x => x.GroupCode == MasterCode);
            // .Include(x => x.ProjectCodeDetail.ProjectCodeMaster)
            return new JsonResult(await QueryData.AsNoTracking().ToListAsync(), this.DefaultJsonSettings);
        }

        #endregion

        #region POST

        // POST: api/Employee/GetPage
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable();
            // Where
            if (!string.IsNullOrEmpty(Scroll.Where))
            {
                
            }
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);
            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.NameEng.ToLower().Contains(keyword) ||
                                                 x.NameThai.ToLower().Contains(keyword) ||
                                                 x.EmpCode.ToLower().Contains(keyword) ||
                                                 x.GroupCode.ToLower().Contains(keyword) ||
                                                 x.GroupName.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "EmpCode":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.EmpCode);
                    else
                        QueryData = QueryData.OrderBy(e => e.EmpCode);
                    break;

                case "NameThai":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.NameThai);
                    else
                        QueryData = QueryData.OrderBy(e => e.NameThai);
                    break;

                case "NameEng":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.NameEng);
                    else
                        QueryData = QueryData.OrderBy(e => e.NameEng);
                    break;

                default:
                    QueryData = QueryData.OrderBy(e => e.NameThai);
                    break;
            }

            // Skip and Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<Employee>
                (Scroll,
                 this.ConvertTable.ConverterTableToViewModel<EmployeeViewModel, Employee>(await QueryData.AsNoTracking().ToListAsync())),
                 this.DefaultJsonSettings);
        }
     
        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Employee nEmployee)
        {
            if (nEmployee != null)
            {
                return new JsonResult(await this.repository.AddAsync(nEmployee), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found employee." });
        }

        #endregion

        #region PUT
        // PUT: api/Employee/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByString(string key, [FromBody]Employee uEmployee)
        {
            if (uEmployee != null)
            {
                return new JsonResult(await this.repository.UpdateAsync(uEmployee, key), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found employee." });
        }

        #endregion

        #region DELETE
        // DELETE: api/Employee/5
        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            return new JsonResult(await this.repository.DeleteAsync(key), this.DefaultJsonSettings);
        }
        #endregion
    }
}
