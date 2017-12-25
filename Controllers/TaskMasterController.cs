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
    [Route("api/TaskMaster")]
    public class TaskMasterController : Controller
    {
        #region PrivateMembers
        // Repository
        private IRepositoryPainting<TaskMaster> repository;
        private IRepositoryPainting<TaskBlastDetail> repositoryTaskBlast;
        private IRepositoryPainting<TaskPaintDetail> repositoryTaskPaint;
        private IRepositoryMachine<Employee> repositoryEmp;
        private IRepositoryMachine<ProjectCodeMaster> repositoryProMaster;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<TaskMaster> helpers;

        private Models.TaskStatus CheckTaskStatus(TaskMaster taskMaster)
        {
            var Result = Models.TaskStatus.Cancel;

            if (taskMaster != null)
            {
                if (taskMaster.TaskProgress.HasValue)
                {
                    if (taskMaster.TaskProgress == 0)
                        Result = Models.TaskStatus.Waiting;
                    else
                        Result = taskMaster.TaskProgress >= 100 ? Models.TaskStatus.Complated : Models.TaskStatus.Tasking;
                }
                else
                    Result = Models.TaskStatus.Waiting;
            }
            return Result;
        }
        #endregion PrivateMenbers

        #region Constructor
        public TaskMasterController(
            IRepositoryPainting<TaskMaster> repo,
            IRepositoryPainting<TaskBlastDetail> repoTaskBlast,
            IRepositoryPainting<TaskPaintDetail> repoTaskPaint,
            IRepositoryMachine<ProjectCodeMaster> repoProMaster,
            IRepositoryMachine<Employee> repoEmp, IMapper map)
        {
            // Repository
            this.repository = repo;
            this.repositoryTaskBlast = repoTaskBlast;
            this.repositoryTaskPaint = repoTaskPaint;
            this.repositoryEmp = repoEmp;
            this.repositoryProMaster = repoProMaster;
            // Mapper
            this.mapper = map;
            this.helpers = new HelpersClass<TaskMaster>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/TaskMaster
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            //var Includes = new List<string> { "PaintTeam" };

            //return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastRoomViewModel, TaskMaster>
            //                     (await this.repository.GetAllWithInclude2Async(Includes)),
            //                      this.DefaultJsonSettings);
        }

        // GET: api/TaskMaster/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "RequirePaintingList.RequirePaintingMaster.ProjectCodeSub" };

            var taskMaster = this.mapper.Map<TaskMaster, TaskMasterViewModel>
                             (await this.repository.GetAsynvWithIncludes(key, "TaskMasterId", Includes));

            if (taskMaster != null)
            {
                if (!string.IsNullOrEmpty(taskMaster.AssignBy))
                {
                    taskMaster.AssignByString =
                        (await this.repositoryEmp.GetAsync(taskMaster.AssignBy))?.NameThai ?? "";
                }

                if (taskMaster?.RequirePaintingList?.RequirePaintingMaster?.ProjectCodeSub != null)
                {
                    taskMaster.ProjectCodeSubString =
                        (await this.repositoryProMaster.GetAsync(taskMaster.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub.ProjectCodeMasterId ?? 0))?.ProjectCode ?? "";

                    taskMaster.ProjectCodeSubString += $"/{taskMaster.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub.Code}";
                    taskMaster.RequirePaintingList = null;
                }
            }

            return new JsonResult(taskMaster, this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST
        // POST: api/TaskMaster/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                           .Include(x => x.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub)
                                           .AsQueryable();

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.TaskNo.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "TaskNo":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.TaskNo);
                    else
                        QueryData = QueryData.OrderBy(e => e.TaskNo);
                    break;

                case "AssignBy":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.AssignBy);
                    else
                        QueryData = QueryData.OrderBy(e => e.AssignBy);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.AssignDate);
                    break;
            }
            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);
            var DataViewModel = this.ConvertTable.ConverterTableToViewModel<TaskMasterViewModel, TaskMaster>
                                    (await QueryData.AsNoTracking().ToListAsync());

            if (DataViewModel.Any())
            {
                foreach(var item in DataViewModel)
                {
                    if (!string.IsNullOrEmpty(item.AssignBy))
                        item.AssignByString = (await this.repositoryEmp.GetAsync(item.AssignBy))?.NameThai ?? "-";

                    if (item?.RequirePaintingList?.RequirePaintingMaster?.ProjectCodeSub != null)
                    {
                        item.ProjectCodeSubString =
                            (await this.repositoryProMaster.GetAsync(item.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub.ProjectCodeMasterId ?? 0))?.ProjectCode ?? "";

                        item.ProjectCodeSubString += $"/{item.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub.Code}";
                        item.RequirePaintingList = null;
                    }
                }
            }

            return new JsonResult
                (new ScrollDataViewModel<TaskMasterViewModel>
                (Scroll, DataViewModel), this.DefaultJsonSettings);
        }

        // POST: api/TaskMaster
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TaskMaster nTaskMaster)
        {
            var Message = "TaskMaster not been found.";
            try
            {
                if (nTaskMaster != null)
                {
                    nTaskMaster = helpers.AddHourMethod(nTaskMaster);

                    nTaskMaster.CreateDate = DateTime.Now;
                    nTaskMaster.Creator = nTaskMaster.Creator ?? "Someone";

                    nTaskMaster.TaskStatus = this.CheckTaskStatus(nTaskMaster);

                    if (nTaskMaster.RequirePaintingList != null)
                        nTaskMaster.RequirePaintingList = null;

                    // Remove null
                    nTaskMaster.TaskBlastDetails.Remove(null);
                    nTaskMaster.TaskPaintDetails.Remove(null);

                    if (nTaskMaster.TaskBlastDetails != null)
                    {
                        // Add BlastWork item
                        foreach (var nTaskBlast in nTaskMaster.TaskBlastDetails)
                        {
                            if (nTaskBlast == null)
                                continue;

                            nTaskBlast.CreateDate = nTaskMaster.CreateDate;
                            nTaskBlast.Creator = nTaskMaster.Creator;

                            // Clear BlastRoom
                            if (nTaskBlast.BlastRoom != null)
                                nTaskBlast.BlastRoom = null;

                            // Clear BlastWorkItem
                            if (nTaskBlast.BlastWorkItem != null)
                                nTaskBlast.BlastWorkItem = null;
                        }
                    }

                    if (nTaskMaster.TaskPaintDetails != null)
                    {
                        foreach (var nTaskPaint in nTaskMaster.TaskPaintDetails)
                        {
                            if (nTaskPaint == null)
                                continue;

                            nTaskPaint.CreateDate = nTaskMaster.CreateDate;
                            nTaskPaint.Creator = nTaskMaster.Creator;

                            // Clear PaintTeam
                            if (nTaskPaint.PaintTeam != null)
                                nTaskPaint.PaintTeam = null;

                            // Clear PaintWorkItem
                            if (nTaskPaint.PaintWorkItem != null)
                                nTaskPaint.PaintWorkItem = null;
                        }
                    }

                    return new JsonResult(await this.repository.AddAsync(nTaskMaster), this.DefaultJsonSettings);
                }

            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }
            return NotFound(new { Error = Message });
        }

        #endregion POST

        #region PUT

        // PUT: api/TaskMaster/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]TaskMaster uTaskMaster)
        {
            var Message = "task master not been found.";
            // For Update
            if (uTaskMaster != null)
            {
                // add hour to DateTime to set Asia/Bangkok
                uTaskMaster = helpers.AddHourMethod(uTaskMaster);
                // set modified
                uTaskMaster.ModifyDate = DateTime.Now;
                uTaskMaster.Modifyer = uTaskMaster.Modifyer ?? "Someone";

                if (uTaskMaster.RequirePaintingList != null)
                    uTaskMaster.RequirePaintingList = null;

                uTaskMaster.TaskStatus = this.CheckTaskStatus(uTaskMaster);

                // Remove null
                uTaskMaster.TaskBlastDetails.Remove(null);
                uTaskMaster.TaskPaintDetails.Remove(null);

                if (uTaskMaster.TaskBlastDetails != null)
                {
                    foreach (var uTaskBlast in uTaskMaster.TaskBlastDetails)
                    {
                        if (uTaskBlast == null)
                            continue;

                        if (uTaskBlast.TaskBlastDetailId > 0)
                        {
                            uTaskBlast.ModifyDate = uTaskMaster.ModifyDate;
                            uTaskBlast.Modifyer = uTaskMaster.Modifyer;
                        }
                        else
                        {
                            uTaskBlast.CreateDate = uTaskMaster.ModifyDate;
                            uTaskBlast.Creator = uTaskMaster.Modifyer;
                        }

                        uTaskBlast.BlastWorkItem = null;
                        uTaskBlast.BlastRoom = null;
                    }
                }

                if (uTaskMaster.TaskPaintDetails != null)
                {
                    foreach (var uTaskPaint in uTaskMaster.TaskPaintDetails)
                    {
                        if (uTaskPaint == null)
                            continue;

                        if (uTaskPaint.TaskPaintDetailId > 0)
                        {
                            uTaskPaint.ModifyDate = uTaskMaster.ModifyDate;
                            uTaskPaint.Modifyer = uTaskMaster.Modifyer;
                        }
                        else
                        {
                            uTaskPaint.CreateDate = uTaskMaster.ModifyDate;
                            uTaskPaint.Creator = uTaskMaster.Modifyer;
                        }

                        uTaskPaint.PaintTeam = null;
                        uTaskPaint.PaintWorkItem = null;
                    }
                }

                // update Master not update Detail it need to update Detail directly
                var updateComplate = await this.repository.UpdateAsync(uTaskMaster, key);
                if (updateComplate != null)
                {
                    // filter
                    Expression<Func<TaskBlastDetail, bool>> conditionBlast = m => m.TaskMasterId == key;
                    var dbTaskBlasts = this.repositoryTaskBlast.FindAll(conditionBlast);
                    Expression<Func<TaskPaintDetail, bool>> conditionPaint = m => m.TaskMasterId == key;
                    var dbTaskPaints = this.repositoryTaskPaint.FindAll(conditionPaint);

                    //Remove TaskBlastDetail if edit remove it
                    foreach (var dbTaskBlast in dbTaskBlasts)
                    {
                        if (!uTaskMaster.TaskBlastDetails.Any(x => x.TaskBlastDetailId == dbTaskBlast.TaskBlastDetailId))
                            await this.repositoryTaskBlast.DeleteAsync(dbTaskBlast.TaskBlastDetailId);
                    }

                    //Remove TaskPaintDetail if edit remove it
                    foreach (var dbTaskPaint in dbTaskPaints)
                    {
                        if (!uTaskMaster.TaskPaintDetails.Any(x => x.TaskPaintDetailId == dbTaskPaint.TaskPaintDetailId))
                            await this.repositoryTaskPaint.DeleteAsync(dbTaskPaint.TaskPaintDetailId);
                    }

                    //Update TaskBlastDetail or New TaskBlastDetail
                    foreach (var uTaskBlast in uTaskMaster.TaskBlastDetails)
                    {
                        if (uTaskBlast == null)
                            continue;

                        if (uTaskBlast.TaskBlastDetailId > 0)
                            await this.repositoryTaskBlast.UpdateAsync(uTaskBlast, uTaskBlast.TaskBlastDetailId);
                        else
                        {
                            if (uTaskBlast.TaskMasterId is null || uTaskBlast.TaskMasterId < 1)
                                uTaskBlast.TaskMasterId = uTaskMaster.TaskMasterId;

                            await this.repositoryTaskBlast.AddAsync(uTaskBlast);
                        }
                    }

                    //Update TaskPaintDetail or New TaskPaintDetail
                    foreach (var uTaskPaint in uTaskMaster.TaskPaintDetails)
                    {
                        if (uTaskPaint == null)
                            continue;

                        if (uTaskPaint.TaskPaintDetailId > 0)
                            await this.repositoryTaskPaint.UpdateAsync(uTaskPaint, uTaskPaint.TaskPaintDetailId);
                        else
                        {
                            if (uTaskPaint.TaskMasterId is null || uTaskPaint.TaskMasterId < 1)
                                uTaskPaint.TaskMasterId = uTaskMaster.TaskMasterId;

                            await this.repositoryTaskPaint.AddAsync(uTaskPaint);
                        }
                    }
                }

                return new JsonResult(updateComplate, this.DefaultJsonSettings);

            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/TaskMaster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
