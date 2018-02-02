using AutoMapper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using VipcoPainting.Models;
using VipcoPainting.Helpers;
using VipcoPainting.ViewModels;
using VipcoPainting.Services.Interfaces;

namespace VipcoPainting.Controllers
{
    [Produces("application/json")]
    [Route("api/InitialRequirePainting")]
    public class InitialRequirePaintingController : Controller
    {
        #region PrivateMenbers
        // Repository
        private IRepositoryPainting<InitialRequirePaintingList> repository;
        private IRepositoryPainting<BlastWorkItem> repositoryBlast;
        private IRepositoryPainting<PaintWorkItem> repositoryPaint;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;

        #endregion PrivateMenbers

        #region Constructor

        public InitialRequirePaintingController(
            IRepositoryPainting<InitialRequirePaintingList> repo, 
            IRepositoryPainting<BlastWorkItem> repoBlast,
            IRepositoryPainting<PaintWorkItem> repoPaint,
            IMapper map)
        {
            // Repository
            this.repository = repo;
            this.repositoryBlast = repoBlast;
            this.repositoryPaint = repoPaint;
            // Mapper
            this.mapper = map;
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/InitialRequirePainting
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            var Includes = new List<string> { "RequirePaintingMaster", "BlastWorkItems", "PaintWorkItems"};

            return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),this.DefaultJsonSettings);
        }

        // GET: api/InitialRequirePainting/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "RequirePaintingMaster", "BlastWorkItems", "PaintWorkItems" };

            return new JsonResult(await this.repository.GetAsynvWithIncludes(key, "InitialRequireId", Includes),this.DefaultJsonSettings);
        }

        // GET: api/InitialRequirePainting/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.InitialRequireId == MasterId)
                                .Include(x => x.RequirePaintingMaster)
                                .Include(x => x.BlastWorkItems)
                                .Include(x => x.PaintWorkItems);

            return new JsonResult(await QueryData.AsNoTracking().ToListAsync(), this.DefaultJsonSettings);
        }
        #endregion GET

        #region POST

        // POST: api/InitialRequirePainting
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]InitialRequirePaintingList nInitialRequirePainting)
        {
            var message = "Initial request painting list.";
            try
            {
                if (nInitialRequirePainting != null)
                {
                    nInitialRequirePainting.CreateDate = DateTime.Now;
                    nInitialRequirePainting.Creator = nInitialRequirePainting.Creator ?? "Someone";

                    if (nInitialRequirePainting.RequirePaintingMaster != null)
                        nInitialRequirePainting.RequirePaintingMaster = null;

                    // Remove null
                    nInitialRequirePainting.BlastWorkItems.Remove(null);
                    nInitialRequirePainting.PaintWorkItems.Remove(null);

                    if (nInitialRequirePainting.BlastWorkItems != null)
                    {
                        // Add BlastWork item
                        foreach (var nBlastWork in nInitialRequirePainting.BlastWorkItems)
                        {
                            if (nBlastWork == null)
                                continue;

                            nBlastWork.CreateDate = nInitialRequirePainting.CreateDate;
                            nBlastWork.Creator = nInitialRequirePainting.Creator;

                            // Clear StandradTimeExt
                            if (nBlastWork.StandradTimeExt != null)
                                nBlastWork.StandradTimeExt = null;

                            // Clear StandradTimeInt
                            if (nBlastWork.StandradTimeInt != null)
                                nBlastWork.StandradTimeInt = null;

                            // Clear SurfaceTypeExt
                            if (nBlastWork.SurfaceTypeExt != null)
                                nBlastWork.SurfaceTypeExt = null;

                            // Clear SurfaceTypeInt
                            if (nBlastWork.SurfaceTypeInt != null)
                                nBlastWork.SurfaceTypeInt = null;
                        }
                    }

                    if (nInitialRequirePainting.PaintWorkItems != null)
                    {
                        foreach (var nPaintWork in nInitialRequirePainting.PaintWorkItems)
                        {
                            if (nPaintWork == null)
                                continue;

                            nPaintWork.CreateDate = nInitialRequirePainting.CreateDate;
                            nPaintWork.Creator = nInitialRequirePainting.Creator;

                            // Clear StandradTimeExt
                            if (nPaintWork.StandradTimeExt != null)
                                nPaintWork.StandradTimeExt = null;

                            // Clear StandradTimeInt
                            if (nPaintWork.StandradTimeInt != null)
                                nPaintWork.StandradTimeInt = null;

                            // Clear SurfaceTypeExt
                            if (nPaintWork.ExtColorItem != null)
                                nPaintWork.ExtColorItem = null;

                            // Clear SurfaceTypeInt
                            if (nPaintWork.IntColorItem != null)
                                nPaintWork.IntColorItem = null;
                        }
                    }

                    return new JsonResult(await this.repository.AddAsync(nInitialRequirePainting), this.DefaultJsonSettings);
                }
            }
            catch(Exception ex)
            {
                message = $"Has error {ex.ToString()}";
            }
          
            return NotFound(new { Error = "Not found BlastWork data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/InitialRequirePainting/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]InitialRequirePaintingList uInitialRequirePainting)
        {
            var Message = "Require painting not been found.";

            if (uInitialRequirePainting != null)
            {
                // set modified
                uInitialRequirePainting.ModifyDate = DateTime.Now;
                uInitialRequirePainting.Modifyer = uInitialRequirePainting.Modifyer ?? "Someone";

                if (uInitialRequirePainting.RequirePaintingMaster != null)
                    uInitialRequirePainting.RequirePaintingMaster = null;

                // Remove null
                uInitialRequirePainting.BlastWorkItems.Remove(null);
                uInitialRequirePainting.PaintWorkItems.Remove(null);

                if (uInitialRequirePainting.BlastWorkItems != null)
                {
                    foreach (var uBlastWork in uInitialRequirePainting.BlastWorkItems)
                    {
                        if (uBlastWork == null)
                            continue;

                        if (uBlastWork.BlastWorkItemId > 0)
                        {
                            uBlastWork.ModifyDate = uInitialRequirePainting.ModifyDate;
                            uBlastWork.Modifyer = uInitialRequirePainting.Modifyer;
                        }
                        else
                        {
                            uBlastWork.CreateDate = uInitialRequirePainting.ModifyDate;
                            uBlastWork.Creator = uInitialRequirePainting.Modifyer;
                        }

                        uBlastWork.StandradTimeExt = null;
                        uBlastWork.StandradTimeInt = null;
                        uBlastWork.SurfaceTypeExt = null;
                        uBlastWork.SurfaceTypeInt = null;
                    }
                }

                if (uInitialRequirePainting.PaintWorkItems != null)
                {
                    foreach (var uPaintWork in uInitialRequirePainting.PaintWorkItems)
                    {
                        if (uPaintWork == null)
                            continue;

                        if (uPaintWork.PaintWorkItemId > 0)
                        {
                            uPaintWork.ModifyDate = uInitialRequirePainting.ModifyDate;
                            uPaintWork.Modifyer = uInitialRequirePainting.Modifyer;
                        }
                        else
                        {
                            uPaintWork.CreateDate = uInitialRequirePainting.ModifyDate;
                            uPaintWork.Creator = uInitialRequirePainting.Modifyer;
                        }

                        uPaintWork.StandradTimeExt = null;
                        uPaintWork.StandradTimeInt = null;
                        uPaintWork.ExtColorItem = null;
                        uPaintWork.IntColorItem = null;
                    }
                }

                // update Master not update Detail it need to update Detail directly
                var updateComplate = await this.repository.UpdateAsync(uInitialRequirePainting, uInitialRequirePainting.InitialRequireId);
                if (updateComplate != null)
                {
                    // filter
                    Expression<Func<BlastWorkItem, bool>> condition = m => m.InitialRequireId == updateComplate.InitialRequireId;
                    var dbBlastWorks = this.repositoryBlast.FindAll(condition);
                    Expression<Func<PaintWorkItem, bool>> condition2 = m => m.InitialRequireId == updateComplate.InitialRequireId;
                    var dbPaintWorks = this.repositoryPaint.FindAll(condition2);

                    //Remove BlastWork if edit remove it
                    foreach (var dbBlastWork in dbBlastWorks)
                    {
                        if (!uInitialRequirePainting.BlastWorkItems.Any(x => x.BlastWorkItemId == dbBlastWork.BlastWorkItemId))
                            await this.repositoryBlast.DeleteAsync(dbBlastWork.BlastWorkItemId);
                    }

                    //Remove PaintWork if edit remove it
                    foreach (var dbPaintWork in dbPaintWorks)
                    {
                        if (!uInitialRequirePainting.PaintWorkItems.Any(x => x.PaintWorkItemId == dbPaintWork.PaintWorkItemId))
                            await this.repositoryPaint.DeleteAsync(dbPaintWork.PaintWorkItemId);
                    }

                    //Update BlastWorkItem or New BlastWorkItem
                    foreach (var uBlastWork in uInitialRequirePainting.BlastWorkItems)
                    {
                        if (uBlastWork == null)
                            continue;

                        if (uBlastWork.BlastWorkItemId > 0)
                            await this.repositoryBlast.UpdateAsync(uBlastWork, uBlastWork.BlastWorkItemId);
                        else
                        {
                            if (uBlastWork.InitialRequireId is null || uBlastWork.InitialRequireId < 1)
                                uBlastWork.InitialRequireId = uInitialRequirePainting.InitialRequireId;

                            await this.repositoryBlast.AddAsync(uBlastWork);
                        }
                    }

                    //Update PaintWorkItem or New PaintWorkItem
                    foreach (var uPaintWork in uInitialRequirePainting.PaintWorkItems)
                    {
                        if (uPaintWork == null)
                            continue;

                        if (uPaintWork.PaintWorkItemId > 0)
                            await this.repositoryPaint.UpdateAsync(uPaintWork, uPaintWork.PaintWorkItemId);
                        else
                        {
                            if (uPaintWork.InitialRequireId is null || uPaintWork.InitialRequireId < 1)
                                uPaintWork.InitialRequireId = uInitialRequirePainting.InitialRequireId;

                            await this.repositoryPaint.AddAsync(uPaintWork);
                        }
                    }
                }

                return new JsonResult(await this.repository.UpdateAsync(uInitialRequirePainting, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/InitialRequirePainting/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
