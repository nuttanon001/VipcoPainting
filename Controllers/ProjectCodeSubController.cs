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
    [Route("api/ProjectCodeSub")]
    public class ProjectCodeSubController : Controller
    {
        #region PrivateMenbers

        private IRepositoryPainting<ProjectCodeSub> repository;
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<ProjectCodeSub> helpers;

        #endregion PrivateMenbers

        #region Constructor

        public ProjectCodeSubController(IRepositoryPainting<ProjectCodeSub> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
            this.helpers = new HelpersClass<ProjectCodeSub>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/ProjectCodeSub
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "ProjectCodeMaster" };
            return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),
                                        this.DefaultJsonSettings);
        }

        // GET: api/ProjectCodeSub/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "ProjectCodeMaster" };
            return new JsonResult(await this.repository.GetAsynvWithIncludes(key, "ProjectCodeSubId", Includes),
                                        this.DefaultJsonSettings);
        }

        // GET: api/ProjectCodeSub/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.ProjectCodeMasterId == MasterId)
                                .Include(x => x.ProjectCodeMaster);

            return new JsonResult(await QueryData.AsNoTracking().ToListAsync(), this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST

        // POST: api/ProjectCodeSub/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable().AsQueryable();
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.Code.ToLower().Contains(keyword) ||
                                                 x.Name.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "Code":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Code);
                    else
                        QueryData = QueryData.OrderBy(e => e.Code);
                    break;

                case "Name":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Name);
                    else
                        QueryData = QueryData.OrderBy(e => e.Name);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.CreateDate);
                    break;
            }

            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<ProjectCodeSub>(Scroll, await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }

        // POST: api/ProjectCodeSub
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ProjectCodeSub nProjectCodeSub)
        {
            if (nProjectCodeSub != null)
            {
                nProjectCodeSub = helpers.AddHourMethod(nProjectCodeSub);

                nProjectCodeSub.CreateDate = DateTime.Now;
                nProjectCodeSub.Creator = nProjectCodeSub.Creator ?? "Someone";

                if (nProjectCodeSub.ProjectCodeMaster != null)
                    nProjectCodeSub.ProjectCodeMaster = null;

                if (nProjectCodeSub.ProjectSubParent != null)
                    nProjectCodeSub.ProjectSubParent = null;

                return new JsonResult(await this.repository.AddAsync(nProjectCodeSub), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found ProjectCodeSub data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/ProjectCodeSub/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]ProjectCodeSub uProjectCodeSub)
        {
            var Message = "Not found ProjectCodeSub data.";
            try
            {
                if (uProjectCodeSub != null)
                {
                    // add hour to DateTime to set Asia/Bangkok
                    uProjectCodeSub = helpers.AddHourMethod(uProjectCodeSub);

                    uProjectCodeSub.ModifyDate = DateTime.Now;
                    uProjectCodeSub.Modifyer = uProjectCodeSub.Modifyer ?? "Someone";

                    if (uProjectCodeSub.ProjectCodeMaster != null)
                        uProjectCodeSub.ProjectCodeMaster = null;

                    if (uProjectCodeSub.ProjectSubParent != null)
                        uProjectCodeSub.ProjectSubParent = null;

                    return new JsonResult(await this.repository.UpdateAsync(uProjectCodeSub, key), this.DefaultJsonSettings);
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

        // DELETE: api/ProjectCodeSub/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
