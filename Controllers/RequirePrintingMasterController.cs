﻿using AutoMapper;
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
            if (ProjectSubId > 0 )
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
                }
            }

            return new JsonResult(requirePaintMaster,this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST

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
                    this.ConvertTable.ConverterTableToViewModel<RequirePaintingMasterViewModel,RequirePaintingMaster>(await QueryData.AsNoTracking().ToListAsync())), 
                    this.DefaultJsonSettings);
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
