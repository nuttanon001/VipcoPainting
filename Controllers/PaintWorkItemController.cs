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
        private IRepositoryPainting<BlastWorkItem> repositoryBlast;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;

        Func<double, double, double, double, double> CalcPaintPlan = 
            (area, thick, vs, loss) => (area * thick) / (vs * 10 * (1 - loss));


        #endregion PrivateMenbers

        #region Constructor

        public PaintWorkItemController(
            IRepositoryPainting<PaintWorkItem> repo,
            IRepositoryPainting<BlastWorkItem> repoBlast, 
            IMapper map)
        {
            // Repository
            this.repository = repo;
            this.repositoryBlast = repoBlast;
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
                                 (await this.repository.GetAsynvWithIncludes(key, "PaintWorkItemId", Includes)),
                                  this.DefaultJsonSettings);
        }

        // GET: api/PaintWorkItem/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.RequirePaintingListId == MasterId &&
                                            x.InitialRequireId == null)
                                .OrderBy(x => x.PaintLevel)
                                .Include(x => x.StandradTimeExt)
                                .Include(x => x.StandradTimeInt)
                                .Include(x => x.ExtColorItem)
                                .Include(x => x.IntColorItem);

            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<PaintWorkItemViewModel, PaintWorkItem>
                                 (await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }

        // GET: api/PaintWorkItem/GetByMaster/5
        [HttpGet("GetByMasterCalculate/{MasterId}")]
        public async Task<IActionResult> GetByMasterCalculate(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.RequirePaintingListId == MasterId && 
                                            x.InitialRequireId == null)
                                .OrderBy(x => x.PaintLevel)
                                .Include(x => x.StandradTimeExt)
                                .Include(x => x.StandradTimeInt)
                                .Include(x => x.ExtColorItem)
                                .Include(x => x.IntColorItem);

            //var GetData = this.ConvertTable.ConverterTableToViewModel<PaintWorkItemViewModel, PaintWorkItem>
            //                    (await QueryData.AsNoTracking().ToListAsync());

            var GetData = await QueryData.AsNoTracking().ToListAsync();
            var BlastWorkItem = await this.repositoryBlast.GetAllAsQueryable()
                                          .Where(x => x.RequirePaintingListId == MasterId)
                                          .Include(x => x.StandradTimeExt)
                                          .Include(x => x.StandradTimeInt).FirstOrDefaultAsync();

            foreach (var item in GetData)
            {
                var hasChange = false;

                if (item.IntCalcColorUsage == null)
                {
                    if (item.IntArea != null && item.IntDFTMin != null && item.IntDFTMax != null && item.IntColorItem != null && BlastWorkItem != null)
                    {
                        double[] thicks = { item.IntDFTMin ?? 0, item.IntDFTMax ?? 0 };
                        item.IntCalcColorUsage = this.CalcPaintPlan(item.IntArea ?? 0, thicks.Average(), (item.IntColorItem.VolumeSolids ?? 0), (BlastWorkItem?.StandradTimeInt?.PercentLoss ?? 0) / 100);
                        hasChange = true;
                    }
                }
             
                if (item.ExtCalcColorUsage == null)
                {
                    if (item.ExtArea != null && item.ExtDFTMin != null && item.ExtDFTMax != null && item.ExtColorItem != null && BlastWorkItem != null)
                    {
                        double[] thicks = { item.ExtDFTMin ?? 0, item.ExtDFTMax ?? 0 };
                        item.ExtCalcColorUsage = this.CalcPaintPlan(item.ExtArea ?? 0, thicks.Average(), (item.ExtColorItem.VolumeSolids ?? 0) , (BlastWorkItem?.StandradTimeExt?.PercentLoss ?? 0) / 100);
                        hasChange = true;
                    }
                }

                // update
                if (hasChange)
                    await this.repository.UpdateAsync(item, item.PaintWorkItemId);
            }

            return new JsonResult(
                this.ConvertTable.ConverterTableToViewModel<PaintWorkItemViewModel,PaintWorkItem>(GetData), 
                this.DefaultJsonSettings);
        }

        // GET: api/PaintWorkItem/GetByMaster/5
        [HttpGet("GetByMaster2/{MasterId}")]
        public async Task<IActionResult> GetByMaster2(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.InitialRequireId == MasterId &&
                                            x.RequirePaintingListId == null)
                                .OrderBy(x => x.PaintLevel)
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
