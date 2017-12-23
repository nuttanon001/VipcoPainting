using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using VipcoPainting.Helpers;
using VipcoPainting.Models;
using VipcoPainting.Services.Interfaces;
using VipcoPainting.ViewModels;

namespace VipcoPainting.Controllers
{
    [Produces("application/json")]
    [Route("api/RequirePaintingMaster")]
    public class RequirePaintingMasterController : Controller
    {
        #region PrivateMenbers

        private IRepositoryPainting<RequirePaintingMaster> repository;
        private IRepositoryPainting<ProjectCodeSub> repositoryProSub;
        private IRepositoryMachine<ProjectCodeMaster> repositoryProMas;
        private IRepositoryMachine<Employee> repositoryEmp;
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<RequirePaintingMaster> helpers;

        private async Task<string> GeneratedCode(int ProjectSubId)
        {
            if (ProjectSubId > 0)
            {
                var ProjectSub = await this.repositoryProSub.GetAsync(ProjectSubId);
                if (ProjectSub != null)
                {
                    var ProjectMaster = await this.repositoryProMas.GetAsync(ProjectSub?.ProjectCodeMasterId ?? 0);
                    if (ProjectMaster != null)
                    {
                        var ListSubId = await this.repositoryProSub.GetAllAsQueryable()
                            .Where(x => x.ProjectCodeMasterId == ProjectMaster.ProjectCodeMasterId)
                            .Select(x => x.ProjectCodeSubId).ToListAsync();

                        var Runing = await this.repository.GetAllAsQueryable()
                                               .CountAsync(x => ListSubId.Contains(x.ProjectCodeSubId ?? 0)) + 1;

                        var Code = ProjectMaster.ProjectCode;
                        if (Code.Trim().IndexOf("misc") != -1)
                        {
                            Code = Code.ToLower().Replace("misc", "").Trim();
                            Code = "M" + Code;
                        }
                        else
                            Code = "J" + Code;

                        return $"{Code}/{Runing.ToString("000")}";
                    }
                }

                // return $"{proDetail.ProjectCodeMaster.ProjectCode}/{typeMachine.TypeMachineCode}/{Runing.ToString("0000")}";
            }

            return "xxxx/xx/xx";
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
        #endregion PrivateMenbers

        #region Constructor

        public RequirePaintingMasterController(
            IRepositoryPainting<RequirePaintingMaster> repo,
            IRepositoryPainting<ProjectCodeSub> repoProSub,
            IRepositoryMachine<ProjectCodeMaster> repoProMas,
            IRepositoryMachine<Employee> repoEmp,
            IMapper map)
        {
            this.repository = repo;
            this.repositoryProSub = repoProSub;
            this.repositoryProMas = repoProMas;
            this.repositoryEmp = repoEmp;
            this.mapper = map;
            this.helpers = new HelpersClass<RequirePaintingMaster>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/RequirePaintingMaster
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "ProjectCodeSub" };
            return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),
                                        this.DefaultJsonSettings);
        }

        // GET: api/RequirePaintingMaster/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "ProjectCodeSub" };
            var requirePaintMaster = this.mapper.Map<RequirePaintingMaster, RequirePaintingMasterViewModel>
                (await this.repository.GetAsynvWithIncludes(key, "RequirePaintingMasterId", Includes));

            if (requirePaintMaster != null)
            {
                if (!string.IsNullOrEmpty(requirePaintMaster.RequireEmp))
                {
                    requirePaintMaster.RequireString =
                        (await this.repositoryEmp.GetAsync(requirePaintMaster.RequireEmp))?.NameThai ?? "";
                }
                if (requirePaintMaster.ProjectCodeSub != null)
                {
                    requirePaintMaster.ProjectCodeSubString =
                        (await this.repositoryProMas.GetAsync(requirePaintMaster.ProjectCodeSub.ProjectCodeMasterId ?? 0))?.ProjectCode ?? "";

                    requirePaintMaster.ProjectCodeSubString += $"/{requirePaintMaster.ProjectCodeSub.Code}";
                    requirePaintMaster.ProjectCodeSub = null;
                }
            }

            return new JsonResult(requirePaintMaster, this.DefaultJsonSettings);
        }

        // GET: api/RequirePaintingMaster/RequirePaintingMasterHasWait/
        [HttpGet("RequirePaintingMasterHasWait")]
        public async Task<IActionResult> GetRequirePaintingMasterHasWait()
        {
            string Message = "";

            try
            {
                var QueryData = this.repository.GetAllAsQueryable()
                                               .Include(x => x.ProjectCodeSub)
                                               .AsQueryable();

                QueryData = QueryData.Where(x => x.RequirePaintingStatus == RequirePaintingStatus.Waiting);

                var GetData = await QueryData.ToListAsync();
                if (GetData.Any())
                {
                    var dataTable = new List<IDictionary<String, Object>>();
                    List<string> columns = new List<string>() { "JobNumber", "Employee" };

                    foreach (var item in GetData.Where(x => x.RequireDate != null)
                                                .OrderBy(x => x.RequireDate)
                                                .GroupBy(x => x.RequireDate.Value.Date)
                                                .Select(x => x.Key))
                        columns.Add(item.ToString("dd/MM/yy"));

                    foreach (var ProjectCodeSub in GetData.GroupBy(x => x.ProjectCodeSub))
                    {
                        foreach (var RequireEmp in ProjectCodeSub.GroupBy(x => x.RequireEmp))
                        {
                            if (RequireEmp == null)
                                continue;
                            else
                            {
                                var Employee = await this.repositoryEmp.GetAsync(RequireEmp.Key);

                                IDictionary<String, Object> rowData = new ExpandoObject();
                                var EmployeeReq = RequireEmp.Key != null ? $"{(Employee?.NameThai ?? "")}" : "No-Data";
                                // add column time
                                rowData.Add(columns[1], EmployeeReq);
                                foreach (var item in RequireEmp)
                                {
                                    // Get ProjectMaster
                                    var ProjectMaster = await this.repositoryProMas.GetAsync(item.ProjectCodeSub.ProjectCodeMasterId ?? 0);

                                    string JobNumber = $"{ProjectMaster?.ProjectCode ?? "No-Data"}/{(item.ProjectCodeSub == null ? "No-Data" : item.ProjectCodeSub.Code)}";
                                    // if don't have type add item to rowdata
                                    if (!rowData.Keys.Any(x => x == "JobNumber"))
                                        rowData.Add(columns[0], JobNumber);

                                    var key = columns.Where(y => y.Contains(item.RequireDate.Value.ToString("dd/MM/yy"))).FirstOrDefault();
                                    // if don't have data add it to rowData
                                    if (!rowData.Keys.Any(x => x == key))
                                        rowData.Add(key, $"คลิกที่ไอคอน เพื่อแสดงข้อมูล#{item.RequirePaintingMasterId}");
                                    else
                                        rowData[key] += $"#{item.RequirePaintingMasterId}";
                                }
                                dataTable.Add(rowData);
                            }
                        }
                    }

                    if (dataTable.Any())
                        return new JsonResult(new
                        {
                            Columns = columns,
                            DataTable = dataTable
                        }, this.DefaultJsonSettings);
                }
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }
            return NotFound(new { Error = Message });
        }

        #endregion GET

        #region POST

        // POST: api/RequirePaintingMaster/GetMultiKey
        [HttpPost("GetMultipleKey")]
        public async Task<IActionResult> GetMultipleKey([FromBody] List<string> ListKey)
        {
            if (ListKey != null)
            {
                var Includes = new List<string> { "ProjectCodeSub" };
                var RequirePaintingMaster = new List<RequirePaintingMasterViewModel>();

                foreach (var key in ListKey)
                {
                    if (int.TryParse(key, out int keyInt))
                    {
                        var requirePaintMaster = (this.mapper.Map<RequirePaintingMaster, RequirePaintingMasterViewModel>
                            (await this.repository.GetAsynvWithIncludes(keyInt, "RequirePaintingMasterId", Includes)));

                        if (!string.IsNullOrEmpty(requirePaintMaster.RequireEmp))
                        {
                            requirePaintMaster.RequireString =
                                (await this.repositoryEmp.GetAsync(requirePaintMaster.RequireEmp))?.NameThai ?? "";
                        }
                        if (requirePaintMaster.ProjectCodeSub != null)
                        {
                            requirePaintMaster.ProjectCodeSubString =
                                (await this.repositoryProMas.GetAsync(requirePaintMaster.ProjectCodeSub.ProjectCodeMasterId ?? 0))?.ProjectCode ?? "";

                            requirePaintMaster.ProjectCodeSubString += $"/{requirePaintMaster.ProjectCodeSub.Code}";
                        }

                        RequirePaintingMaster.Add(requirePaintMaster);
                    }
                }

                if (RequirePaintingMaster.Any())
                    return new JsonResult(RequirePaintingMaster, this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not Found Key." });
        }

        // POST: api/RequirePaintingMaster/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var Message = "Data been not found.";
            try
            {
                var QueryData = this.repository.GetAllAsQueryable()
                                    .Include(x => x.ProjectCodeSub)
                                    .AsQueryable();
                // Filter
                var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                    : Scroll.Filter.ToLower().Split(null);
                // foreach
                foreach (var keyword in filters)
                {
                    QueryData = QueryData.Where(x => x.RequireNo.ToLower().Contains(keyword) ||
                                                     x.PaintingSchedule.ToLower().Contains(keyword) ||
                                                     x.ProjectCodeSub.Code.ToLower().Contains(keyword));
                }

                // Order
                switch (Scroll.SortField)
                {
                    case "RequireNo":
                        if (Scroll.SortOrder == -1)
                            QueryData = QueryData.OrderByDescending(e => e.RequireNo);
                        else
                            QueryData = QueryData.OrderBy(e => e.RequireNo);
                        break;

                    case "JobCode":
                        if (Scroll.SortOrder == -1)
                            QueryData = QueryData.OrderByDescending(e => e.ProjectCodeSub.Code);
                        else
                            QueryData = QueryData.OrderBy(e => e.ProjectCodeSub.Code);
                        break;

                    case "RequireDate":
                        if (Scroll.SortOrder == -1)
                            QueryData = QueryData.OrderByDescending(e => e.RequireDate);
                        else
                            QueryData = QueryData.OrderBy(e => e.RequireDate);
                        break;

                    default:
                        QueryData = QueryData.OrderByDescending(e => e.RequireDate);
                        break;
                }
                QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

                return new JsonResult(new ScrollDataViewModel<RequirePaintingMasterViewModel>
                    (Scroll,
                    this.ConvertTable.ConverterTableToViewModel<RequirePaintingMasterViewModel, RequirePaintingMaster>(await QueryData.AsNoTracking().ToListAsync())),
                    this.DefaultJsonSettings);
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }
            return NotFound(new { Error = Message });
        }

        // POST: api/RequirePaintingMaster/RequirePaintSchedule
        [HttpPost("RequirePaintSchedule")]
        public async Task<IActionResult> RequirePaintSchedule([FromBody] OptionRequirePaintSchedule Scehdule)
        {
            string Message = "";
            try
            {
                var QueryData = this.repository.GetAllAsQueryable()
                                               .Where(x => x.RequireDate != null)
                                               .Include(x => x.ProjectCodeSub)
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
                    {
                        QueryData = QueryData.Where(x => x.ProjectCodeSubId == Scehdule.ProjectId);
                    }
                    // Option SDate
                    if (Scehdule.SDate.HasValue)
                    {
                    }

                    // Option EDate
                    if (Scehdule.EDate.HasValue)
                    {
                    }

                    // Option Status
                    if (Scehdule.Status.HasValue)
                    {
                        if (Scehdule.Status == 1)
                            QueryData = QueryData.Where(x => x.RequirePaintingStatus == RequirePaintingStatus.Waiting);
                        else if (Scehdule.Status == 2)
                            QueryData = QueryData.Where(x => x.RequirePaintingStatus == RequirePaintingStatus.Tasking);
                        else
                            QueryData = QueryData.Where(x => x.RequirePaintingStatus != RequirePaintingStatus.Cancel);
                    }
                    else
                    {
                        QueryData = QueryData.Where(x => x.RequirePaintingStatus == RequirePaintingStatus.Waiting);
                    }
                    
                    TotalRow = await QueryData.CountAsync();

                    // Option Skip and Task
                    if (Scehdule.Skip.HasValue && Scehdule.Take.HasValue)
                        QueryData = QueryData.Skip(Scehdule.Skip ?? 0).Take(Scehdule.Take ?? 10);
                    else
                        QueryData = QueryData.Skip(0).Take(10);
                }
                else
                {
                    TotalRow = await QueryData.CountAsync();
                }

                var GetData = await QueryData.ToListAsync();
                if (GetData.Any())
                {
                    List<string> Columns = new List<string>();

                    var MinDate = GetData.Min(x => x.RequireDate);
                    var MaxDate = GetData.Max(x => x.RequireDate);

                    if (MinDate == null && MaxDate == null)
                    {
                        return NotFound(new { Error = "Data not found" });
                    }

                    foreach (DateTime day in EachDay(MinDate.Value, MaxDate.Value))
                    {
                        if (GetData.Any(x => x.RequireDate.Value.Date == day.Date))
                            Columns.Add(day.Date.ToString("dd/MM/yy"));
                    }

                    var DataTable = new List<IDictionary<String, Object>>();

                    foreach (var Data in GetData.OrderBy(x => x.ProjectCodeSub.ProjectCodeMasterId))
                    {
                        var ProjectMaster = await this.repositoryProMas.GetAsync(Data.ProjectCodeSub.ProjectCodeMasterId ?? 0);
                        var JobNumber = $"{ProjectMaster?.ProjectCode ?? "No-Data"}/{(Data.ProjectCodeSub == null ? "No-Data" : Data.ProjectCodeSub.Code)}";

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

                        if (Data.RequireDate != null)
                        {
                            //Get Employee Name
                            var Employee = await this.repositoryEmp.GetAsync(Data.RequireEmp);
                            var EmployeeReq = Employee != null ? $"คุณ{(Employee?.NameThai ?? "")}" : "No-Data";

                            var Key = Data.RequireDate.Value.ToString("dd/MM/yy");
                            if (rowData.Any(x => x.Key == Key))
                            {
                                // New Value
                                var ListMaster = (List<RequirePaintingMasterViewModel>)rowData[Key];
                                ListMaster.Add(new RequirePaintingMasterViewModel
                                {
                                    RequirePaintingMasterId = Data.RequirePaintingMasterId,
                                    RequireString = $"{EmployeeReq} | No.{Data.RequireNo}",
                                });

                                rowData[Key] = ListMaster;
                            }
                            else // add new
                            {
                                var Master = new RequirePaintingMasterViewModel()
                                {
                                    RequirePaintingMasterId = Data.RequirePaintingMasterId,
                                    RequireString = $"{EmployeeReq} | No.{Data.RequireNo}",
                                };
                                rowData.Add(Key, new List<RequirePaintingMasterViewModel>() { Master });
                            }
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

        // POST: api/RequirePaintingMaster
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RequirePaintingMaster nRequirePaintingMaster)
        {
            if (nRequirePaintingMaster != null)
            {
                nRequirePaintingMaster = helpers.AddHourMethod(nRequirePaintingMaster);

                nRequirePaintingMaster.CreateDate = DateTime.Now;
                nRequirePaintingMaster.Creator = nRequirePaintingMaster.Creator ?? "Someone";

                nRequirePaintingMaster.RequireNo = await this.GeneratedCode(nRequirePaintingMaster.ProjectCodeSubId ?? -1);

                if (nRequirePaintingMaster.RequirePaintingLists != null)
                    nRequirePaintingMaster.RequirePaintingLists = null;

                return new JsonResult(await this.repository.AddAsync(nRequirePaintingMaster), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found RequirePaintingMaster data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/RequirePaintingMaster/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]RequirePaintingMaster uRequirePaintingMaster)
        {
            var Message = "Require painting not been found.";

            if (uRequirePaintingMaster != null)
            {
                // add hour to DateTime to set Asia/Bangkok
                uRequirePaintingMaster = helpers.AddHourMethod(uRequirePaintingMaster);
                // set modified
                uRequirePaintingMaster.ModifyDate = DateTime.Now;
                uRequirePaintingMaster.Modifyer = uRequirePaintingMaster.Modifyer ?? "Someone";

                if (uRequirePaintingMaster.RequirePaintingLists != null)
                    uRequirePaintingMaster.RequirePaintingLists = null;

                // update Master not update Detail it need to update Detail directly
                return new JsonResult(await this.repository.UpdateAsync(uRequirePaintingMaster, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/RequirePaintingMaster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}