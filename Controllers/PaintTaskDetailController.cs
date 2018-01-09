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
        private IRepositoryPainting<RequisitionMaster> repositoryRequisition;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        #endregion PrivateMenbers

        #region Constructor
        public PaintTaskDetailController(
            IRepositoryPainting<PaintTaskDetail> repo, 
            IRepositoryPainting<RequisitionMaster> repoRequisition,
            IMapper map)
        {
            // Repository
            this.repository = repo;
            this.repositoryRequisition = repoRequisition;
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
    }
}
