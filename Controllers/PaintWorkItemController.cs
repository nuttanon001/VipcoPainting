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
    [Route("api/PaintWorkItem")]
    public class PaintWorkItemController : Controller
    {
        #region PrivateMenbers
        // Repository
        private IRepositoryPainting<PaintWorkItem> repository;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;

        #endregion PrivateMenbers

        #region Constructor

        public PaintWorkItemController(
            IRepositoryPainting<PaintWorkItem> repo, IMapper map)
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

        // GET: api/PaintWorkItem
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "StandradTimeInt", "StandradTimeExt", "ExtColorItem", "IntColorItem" };

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<PaintWorkItemViewModel, PaintWorkItem>
                                 (await this.repository.GetAllWithInclude2Async(Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/PaintWorkItem/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "StandradTimeInt", "StandradTimeExt", "ExtColorItem", "IntColorItem" };

            return new JsonResult(this.mapper.Map<PaintWorkItem, PaintWorkItemViewModel>
                                 (await this.repository.GetAsynvWithIncludes(key, "BlastWorkItemId", Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/PaintWorkItem/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.RequirePaintingListId == MasterId)
                                .Include(x => x.StandradTimeExt)
                                .Include(x => x.StandradTimeInt)
                                .Include(x => x.ExtColorItem)
                                .Include(x => x.IntColorItem);

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<PaintWorkItemViewModel, PaintWorkItem>
                                 (await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }
        #endregion GET

        #region POST

        // POST: api/PaintWorkItem
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PaintWorkItem nBlastWorkItem)
        {
            if (nBlastWorkItem != null)
            {
                nBlastWorkItem.CreateDate = DateTime.Now;
                nBlastWorkItem.Creator = nBlastWorkItem.Creator ?? "Someone";

                if (nBlastWorkItem.IntColorItem != null)
                    nBlastWorkItem.IntColorItem = null;

                if (nBlastWorkItem.ExtColorItem != null)
                    nBlastWorkItem.ExtColorItem = null;

                if (nBlastWorkItem.StandradTimeExt != null)
                    nBlastWorkItem.StandradTimeExt = null;

                if (nBlastWorkItem.StandradTimeInt != null)
                    nBlastWorkItem.StandradTimeInt = null;

                return new JsonResult(await this.repository.AddAsync(nBlastWorkItem), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found PaintWorkItem data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/PaintWorkItem/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]PaintWorkItem uBlastWorkItem)
        {
            var Message = "Require painting not been found.";

            if (uBlastWorkItem != null)
            {
                // set modified
                uBlastWorkItem.ModifyDate = DateTime.Now;
                uBlastWorkItem.Modifyer = uBlastWorkItem.Modifyer ?? "Someone";

                if (uBlastWorkItem.ExtColorItem != null)
                    uBlastWorkItem.ExtColorItem = null;

                if (uBlastWorkItem.IntColorItem != null)
                    uBlastWorkItem.IntColorItem = null;

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

        // DELETE: api/PaintWorkItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
