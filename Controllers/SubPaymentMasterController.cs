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
        private IRepositoryPainting<SubPaymentDetail> repositoryPaymentDetail;
        private IRepositoryPainting<PaintTeam> repositoryPaintTeam;
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
        private async Task<string> GeneratedCode(int ProjectMasterId,int PaintTeamId)
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
            IRepositoryPainting<SubPaymentDetail> repoPaymentDetail,
            IRepositoryPainting<PaintTeam> repoPaintTeam,
            IRepositoryMachine<Employee> repoEmp,
            IRepositoryMachine<ProjectCodeMaster> repoProject,
            IMapper map)
        {
            // Repository Painting
            this.repository = repo;
            this.repositoryPaintTeam = repoPaintTeam;
            this.repositoryPaymentDetail = repoPaymentDetail;
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
            var Includes = new List<string> { "PaintTeam"};

            var GetData = this.mapper.Map<SubPaymentMaster, SubPaymentMasterViewModel>
                            (await this.repository.GetAsynvWithIncludes(key, "SubPaymentMasterId", Includes));

            if (GetData != null)
                GetData = await this.ModelGetViewModelNoRelation(GetData);

            return new JsonResult(GetData,this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST
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
                    //Relation
                    nSubPaymentDetail.PaymentCostHistory = null;
                    nSubPaymentDetail.PaintTaskDetail = null;
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
                    }
                    else
                    {
                        uSubPaymentDetail.CreateDate = uSubPaymentMaster.ModifyDate;
                        uSubPaymentDetail.Creator = uSubPaymentMaster.Modifyer;
                    }

                    uSubPaymentDetail.PaymentCostHistory = null;
                    uSubPaymentDetail.PaintTaskDetail = null;
                }

                // update Master not update Detail it need to update Detail directly
                var updateComplate = await this.repository.UpdateAsync(uSubPaymentMaster, key);
                if (updateComplate != null)
                {
                    // filter
                    Expression<Func<SubPaymentDetail, bool>> conditionSubDetail = m => m.SubPaymentMasterId == key;
                    var dbSubDetails = this.repositoryPaymentDetail.FindAll(conditionSubDetail);

                    //Remove SubPaymentDetail if edit remove it
                    foreach (var dbSubDetail in dbSubDetails)
                    {
                        if (!uSubPaymentMaster.SubPaymentDetails.Any(x => x.SubPaymentDetailId == dbSubDetail.SubPaymentDetailId))
                            await this.repositoryPaymentDetail.DeleteAsync(dbSubDetail.SubPaymentDetailId);
                    }

                    //Update SubPaymentDetail or New SubPaymentDetail
                    foreach (var uSubPaymentDetail in uSubPaymentMaster.SubPaymentDetails)
                    {
                        if (uSubPaymentDetail == null)
                            continue;

                        if (uSubPaymentDetail.SubPaymentDetailId > 0)
                            await this.repositoryPaymentDetail.UpdateAsync(uSubPaymentDetail, uSubPaymentDetail.SubPaymentDetailId);
                        else
                        {
                            if (uSubPaymentDetail.SubPaymentMasterId is null || uSubPaymentDetail.SubPaymentMasterId < 1)
                                uSubPaymentDetail.SubPaymentMasterId = uSubPaymentMaster.SubPaymentMasterId;

                            await this.repositoryPaymentDetail.AddAsync(uSubPaymentDetail);
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
    }
}
