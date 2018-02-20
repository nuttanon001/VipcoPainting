using AutoMapper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using VipcoPainting.Models;
using VipcoPainting.Helpers;
using VipcoPainting.ViewModels;
using VipcoPainting.Services.Interfaces;
using System.Dynamic;

namespace VipcoPainting.Controllers
{
    [Produces("application/json")]
    [Route("api/InitialRequirePainting")]
    public class InitialRequirePaintingController : Controller
    {
        #region PrivateMenbers
        // Repository Painting
        private IRepositoryPainting<InitialRequirePaintingList> repository;
        private IRepositoryPainting<RequirePaintingMaster> repositroyRequireMaster;
        private IRepositoryPainting<BlastWorkItem> repositoryBlast;
        private IRepositoryPainting<PaintWorkItem> repositoryPaint;
        // Repository Machine
        private IRepositoryMachine<ProjectCodeMaster> repositoryProMaster;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;

        #endregion PrivateMenbers

        #region Constructor

        public InitialRequirePaintingController(
            IRepositoryPainting<InitialRequirePaintingList> repo, 
            IRepositoryPainting<RequirePaintingMaster> repoRequireMaster,
            IRepositoryPainting<BlastWorkItem> repoBlast,
            IRepositoryPainting<PaintWorkItem> repoPaint,
            IRepositoryMachine<ProjectCodeMaster> repoProMaster,
            IMapper map)
        {
            // Repository Paint
            this.repository = repo;
            this.repositroyRequireMaster = repoRequireMaster;
            this.repositoryBlast = repoBlast;
            this.repositoryPaint = repoPaint;
            // Repository Machine
            this.repositoryProMaster = repoProMaster;
            // Mapper
            this.mapper = map;
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/InitialRequirePainting
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "RequirePaintingMaster", "BlastWorkItems", "PaintWorkItems"};

            return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),this.DefaultJsonSettings);
        }

        // GET: api/InitialRequirePainting/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            var hasData = await this.repository.GetAsync(key);

            if (hasData != null)
            {
                var newData = this.mapper.Map<InitialRequirePaintingList, InitialRequirePaintingViewModel>(hasData);
                var paintWorks = await this.repositoryPaint.GetAllAsQueryable()
                                                            .Where(x => x.InitialRequireId == hasData.InitialRequireId &&
                                                                        x.RequirePaintingListId == null)
                                                            .AsNoTracking()
                                                            .ToListAsync();
                var blastWorks = await this.repositoryBlast.GetAllAsQueryable()
                                                            .Where(x => x.InitialRequireId == hasData.InitialRequireId &&
                                                                        x.RequirePaintingListId == null)
                                                            .AsNoTracking()
                                                            .ToListAsync();

                newData.BlastWorkItems = new List<BlastWorkItem>();
                newData.PaintWorkItems = new List<PaintWorkItem>();

                foreach (var paintWorkitem in paintWorks)
                    newData.PaintWorkItems.Add(paintWorkitem);
                foreach (var blastWorkitem in blastWorks)
                    newData.BlastWorkItems.Add(blastWorkitem);

                newData.NeedExternal = newData.BlastWorkItems.Any(x => x.StandradTimeExtId != null || x.SurfaceTypeExtId != null) || 
                                       newData.PaintWorkItems.Any(x => x.StandradTimeExtId != null || x.ExtColorItemId != null);

                newData.NeedInternal = newData.BlastWorkItems.Any(x => x.StandradTimeIntId != null || x.SurfaceTypeIntId != null) ||
                                       newData.PaintWorkItems.Any(x => x.StandradTimeIntId != null || x.IntColorItemId != null);

                return new JsonResult(newData, this.DefaultJsonSettings);
            }

            return NotFound(new { Error = "Data not been found." });
        }

        // GET: api/InitialRequirePainting/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.RequirePaintingMasterId == MasterId)
                                .Include(x => x.RequirePaintingMaster)
                                .Include(x => x.BlastWorkItems)
                                .Include(x => x.PaintWorkItems);

            var HasData = await QueryData.AsNoTracking().ToListAsync();

            return new JsonResult(HasData, this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST

        // POST: api/RequirePaintingMaster/RequirePaintSchedule
        [HttpPost("InitialRequirePaintSchedule")]
        public async Task<IActionResult> InitialRequirePaintSchedule([FromBody] OptionRequirePaintSchedule Scehdule)
        {
            string Message = "";
            try
            {
                var QueryData = this.repositroyRequireMaster.GetAllAsQueryable()
                                    .Where(x => x.InitialRequirePaintingList != null)
                                    .Include(x => x.ProjectCodeSub)
                                    .Include(x => x.InitialRequirePaintingList)
                                    .AsQueryable();
                int TotalRow;

                if (Scehdule != null)
                {
                    if (!string.IsNullOrEmpty(Scehdule.Filter))
                    {
                        // Filter
                        var filters = string.IsNullOrEmpty(Scehdule.Filter) ? new string[] { "" }
                                            : Scehdule.Filter.ToLower().Split(null);
                        foreach (var keyword in filters)
                        {
                            QueryData = QueryData.Where(x => x.RequireNo.ToLower().Contains(keyword) ||
                                                             x.PaintingSchedule.ToLower().Contains(keyword) ||
                                                             x.ProjectCodeSub.Code.ToLower().Contains(keyword));
                        }
                    }

                    // Option ProjectCodeMaster
                    if (Scehdule.ProjectId.HasValue)
                        QueryData = QueryData.Where(x => x.ProjectCodeSubId == Scehdule.ProjectId);

                    // Option Status
                    QueryData = QueryData.Where(x => x.RequirePaintingStatus != RequirePaintingStatus.Cancel);
                    // Get Total Row
                    TotalRow = await QueryData.CountAsync();

                    // Option Skip and Task
                    if (Scehdule.Skip.HasValue && Scehdule.Take.HasValue)
                        QueryData = QueryData.Skip(Scehdule.Skip ?? 0).Take(Scehdule.Take ?? 10);
                    else
                        QueryData = QueryData.Skip(0).Take(10);
                }
                else
                    TotalRow = await QueryData.CountAsync();

                // OrderBy
                QueryData = QueryData.OrderByDescending(x => x.RequireDate);

                var GetData = await QueryData.ToListAsync();
                if (GetData.Any())
                {
                    List<string> Columns = new List<string>();
                    foreach (var ProjectSub in GetData.GroupBy(x => x.ProjectCodeSub.Code))
                        Columns.Add(ProjectSub.Key);

                    var DataTable = new List<IDictionary<String, Object>>();

                    foreach (var Data in GetData.OrderBy(x => x.ProjectCodeSub.ProjectCodeMasterId))
                    {
                        var ProjectMaster = await this.repositoryProMaster.GetAsync(Data.ProjectCodeSub.ProjectCodeMasterId ?? 0);
                        // var JobNumber = $"{ProjectMaster?.ProjectCode ?? "No-Data"}/{(Data.ProjectCodeSub == null ? "No-Data" : Data.ProjectCodeSub.Code)}";
                        var JobNumber = $"{ProjectMaster?.ProjectCode ?? "No-Data"}";

                        IDictionary<String, Object> rowData;
                        bool update = false;
                        if (DataTable.Any(x => (string)x["JobNumber"] == JobNumber))
                        {
                            var FirstData = DataTable.FirstOrDefault(x => (string)x["JobNumber"] == JobNumber);
                            if (FirstData != null)
                            {
                                rowData = FirstData;
                                update = true;
                            }
                            else
                                rowData = new ExpandoObject();
                        }
                        else
                            rowData = new ExpandoObject();

                        if (Data.ProjectCodeSub != null)
                        {
                            //Get Employee Name
                            // var Employee = await this.repositoryEmp.GetAsync(Data.RequireEmp);
                            // var EmployeeReq = Employee != null ? $"คุณ{(Employee?.NameThai ?? "")}" : "No-Data";

                            var Key = Data.ProjectCodeSub.Code;
                            var Master = new RequirePaintingMasterViewModel()
                            {
                                RequirePaintingMasterId = Data.RequirePaintingMasterId,
                                InitialRequireId = Data?.InitialRequirePaintingList?.InitialRequireId ?? 0,
                                // RequireString = $"{EmployeeReq} | No.{Data.RequireNo}",
                                RequireString = $"No.{Data.RequireNo}",
                            };

                            if (rowData.Any(x => x.Key == Key))
                            {
                                // New Value
                                var ListMaster = (List<RequirePaintingMasterViewModel>)rowData[Key];
                                ListMaster.Add(Master);
                                // Add to rowData by key
                                rowData[Key] = ListMaster;
                            }
                            else // New to rowData
                                rowData.Add(Key, new List<RequirePaintingMasterViewModel>() { Master });
                        }

                        if (!update)
                        {
                            rowData.Add("JobNumber", JobNumber);
                            DataTable.Add(rowData);
                        }
                    }

                    return new JsonResult(new
                    {
                        TotalRow = TotalRow,
                        Columns = Columns,
                        DataTable = DataTable
                    }, this.DefaultJsonSettings);
                }

            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }

            return NotFound(new { Error = Message });
        }

        // POST: api/InitialRequirePainting
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]InitialRequirePaintingList nInitialRequirePainting)
        {
            var message = "Initial request painting list.";
            try
            {
                if (nInitialRequirePainting != null)
                {
                    nInitialRequirePainting.CreateDate = DateTime.Now;
                    nInitialRequirePainting.Creator = nInitialRequirePainting.Creator ?? "Someone";

                    if (nInitialRequirePainting.RequirePaintingMaster != null)
                        nInitialRequirePainting.RequirePaintingMaster = null;

                    // Remove null
                    nInitialRequirePainting.BlastWorkItems.Remove(null);
                    nInitialRequirePainting.PaintWorkItems.Remove(null);

                    if (nInitialRequirePainting.BlastWorkItems != null)
                    {
                        // Add BlastWork item
                        foreach (var nBlastWork in nInitialRequirePainting.BlastWorkItems)
                        {
                            if (nBlastWork == null)
                                continue;

                            nBlastWork.CreateDate = nInitialRequirePainting.CreateDate;
                            nBlastWork.Creator = nInitialRequirePainting.Creator;

                            // Clear StandradTimeExt
                            if (nBlastWork.StandradTimeExt != null)
                                nBlastWork.StandradTimeExt = null;

                            // Clear StandradTimeInt
                            if (nBlastWork.StandradTimeInt != null)
                                nBlastWork.StandradTimeInt = null;

                            // Clear SurfaceTypeExt
                            if (nBlastWork.SurfaceTypeExt != null)
                                nBlastWork.SurfaceTypeExt = null;

                            // Clear SurfaceTypeInt
                            if (nBlastWork.SurfaceTypeInt != null)
                                nBlastWork.SurfaceTypeInt = null;
                        }
                    }

                    if (nInitialRequirePainting.PaintWorkItems != null)
                    {
                        foreach (var nPaintWork in nInitialRequirePainting.PaintWorkItems)
                        {
                            if (nPaintWork == null)
                                continue;

                            nPaintWork.CreateDate = nInitialRequirePainting.CreateDate;
                            nPaintWork.Creator = nInitialRequirePainting.Creator;

                            // Clear StandradTimeExt
                            if (nPaintWork.StandradTimeExt != null)
                                nPaintWork.StandradTimeExt = null;

                            // Clear StandradTimeInt
                            if (nPaintWork.StandradTimeInt != null)
                                nPaintWork.StandradTimeInt = null;

                            // Clear SurfaceTypeExt
                            if (nPaintWork.ExtColorItem != null)
                                nPaintWork.ExtColorItem = null;

                            // Clear SurfaceTypeInt
                            if (nPaintWork.IntColorItem != null)
                                nPaintWork.IntColorItem = null;
                        }
                    }

                    return new JsonResult(await this.repository.AddAsync(nInitialRequirePainting), this.DefaultJsonSettings);
                }
            }
            catch(Exception ex)
            {
                message = $"Has error {ex.ToString()}";
            }
          
            return NotFound(new { Error = "Not found BlastWork data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/InitialRequirePainting/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]InitialRequirePaintingList uInitialRequirePainting)
        {
            var Message = "Require painting not been found.";

            if (uInitialRequirePainting != null)
            {
                // set modified
                uInitialRequirePainting.ModifyDate = DateTime.Now;
                uInitialRequirePainting.Modifyer = uInitialRequirePainting.Modifyer ?? "Someone";

                if (uInitialRequirePainting.RequirePaintingMaster != null)
                    uInitialRequirePainting.RequirePaintingMaster = null;

                // Remove null
                uInitialRequirePainting.BlastWorkItems.Remove(null);
                uInitialRequirePainting.PaintWorkItems.Remove(null);

                if (uInitialRequirePainting.BlastWorkItems != null)
                {
                    foreach (var uBlastWork in uInitialRequirePainting.BlastWorkItems)
                    {
                        if (uBlastWork == null)
                            continue;

                        if (uBlastWork.BlastWorkItemId > 0)
                        {
                            uBlastWork.ModifyDate = uInitialRequirePainting.ModifyDate;
                            uBlastWork.Modifyer = uInitialRequirePainting.Modifyer;
                        }
                        else
                        {
                            uBlastWork.CreateDate = uInitialRequirePainting.ModifyDate;
                            uBlastWork.Creator = uInitialRequirePainting.Modifyer;
                        }

                        uBlastWork.StandradTimeExt = null;
                        uBlastWork.StandradTimeInt = null;
                        uBlastWork.SurfaceTypeExt = null;
                        uBlastWork.SurfaceTypeInt = null;
                    }
                }

                if (uInitialRequirePainting.PaintWorkItems != null)
                {
                    foreach (var uPaintWork in uInitialRequirePainting.PaintWorkItems)
                    {
                        if (uPaintWork == null)
                            continue;

                        if (uPaintWork.PaintWorkItemId > 0)
                        {
                            uPaintWork.ModifyDate = uInitialRequirePainting.ModifyDate;
                            uPaintWork.Modifyer = uInitialRequirePainting.Modifyer;
                        }
                        else
                        {
                            uPaintWork.CreateDate = uInitialRequirePainting.ModifyDate;
                            uPaintWork.Creator = uInitialRequirePainting.Modifyer;
                        }

                        uPaintWork.StandradTimeExt = null;
                        uPaintWork.StandradTimeInt = null;
                        uPaintWork.ExtColorItem = null;
                        uPaintWork.IntColorItem = null;
                    }
                }

                // update Master not update Detail it need to update Detail directly
                var updateComplate = await this.repository.UpdateAsync(uInitialRequirePainting, uInitialRequirePainting.InitialRequireId);
                if (updateComplate != null)
                {
                    // filter
                    Expression<Func<BlastWorkItem, bool>> condition = m => m.InitialRequireId == updateComplate.InitialRequireId;
                    var dbBlastWorks = this.repositoryBlast.FindAll(condition);
                    Expression<Func<PaintWorkItem, bool>> condition2 = m => m.InitialRequireId == updateComplate.InitialRequireId;
                    var dbPaintWorks = this.repositoryPaint.FindAll(condition2);

                    //Remove BlastWork if edit remove it
                    foreach (var dbBlastWork in dbBlastWorks)
                    {
                        if (!uInitialRequirePainting.BlastWorkItems.Any(x => x.BlastWorkItemId == dbBlastWork.BlastWorkItemId))
                            await this.repositoryBlast.DeleteAsync(dbBlastWork.BlastWorkItemId);
                    }

                    //Remove PaintWork if edit remove it
                    foreach (var dbPaintWork in dbPaintWorks)
                    {
                        if (!uInitialRequirePainting.PaintWorkItems.Any(x => x.PaintWorkItemId == dbPaintWork.PaintWorkItemId))
                            await this.repositoryPaint.DeleteAsync(dbPaintWork.PaintWorkItemId);
                    }

                    //Update BlastWorkItem or New BlastWorkItem
                    foreach (var uBlastWork in uInitialRequirePainting.BlastWorkItems)
                    {
                        if (uBlastWork == null)
                            continue;

                        if (uBlastWork.BlastWorkItemId > 0)
                            await this.repositoryBlast.UpdateAsync(uBlastWork, uBlastWork.BlastWorkItemId);
                        else
                        {
                            if (uBlastWork.InitialRequireId is null || uBlastWork.InitialRequireId < 1)
                                uBlastWork.InitialRequireId = uInitialRequirePainting.InitialRequireId;

                            await this.repositoryBlast.AddAsync(uBlastWork);
                        }
                    }

                    //Update PaintWorkItem or New PaintWorkItem
                    foreach (var uPaintWork in uInitialRequirePainting.PaintWorkItems)
                    {
                        if (uPaintWork == null)
                            continue;

                        if (uPaintWork.PaintWorkItemId > 0)
                            await this.repositoryPaint.UpdateAsync(uPaintWork, uPaintWork.PaintWorkItemId);
                        else
                        {
                            if (uPaintWork.InitialRequireId is null || uPaintWork.InitialRequireId < 1)
                                uPaintWork.InitialRequireId = uInitialRequirePainting.InitialRequireId;

                            await this.repositoryPaint.AddAsync(uPaintWork);
                        }
                    }
                }

                return new JsonResult(await this.repository.UpdateAsync(uInitialRequirePainting, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/InitialRequirePainting/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
