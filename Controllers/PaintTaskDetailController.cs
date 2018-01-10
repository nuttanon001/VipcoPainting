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
    [Route("api/PaintTaskDetail")]
    public class PaintTaskDetailController : Controller
    {
        // GET: api/PaintTaskDetail
        #region PrivateMembers
        // Repository
        private IRepositoryPainting<PaintTaskDetail> repository;
        private IRepositoryPainting<BlastRoom> repositoryBlastRoom;
        private IRepositoryPainting<RequisitionMaster> repositoryRequisition;
        private IRepositoryMachine<ProjectCodeMaster> repositoryProMaster;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        #endregion PrivateMenbers

        #region Constructor
        public PaintTaskDetailController(
            IRepositoryPainting<PaintTaskDetail> repo, 
            IRepositoryPainting<RequisitionMaster> repoRequisition,
            IRepositoryPainting<BlastRoom> repoBlastRoom,
            IRepositoryMachine<ProjectCodeMaster> repoProMaster,
            IMapper map)
        {
            // Repository
            this.repository = repo;
            this.repositoryBlastRoom = repoBlastRoom;
            this.repositoryRequisition = repoRequisition;
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

        // GET: api/PaintTaskDetail
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "PaintTeam", "PaintWorkItem","BlastRoom","BlastWorkItem" };

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<PaintTaskDetailViewModel, PaintTaskDetail>
                                 (await this.repository.GetAllWithInclude2Async(Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/PaintTaskDetail/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "PaintTeam", "PaintWorkItem", "BlastRoom", "BlastWorkItem" };

            return new JsonResult(this.mapper.Map<PaintTaskDetail, PaintTaskDetailViewModel>
                                 (await this.repository.GetAsynvWithIncludes(key, "PaintTaskDetailId", Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/PaintTaskDetail/WithCustom/5
        [HttpGet("WithCustom/{key}")]
        public async Task<IActionResult> GetWithCustom(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "PaintWorkItem", "PaintTaskMaster" };
            var HasData = this.mapper.Map<PaintTaskDetail, PaintTaskDetailViewModel>
                                 (await this.repository.GetAsynvWithIncludes(key, "PaintTaskDetailId", Includes));
            
            if (HasData != null)
            {
                HasData.CommonText = HasData?.PaintTaskMaster?.TaskPaintNo;
                HasData.CommonText += " | " + (HasData.PaintTaskDetailLayer == PaintTaskDetailLayer.Internal ? "Internal" : "External");
                if (HasData.PaintWorkItem != null)
                {
                    HasData.CommonText =  " | " + (HasData.PaintWorkItem.PaintLevel == PaintLevel.PrimerCoat ? "PrimerCoat" :
                                         (HasData.PaintWorkItem.PaintLevel == PaintLevel.MidCoat ? "MidCoat" :
                                         (HasData.PaintWorkItem.PaintLevel == PaintLevel.IntermediateCoat ? "IntermediateCoat" : "TopCoat")));
                }

                return new JsonResult(HasData, this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "" });
        }

        // GET: api/PaintTaskDetail/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.PaintTaskMasterId == MasterId)
                                .Include(x => x.PaintTeam)
                                .Include(x => x.PaintWorkItem.IntColorItem)
                                .Include(x => x.PaintWorkItem.ExtColorItem)
                                .Include(x => x.BlastRoom)
                                .Include(x => x.BlastWorkItem.SurfaceTypeInt)
                                .Include(x => x.BlastWorkItem.SurfaceTypeExt)
                                .Include(x => x.RequisitionMasters);

            var GetData = this.ConvertTable.ConverterTableToViewModel<PaintTaskDetailViewModel, PaintTaskDetail>
                            (await QueryData.AsNoTracking().ToListAsync());
            foreach (var item in GetData)
            {
                item.PaintWorkItem = this.mapper.Map<PaintWorkItemViewModel>(item.PaintWorkItem);
                item.BlastWorkItem = this.mapper.Map<BlastWorkItemViewModel>(item.BlastWorkItem);
                item.SummaryActual = item?.RequisitionMasters.Sum(x => x.Quantity ?? 0) ?? 0;
                item.RequisitionMasters = null;
            }
            if (GetData.Any())
                return new JsonResult(GetData, this.DefaultJsonSettings);
            else
                return NotFound(new { Error = "Paint task detail not been found." });
        }

        // GET: api/PaintTaskDetail/GetRequisitionSum/5
        [HttpGet("GetRequisitionSum/{key}")]
        public async Task<IActionResult> GetRequisitionSum(int key)
        {
            var QueryData = this.repositoryRequisition.GetAllAsQueryable()
                               .Where(x => x.PaintTaskDetailId == key);

            return new JsonResult(new { TotalSummary = await QueryData.SumAsync(x => x.Quantity ?? 0) }, this.DefaultJsonSettings);
        }
        #endregion GET

        #region POST

        // POST: api/PaintTaskDetail
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PaintTaskDetail nPaintTaskDetail)
        {
            if (nPaintTaskDetail != null)
            {
                nPaintTaskDetail.CreateDate = DateTime.Now;
                nPaintTaskDetail.Creator = nPaintTaskDetail.Creator ?? "Someone";

                if (nPaintTaskDetail.PaintTeam != null)
                    nPaintTaskDetail.PaintTeam = null;

                if (nPaintTaskDetail.PaintWorkItem != null)
                    nPaintTaskDetail.PaintWorkItem = null;

                return new JsonResult(await this.repository.AddAsync(nPaintTaskDetail), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found paint task detail data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/PaintTaskDetail/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]PaintTaskDetail uPaintTaskDetail)
        {
            var Message = "Paint task detail not been found.";

            if (uPaintTaskDetail != null)
            {
                // set modified
                uPaintTaskDetail.ModifyDate = DateTime.Now;
                uPaintTaskDetail.Modifyer = uPaintTaskDetail.Modifyer ?? "Someone";

                if (uPaintTaskDetail.PaintTeam != null)
                    uPaintTaskDetail.PaintTeam = null;

                if (uPaintTaskDetail.PaintWorkItem != null)
                    uPaintTaskDetail.PaintWorkItem = null;

                return new JsonResult(await this.repository.UpdateAsync(uPaintTaskDetail, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/PaintTaskDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE

        #region REPORT

        // GET: api/GetReportPatinTaskDetailPaintPdf/5
        [HttpGet("GetReportPatinTaskDetailPaintPdf/{PaintTaskDetailId}")]
        public async Task<IActionResult> GetReportPatinTaskDetailPaintPdf(int PaintTaskDetailId)
        {
            string Message = "Not found overtime masterid.";
            try
            {
                if (PaintTaskDetailId > 0)
                {
                    var QueryData = await this.repository.GetAllAsQueryable()
                                                         .Include(x => x.RequisitionMasters)
                                                         .Include(x => x.PaintTeam)
                                                         .Include(x => x.PaintWorkItem)
                                                         .Include(x => x.PaintTaskMaster.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub)
                                                         .FirstOrDefaultAsync(x => x.PaintTaskDetailId == PaintTaskDetailId);

                    if (QueryData != null)
                    {
                        var JobNumber = (await this.repositoryProMaster.GetAsync(QueryData.PaintTaskMaster.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub.ProjectCodeMasterId ?? 0))?.ProjectCode ?? "";
                        var JobName = $"{QueryData.PaintTaskMaster.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub.Code}";

                        // Check year Thai
                        string year = QueryData.PaintTaskMaster.AssignDate.Value.Year > 2500 ?
                                      QueryData.PaintTaskMaster.AssignDate.Value.Year.ToString() :
                                     (QueryData.PaintTaskMaster.AssignDate.Value.Year + 543).ToString();

                        string LevelString = QueryData.PaintWorkItem.PaintLevel == PaintLevel.PrimerCoat ? "PrimerCoat" :
                                            (QueryData.PaintWorkItem.PaintLevel == PaintLevel.MidCoat ? "MidCoat" :
                                            (QueryData.PaintWorkItem.PaintLevel == PaintLevel.IntermediateCoat ? "IntermediateCoat" : "TopCoat"));

                        // Get ReportOverTimeMaster
                        var ReportPaintTaskDetail = new 
                        {
                            JobNumber = JobNumber,
                            PaintDate = QueryData?.PaintTaskMaster?.AssignDate.Value.ToString("dd/MM/yy"),
                            JobName = JobName,
                            DocNo = QueryData?.PaintTaskMaster?.TaskPaintNo,
                            NPT = QueryData?.PaintTeam?.TeamName.ToLower().Contains("npt"),
                            BPS = QueryData?.PaintTeam?.TeamName.ToLower().Contains("bps"),
                            VIPCO = QueryData?.PaintTeam?.TeamName.ToLower().Contains("vipco"),
                            ITP = QueryData?.PaintTaskMaster?.RequirePaintingList.ITP,
                            FieldWeld = QueryData?.PaintTaskMaster?.RequirePaintingList.FieldWeld,
                            Schedule = !string.IsNullOrEmpty(QueryData.PaintTaskMaster?.RequirePaintingList?.RequirePaintingMaster?.PaintingSchedule),
                            EstimateTime = $"{QueryData.PlanSDate.ToString("dd/MM/yy")} - {QueryData.PlanEDate.ToString("dd/MM/yy")}" ,
                            ActualTime = $"{QueryData.ActualSDate?.ToString("dd/MM/yy") ?? "NoData"} - {QueryData.ActualEDate?.ToString("dd/MM/yy") ?? "NoData"}",
                            // Detail
                            Details = new []
                            {
                                new
                                {
                                    RowNumber = 1,
                                    PartNo = QueryData?.PaintTaskMaster?.RequirePaintingList?.MarkNo,
                                    PartName = QueryData?.PaintTaskMaster?.RequirePaintingList?.Description,
                                    UnitNo = QueryData?.PaintTaskMaster?.RequirePaintingList?.UnitNo,
                                    Qty = QueryData?.PaintTaskMaster?.RequirePaintingList?.Quantity,
                                    Plan = (QueryData?.PaintTaskDetailLayer == PaintTaskDetailLayer.Internal 
                                            ? QueryData?.PaintWorkItem?.IntCalcColorUsage : QueryData?.PaintWorkItem?.ExtCalcColorUsage) ?? 0,
                                    ActualNo = 1,
                                    ActualQty = QueryData?.RequisitionMasters?.Sum(x => x.Quantity),
                                    Event = "พ่นสี",
                                    Layer = LevelString,
                                    AreaInt = QueryData?.PaintTaskDetailLayer == PaintTaskDetailLayer.Internal ? QueryData?.PaintWorkItem?.IntArea : -1,
                                    AreaExt = QueryData?.PaintTaskDetailLayer == PaintTaskDetailLayer.External ? QueryData?.PaintWorkItem?.ExtArea : -1,
                                }
                            },
                        };

                        // Get ReportOverTimeDetail
                        return new JsonResult(ReportPaintTaskDetail, this.DefaultJsonSettings);
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

        // GET: api/GetReportPatinTaskDetailPaintPdf/5
        [HttpGet("GetReportPatinTaskDetailBlastPdf/{PaintTaskDetailId}")]
        public async Task<IActionResult> GetReportPatinTaskDetailBlastPdf(int PaintTaskDetailId)
        {
            string Message = "Not found overtime masterid.";
            try
            {
                if (PaintTaskDetailId > 0)
                {
                    var QueryData = await this.repository.GetAllAsQueryable()
                                                         .Include(x => x.RequisitionMasters)
                                                         .Include(x => x.BlastRoom.PaintTeam)
                                                         .Include(x => x.BlastWorkItem.SurfaceTypeExt)
                                                         .Include(x => x.BlastWorkItem.SurfaceTypeInt)
                                                         .Include(x => x.PaintTaskMaster.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub)
                                                         .FirstOrDefaultAsync(x => x.PaintTaskDetailId == PaintTaskDetailId);

                    var AllRoom = await this.repositoryBlastRoom.GetAllAsQueryable()
                                                                .Include(x => x.PaintTeam)
                                                                .Select(x => new
                                                                {
                                                                    id = x.BlastRoomNumber,
                                                                    name = x.BlastRoomName + " " + x.PaintTeam.TeamName
                                                                }).ToListAsync();

                    if (QueryData != null && AllRoom != null)
                    {
                        var JobNumber = (await this.repositoryProMaster.GetAsync(QueryData.PaintTaskMaster.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub.ProjectCodeMasterId ?? 0))?.ProjectCode ?? "";
                        var JobName = $"{QueryData.PaintTaskMaster.RequirePaintingList.RequirePaintingMaster.ProjectCodeSub.Code}";

                        // Check year Thai
                        string year = QueryData.PaintTaskMaster.AssignDate.Value.Year > 2500 ?
                                      QueryData.PaintTaskMaster.AssignDate.Value.Year.ToString() :
                                     (QueryData.PaintTaskMaster.AssignDate.Value.Year + 543).ToString();

                        string SurfaceName = QueryData.PaintTaskDetailLayer == PaintTaskDetailLayer.Internal ? 
                            QueryData.BlastWorkItem.SurfaceTypeInt.SurfaceName : QueryData.BlastWorkItem.SurfaceTypeExt.SurfaceName;

                        // Get ReportOverTimeMaster
                        var ReportPaintTaskDetail = new
                        {
                            JobNumber = JobNumber,
                            PaintDate = QueryData?.PaintTaskMaster?.AssignDate.Value.ToString("dd/MM/yy"),
                            JobName = JobName,
                            DocNo = QueryData?.PaintTaskMaster?.TaskPaintNo,
                            R1 = QueryData.BlastRoom.BlastRoomNumber == 1,
                            R2 = QueryData.BlastRoom.BlastRoomNumber == 2,
                            R3 = QueryData.BlastRoom.BlastRoomNumber == 3,
                            R4 = QueryData.BlastRoom.BlastRoomNumber == 4,
                            R5 = QueryData.BlastRoom.BlastRoomNumber == 5,
                            R1Name = AllRoom.FirstOrDefault(x => x.id == 1).name,
                            R2Name = AllRoom.FirstOrDefault(x => x.id == 2).name,
                            R3Name = AllRoom.FirstOrDefault(x => x.id == 3).name,
                            R4Name = AllRoom.FirstOrDefault(x => x.id == 4).name,
                            R5Name = AllRoom.FirstOrDefault(x => x.id == 5).name,
                            ITP = QueryData?.PaintTaskMaster?.RequirePaintingList.ITP,
                            FieldWeld = QueryData?.PaintTaskMaster?.RequirePaintingList.FieldWeld,
                            Schedule = !string.IsNullOrEmpty(QueryData.PaintTaskMaster?.RequirePaintingList?.RequirePaintingMaster?.PaintingSchedule),
                            EstimateTime = $"{QueryData.PlanSDate.ToString("dd/MM/yy")} - {QueryData.PlanEDate.ToString("dd/MM/yy")}",
                            ActualTime = $"{QueryData.ActualSDate?.ToString("dd/MM/yy") ?? "NoData"} - {QueryData.ActualEDate?.ToString("dd/MM/yy") ?? "NoData"}",
                            // Detail
                            Details = new[]
                            {
                                new
                                {
                                    RowNumber = 1,
                                    PartNo = QueryData?.PaintTaskMaster?.RequirePaintingList?.MarkNo,
                                    PartName = QueryData?.PaintTaskMaster?.RequirePaintingList?.Description,
                                    UnitNo = QueryData?.PaintTaskMaster?.RequirePaintingList?.UnitNo,
                                    Qty = QueryData?.PaintTaskMaster?.RequirePaintingList?.Quantity,
                                    Weight = QueryData?.PaintTaskMaster?.RequirePaintingList.Weight,
                                    SurfaceName = SurfaceName,
                                    AreaInt = QueryData?.PaintTaskDetailLayer == PaintTaskDetailLayer.Internal ? QueryData?.BlastWorkItem?.IntArea : -1,
                                    AreaExt = QueryData?.PaintTaskDetailLayer == PaintTaskDetailLayer.External ? QueryData?.BlastWorkItem?.ExtArea : -1,
                                }
                            },
                        };

                        // Get ReportOverTimeDetail
                        return new JsonResult(ReportPaintTaskDetail, this.DefaultJsonSettings);
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
