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
    [Route("api/BlastWorkItem")]
    public class BlastWorkController : Controller
    {
        #region PrivateMenbers
        // Repository
        private IRepositoryPainting<BlastWorkItem> repository;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;

        #endregion PrivateMenbers

        #region Constructor

        public BlastWorkController(
            IRepositoryPainting<BlastWorkItem> repo, IMapper map)
        {
            // Repository
            this.repository = repo;
            // Mapper
            this.mapper = map;
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/BlastWorkItem
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "StandradTimeInt", "StandradTimeExt", "SurfaceTypeInt", "SurfaceTypeExt" };

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastWorkItemViewModel,BlastWorkItem>
                                 (await this.repository.GetAllWithInclude2Async(Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/BlastWorkItem/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "StandradTimeInt", "StandradTimeExt", "SurfaceTypeInt", "SurfaceTypeExt" };

            return new JsonResult(this.mapper.Map<BlastWorkItem,BlastWorkItemViewModel>
                                 (await this.repository.GetAsynvWithIncludes(key, "BlastWorkItemId", Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/BlastWorkItem/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.RequirePaintingListId == MasterId)
                                .Include(x => x.StandradTimeExt)
                                .Include(x => x.StandradTimeInt)
                                .Include(x => x.SurfaceTypeExt)
                                .Include(x => x.SurfaceTypeInt);

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastWorkItemViewModel,BlastWorkItem>
                                 (await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }

        // GET: api/BlastWorkItem/GetByMaster/5
        [HttpGet("GetByMaster2/{MasterId}")]
        public async Task<IActionResult> GetByMaster2(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.InitialRequireId == MasterId)
                                .Include(x => x.StandradTimeExt)
                                .Include(x => x.StandradTimeInt)
                                .Include(x => x.SurfaceTypeExt)
                                .Include(x => x.SurfaceTypeInt);

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastWorkItemViewModel, BlastWorkItem>
                                 (await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }
        #endregion GET

        #region POST

        // POST: api/BlastWorkItem
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]BlastWorkItem nBlastWorkItem)
        {
            if (nBlastWorkItem != null)
            {
                nBlastWorkItem.CreateDate = DateTime.Now;
                nBlastWorkItem.Creator = nBlastWorkItem.Creator ?? "Someone";

                if (nBlastWorkItem.SurfaceTypeInt != null)
                    nBlastWorkItem.SurfaceTypeInt = null;

                if (nBlastWorkItem.SurfaceTypeExt != null)
                    nBlastWorkItem.SurfaceTypeExt = null;

                if (nBlastWorkItem.StandradTimeExt != null)
                    nBlastWorkItem.StandradTimeExt = null;

                if (nBlastWorkItem.StandradTimeInt != null)
                    nBlastWorkItem.StandradTimeInt = null;

                return new JsonResult(await this.repository.AddAsync(nBlastWorkItem), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found BlastWork data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/BlastWorkItem/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]BlastWorkItem uBlastWorkItem)
        {
            var Message = "Require painting not been found.";

            if (uBlastWorkItem != null)
            {
                // set modified
                uBlastWorkItem.ModifyDate = DateTime.Now;
                uBlastWorkItem.Modifyer = uBlastWorkItem.Modifyer ?? "Someone";

                if (uBlastWorkItem.SurfaceTypeInt != null)
                    uBlastWorkItem.SurfaceTypeInt = null;

                if (uBlastWorkItem.SurfaceTypeExt != null)
                    uBlastWorkItem.SurfaceTypeExt = null;

                if (uBlastWorkItem.StandradTimeExt != null)
                    uBlastWorkItem.StandradTimeExt = null;

                if (uBlastWorkItem.StandradTimeInt != null)
                    uBlastWorkItem.StandradTimeInt = null;

                return new JsonResult(await this.repository.UpdateAsync(uBlastWorkItem, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/BlastWorkItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
