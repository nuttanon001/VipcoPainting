using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using System;
using System.Linq;
using System.Dynamic;
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
    [Route("api/PaintTaskMaster")]
    public class PaintTaskMasterController : Controller
    {
        #region PrivateMembers
        // Repository
        private IRepositoryPainting<PaintTaskMaster> repository;
        private IRepositoryPainting<PaintTaskDetail> repositoryDetail;
        private IRepositoryMachine<Employee> repositoryEmp;
        private IRepositoryMachine<ProjectCodeMaster> repositoryProMaster;
        private IRepositoryPainting<RequirePaintingList> repositoryReqPaintingList;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<PaintTaskMaster> helpers;
        private HelpersClass<PaintTaskDetail> helpersDetail;

        private PaintTaskStatus CheckTaskStatus(PaintTaskMaster taskMaster)
        {
            var Result = PaintTaskStatus.Cancel;

            if (taskMaster != null)
            {
                if (taskMaster.MainProgress.HasValue)
                {
                    Result = taskMaster.MainProgress >= 100 ? PaintTaskStatus.Complated : PaintTaskStatus.Waiting;
                }
                else
                    Result = PaintTaskStatus.Waiting;
            }
            return Result;
        }
        private async Task<string> GeneratedCode(int RequirePaintingListId)
        {
            if (RequirePaintingListId > 0)
            {
                var reqPaintingList = await this.repositoryReqPaintingList.GetAllAsQueryable()
                                            .Where(x => x.RequirePaintingListId == RequirePaintingListId)
                                            .Include(x => x.RequirePaintingMaster)
                                            .AsNoTracking().FirstOrDefaultAsync();

                if (reqPaintingList != null)
                {
                    if (reqPaintingList.RequirePaintingMaster != null)
                    {
                        var RequireNo = reqPaintingList.RequirePaintingMaster.RequireNo;
                        var Runing = await this.repository.GetAllAsQueryable()
                                           .CountAsync(x => x.TaskPaintNo.StartsWith(RequireNo)) + 1;

                        return $"{RequireNo}-{Runing.ToString("000")}";
                    }
                }

                // return $"{proDetail.ProjectCodeMaster.ProjectCode}/{typeMachine.TypeMachineCode}/{Runing.ToString("0000")}";
            }

            return "xxxx/xx-xxx";
        }
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        private Func<DateTime, DateTime> ChangeTimeZone = d => d.AddHours(+7);

        #endregion PrivateMenbers

        #region Constructor
        public PaintTaskMasterController(
            IRepositoryPainting<PaintTaskMaster> repo,
            IRepositoryPainting<PaintTaskDetail> repoDetail,
            IRepositoryPainting<RequirePaintingList> repoPaintingList,
            IRepositoryMachine<ProjectCodeMaster> repoProMaster,
            IRepositoryMachine<Employee> repoEmp, IMapper map)
        {
            // Repository
            this.repository = repo;
            this.repositoryDetail = repoDetail;
            this.repositoryReqPaintingList = repoPaintingList;
            this.repositoryEmp = repoEmp;
            this.repositoryProMaster = repoProMaster;
            // Mapper
            this.mapper = map;
            this.helpers = new HelpersClass<PaintTaskMaster>();
            this.helpersDetail = new HelpersClass<PaintTaskDetail>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/PaintTaskMaster
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            // With Includes
            //var Includes = new List<string> { "PaintTeam" };
            //return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastRoomViewModel, PaintTaskMaster>
            //                     (await this.repository.GetAllWithInclude2Async(Includes)),
            //                      this.DefaultJsonSettings);
        }

        // GET: api/PaintTaskMaster/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            // With Includes
            var Includes = new List<string> { "RequirePaintingList.RequirePaintingMaster.ProjectCodeSub" };

            var taskMaster = this.mapper.Map<PaintTaskMaster, PaintTaskMasterViewModel>
                             (await this.repository.GetAsynvWithIncludes(key, "PaintTaskMasterId", Includes));

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
        // POST: api/PaintTaskMaster/TaskMasterSchedule
        [HttpPost("TaskMasterSchedule")]
        public async Task<IActionResult> TaskMasterSchedule([FromBody] OptionTaskMasterSchedule Scehdule)
        {
            string Message = "";

            try
            {
                var QueryData = this.repository.GetAllAsQueryable()
                                               .Include(x => x.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub)
                                               .Include(x => x.PaintTaskDetails)
                                               .AsQueryable();
                int TotalRow;

                if (Scehdule != null)
                {
                    QueryData = QueryData.OrderByDescending(x => x.RequirePaintingList.PlanStart);

                    if (Scehdule.TaskMasterId.HasValue)
                        QueryData = QueryData.Where(x => x.PaintTaskMasterId == Scehdule.TaskMasterId);

                    if (!string.IsNullOrEmpty(Scehdule.Filter))
                    {
                        var filters = string.IsNullOrEmpty(Scehdule.Filter) ? new string[] { "" }
                                   : Scehdule.Filter.ToLower().Split(null);
                        foreach (var keyword in filters)
                        {
                            QueryData = QueryData.Where(x => x.TaskPaintNo.ToLower().Contains(keyword));
                        }
                    }

                    // Option Create
                    if (!string.IsNullOrEmpty(Scehdule.Creator))
                        QueryData = QueryData.Where(x =>
                            x.RequirePaintingList.RequirePaintingMaster.RequireEmp == Scehdule.Creator);

                    // Option ProjectMasterId
                    if (Scehdule.ProjectMasterId.HasValue)
                        QueryData = QueryData.Where(x =>
                            x.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub.ProjectCodeMasterId == Scehdule.ProjectMasterId);

                    // Option Level
                    if (Scehdule.ProjectSubId.HasValue)
                        QueryData = QueryData.Where(x =>
                            x.RequirePaintingList.RequirePaintingMaster.ProjectCodeSubId == Scehdule.ProjectSubId);

                    // Option Mode
                    if (Scehdule.Mode != null)
                    {
                        if (Scehdule.Mode == 2)
                            QueryData = QueryData.Where(x => x.PaintTaskStatus == PaintTaskStatus.Waiting);
                        else
                            QueryData = QueryData.Where(x => x.PaintTaskStatus != PaintTaskStatus.Cancel);
                    }

                    TotalRow = await QueryData.CountAsync();

                    // Option Skip and Task
                    // if (Scehdule.Skip.HasValue && Scehdule.Take.HasValue)
                    QueryData = QueryData.Skip(Scehdule.Skip ?? 0).Take(Scehdule.Take ?? 5);
                }
                else
                    TotalRow = await QueryData.CountAsync();

                var GetData = await QueryData.ToListAsync();
                if (GetData.Any())
                {
                    IDictionary<string, int> ColumnGroupTop = new Dictionary<string, int>();
                    IDictionary<DateTime, string> ColumnGroupBtm = new Dictionary<DateTime, string>();
                    List<string> ColumnsAll = new List<string>();
                    // PlanDate
                    List<DateTime?> ListDate = new List<DateTime?>()
                    {
                        //START Date
                        GetData.Select(x => x.PaintTaskDetails.Min(z => z.ActualSDate)).OrderBy(x => x).FirstOrDefault() ?? null,
                        GetData.Select(x => x.PaintTaskDetails.Min(z => z.PlanSDate)).OrderBy(x => x).FirstOrDefault(),
                        GetData.Min(x => x.RequirePaintingList.PlanStart) ?? null,
                        //END Date
                        GetData.Select(x => x.PaintTaskDetails.Max(z => z.ActualEDate)).OrderByDescending(x => x).FirstOrDefault() ?? null,
                        GetData.Select(x => x.PaintTaskDetails.Max(z => z.PlanEDate)).OrderByDescending(x => x).FirstOrDefault(),
                        GetData.Max(x => x.RequirePaintingList.PlanEnd) ?? null
                    };

                    //var Plan2Start = GetData.Select(x => x.PaintTaskDetails.Min(z => z.PlanSDate)).FirstOrDefault();
                    //var ActualStart = GetData.Select(x => x.PaintTaskDetails.Min(z => z.ActualSDate)).FirstOrDefault();

                    //var Plan2End = GetData.Select(x => x.PaintTaskDetails.Max(z => z.PlanEDate)).FirstOrDefault();
                    //var ActualEnd = GetData.Select(x => x.PaintTaskDetails.Max(z => z.ActualEDate)).FirstOrDefault();

                    DateTime? MinDate = ListDate.Min();
                    DateTime? MaxDate = ListDate.Max();

                    // Min
                    //if (ActualStart != null)
                    //    MinDate = Plan2Start.Date < ActualStart.Value.Date ? Plan2Start : ActualStart.Value;
                    //else
                    //    MinDate = Plan2Start;
                    //// Max
                    //if (ActualEnd != null)
                    //    MaxDate = Plan2End > ActualEnd.Value ? Plan2End : ActualEnd.Value;
                    //else
                    //    MaxDate = Plan2End;

                    if (MinDate == null && MaxDate == null)
                        return NotFound(new { Error = "Data not found" });

                    int countCol = 1;
                    // add Date to max
                    MaxDate = MaxDate.Value.AddDays(2);
                    MinDate = MinDate.Value.AddDays(-2);
                    foreach (DateTime day in EachDay(MinDate.Value, MaxDate.Value))
                    {
                        // Get Month
                        if (ColumnGroupTop.Any(x => x.Key == day.ToString("MMMM")))
                            ColumnGroupTop[day.ToString("MMMM")] += 1;
                        else
                            ColumnGroupTop.Add(day.ToString("MMMM"), 1);

                        ColumnGroupBtm.Add(day.Date, $"Col{countCol.ToString("00")}");
                        countCol++;
                    }

                    var DataTable = new List<IDictionary<String, Object>>();
                    // OrderBy(x => x.Machine.TypeMachineId).ThenBy(x => x.Machine.MachineCode)
                    foreach (var Data in GetData.OrderBy(x => x.RequirePaintingList.PlanStart.Value).ThenBy(x => x.RequirePaintingList.PlanEnd.Value))
                    {
                        IDictionary<String, Object> rowData = new ExpandoObject();
                        var Progress = Data.MainProgress ?? 0;
                        var ProjectMaster = "NoData";
                        if (Data?.RequirePaintingList?.RequirePaintingMaster?.ProjectCodeSub != null)
                        {
                            ProjectMaster = (await this.repositoryProMaster.
                                        GetAsync(Data.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub.ProjectCodeMasterId ?? 0))?.ProjectCode ?? "";

                            ProjectMaster += $"/{Data.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub.Code}";
                        }

                        // add column time
                        rowData.Add("ProjectMaster", ProjectMaster);
                        rowData.Add("WorkItem", (Data.RequirePaintingList == null ? "" : Data.RequirePaintingList.Description) +
                                               (Data.RequirePaintingList == null ? "" : $" | {Data.RequirePaintingList.MarkNo}") +
                                               (Data.RequirePaintingList == null ? "" : $" | UnitNo({(Data.RequirePaintingList.UnitNo ?? 0).ToString("00")})"));
                        rowData.Add("Progress", Progress.ToString("0.0") + "%");
                        rowData.Add("PaintTaskMasterId", Data?.PaintTaskMasterId ?? 1);

                        // Data is 1:Plan1,2:Plan2,3:Plan1AndPlan2,
                        // 4:Actual,5:Plan1AndActual,6:Plan2AndActual,7:Plan1,Plan2AndActual
                        // For Plan1
                        if (Data.RequirePaintingList.PlanStart.Value != null && Data.RequirePaintingList.PlanEnd.Value != null)
                        {
                            foreach (DateTime day in EachDay(Data.RequirePaintingList.PlanStart.Value, Data.RequirePaintingList.PlanEnd.Value))
                            {
                                if (ColumnGroupBtm.Any(x => x.Key == day.Date))
                                    rowData.Add(ColumnGroupBtm.FirstOrDefault(x => x.Key == day.Date).Value, 1);
                            }
                        }

                        // For Plan2
                        var DetailPlanStart = Data.PaintTaskDetails.Min(x => x.PlanSDate);
                        var DetailPlanEnd = Data.PaintTaskDetails.Max(x => x.PlanEDate);
                        if (DetailPlanStart != null && DetailPlanEnd != null)
                        {
                            foreach (DateTime day in EachDay(DetailPlanStart, DetailPlanEnd))
                            {
                                if (ColumnGroupBtm.Any(x => x.Key == day.Date))
                                {
                                    var Col = ColumnGroupBtm.FirstOrDefault(x => x.Key == day.Date);

                                    // if Have Plan1 change value to 3
                                    if (rowData.Keys.Any(x => x == Col.Value))
                                        rowData[Col.Value] = 3;
                                    else // else Don't have plan1 change value is 2 for plan2
                                        rowData.Add(Col.Value, 2);
                                }
                            }
                        }

                        //For Actual
                        var DetailActualStart = Data.PaintTaskDetails.Min(x => x.ActualSDate);
                        if (DetailActualStart != null)
                        {
                            var EndDate = Data.PaintTaskDetails.Max(x => x.ActualEDate);
                            var LastDate = (MaxDate > DateTime.Today ? DateTime.Today : MaxDate);

                            if (Data.PaintTaskDetails.Any(x => x.ActualEDate == null))
                                EndDate = LastDate;
                            else if (EndDate == null)
                                EndDate = LastDate;
                            

                            foreach (DateTime day in EachDay(DetailActualStart.Value, EndDate.Value))
                            {
                                if (ColumnGroupBtm.Any(x => x.Key == day.Date))
                                {
                                    var Col = ColumnGroupBtm.FirstOrDefault(x => x.Key == day.Date);
                                    // if Have Plan change value to 3
                                    if (rowData.Keys.Any(x => x == Col.Value))
                                    {
                                        if ((int)rowData[Col.Value] == 1)
                                            rowData[Col.Value] = 5;
                                        else if ((int)rowData[Col.Value] == 2)
                                            rowData[Col.Value] = 6;
                                        else if ((int)rowData[Col.Value] == 3)
                                            rowData[Col.Value] = 7;
                                    }
                                    else // else Don't have plan1 and plan2 change value is 4
                                        rowData.Add(Col.Value, 4);
                                }
                            }
                        }
                        DataTable.Add(rowData);
                    }

                    if (DataTable.Any())
                        ColumnGroupBtm.OrderBy(x => x.Key.Date).Select(x => x.Value)
                            .ToList().ForEach(item => ColumnsAll.Add(item));

                    return new JsonResult(new
                    {
                        TotalRow = TotalRow,
                        ColumnsTop = ColumnGroupTop.Select(x => new
                        {
                            Name = x.Key,
                            Value = x.Value
                        }),
                        ColumnsLow = ColumnGroupBtm.OrderBy(x => x.Key.Date).Select(x => x.Key.Day),
                        ColumnsAll = ColumnsAll,
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

        // POST: api/PaintTaskMaster/OnlyMasterSchedule
        [HttpPost("OnlyMasterSchedule")]
        public async Task<IActionResult> OnlyTaskMasterSchedule([FromBody] OptionTaskMasterSchedule Scehdule)
        {
            string Message = "";

            try
            {
                var QueryData = this.repositoryDetail.GetAllAsQueryable()
                                               .Where(x => x.PaintTaskMasterId == Scehdule.TaskMasterId)
                                               .Include(x => x.PaintTaskMaster.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub)
                                               .Include(x => x.BlastWorkItem.SurfaceTypeExt)
                                               .Include(x => x.BlastWorkItem.SurfaceTypeInt)
                                               .Include(x => x.BlastRoom)
                                               .Include(x => x.PaintWorkItem.ExtColorItem)
                                               .Include(x => x.PaintWorkItem.IntColorItem)
                                               .Include(x => x.PaintTeam)
                                               .AsQueryable();
                int TotalRow;

                if (Scehdule != null)
                {
                    TotalRow = await QueryData.CountAsync();

                    // Option Skip and Task
                    // if (Scehdule.Skip.HasValue && Scehdule.Take.HasValue)
                    QueryData = QueryData.Skip(Scehdule.Skip ?? 0).Take(Scehdule.Take ?? 5);
                }
                else
                    TotalRow = await QueryData.CountAsync();

                var GetData = await QueryData.ToListAsync();
                if (GetData.Any())
                {
                    IDictionary<string, int> ColumnGroupTop = new Dictionary<string, int>();
                    IDictionary<DateTime, string> ColumnGroupBtm = new Dictionary<DateTime, string>();
                    List<string> ColumnsAll = new List<string>();
                    // PlanDate
                    List<DateTime?> ListDate = new List<DateTime?>()
                    {
                        //START Date
                        GetData.Min(x => x.ActualSDate),
                        GetData.Min(x => x.PlanSDate),
                        //END Date
                        GetData.Max(z => z.ActualEDate),
                        GetData.Max(z => z.PlanEDate),
                    };

                    DateTime? MinDate = ListDate.Min();
                    DateTime? MaxDate = ListDate.Max();

                    if (MinDate == null && MaxDate == null)
                        return NotFound(new { Error = "Data not found" });

                    int countCol = 1;
                    // add Date to max
                    MaxDate = MaxDate.Value.AddDays(2);
                    MinDate = MinDate.Value.AddDays(-2);
                    foreach (DateTime day in EachDay(MinDate.Value, MaxDate.Value))
                    {
                        // Get Month
                        if (ColumnGroupTop.Any(x => x.Key == day.ToString("MMMM")))
                            ColumnGroupTop[day.ToString("MMMM")] += 1;
                        else
                            ColumnGroupTop.Add(day.ToString("MMMM"), 1);

                        ColumnGroupBtm.Add(day.Date, $"Col{countCol.ToString("00")}");
                        countCol++;
                    }

                    var DataTable = new List<IDictionary<String, Object>>();
                    // OrderBy(x => x.Machine.TypeMachineId).ThenBy(x => x.Machine.MachineCode)
                    foreach (var Data in GetData.OrderBy(x => x.PlanSDate)
                                                .ThenBy(x => x.PaintTaskDetailLayer)
                                                .ThenBy(x => x.PlanEDate))
                    {
                        IDictionary<String, Object> rowData = new ExpandoObject();
                        var Progress = Data.TaskDetailProgress ?? 0;
                        var TaskType = "NoData";
                        var WorkItem = "NoData";
                        if (Data?.PaintTaskDetailType != null)
                        {
                            if (Data.PaintTaskDetailType.Value == PaintTaskDetailType.Blast)
                            {
                                TaskType = "Blast-Work" + (Data.PaintTaskDetailLayer == PaintTaskDetailLayer.Internal ? "/Internal" : "/External");
                                WorkItem = (Data?.BlastWorkItem?.SurfaceTypeInt?.SurfaceName ?? "") + (Data?.BlastWorkItem?.SurfaceTypeExt?.SurfaceName ?? "");
                                WorkItem += " | " +Data?.BlastRoom?.BlastRoomName ?? "" ;
                            }
                            else
                            {
                                TaskType = "Paint-Work" + (Data.PaintTaskDetailLayer == PaintTaskDetailLayer.Internal ? "/Internal" : "/External");
                                WorkItem = Data?.PaintWorkItem?.PaintLevel == null ? "" :
                                           (Data?.PaintWorkItem?.PaintLevel == PaintLevel.PrimerCoat ? "PrimerCoat" :
                                           (Data?.PaintWorkItem?.PaintLevel == PaintLevel.MidCoat ? "MidCoat" :
                                           (Data?.PaintWorkItem?.PaintLevel == PaintLevel.IntermediateCoat ? "IntermediateCoat" : "TopCoat")));
                                WorkItem += " | " + (Data.PaintTaskDetailLayer == PaintTaskDetailLayer.Internal ?
                                    (Data?.PaintWorkItem?.IntColorItem?.ColorName ?? "") : (Data?.PaintWorkItem?.ExtColorItem?.ColorName ?? ""));
                                WorkItem += " | " + Data?.PaintTeam?.TeamName ?? "";
                            }
                        }

                        // add column time
                        rowData.Add("TaskType", TaskType);
                        rowData.Add("WorkItem", WorkItem);
                        rowData.Add("Progress", Progress.ToString("00.0") + "%");
                        rowData.Add("PaintTaskDetailId", Data?.PaintTaskDetailId ?? 1);

                        // Data is 1:Plan1,2:Plan2,3:Plan1AndPlan2,
                        // For Plan1
                        if (Data.PlanSDate != null && Data.PlanEDate != null)
                        {
                            foreach (DateTime day in EachDay(Data.PlanSDate, Data.PlanEDate))
                            {
                                if (ColumnGroupBtm.Any(x => x.Key == day.Date))
                                    rowData.Add(ColumnGroupBtm.FirstOrDefault(x => x.Key == day.Date).Value, 1);
                            }
                        }

                        //For Actual
                        if (Data.ActualSDate != null)
                        {
                            var EndDate = Data.ActualEDate ?? (MaxDate > DateTime.Today ? DateTime.Today : MaxDate);

                            foreach (DateTime day in EachDay(Data.ActualSDate.Value, EndDate.Value))
                            {
                                if (ColumnGroupBtm.Any(x => x.Key == day.Date))
                                {

                                    var Col = ColumnGroupBtm.FirstOrDefault(x => x.Key == day.Date);

                                    // if Have Plan change value to 3
                                    if (rowData.Keys.Any(x => x == Col.Value))
                                        rowData[Col.Value] = 3;
                                    else // else Don't have plan value is 2
                                        rowData.Add(Col.Value, 2);
                                }
                            }
                        }

                        DataTable.Add(rowData);
                    }

                    if (DataTable.Any())
                        ColumnGroupBtm.OrderBy(x => x.Key.Date).Select(x => x.Value)
                            .ToList().ForEach(item => ColumnsAll.Add(item));

                    return new JsonResult(new
                    {
                        TotalRow = TotalRow,
                        ColumnsTop = ColumnGroupTop.Select(x => new
                        {
                            Name = x.Key,
                            Value = x.Value
                        }),
                        ColumnsLow = ColumnGroupBtm.OrderBy(x => x.Key.Date).Select(x => x.Key.Day),
                        ColumnsAll = ColumnsAll,
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

        // POST: api/PaintTaskMaster/GetScroll
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
                QueryData = QueryData.Where(x => x.TaskPaintNo.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "TaskNo":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.TaskPaintNo);
                    else
                        QueryData = QueryData.OrderBy(e => e.TaskPaintNo);
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
            var DataViewModel = this.ConvertTable.ConverterTableToViewModel<PaintTaskMasterViewModel, PaintTaskMaster>
                                    (await QueryData.AsNoTracking().ToListAsync());

            if (DataViewModel.Any())
            {
                foreach (var item in DataViewModel)
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
                (new ScrollDataViewModel<PaintTaskMasterViewModel>
                (Scroll, DataViewModel), this.DefaultJsonSettings);
        }

        // POST: api/PaintTaskMaster
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PaintTaskMaster nPaintTaskMaster)
        {
            var Message = "PaintTaskMaster not been found.";
            try
            {
                if (nPaintTaskMaster != null && nPaintTaskMaster.PaintTaskDetails != null)
                {
                    nPaintTaskMaster = helpers.AddHourMethod(nPaintTaskMaster);

                    nPaintTaskMaster.CreateDate = DateTime.Now;
                    nPaintTaskMaster.Creator = nPaintTaskMaster.Creator ?? "Someone";

                    nPaintTaskMaster.PaintTaskStatus = this.CheckTaskStatus(nPaintTaskMaster);

                    if (nPaintTaskMaster.RequirePaintingListId.HasValue)
                        nPaintTaskMaster.TaskPaintNo = await this.GeneratedCode(nPaintTaskMaster.RequirePaintingListId.Value);

                    if (nPaintTaskMaster.RequirePaintingList != null)
                        nPaintTaskMaster.RequirePaintingList = null;

                    // Remove null
                    nPaintTaskMaster.PaintTaskDetails.Remove(null);

                    if (nPaintTaskMaster.PaintTaskDetails != null)
                    {
                        // Add BlastWork item
                        foreach (var nPaintTaskDetail in nPaintTaskMaster.PaintTaskDetails)
                        {
                            if (nPaintTaskDetail == null)
                                continue;
                            // Change TimeZone
                            if (nPaintTaskDetail.ActualEDate.HasValue)
                                nPaintTaskDetail.ActualEDate = ChangeTimeZone(nPaintTaskDetail.ActualEDate.Value);
                            if (nPaintTaskDetail.ActualSDate.HasValue)
                                nPaintTaskDetail.ActualSDate = ChangeTimeZone(nPaintTaskDetail.ActualSDate.Value);
                            if (nPaintTaskDetail.PlanEDate != null)
                                nPaintTaskDetail.PlanEDate = ChangeTimeZone(nPaintTaskDetail.PlanEDate);
                            if (nPaintTaskDetail.PlanSDate != null)
                                nPaintTaskDetail.PlanSDate = ChangeTimeZone(nPaintTaskDetail.PlanSDate);

                            nPaintTaskDetail.CreateDate = nPaintTaskMaster.CreateDate;
                            nPaintTaskDetail.Creator = nPaintTaskMaster.Creator;

                            // Clear BlastRoom
                            if (nPaintTaskDetail.BlastRoom != null)
                                nPaintTaskDetail.BlastRoom = null;

                            // Clear BlastWorkItem
                            if (nPaintTaskDetail.BlastWorkItem != null)
                                nPaintTaskDetail.BlastWorkItem = null;

                            // Clear PaintTeam
                            if (nPaintTaskDetail.PaintTeam != null)
                                nPaintTaskDetail.PaintTeam = null;

                            // Clear PaintWorkItem
                            if (nPaintTaskDetail.PaintWorkItem != null)
                                nPaintTaskDetail.PaintWorkItem = null;
                        }
                    }
                    else
                        return NotFound(new { Error = Message });

                    return new JsonResult(await this.repository.AddAsync(nPaintTaskMaster), this.DefaultJsonSettings);
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

        // PUT: api/PaintTaskMaster/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]PaintTaskMaster uPaintTaskMaster)
        {
            var Message = "task master not been found.";
            // For Update
            if (uPaintTaskMaster != null && uPaintTaskMaster.PaintTaskDetails != null)
            {
                // add hour to DateTime to set Asia/Bangkok
                uPaintTaskMaster = helpers.AddHourMethod(uPaintTaskMaster);
                // set modified
                uPaintTaskMaster.ModifyDate = DateTime.Now;
                uPaintTaskMaster.Modifyer = uPaintTaskMaster.Modifyer ?? "Someone";

                if (uPaintTaskMaster.RequirePaintingList != null)
                    uPaintTaskMaster.RequirePaintingList = null;

                uPaintTaskMaster.PaintTaskStatus = this.CheckTaskStatus(uPaintTaskMaster);

                // Remove null
                uPaintTaskMaster.PaintTaskDetails.Remove(null);

                if (uPaintTaskMaster.PaintTaskDetails != null)
                {
                    foreach (var uPaintTaskDetail in uPaintTaskMaster.PaintTaskDetails)
                    {
                        if (uPaintTaskDetail == null)
                            continue;

                        // Change TimeZone
                        if (uPaintTaskDetail.ActualEDate.HasValue)
                            uPaintTaskDetail.ActualEDate = ChangeTimeZone(uPaintTaskDetail.ActualEDate.Value);
                        if (uPaintTaskDetail.ActualSDate.HasValue)
                            uPaintTaskDetail.ActualSDate = ChangeTimeZone(uPaintTaskDetail.ActualSDate.Value);
                        if (uPaintTaskDetail.PlanEDate != null)
                            uPaintTaskDetail.PlanEDate = ChangeTimeZone(uPaintTaskDetail.PlanEDate);
                        if (uPaintTaskDetail.PlanSDate != null)
                            uPaintTaskDetail.PlanSDate = ChangeTimeZone(uPaintTaskDetail.PlanSDate);

                        if (uPaintTaskDetail.PaintTaskDetailId > 0)
                        {
                            uPaintTaskDetail.ModifyDate = uPaintTaskMaster.ModifyDate;
                            uPaintTaskDetail.Modifyer = uPaintTaskMaster.Modifyer;
                        }
                        else
                        {
                            uPaintTaskDetail.CreateDate = uPaintTaskMaster.ModifyDate;
                            uPaintTaskDetail.Creator = uPaintTaskMaster.Modifyer;
                        }

                        uPaintTaskDetail.BlastWorkItem = null;
                        uPaintTaskDetail.BlastRoom = null;
                        uPaintTaskDetail.PaintWorkItem = null;
                        uPaintTaskDetail.PaintTeam = null;
                    }
                }

                // update Master not update Detail it need to update Detail directly
                var updateComplate = await this.repository.UpdateAsync(uPaintTaskMaster, key);
                if (updateComplate != null)
                {
                    // filter
                    Expression<Func<PaintTaskDetail, bool>> conditionBlast = m => m.PaintTaskMasterId == key;
                    var dbPaintTaskDetails = this.repositoryDetail.FindAll(conditionBlast);

                    //Remove TaskBlastDetail if edit remove it
                    foreach (var dbPaintTaskDetail in dbPaintTaskDetails)
                    {
                        if (!uPaintTaskMaster.PaintTaskDetails.Any(x => x.PaintTaskDetailId == dbPaintTaskDetail.PaintTaskDetailId))
                            await this.repositoryDetail.DeleteAsync(dbPaintTaskDetail.PaintTaskDetailId);
                    }

                    //Update TaskBlastDetail or New TaskBlastDetail
                    foreach (var uPaintTaskDetail in uPaintTaskMaster.PaintTaskDetails)
                    {
                        if (uPaintTaskDetail == null)
                            continue;

                        if (uPaintTaskDetail.PaintTaskDetailId > 0)
                            await this.repositoryDetail.UpdateAsync(uPaintTaskDetail, uPaintTaskDetail.PaintTaskDetailId);
                        else
                        {
                            if (uPaintTaskDetail.PaintTaskMasterId is null || uPaintTaskDetail.PaintTaskMasterId < 1)
                                uPaintTaskDetail.PaintTaskMasterId = uPaintTaskMaster.PaintTaskMasterId;

                            await this.repositoryDetail.AddAsync(uPaintTaskDetail);
                        }
                    }
                }

                return new JsonResult(updateComplate, this.DefaultJsonSettings);

            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/PaintTaskMaster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
