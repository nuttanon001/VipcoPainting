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
    [Route("api/BlastRoom")]
    public class BlastRoomController : Controller
    {
        #region PrivateMembers
        // Repository
        private IRepositoryPainting<BlastRoom> repository;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        #endregion PrivateMenbers

        #region Constructor

        public BlastRoomController(IRepositoryPainting<BlastRoom> repo, IMapper map)
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

        // GET: api/BlastRoom
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "PaintTeam" };

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastRoomViewModel, BlastRoom>
                                 (await this.repository.GetAllWithInclude2Async(Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/BlastRoom/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "PaintTeam"};

            return new JsonResult(this.mapper.Map<BlastRoom, BlastRoomViewModel>
                                 (await this.repository.GetAsynvWithIncludes(key, "BlastRoomId", Includes)),
                                  this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST

        // POST: api/BlastRoom
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]BlastRoom nBlastWorkItem)
        {
            if (nBlastWorkItem != null)
            {
                nBlastWorkItem.CreateDate = DateTime.Now;
                nBlastWorkItem.Creator = nBlastWorkItem.Creator ?? "Someone";

                if (nBlastWorkItem.PaintTeam != null)
                    nBlastWorkItem.PaintTeam = null;

                return new JsonResult(await this.repository.AddAsync(nBlastWorkItem), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found blast room data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/BlastRoom/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]BlastRoom uBlastWorkItem)
        {
            var Message = "Blast room not been found.";

            if (uBlastWorkItem != null)
            {
                // set modified
                uBlastWorkItem.ModifyDate = DateTime.Now;
                uBlastWorkItem.Modifyer = uBlastWorkItem.Modifyer ?? "Someone";

                if (uBlastWorkItem.PaintTeam != null)
                    uBlastWorkItem.PaintTeam = null;

                return new JsonResult(await this.repository.UpdateAsync(uBlastWorkItem, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/BlastRoom/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE

    }
}
