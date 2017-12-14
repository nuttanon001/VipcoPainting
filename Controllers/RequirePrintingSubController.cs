using AutoMapper;
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

namespace VipcoPainting.Controllers
{
    [Produces("application/json")]
    [Route("api/RequirePaintingSub")]
    public class RequirePaintingSubController : Controller
    {
        #region PrivateMenbers

        private IRepositoryPainting<RequirePaintingSub> repository;
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<RequirePaintingSub> helpers;

        #endregion PrivateMenbers

        #region Constructor

        public RequirePaintingSubController(IRepositoryPainting<RequirePaintingSub> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
            this.helpers = new HelpersClass<RequirePaintingSub>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/RequirePaintingSub
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "ColorItem", "StandradTime", "SurfaceType" };
            return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),
                                        this.DefaultJsonSettings);
        }

        // GET: api/RequirePaintingSub/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "ColorItem", "StandradTime", "SurfaceType" };
            return new JsonResult(await this.repository.GetAsynvWithIncludes(key, "RequirePaintingSubId", Includes),
                                        this.DefaultJsonSettings);
        }

        // GET: api/RequirePaintingSub/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.RequirePaintingListId == MasterId)
                                .Include(x => x.ColorItem)
                                .Include(x => x.StandradTime)
                                .Include(x => x.SurfaceType);

            return new JsonResult(await QueryData.AsNoTracking().ToListAsync(), this.DefaultJsonSettings);
        }
        #endregion GET

        #region POST

        // POST: api/RequirePaintingSub
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RequirePaintingSub nRequirePaintingSub)
        {
            if (nRequirePaintingSub != null)
            {
                nRequirePaintingSub = helpers.AddHourMethod(nRequirePaintingSub);

                nRequirePaintingSub.CreateDate = DateTime.Now;
                nRequirePaintingSub.Creator = nRequirePaintingSub.Creator ?? "Someone";

                if (nRequirePaintingSub.StandradTime != null)
                    nRequirePaintingSub.StandradTime = null;

                if (nRequirePaintingSub.ColorItem != null)
                    nRequirePaintingSub.ColorItem = null;

                if (nRequirePaintingSub.SurfaceType != null)
                    nRequirePaintingSub.SurfaceType = null;

                if (nRequirePaintingSub.RequirePaintingList != null)
                    nRequirePaintingSub.RequirePaintingList = null;

                return new JsonResult(await this.repository.AddAsync(nRequirePaintingSub), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found RequirePaintingSub data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/RequirePaintingSub/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]RequirePaintingSub uRequirePaintingSub)
        {
            var Message = "Not found RequirePaintingSub data.";
            try
            {
                if (uRequirePaintingSub != null)
                {
                    // add hour to DateTime to set Asia/Bangkok
                    uRequirePaintingSub = helpers.AddHourMethod(uRequirePaintingSub);

                    uRequirePaintingSub.ModifyDate = DateTime.Now;
                    uRequirePaintingSub.Modifyer = uRequirePaintingSub.Modifyer ?? "Someone";

                    if (uRequirePaintingSub.StandradTime != null)
                        uRequirePaintingSub.StandradTime = null;

                    if (uRequirePaintingSub.ColorItem != null)
                        uRequirePaintingSub.ColorItem = null;

                    if (uRequirePaintingSub.SurfaceType != null)
                        uRequirePaintingSub.SurfaceType = null;

                    if (uRequirePaintingSub.RequirePaintingList != null)
                        uRequirePaintingSub.RequirePaintingList = null;

                    return new JsonResult(await this.repository.UpdateAsync(uRequirePaintingSub, key), this.DefaultJsonSettings);
                }
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/RequirePaintingSub/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
