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
    [Route("api/TaskBlastDetail")]
    public class TaskBlastDetailController : Controller
    {
        #region PrivateMembers
        // Repository
        private IRepositoryPainting<TaskBlastDetail> repository;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        #endregion PrivateMenbers

        #region Constructor
        public TaskBlastDetailController(IRepositoryPainting<TaskBlastDetail> repo, IMapper map)
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

        // GET: api/TaskBlastDetail
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "BlastRoom.PaintTeam", "BlastWorkItem" };

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<TaskBlastDetailViewModel, TaskBlastDetail>
                                 (await this.repository.GetAllWithInclude2Async(Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/TaskBlastDetail/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "BlastRoom.PaintTeam", "BlastWorkItem" };

            return new JsonResult(this.mapper.Map<TaskBlastDetail, TaskBlastDetailViewModel>
                                 (await this.repository.GetAsynvWithIncludes(key, "TaskBlastDetailId", Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/TaskBlastDetail/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.TaskMasterId == MasterId)
                                .Include(x => x.BlastRoom.PaintTeam)
                                .Include(x => x.BlastWorkItem.SurfaceTypeInt)
                                .Include(x => x.BlastWorkItem.SurfaceTypeExt);

            var GetData = this.ConvertTable.ConverterTableToViewModel<TaskBlastDetailViewModel, TaskBlastDetail>
                           (await QueryData.AsNoTracking().ToListAsync());
            foreach (var item in GetData)
                item.BlastWorkItem = this.mapper.Map<BlastWorkItemViewModel>(item.BlastWorkItem);

            return new JsonResult(GetData, this.DefaultJsonSettings);
        }
        #endregion GET

        #region POST

        // POST: api/TaskBlastDetail
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TaskBlastDetail nBlastWorkItem)
        {
            if (nBlastWorkItem != null)
            {
                nBlastWorkItem.CreateDate = DateTime.Now;
                nBlastWorkItem.Creator = nBlastWorkItem.Creator ?? "Someone";

                if (nBlastWorkItem.BlastRoom != null)
                    nBlastWorkItem.BlastRoom = null;

                if (nBlastWorkItem.BlastWorkItem != null)
                    nBlastWorkItem.BlastWorkItem = null;

                return new JsonResult(await this.repository.AddAsync(nBlastWorkItem), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found task blast detail data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/TaskBlastDetail/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]TaskBlastDetail uBlastWorkItem)
        {
            var Message = "Task blast detail not been found.";

            if (uBlastWorkItem != null)
            {
                // set modified
                uBlastWorkItem.ModifyDate = DateTime.Now;
                uBlastWorkItem.Modifyer = uBlastWorkItem.Modifyer ?? "Someone";

                if (uBlastWorkItem.BlastRoom != null)
                    uBlastWorkItem.BlastRoom = null;

                if (uBlastWorkItem.BlastWorkItem != null)
                    uBlastWorkItem.BlastWorkItem = null;

                return new JsonResult(await this.repository.UpdateAsync(uBlastWorkItem, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/TaskBlastDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
