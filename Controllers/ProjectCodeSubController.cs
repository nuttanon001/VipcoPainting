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
        private IRepositoryMachine<ProjectCodeDetail> repositoryDetail;
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<ProjectCodeSub> helpers;

        #endregion PrivateMenbers

        #region Constructor

        public ProjectCodeSubController(
            IRepositoryPainting<ProjectCodeSub> repo, 
            IRepositoryMachine<ProjectCodeDetail> repoDetail,
            IMapper map)
        {
            this.repository = repo;
            this.repositoryDetail = repoDetail;
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
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            //var Includes = new List<string> { "ProjectCodeMaster" };
            //return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),
            //                            this.DefaultJsonSettings);
        }

        // GET: api/ProjectCodeSub/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            //var Includes = new List<string> { "ProjectCodeMaster" };
            //return new JsonResult(await this.repository.GetAsynvWithIncludes(key, "ProjectCodeSubId", Includes),
            //                            this.DefaultJsonSettings);
        }

        // GET: api/ProjectCodeSub/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = await this.repository.GetAllAsQueryable()
                                        .Where(x => x.ProjectCodeMasterId == MasterId).ToListAsync();

            var QueryDetailMiss = await this.repositoryDetail.GetAllAsQueryable()
                                            .Where(x => x.ProjectCodeMasterId == MasterId &&
                                                        !QueryData.Select(z => z.Code).Contains(x.ProjectCodeDetailCode))
                                            .ToListAsync();
            
            if (QueryDetailMiss.Any())
            {
                foreach(var item in QueryDetailMiss)
                {
                    var nProjectCodeSub = new ProjectCodeSub
                    {
                        CreateDate = DateTime.Now,
                        Creator = "ByCoding",
                        ProjectCodeMasterId = MasterId,
                        Code = item.ProjectCodeDetailCode,
                        Name = item.Description,
                    };
                    await this.repository.AddAsync(nProjectCodeSub);
                }

                QueryData = await this.repository.GetAllAsQueryable()
                                        .Where(x => x.ProjectCodeMasterId == MasterId).ToListAsync();
            }

            return new JsonResult(QueryData, this.DefaultJsonSettings);
        }

        // GET: api/ProjectCodeSub/GetAutoComplate/
        [HttpGet("GetAutoComplate")]
        public async Task<IActionResult> GetAutoComplate()
        {
            var Message = "";
            try
            {
                var autoComplate = new List<string>();
                var projectSubs = await this.repository.GetAllAsync();
                if (projectSubs != null)
                {
                    foreach (var item in projectSubs.Select(x => x.Code)
                                                    .Distinct())
                    {
                        autoComplate.Add(item);
                    }
                }

                if (autoComplate.Any())
                    return new JsonResult(autoComplate, this.DefaultJsonSettings);
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }
            return NotFound(new { Error = Message });
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
                var temp = await this.repository.GetAllAsQueryable()
                                                .Where(x => x.ProjectCodeMasterId == nProjectCodeSub.ProjectCodeMasterId &&
                                                            x.Code.ToLower().Trim().Equals(nProjectCodeSub.Code.ToLower().Trim()))
                                                .FirstOrDefaultAsync();
                if (temp == null)
                {
                    nProjectCodeSub = helpers.AddHourMethod(nProjectCodeSub);

                    nProjectCodeSub.CreateDate = DateTime.Now;
                    nProjectCodeSub.Creator = nProjectCodeSub.Creator ?? "Someone";
                    // Trim
                    nProjectCodeSub.Code = nProjectCodeSub.Code.Trim();

                    //if (nProjectCodeSub.ProjectCodeMaster != null)
                    //    nProjectCodeSub.ProjectCodeMaster = null;

                    if (nProjectCodeSub.ProjectSubParent != null)
                        nProjectCodeSub.ProjectSubParent = null;

                    return new JsonResult(await this.repository.AddAsync(nProjectCodeSub), this.DefaultJsonSettings);
                }
                else
                {
                    return new JsonResult(temp, this.DefaultJsonSettings);
                }

                
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

                    //if (uProjectCodeSub.ProjectCodeMaster != null)
                    //    uProjectCodeSub.ProjectCodeMaster = null;

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
