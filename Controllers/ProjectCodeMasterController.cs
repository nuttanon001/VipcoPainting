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
using System.Linq.Expressions;

namespace VipcoPainting.Controllers
{
    [Produces("application/json")]
    [Route("api/ProjectCodeMaster")]
    public class ProjectCodeMasterController : Controller
    {
        #region PrivateMenbers

        private IRepositoryMachine<ProjectCodeMaster> repository;
        private IRepositoryPainting<ProjectCodeSub> repositorySub;
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<ProjectCodeMaster> helpers;

        #endregion PrivateMenbers

        #region Constructor

        public ProjectCodeMasterController(
            IRepositoryMachine<ProjectCodeMaster> repo,
            IRepositoryPainting<ProjectCodeSub> repoSub, IMapper map)
        {
            this.repository = repo;
            this.repositorySub = repoSub;
            this.mapper = map;
            this.helpers = new HelpersClass<ProjectCodeMaster>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/ProjectCodeMaster
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            // var Includes = new List<string> { "" };
            // return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),
            //                            this.DefaultJsonSettings);
        }

        // GET: api/ProjectCodeMaster/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            // var Includes = new List<string> { "" };
            // return new JsonResult(await this.repository.GetAsynvWithIncludes(key, "ProjectCodeMasterId", Includes),
            //                            this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST

        // POST: api/ProjectCodeMaster/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable().AsQueryable();
            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                             : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.ProjectCode.ToLower().Contains(keyword) ||
                                                 x.ProjectName.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "ProjectCode":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.ProjectCode);
                    else
                        QueryData = QueryData.OrderBy(e => e.ProjectCode);
                    break;

                case "ProjectName":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.ProjectName);
                    else
                        QueryData = QueryData.OrderBy(e => e.ProjectName);
                    break;

                case "StartDate":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.StartDate);
                    else
                        QueryData = QueryData.OrderBy(e => e.StartDate);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.ProjectCode);
                    break;
            }

            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<ProjectCodeMaster>
                (Scroll, await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }

        // POST: api/ProjectCodeMaster
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ProjectMasterViewModel nProjectCodeMasterVM)
        {
            if (nProjectCodeMasterVM != null)
            {
                var nProjectCodeMaster = this.mapper.Map<ProjectMasterViewModel, ProjectCodeMaster>(nProjectCodeMasterVM);

                // add hour to DateTime to set Asia/Bangkok
                nProjectCodeMaster = helpers.AddHourMethod(nProjectCodeMaster);

                nProjectCodeMaster.CreateDate = DateTime.Now;
                nProjectCodeMaster.Creator = nProjectCodeMaster.Creator ?? "Someone";

                var insertComplate = await this.repository.AddAsync(nProjectCodeMaster);
                if (insertComplate != null)
                {
                    if (nProjectCodeMasterVM.ProjectSubs != null)
                    {
                        foreach (var nDetail in nProjectCodeMasterVM.ProjectSubs)
                        {
                            nDetail.CreateDate = nProjectCodeMaster.CreateDate;
                            nDetail.Creator = nProjectCodeMaster.Creator;
                            nDetail.ProjectCodeMasterId = insertComplate.ProjectCodeMasterId;
                            nDetail.ProjectSubStatus = ProjectSubStatus.Use;

                            await this.repositorySub.AddAsync(nDetail);
                        }
                    }
                }
                return new JsonResult(insertComplate, this.DefaultJsonSettings);
            }

            return NotFound(new { Error = "ProjectMaster not found. " });
        }

        #endregion POST

        #region PUT

        // PUT: api/ProjectCodeMaster/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]ProjectMasterViewModel uProjectCodeMasterVm)
        {
            var Message = "Job can't update";
            try
            {
                if (uProjectCodeMasterVm != null)
                {
                    var uProjectCodeMaster = this.mapper.Map<ProjectMasterViewModel, ProjectCodeMaster>(uProjectCodeMasterVm);

                    // add hour to DateTime to set Asia/Bangkok
                    uProjectCodeMaster = helpers.AddHourMethod(uProjectCodeMaster);
                    // set modified
                    uProjectCodeMaster.ModifyDate = DateTime.Now;
                    uProjectCodeMaster.Modifyer = uProjectCodeMaster.Modifyer ?? "Someone";

                    // update Master not update Detail it need to update Detail directly
                    var updateComplate = await this.repository.UpdateAsync(uProjectCodeMaster, key);

                    if (updateComplate != null)
                    {
                        // filter
                        Expression<Func<ProjectCodeSub, bool>> condition = m => m.ProjectCodeMasterId == key;
                        var dbProjectSubs = this.repositorySub.FindAll(condition);

                        //Remove ProjectCodeDetails if edit remove it
                        foreach (var dbDetail in dbProjectSubs)
                        {
                            try
                            {
                                if (!uProjectCodeMasterVm.ProjectSubs.Any(x => x.ProjectCodeSubId == dbDetail.ProjectCodeSubId))
                                {
                                    dbDetail.ProjectSubStatus = ProjectSubStatus.NotUse;
                                    await this.repositorySub.UpdateAsync(dbDetail,dbDetail.ProjectCodeSubId);
                                }
                            }
                            catch (Exception ex)
                            {
                                Message = ex.ToString();
                            }

                        }
                        //Update ProjectCodeDetails
                        foreach (var uProjectSub in uProjectCodeMasterVm.ProjectSubs)
                        {
                            // Set Value
                            uProjectSub.ProjectSubStatus = uProjectSub.ProjectSubStatus ?? ProjectSubStatus.Use;

                            if (uProjectSub.ProjectCodeSubId > 0)
                            {
                                uProjectSub.ModifyDate = uProjectCodeMaster.ModifyDate;
                                uProjectSub.Modifyer = uProjectCodeMaster.Modifyer;

                                await this.repositorySub.UpdateAsync(uProjectSub, uProjectSub.ProjectCodeSubId);
                            }
                            else
                            {
                                if (uProjectSub.ProjectCodeMasterId < 1 || uProjectSub.ProjectCodeMasterId == null)
                                    uProjectSub.ProjectCodeMasterId = updateComplate.ProjectCodeMasterId;

                                uProjectSub.CreateDate = uProjectCodeMaster.ModifyDate;
                                uProjectSub.Creator = uProjectCodeMaster.Modifyer;

                                await this.repositorySub.AddAsync(uProjectSub);
                            }
                        }
                    }

                    return new JsonResult(updateComplate, this.DefaultJsonSettings);
                }
            }
            catch(Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }
            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/ProjectCodeMaster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
