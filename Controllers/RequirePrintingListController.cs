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
    [Route("api/RequirePaintingList")]
    public class RequirePaintingListController : Controller
    {
        #region PrivateMenbers
        // Repository
        private IRepositoryPainting<RequirePaintingList> repository;
        private IRepositoryPainting<ColorItem> repositoryColor;
        private IRepositoryPainting<StandradTime> repositoryStandrad;
        private IRepositoryPainting<SurfaceType> repositorySurface;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<RequirePaintingList> helpers;

        #endregion PrivateMenbers

        #region Constructor

        public RequirePaintingListController(
            IRepositoryPainting<RequirePaintingList> repo, IRepositoryPainting<ColorItem> repoCol ,
            IRepositoryPainting<StandradTime> repoStand, IRepositoryPainting<SurfaceType> repoSurf ,
            IMapper map)
        {
            // Repository
            this.repository = repo;
            this.repositoryColor = repoCol;
            this.repositoryStandrad = repoStand;
            this.repositorySurface = repoSurf;
            // Mapper
            this.mapper = map;
            this.helpers = new HelpersClass<RequirePaintingList>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/RequirePaintingList
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
            //var Includes = new List<string> {"BlastWorkItem" };

            //return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),
            //                            this.DefaultJsonSettings);
        }

        // GET: api/RequirePaintingList/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            //var Includes = new List<string> { "RequirePaintingSubs" };

            //return new JsonResult(await this.repository.GetAsynvWithIncludes(key, "RequirePaintingListId", Includes),
            //                            this.DefaultJsonSettings);
        }

        // GET: api/RequirePaintingList/GetByMaster/5
        [HttpGet("GetByMaster/{MasterId}")]
        public async Task<IActionResult> GetByMaster(int MasterId)
        {
            var QueryData = this.repository.GetAllAsQueryable()
                                .Where(x => x.RequirePaintingMasterId == MasterId);

            return new JsonResult(await QueryData.AsNoTracking().ToListAsync(),this.DefaultJsonSettings);
        }
        #endregion GET

        #region POST

        // POST: api/RequirePaintingList
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RequirePaintingList nRequirePaintingList)
        {
            if (nRequirePaintingList != null)
            {
                nRequirePaintingList = helpers.AddHourMethod(nRequirePaintingList);

                nRequirePaintingList.CreateDate = DateTime.Now;
                nRequirePaintingList.Creator = nRequirePaintingList.Creator ?? "Someone";

                if (nRequirePaintingList.RequirePaintingMaster != null)
                    nRequirePaintingList.RequirePaintingMaster = null;

                //if (nRequirePaintingList.RequirePaintingSubs != null)
                //{
                //    foreach (var nRequireSub in nRequirePaintingList.RequirePaintingSubs)
                //    {
                //        nRequireSub.CreateDate = nRequirePaintingList.CreateDate;
                //        nRequireSub.Creator = nRequirePaintingList.Creator;

                //        // Insert ColorItem
                //        if (nRequireSub.ColorItemId < 1 && nRequireSub.ColorItem != null)
                //        {
                //            nRequireSub.ColorItem.CreateDate = nRequireSub.CreateDate;
                //            nRequireSub.ColorItem.Creator = nRequireSub.Creator;
                //        }
                //        else
                //            nRequireSub.ColorItem = null;

                //        // Insert StandradTime
                //        if (nRequireSub.StandradTimeId < 1 && nRequireSub.StandradTime != null)
                //        {
                //            nRequireSub.StandradTime.CreateDate = nRequireSub.CreateDate;
                //            nRequireSub.StandradTime.Creator = nRequireSub.Creator;
                //        }
                //        else
                //            nRequireSub.StandradTime = null;

                //        // Insert SurfaceType
                //        if (nRequireSub.SurfaceTypeId < 1 && nRequireSub.SurfaceType != null)
                //        {
                //            nRequireSub.SurfaceType.CreateDate = nRequireSub.CreateDate;
                //            nRequireSub.SurfaceType.Creator = nRequireSub.Creator;
                //        }
                //        else
                //            nRequireSub.SurfaceType = null;
                //    }
                //}

                return new JsonResult(await this.repository.AddAsync(nRequirePaintingList), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found RequirePaintingList data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/RequirePaintingList/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]RequirePaintingList uRequirePaintingList)
        {
            var Message = "Require painting not been found.";

            if (uRequirePaintingList != null)
            {
                // add hour to DateTime to set Asia/Bangkok
                uRequirePaintingList = helpers.AddHourMethod(uRequirePaintingList);
                // set modified
                uRequirePaintingList.ModifyDate = DateTime.Now;
                uRequirePaintingList.Modifyer = uRequirePaintingList.Modifyer ?? "Someone";

                if (uRequirePaintingList.RequirePaintingMaster != null)
                    uRequirePaintingList.RequirePaintingMaster = null;

                //if (uRequirePaintingList.RequirePaintingSubs != null)
                //{
                //    foreach (var uRequireSub in uRequirePaintingList.RequirePaintingSubs)
                //    {
                //        if (uRequireSub.RequirePaintingSubId > 0)
                //        {
                //            uRequireSub.ModifyDate = uRequirePaintingList.ModifyDate;
                //            uRequireSub.Modifyer = uRequirePaintingList.Modifyer;
                //        }
                //        else
                //        {
                //            uRequireSub.CreateDate = uRequirePaintingList.ModifyDate;
                //            uRequireSub.Creator = uRequirePaintingList.Modifyer;
                //        }

                //        // Insert ColorItem
                //        if (uRequireSub.ColorItemId < 1 && uRequireSub.ColorItem != null)
                //        {
                //            var nColorItem = VipcoPainting.Helpers.CloneObject.Clone<ColorItem>(uRequireSub.ColorItem);
                //            if (nColorItem != null)
                //            {
                //                nColorItem.CreateDate = uRequireSub.ModifyDate;
                //                nColorItem.Creator = uRequireSub.Modifyer;

                //                nColorItem = await this.repositoryColor.AddAsync(nColorItem);
                //                uRequireSub.ColorItemId = nColorItem.ColorItemId;
                //            }
                //        }

                //        // Insert StandradTime
                //        if (uRequireSub.StandradTimeId < 1 && uRequireSub.StandradTime != null)
                //        {
                //            var nStandradTime = VipcoPainting.Helpers.CloneObject.Clone<StandradTime>(uRequireSub.StandradTime);
                //            if (nStandradTime != null)
                //            {
                //                nStandradTime.CreateDate = uRequireSub.ModifyDate;
                //                nStandradTime.Creator = uRequireSub.Modifyer;

                //                nStandradTime = await this.repositoryStandrad.AddAsync(nStandradTime);
                //                uRequireSub.StandradTimeId = nStandradTime.StandradTimeId;
                //            }
                //        }

                //        // Insert SurfaceType 
                //        if (uRequireSub.SurfaceTypeId < 1 && uRequireSub.SurfaceType != null)
                //        {
                //            var nSurfaceType = VipcoPainting.Helpers.CloneObject.Clone<SurfaceType>(uRequireSub.SurfaceType);
                //            if (nSurfaceType != null)
                //            {
                //                nSurfaceType.CreateDate = uRequireSub.ModifyDate;
                //                nSurfaceType.Creator = uRequireSub.Modifyer;

                //                nSurfaceType = await this.repositorySurface.AddAsync(nSurfaceType);
                //                uRequireSub.SurfaceTypeId = nSurfaceType.SurfaceTypeId;
                //            }
                //        }

                //        uRequireSub.ColorItem = null;
                //        uRequireSub.StandradTime = null;
                //        uRequireSub.SurfaceType = null;
                //    }
                //}

                // update Master not update Detail it need to update Detail directly
                //var updateComplate = await this.repository.UpdateAsync(uRequirePaintingList, key);
                //if (updateComplate != null)
                //{
                //    // filter
                //    Expression<Func<RequirePaintingSub, bool>> condition = m => m.RequirePaintingSubId == key;
                //    var dbRequireSubs = this.repositoryRequireSub.FindAll(condition);

                //    //Remove Require if edit remove it
                //    foreach (var dbRequire in dbRequireSubs)
                //    {
                //        if (!uRequirePaintingList.RequirePaintingSubs.Any(x => x.RequirePaintingSubId == dbRequire.RequirePaintingSubId))
                //            await this.repositoryRequireSub.DeleteAsync(dbRequire.RequirePaintingSubId);
                //    }

                //    //Update RequireSub or New RequireSub
                //    foreach (var uRequireSub in uRequirePaintingList.RequirePaintingSubs)
                //    {
                //        if (uRequireSub.RequirePaintingSubId > 0)
                //            await this.repositoryRequireSub.UpdateAsync(uRequireSub, uRequireSub.RequirePaintingSubId);
                //        else
                //        {
                //            if (uRequireSub.RequirePaintingSubId < 1)
                //                uRequireSub.RequirePaintingListId = uRequirePaintingList.RequirePaintingListId;

                //            await this.repositoryRequireSub.AddAsync(uRequireSub);
                //        }
                //    }
                //}

                return new JsonResult(await this.repository.UpdateAsync(uRequirePaintingList, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/RequirePaintingList/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
