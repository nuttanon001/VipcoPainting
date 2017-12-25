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
    [Route("api/TaskPaintDetail")]
    public class TaskPaintDetailController : Controller
    {
        #region PrivateMembers
        // Repository
        private IRepositoryPainting<TaskPaintDetail> repository;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        #endregion PrivateMenbers

        #region Constructor
        public TaskPaintDetailController(IRepositoryPainting<TaskPaintDetail> repo, IMapper map)
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

        // GET: api/TaskPaintDetail
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "PaintTeam", "PaintWorkItem" };

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<TaskPaintDetailViewModel, TaskPaintDetail>
                                 (await this.repository.GetAllWithInclude2Async(Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/TaskPaintDetail/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "PaintTeam","PaintWorkItem" };

            return new JsonResult(this.mapper.Map<TaskPaintDetail, TaskPaintDetailViewModel>
                                 (await this.repository.GetAsynvWithIncludes(key, "TaskPaintDetailId", Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/TaskPaintDetail/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.TaskMasterId == MasterId)
                                .Include(x => x.PaintTeam);

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<TaskPaintDetailViewModel, TaskPaintDetail>
                                 (await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }
        #endregion GET

        #region POST

        // POST: api/TaskPaintDetail
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TaskPaintDetail nBlastWorkItem)
        {
            if (nBlastWorkItem != null)
            {
                nBlastWorkItem.CreateDate = DateTime.Now;
                nBlastWorkItem.Creator = nBlastWorkItem.Creator ?? "Someone";

                if (nBlastWorkItem.PaintTeam != null)
                    nBlastWorkItem.PaintTeam = null;

                if (nBlastWorkItem.PaintWorkItem != null)
                    nBlastWorkItem.PaintWorkItem = null;

                return new JsonResult(await this.repository.AddAsync(nBlastWorkItem), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found task blast detail data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/TaskPaintDetail/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]TaskPaintDetail uBlastWorkItem)
        {
            var Message = "Task blast detail not been found.";

            if (uBlastWorkItem != null)
            {
                // set modified
                uBlastWorkItem.ModifyDate = DateTime.Now;
                uBlastWorkItem.Modifyer = uBlastWorkItem.Modifyer ?? "Someone";

                if (uBlastWorkItem.PaintTeam != null)
                    uBlastWorkItem.PaintTeam = null;

                if (uBlastWorkItem.PaintWorkItem != null)
                    uBlastWorkItem.PaintWorkItem = null;

                return new JsonResult(await this.repository.UpdateAsync(uBlastWorkItem, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/TaskPaintDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
