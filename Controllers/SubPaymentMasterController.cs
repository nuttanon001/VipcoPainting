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
    [Route("api/SubPaymentMaster")]
    public class SubPaymentMasterController : Controller
    {
        #region PrivateMenbers

        // Repository Painting
        private IRepositoryPainting<SubPaymentMaster> repository;
        private IRepositoryPainting<SubPaymentDetail> repositorySubPaymentDetail;
        private IRepositoryPainting<PaymentDetail> repositoryPaymentDetail;
        private IRepositoryPainting<PaintTeam> repositoryPaintTeam;
        private IRepositoryPainting<PaintTaskDetail> repositoryPaintTaskDetail;

        // Repositry Machines
        private IRepositoryMachine<Employee> repositoryEmp;
        private IRepositoryMachine<ProjectCodeMaster> repositoryProject;

        // Mapper
        private IMapper mapper;

        // Helpers
        private HelpersClass<SubPaymentMaster> helpers;

        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;

        private async Task<SubPaymentMasterViewModel> ModelGetViewModelNoRelation(SubPaymentMasterViewModel Model)
        {
            if (!string.IsNullOrEmpty(Model.EmpApproved1))
                Model.EmpApproved1String = (await this.repositoryEmp.GetAsync(Model.EmpApproved1)).NameThai ?? "-";
            if (!string.IsNullOrEmpty(Model.EmpApproved2))
                Model.EmpApproved2String = (await this.repositoryEmp.GetAsync(Model.EmpApproved2)).NameThai ?? "-";
            if (Model.ProjectCodeMasterId.HasValue)
                Model.ProjectCodeMasterString = (await this.repositoryProject.GetAsync(Model.ProjectCodeMasterId.Value)).ProjectName ?? "-";

            return Model;
        }

        private async Task<string> GeneratedCode(int ProjectMasterId, int PaintTeamId)
        {
            if (ProjectMasterId > 0 && PaintTeamId > 0)
            {
                var projectMaster = await this.repositoryProject.GetAsync(ProjectMasterId);
                var paintTeam = await this.repositoryPaintTeam.GetAsync(PaintTeamId);

                if (projectMaster != null && paintTeam != null)
                {
                    var Runing = await this.repository.GetAllAsQueryable()
                                           .Where(x => x.ProjectCodeMasterId == ProjectMasterId &&
                                                       x.PaintTeamId == PaintTeamId)
                                           .CountAsync() + 1;
                    var Code = projectMaster.ProjectCode;
                    if (Code.Trim().IndexOf("misc") != -1)
                    {
                        Code = Code.ToLower().Replace("misc", "").Trim();
                        Code = "M" + Code;
                    }
                    else
                        Code = "J" + Code;

                    return $"{paintTeam.TeamName}/{Code}/{Runing.ToString("00")}";
                }

                // return $"{proDetail.ProjectCodeMaster.ProjectCode}/{typeMachine.TypeMachineCode}/{Runing.ToString("0000")}";
            }

            return "xxxx/xx-xxx";
        }

        private Func<DateTime, DateTime> ChangeTimeZone = d => d.AddHours(+7);
        #endregion PrivateMenbers

        #region Constructor

        public SubPaymentMasterController(
            IRepositoryPainting<SubPaymentMaster> repo,
            IRepositoryPainting<SubPaymentDetail> repoSubPaymentDetail,
            IRepositoryPainting<PaymentDetail> repoPaymentDetail,
            IRepositoryPainting<PaintTeam> repoPaintTeam,
            IRepositoryPainting<PaintTaskDetail> repoPaintTaskDetail,
            IRepositoryMachine<Employee> repoEmp,
            IRepositoryMachine<ProjectCodeMaster> repoProject,
            IMapper map)
        {
            // Repository Painting
            this.repository = repo;
            this.repositoryPaintTeam = repoPaintTeam;
            this.repositorySubPaymentDetail = repoSubPaymentDetail;
            this.repositoryPaymentDetail = repoPaymentDetail;
            this.repositoryPaintTaskDetail = repoPaintTaskDetail;
            // Repository Machie
            this.repositoryEmp = repoEmp;
            this.repositoryProject = repoProject;
            // Mapper
            this.mapper = map;
            //Helpers
            this.helpers = new HelpersClass<SubPaymentMaster>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/SubPaymentMaster
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            //var Includes = new List<string> { "StandradTimeInt", "StandradTimeExt", "SurfaceTypeInt", "SurfaceTypeExt" };

            //return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastWorkItemViewModel, SubPaymentMaster>
            //                     (await this.repository.GetAllWithInclude2Async(Includes)),
            //                      this.DefaultJsonSettings);
        }

        // GET: api/SubPaymentMaster/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "PaintTeam" };

            var GetData = this.mapper.Map<SubPaymentMaster, SubPaymentMasterViewModel>
                            (await this.repository.GetAsynvWithIncludes(key, "SubPaymentMasterId", Includes));

            if (GetData != null)
                GetData = await this.ModelGetViewModelNoRelation(GetData);

            return new JsonResult(GetData, this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST

        // POST: api/SubPaymentMaster/CaleSubPaymentMaster
        [HttpPost("CalclateSubPaymentMaster")]
        public async Task<IActionResult> CalclateSubPaymentMaster([FromBody]SubPaymentMaster CurrentSubPayment)
        {
            var message = "Not beed found data.";

            try
            {
                if (CurrentSubPayment != null)
                {
                    if (CurrentSubPayment.ProjectCodeMasterId > 0 && CurrentSubPayment.PaintTeamId > 0)
                    {
                        #region PaintProgress
                        // get WorkLoad for paint team and project
                        var QueryPaintData = this.repositoryPaintTaskDetail.GetAllAsQueryable()
                                            .Where(x => x.PaintTeamId == CurrentSubPayment.PaintTeamId &&
                                                        x.PaintTaskMaster.RequirePaintingList
                                                         .RequirePaintingMaster.ProjectCodeSub
                                                         .ProjectCodeMasterId == CurrentSubPayment.ProjectCodeMasterId &&
                                                        x.PaintTaskMaster.PaintTaskStatus != PaintTaskStatus.Cancel &&
                                                        x.PaymentDetailId != null &&
                                                        x.PaintWorkItemId != null)
                                            .Include(x => x.PaintTaskMaster)
                                            .Include(x => x.PaintWorkItem)
                                            .Include(x => x.PaymentDetail);

                        var DataPaint = await QueryPaintData.Select(x => new CalclateProgressViewModel
                        {
                            PaymentDetailId = x.PaymentDetailId,
                            Description = x.PaymentDetail.Description,
                            LastCost = x.PaymentDetail.LastCost,
                            PaintTaskDetailId = x.PaintTaskDetailId,
                            AreaPaintIn = x.PaintWorkItem != null && x.PaintTaskDetailLayer == PaintTaskDetailLayer.Internal ?
                                            (x.PaintWorkItem.IntArea ?? 0) * ((x.TaskDetailProgress ?? 0) / 100) : 0,
                            AreaPaintEx = x.PaintWorkItem != null && x.PaintTaskDetailLayer == PaintTaskDetailLayer.External ?
                                            (x.PaintWorkItem.IntArea ?? 0) * ((x.TaskDetailProgress ?? 0) / 100) : 0,
                        }).ToListAsync();
                        #endregion

                        #region BlastProgress
                        // get WorkLoad for paint team and project
                        var QueryBlastData = this.repositoryPaintTaskDetail.GetAllAsQueryable()
                                            .Where(x => x.BlastRoom.PaintTeamId == CurrentSubPayment.PaintTeamId &&
                                                        x.PaintTaskMaster.RequirePaintingList
                                                         .RequirePaintingMaster.ProjectCodeSub
                                                         .ProjectCodeMasterId == CurrentSubPayment.ProjectCodeMasterId &&
                                                        x.PaintTaskMaster.PaintTaskStatus != PaintTaskStatus.Cancel &&
                                                        x.PaymentDetailId != null &&
                                                        x.BlastWorkItemId != null)
                                            .Include(x => x.PaintTaskMaster)
                                            .Include(x => x.BlastRoom)
                                            .Include(x => x.BlastWorkItem)
                                            .Include(x => x.PaymentDetail);

                        var DataBlast = await QueryBlastData.Select(x => new CalclateProgressViewModel
                        {
                            PaymentDetailId = x.PaymentDetailId,
                            Description = x.PaymentDetail.Description,
                            LastCost = x.PaymentDetail.LastCost,
                            PaintTaskDetailId = x.PaintTaskDetailId,
                            AreaBlastIn = x.BlastWorkItem != null && x.PaintTaskDetailLayer == PaintTaskDetailLayer.Internal ?
                                            (x.BlastWorkItem.IntArea ?? 0) * ((x.TaskDetailProgress ?? 0) / 100) : 0,
                            AreaBlastEx = x.BlastWorkItem != null && x.PaintTaskDetailLayer == PaintTaskDetailLayer.External ?
                                            (x.BlastWorkItem.ExtArea ?? 0) * ((x.TaskDetailProgress ?? 0) / 100) : 0,
                        }).ToListAsync();
                        #endregion



                        var dbData = new List<CalclateProgressViewModel>();
                        dbData.AddRange(DataPaint.Where(x => x.AreaPaintEx > 0 || x.AreaPaintIn > 0));
                        dbData.AddRange(DataBlast.Where(x => x.AreaBlastEx > 0 || x.AreaBlastIn > 0));

                        if (dbData.Any())
                        {
                            Expression<Func<SubPaymentMaster, bool>> condition =
                                   p => p.PaintTeamId == CurrentSubPayment.PaintTeamId &&
                                   p.ProjectCodeMasterId == CurrentSubPayment.ProjectCodeMasterId;

                            // any subpayment for project and paint team
                            if (await this.repository.AnyDataAsync(condition))
                            {
                                var DbSubPayment = (await this.repositorySubPaymentDetail.GetAllAsQueryable()
                                                            .Where(x => x.SubPaymentMaster.PaintTeamId == CurrentSubPayment.PaintTeamId &&
                                                                        x.SubPaymentMaster.ProjectCodeMasterId == CurrentSubPayment.ProjectCodeMasterId)
                                                            .GroupBy(x => x.PaymentDetailId)
                                                            .ToListAsync())
                                                            .Select(x => new
                                                            {
                                                                PaymentDetailId = x.Key.Value,
                                                                TotalArea = x.Sum(z => z.AreaWorkLoad),
                                                            });

                                var SubPaymentDetails = dbData.GroupBy(x => x.PaymentDetailId)
                                                            .Select(x => new SubPaymentDetailViewModel
                                                            {
                                                                AreaWorkLoad = DbSubPayment.FirstOrDefault(z => z.PaymentDetailId == x.Key) != null ?
                                                                    x.Sum(y => (y.AreaBlastIn ?? 0) + (y.AreaBlastEx ?? 0) + (y.AreaPaintIn ?? 0) + (y.AreaPaintEx ?? 0)) -
                                                                    (DbSubPayment.FirstOrDefault(z => z.PaymentDetailId == x.Key).TotalArea ?? 0) :
                                                                    x.Sum(y => (y.AreaBlastIn ?? 0) + (y.AreaBlastEx ?? 0) + (y.AreaPaintIn ?? 0) + (y.AreaPaintEx ?? 0)),
                                                                CurrentCost = x.Average(y => y.LastCost),
                                                                PaymentDetailId = x.Key,
                                                                PaymentDetailString = x.FirstOrDefault().Description ?? "-"
                                                            }).ToList();

                                return new JsonResult(SubPaymentDetails.Where(x => x.AreaWorkLoad > 0), this.DefaultJsonSettings);
                            }
                            else
                            {
                                var SubPaymentDetails = dbData.GroupBy(x => x.PaymentDetailId)
                                                            .Select(x => new SubPaymentDetailViewModel
                                                            {
                                                                AreaWorkLoad = x.Sum(y => (y.AreaBlastIn ?? 0) + (y.AreaBlastEx ?? 0) + (y.AreaPaintIn ?? 0) + (y.AreaPaintEx ?? 0)),
                                                                CurrentCost = x.Average(y => y.LastCost),
                                                                PaymentDetailId = x.Key,
                                                                PaymentDetailString = x.FirstOrDefault().Description ?? "-"
                                                            }).ToList();

                                return new JsonResult(SubPaymentDetails, this.DefaultJsonSettings);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = $"Has error {ex.ToString()}";
            }
            return NotFound(new { Error = message });
        }

        // POST: api/SubPaymentMaster/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                           .Include(x => x.PaintTeam)
                                           .AsQueryable();

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.PaintTeam.TeamName.ToLower().Contains(keyword) &&
                                                 x.Remark.ToLower().Contains(keyword) &&
                                                 x.SubPaymentNo.ToLower().Contains(keyword));
            }
            // Order
            switch (Scroll.SortField)
            {
                case "SubPaymentNo":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.SubPaymentNo);
                    else
                        QueryData = QueryData.OrderBy(e => e.SubPaymentNo);
                    break;

                case "PaintTeamString":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.PaintTeam.TeamName);
                    else
                        QueryData = QueryData.OrderBy(e => e.PaintTeam.TeamName);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.SubPaymentDate);
                    break;
            }
            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);
            var DataViewModel = this.ConvertTable.ConverterTableToViewModel<SubPaymentMasterViewModel, SubPaymentMaster>
                                    (await QueryData.AsNoTracking().ToListAsync());

            var DataViewModel2 = new List<SubPaymentMasterViewModel>();

            if (DataViewModel.Any())
            {
                foreach (var item in DataViewModel)
                    DataViewModel2.Add(await this.ModelGetViewModelNoRelation(item));
            }

            return new JsonResult
                (new ScrollDataViewModel<SubPaymentMasterViewModel>
                (Scroll, DataViewModel2.Any() ? DataViewModel2 : DataViewModel), this.DefaultJsonSettings);
        }

        // POST: api/SubPaymentMaster
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SubPaymentMaster nSubPaymentMaster)
        {
            if (nSubPaymentMaster != null && nSubPaymentMaster.SubPaymentDetails != null)
            {
                nSubPaymentMaster = this.helpers.AddHourMethod(nSubPaymentMaster);
                nSubPaymentMaster.SubPaymentNo = await this.GeneratedCode(nSubPaymentMaster.ProjectCodeMasterId.Value,
                                                                         nSubPaymentMaster.PaintTeamId.Value);

                nSubPaymentMaster.CreateDate = DateTime.Now;
                nSubPaymentMaster.Creator = nSubPaymentMaster.Creator ?? "Someone";
                // Sub payment detail
                foreach (var nSubPaymentDetail in nSubPaymentMaster.SubPaymentDetails)
                {
                    if (nSubPaymentDetail == null)
                        continue;

                    nSubPaymentDetail.CreateDate = DateTime.Now;
                    nSubPaymentDetail.Creator = nSubPaymentMaster.Creator ?? "Someone";
                    //Date change timezone
                    if (nSubPaymentDetail.PaymentDate.HasValue)
                        nSubPaymentDetail.PaymentDate = this.ChangeTimeZone(nSubPaymentDetail.PaymentDate.Value);
                    else
                        nSubPaymentDetail.PaymentDate = DateTime.Now;
                    //Relation
                }

                return new JsonResult(await this.repository.AddAsync(nSubPaymentMaster), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found sub payment master data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/SubPaymentMaster/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]SubPaymentMaster uSubPaymentMaster)
        {
            var Message = "Sub payment master not been found.";

            if (uSubPaymentMaster != null && uSubPaymentMaster.SubPaymentDetails != null)
            {
                uSubPaymentMaster = this.helpers.AddHourMethod(uSubPaymentMaster);
                // set modified
                uSubPaymentMaster.ModifyDate = DateTime.Now;
                uSubPaymentMaster.Modifyer = uSubPaymentMaster.Modifyer ?? "Someone";

                foreach (var uSubPaymentDetail in uSubPaymentMaster.SubPaymentDetails)
                {
                    if (uSubPaymentDetail == null)
                        continue;

                    if (uSubPaymentDetail.SubPaymentDetailId > 0)
                    {
                        uSubPaymentDetail.ModifyDate = uSubPaymentMaster.ModifyDate;
                        uSubPaymentDetail.Modifyer = uSubPaymentMaster.Modifyer;
                        uSubPaymentDetail.PaymentDate = this.ChangeTimeZone(uSubPaymentDetail.PaymentDate.Value);
                    }
                    else
                    {
                        uSubPaymentDetail.CreateDate = uSubPaymentMaster.ModifyDate;
                        uSubPaymentDetail.Creator = uSubPaymentMaster.Modifyer;
                        if (uSubPaymentDetail.PaymentDate.HasValue)
                            uSubPaymentDetail.PaymentDate = this.ChangeTimeZone(uSubPaymentDetail.PaymentDate.Value);
                        else
                            uSubPaymentDetail.PaymentDate = DateTime.Now;
                    }
                }

                // update Master not update Detail it need to update Detail directly
                var updateComplate = await this.repository.UpdateAsync(uSubPaymentMaster, key);
                if (updateComplate != null)
                {
                    // filter
                    Expression<Func<SubPaymentDetail, bool>> conditionSubDetail = m => m.SubPaymentMasterId == key;
                    var dbSubDetails = this.repositorySubPaymentDetail.FindAll(conditionSubDetail);

                    //Remove SubPaymentDetail if edit remove it
                    foreach (var dbSubDetail in dbSubDetails)
                    {
                        if (!uSubPaymentMaster.SubPaymentDetails.Any(x => x.SubPaymentDetailId == dbSubDetail.SubPaymentDetailId))
                            await this.repositorySubPaymentDetail.DeleteAsync(dbSubDetail.SubPaymentDetailId);
                    }

                    //Update SubPaymentDetail or New SubPaymentDetail
                    foreach (var uSubPaymentDetail in uSubPaymentMaster.SubPaymentDetails)
                    {
                        if (uSubPaymentDetail == null)
                            continue;

                        if (uSubPaymentDetail.SubPaymentDetailId > 0)
                            await this.repositorySubPaymentDetail.UpdateAsync(uSubPaymentDetail, uSubPaymentDetail.SubPaymentDetailId);
                        else
                        {
                            if (uSubPaymentDetail.SubPaymentMasterId is null || uSubPaymentDetail.SubPaymentMasterId < 1)
                                uSubPaymentDetail.SubPaymentMasterId = uSubPaymentMaster.SubPaymentMasterId;

                            await this.repositorySubPaymentDetail.AddAsync(uSubPaymentDetail);
                        }
                    }
                }

                return new JsonResult(updateComplate, this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/SubPaymentMaster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE

        #region Report

        // GET: api/GetReports/5
        [HttpGet("GetReports/{SubPaymentMasterId}")]
        public async Task<IActionResult> GetReports(int SubPaymentMasterId)
        {
            string Message = "Not found been sub payment master.";
            try
            {
                if (SubPaymentMasterId > 0)
                {
                    var dbData = await this.repository.GetAllAsQueryable()
                                                        .Include(x => x.SubPaymentDetails)
                                                        .Include(x => x.PaintTeam)
                                                        .FirstOrDefaultAsync(x => x.SubPaymentMasterId == SubPaymentMasterId);

                    var dbDataAll = await this.repositorySubPaymentDetail.GetAllAsQueryable()
                                                .Where(x => x.SubPaymentMaster.PaintTeamId == dbData.PaintTeamId &&
                                                            x.SubPaymentMaster.ProjectCodeMasterId == dbData.ProjectCodeMasterId &&
                                                            x.SubPaymentMasterId < dbData.SubPaymentMasterId)
                                                .ToListAsync();

                    if (dbData != null)
                    {
                        // Get ReportOverTimeMaster
                        var ReportSubPayment = new ReportSubPaymentMasterViewModel
                        {
                            SubPaymentNo = dbData.SubPaymentNo,
                            SubPaymentDate = dbData.SubPaymentDate.Value.ToString("dd / MM / yyyy"),
                            TotalArea = dbData.SubPaymentDetails.Sum(x => (x.AreaWorkLoad ?? 0) + (x.AdditionArea ?? 0)),
                            TotalCost = dbData.SubPaymentDetails.Sum(x => (x.CalcCost ?? 0) + (x.AdditionCost ?? 0)),
                            TotalAllArea = dbDataAll?.Sum(x => (x.AreaWorkLoad ?? 0) + (x.AdditionArea ?? 0)) ?? 0,
                            TotalAllCost = dbDataAll?.Sum(x => (x.CalcCost ?? 0) + (x.AdditionCost ?? 0)) ?? 0,
                            Approved1 = !string.IsNullOrEmpty(dbData.EmpApproved1) ? (await this.repositoryEmp.GetAsync(dbData.EmpApproved1))?.NameThai ?? "-" : "-",
                            Approved2 = !string.IsNullOrEmpty(dbData.EmpApproved2) ? (await this.repositoryEmp.GetAsync(dbData.EmpApproved2))?.NameThai ?? "-" : "-",
                            // Detail
                            Details = new List<ReportSubPaymentDetailViewModel>()
                        };

                        var dbPaymentDetail = await this.repositoryPaymentDetail.GetAllAsync();
                        var RuningNumber = 1;
                        foreach(var item in dbPaymentDetail.OrderBy(x => x.PaymentDetailId))
                        {
                            var subPayment = dbData.SubPaymentDetails.FirstOrDefault(x => x.PaymentDetailId == item.PaymentDetailId);

                            ReportSubPayment.Details.Add(new ReportSubPaymentDetailViewModel()
                            {
                                RowNumber = RuningNumber,
                                Description = item.Description,
                                Cost = item.LastCost,
                                AreaWorkLoad = subPayment != null ? subPayment.AreaWorkLoad ?? 0 : 0,
                                CalcCost = subPayment != null ? subPayment.CalcCost ?? 0 : 0,
                                AreaTotal = dbDataAll.Where(x => x.PaymentDetailId == item.PaymentDetailId)?.Sum(x => (x.AreaWorkLoad ?? 0) + (x.AdditionArea ?? 0)) ?? 0,
                                CostTotal = dbDataAll.Where(x => x.PaymentDetailId == item.PaymentDetailId)?.Sum(x => (x.CalcCost ?? 0) + (x.AdditionCost ?? 0)) ?? 0,
                            });

                            RuningNumber++;
                        }

                        // Get ReportOverTimeDetail
                        return new JsonResult(ReportSubPayment, this.DefaultJsonSettings);
                    }
                }

            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }
            return NotFound(new { Error = Message });

            //return new ContentResult();
        }

        #endregion
    }
}