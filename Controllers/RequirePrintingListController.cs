using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

using AutoMapper;
using Newtonsoft.Json;

using System;
using System.IO;
using System.Linq;
using System.Dynamic;
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
        private IRepositoryPainting<PaintWorkItem> repositoryPaint;
        private IRepositoryPainting<BlastWorkItem> repositoryBlast;
        private IRepositoryPainting<StandradTime> repositoryStandradTime;
        private IRepositoryPainting<RequirePaintingListHasAttach> repositoryHasAttach;
        private IRepositoryPainting<RequirePaintingListOption> repositoryOption;
        // Repository Machine
        private IRepositoryMachine<ProjectCodeMaster> repositoryProMaster;
        private IRepositoryMachine<AttachFile> repositoryAttach;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<RequirePaintingList> helpers;
        private IHostingEnvironment appEnvironment;
        // 
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        #endregion PrivateMenbers

        #region Constructor

        public RequirePaintingListController(
            IRepositoryPainting<RequirePaintingList> repo, IRepositoryPainting<PaintWorkItem> repoPaint ,
            IRepositoryPainting<StandradTime> repoStandradTime,
            IRepositoryPainting<BlastWorkItem> repoBlast,
            IRepositoryPainting<RequirePaintingListHasAttach> repoHasAttach,
            IRepositoryPainting<RequirePaintingListOption> repoOptino,
            IRepositoryMachine<AttachFile> repoAttach,
            IRepositoryMachine<ProjectCodeMaster> repoProMaster,
            IHostingEnvironment appEnv,
            IMapper map)
        {
            // Repository
            this.repository = repo;
            this.repositoryBlast = repoBlast;
            this.repositoryPaint = repoPaint;
            this.repositoryStandradTime = repoStandradTime;
            this.repositoryHasAttach = repoHasAttach;
            this.repositoryOption = repoOptino;
            // Repositroy Machine
            this.repositoryProMaster = repoProMaster;
            this.repositoryAttach = repoAttach;
            // Mapper
            this.mapper = map;
            this.helpers = new HelpersClass<RequirePaintingList>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
            // HostingEnvironment
            this.appEnvironment = appEnv;
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
            // var Includes = new List<string> { "BlastWorkItems","PaintWorkItems" };

            // return new JsonResult(await this.repository.GetAsynvWithIncludes(key, "RequirePaintingListId", Includes),
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

        // GET: api/RequirePaintingList/SetReceiveWorkItem/5
        [HttpGet("SetReceiveWorkItem/{key}/{Create}")]
        public async Task<IActionResult> SetReceiveWorkItem(int key,string Create)
        {
            if (key > 0)
            {
                Expression<Func<RequirePaintingListOption, bool>> match = e => e.RequirePaintingListId == key;
                if (!(await this.repositoryOption.AnyDataAsync(match)))
                {
                    var InsertData = await this.repositoryOption.AddAsync(new RequirePaintingListOption()
                    {
                        CreateDate = DateTime.Now,
                        Creator = Create,
                        ReceiveWorkItem = DateTime.Now,
                        RequirePaintingListId = key,
                    });

                    return new JsonResult(InsertData, this.DefaultJsonSettings);
                }

            }
            return NotFound(new { Error = "Key not been found." });
        }

        #endregion GET

        #region POST

        // POST: api/RequirePaintingMaster/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var Message = "Data been not found.";
            try
            {
                var QueryData = this.repository.GetAllAsQueryable()
                                    .Include(x => x.RequirePaintingMaster.ProjectCodeSub)
                                    .AsQueryable();
                // Where
                if (!string.IsNullOrEmpty(Scroll.Where))
                {
                    QueryData = QueryData.Where(x => x.Creator == Scroll.Where);
                }
                // Filter
                var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                    : Scroll.Filter.ToLower().Split(null);
                // foreach
                foreach (var keyword in filters)
                {
                    QueryData = QueryData.Where(x => x.Description.ToLower().Contains(keyword) ||
                                                     x.MarkNo.ToLower().Contains(keyword) ||
                                                     x.RequirePaintingMaster.ProjectCodeSub.Code.ToLower().Contains(keyword) ||
                                                     x.RequirePaintingMaster.RequireNo.ToLower().Contains(keyword));
                }

                // Order
                switch (Scroll.SortField)
                {
                    case "Description":
                        if (Scroll.SortOrder == -1)
                            QueryData = QueryData.OrderByDescending(e => e.Description);
                        else
                            QueryData = QueryData.OrderBy(e => e.Description);
                        break;

                    case "MarkNo":
                        if (Scroll.SortOrder == -1)
                            QueryData = QueryData.OrderByDescending(e => e.MarkNo);
                        else
                            QueryData = QueryData.OrderBy(e => e.MarkNo);
                        break;

                    case "SendWorkItem":
                        if (Scroll.SortOrder == -1)
                            QueryData = QueryData.OrderByDescending(e => e.SendWorkItem);
                        else
                            QueryData = QueryData.OrderBy(e => e.SendWorkItem);
                        break;

                    default:
                        QueryData = QueryData.OrderByDescending(e => e.SendWorkItem);
                        break;
                }
                QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

                return new JsonResult(new ScrollDataViewModel<RequirePaintingListViewModel>
                    (Scroll,
                    this.ConvertTable.ConverterTableToViewModel<RequirePaintingListViewModel, RequirePaintingList>(await QueryData.AsNoTracking().ToListAsync())),
                    this.DefaultJsonSettings);
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }
            return NotFound(new { Error = Message });
        }

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

        // POST: api/RequirePaintingList/PostWithInitialRequire
        [HttpPost("PostWithInitialRequire")]
        public async Task<IActionResult> PostWithInitialRequire([FromBody]RequirePaintingList nRequirePaintingList)
        {
            if (nRequirePaintingList != null)
            {
                nRequirePaintingList = helpers.AddHourMethod(nRequirePaintingList);

                nRequirePaintingList.CreateDate = DateTime.Now;
                nRequirePaintingList.Creator = nRequirePaintingList.Creator ?? "Someone";

                if (nRequirePaintingList.RequirePaintingMaster != null)
                    nRequirePaintingList.RequirePaintingMaster = null;


                if (nRequirePaintingList.BlastWorkItems != null)
                {
                    // Add BlastWork item
                    foreach (var nBlastWork in nRequirePaintingList.BlastWorkItems)
                    {
                        nBlastWork.CreateDate = nRequirePaintingList.CreateDate;
                        nBlastWork.Creator = nRequirePaintingList.Creator;

                        if (nBlastWork.StandradTimeExtId.HasValue && nBlastWork.ExtArea.HasValue)
                        {
                            if (nBlastWork.StandradTimeExtId > 0 && nBlastWork.ExtArea > 0)
                            {
                                var standradTime = await this.repositoryStandradTime.GetAllAsQueryable()
                                                             .Where(x => x.LinkStandradTime != null)
                                                             .Include(x => x.LinkStandradTime)
                                                             .FirstOrDefaultAsync(x => x.StandradTimeId == nBlastWork.StandradTimeExtId);
                                // Check Link Standrad Time
                                if (standradTime != null)
                                {
                                    if (standradTime.Codition == Codition.Less)
                                        nBlastWork.StandradTimeExtId = nBlastWork.ExtArea < standradTime.AreaCodition ? nBlastWork.StandradTimeExtId : standradTime.LinkStandardTimeId;
                                    else if (standradTime.Codition == Codition.Over)
                                        nBlastWork.StandradTimeExtId = nBlastWork.ExtArea > standradTime.AreaCodition ? nBlastWork.StandradTimeExtId : standradTime.LinkStandardTimeId;
                                }
                            }

                            if (nBlastWork.StandradTimeIntId > 0 && nBlastWork.IntArea > 0)
                            {
                                var standradTime = await this.repositoryStandradTime.GetAllAsQueryable()
                                                             .Where(x => x.LinkStandradTime != null)
                                                             .Include(x => x.LinkStandradTime)
                                                             .FirstOrDefaultAsync(x => x.StandradTimeId == nBlastWork.StandradTimeIntId);
                                // Check Link Standrad Time
                                if (standradTime != null)
                                {
                                    if (standradTime.Codition == Codition.Less)
                                        nBlastWork.StandradTimeIntId = nBlastWork.IntArea < standradTime.AreaCodition ? nBlastWork.StandradTimeIntId : standradTime.LinkStandardTimeId;
                                    else if (standradTime.Codition == Codition.Over)
                                        nBlastWork.StandradTimeIntId = nBlastWork.IntArea > standradTime.AreaCodition ? nBlastWork.StandradTimeIntId : standradTime.LinkStandardTimeId;
                                }
                            }
                        }
                    }
                }

                if (nRequirePaintingList.PaintWorkItems != null)
                {
                    foreach (var nPaintWork in nRequirePaintingList.PaintWorkItems)
                    {
                        nPaintWork.CreateDate = nRequirePaintingList.CreateDate;
                        nPaintWork.Creator = nRequirePaintingList.Creator;
                    }
                }

                return new JsonResult(await this.repository.AddAsync(nRequirePaintingList), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found RequirePaintingList data !!!" });
        }

        // POST: api/RequirePaintingList/Lists
        [HttpPost("Lists")]
        public async Task<IActionResult> PostLists([FromBody]IList<RequirePaintingList> nRequirePaintingLists)
        {
            var Message = "Not found RequirePaintingList data !!!";
            try
            {
                if (nRequirePaintingLists != null)
                {
                    foreach (var item in nRequirePaintingLists)
                    {
                        var nRequirePaintingList = helpers.AddHourMethod(item);

                        nRequirePaintingList.CreateDate = DateTime.Now;
                        nRequirePaintingList.Creator = nRequirePaintingList.Creator ?? "Someone";

                        if (nRequirePaintingList.RequirePaintingMaster != null)
                            nRequirePaintingList.RequirePaintingMaster = null;

                        if (nRequirePaintingList.BlastWorkItems != null)
                        {
                            // Add BlastWork item
                            foreach (var nBlastWork in nRequirePaintingList.BlastWorkItems)
                            {
                                nBlastWork.CreateDate = nRequirePaintingList.CreateDate;
                                nBlastWork.Creator = nRequirePaintingList.Creator;

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

                        if (nRequirePaintingList.PaintWorkItems != null)
                        {
                            foreach (var nPaintWork in nRequirePaintingList.PaintWorkItems)
                            {
                                nPaintWork.CreateDate = nRequirePaintingList.CreateDate;
                                nPaintWork.Creator = nRequirePaintingList.Creator;

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

                        await this.repository.AddAsync(nRequirePaintingList);
                    }

                    return new JsonResult(new { Status = "Complate" }, this.DefaultJsonSettings);
                }

            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }

            return NotFound(new { Error = Message });

        }

        // POST: api/RequirePaintingList/List
        [HttpPost("Lists2")]
        public async Task<IActionResult> PostLists2([FromBody]IList<RequirePaintingList> uRequirePaintingLists)
        {
            var Message = "Require painting not been found.";
            try
            {
                if (uRequirePaintingLists != null)
                {
                    foreach (var item in uRequirePaintingLists)
                    {
                        // For Update
                        if (item.RequirePaintingListId > 0)
                        {
                            // add hour to DateTime to set Asia/Bangkok
                            var uRequirePaintingList = helpers.AddHourMethod(item);
                            // set modified
                            uRequirePaintingList.ModifyDate = DateTime.Now;
                            uRequirePaintingList.Modifyer = uRequirePaintingList.Modifyer ?? "Someone";

                            if (uRequirePaintingList.RequirePaintingMaster != null)
                                uRequirePaintingList.RequirePaintingMaster = null;

                            // Remove null
                            uRequirePaintingList.BlastWorkItems.Remove(null);
                            uRequirePaintingList.PaintWorkItems.Remove(null);

                            if (uRequirePaintingList.BlastWorkItems != null)
                            {
                                foreach (var uBlastWork in uRequirePaintingList.BlastWorkItems)
                                {
                                    if (uBlastWork == null)
                                        continue;

                                    if (uBlastWork.BlastWorkItemId > 0)
                                    {
                                        uBlastWork.ModifyDate = uRequirePaintingList.ModifyDate;
                                        uBlastWork.Modifyer = uRequirePaintingList.Modifyer;
                                    }
                                    else
                                    {
                                        uBlastWork.CreateDate = uRequirePaintingList.ModifyDate;
                                        uBlastWork.Creator = uRequirePaintingList.Modifyer;
                                    }

                                    uBlastWork.StandradTimeExt = null;
                                    uBlastWork.StandradTimeInt = null;
                                    uBlastWork.SurfaceTypeExt = null;
                                    uBlastWork.SurfaceTypeInt = null;
                                }
                            }

                            if (uRequirePaintingList.PaintWorkItems != null)
                            {
                                foreach (var uPaintWork in uRequirePaintingList.PaintWorkItems)
                                {
                                    if (uPaintWork == null)
                                        continue;

                                    if (uPaintWork.PaintWorkItemId > 0)
                                    {
                                        uPaintWork.ModifyDate = uRequirePaintingList.ModifyDate;
                                        uPaintWork.Modifyer = uRequirePaintingList.Modifyer;
                                    }
                                    else
                                    {
                                        uPaintWork.CreateDate = uRequirePaintingList.ModifyDate;
                                        uPaintWork.Creator = uRequirePaintingList.Modifyer;
                                    }

                                    uPaintWork.StandradTimeExt = null;
                                    uPaintWork.StandradTimeInt = null;
                                    uPaintWork.ExtColorItem = null;
                                    uPaintWork.IntColorItem = null;
                                }
                            }

                            // update Master not update Detail it need to update Detail directly
                            var updateComplate = await this.repository.UpdateAsync(uRequirePaintingList, uRequirePaintingList.RequirePaintingListId);
                            if (updateComplate != null)
                            {
                                // filter
                                Expression<Func<BlastWorkItem, bool>> condition = m => m.RequirePaintingListId == updateComplate.RequirePaintingListId;
                                var dbBlastWorks = this.repositoryBlast.FindAll(condition);
                                Expression<Func<PaintWorkItem, bool>> condition2 = m => m.RequirePaintingListId == updateComplate.RequirePaintingListId;
                                var dbPaintWorks = this.repositoryPaint.FindAll(condition2);

                                //Remove BlastWork if edit remove it
                                foreach (var dbBlastWork in dbBlastWorks)
                                {
                                    if (!uRequirePaintingList.BlastWorkItems.Any(x => x.BlastWorkItemId == dbBlastWork.BlastWorkItemId))
                                        await this.repositoryBlast.DeleteAsync(dbBlastWork.BlastWorkItemId);
                                }

                                //Remove PaintWork if edit remove it
                                foreach (var dbPaintWork in dbPaintWorks)
                                {
                                    if (!uRequirePaintingList.PaintWorkItems.Any(x => x.PaintWorkItemId == dbPaintWork.PaintWorkItemId))
                                        await this.repositoryPaint.DeleteAsync(dbPaintWork.PaintWorkItemId);
                                }

                                //Update BlastWorkItem or New BlastWorkItem
                                foreach (var uBlastWork in uRequirePaintingList.BlastWorkItems)
                                {
                                    if (uBlastWork == null)
                                        continue;

                                    if (uBlastWork.BlastWorkItemId > 0)
                                        await this.repositoryBlast.UpdateAsync(uBlastWork, uBlastWork.BlastWorkItemId);
                                    else
                                    {
                                        if (uBlastWork.RequirePaintingListId is null || uBlastWork.RequirePaintingListId < 1)
                                            uBlastWork.RequirePaintingListId = uRequirePaintingList.RequirePaintingListId;

                                        await this.repositoryBlast.AddAsync(uBlastWork);
                                    }
                                }

                                //Update PaintWorkItem or New PaintWorkItem
                                foreach (var uPaintWork in uRequirePaintingList.PaintWorkItems)
                                {
                                    if (uPaintWork == null)
                                        continue;

                                    if (uPaintWork.PaintWorkItemId > 0)
                                        await this.repositoryPaint.UpdateAsync(uPaintWork, uPaintWork.PaintWorkItemId);
                                    else
                                    {
                                        if (uPaintWork.RequirePaintingListId is null || uPaintWork.RequirePaintingListId < 1)
                                            uPaintWork.RequirePaintingListId = uRequirePaintingList.RequirePaintingListId;

                                        await this.repositoryPaint.AddAsync(uPaintWork);
                                    }
                                }
                            }
                        }
                        //For Insert
                        else
                        {
                            if (item.RequirePaintingListStatus.HasValue)
                            {
                                if (item.RequirePaintingListStatus.Value == RequirePaintingListStatus.Cancel)
                                    continue;
                            }

                            var nRequirePaintingList = helpers.AddHourMethod(item);

                            nRequirePaintingList.CreateDate = DateTime.Now;
                            nRequirePaintingList.Creator = nRequirePaintingList.Creator ?? "Someone";

                            if (nRequirePaintingList.RequirePaintingMaster != null)
                                nRequirePaintingList.RequirePaintingMaster = null;

                            if (nRequirePaintingList.RequirePaintingListStatus == null)
                                nRequirePaintingList.RequirePaintingListStatus = RequirePaintingListStatus.Waiting;

                            // Remove null
                            nRequirePaintingList.BlastWorkItems.Remove(null);
                            nRequirePaintingList.PaintWorkItems.Remove(null);

                            if (nRequirePaintingList.BlastWorkItems != null)
                            {
                                // Add BlastWork item
                                foreach (var nBlastWork in nRequirePaintingList.BlastWorkItems)
                                {
                                    if (nBlastWork == null)
                                        continue;

                                    nBlastWork.CreateDate = nRequirePaintingList.CreateDate;
                                    nBlastWork.Creator = nRequirePaintingList.Creator;

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

                            if (nRequirePaintingList.PaintWorkItems != null)
                            {
                                foreach (var nPaintWork in nRequirePaintingList.PaintWorkItems)
                                {
                                    if (nPaintWork == null)
                                        continue;

                                    nPaintWork.CreateDate = nRequirePaintingList.CreateDate;
                                    nPaintWork.Creator = nRequirePaintingList.Creator;

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

                            await this.repository.AddAsync(nRequirePaintingList);
                        }
                    }

                    return new JsonResult(new { Status = "Complate" }, this.DefaultJsonSettings);
                }
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }
            return NotFound(new { Error = Message });
        }

        // POST: api/RequirePaintingList/RequirePaintSchedule
        [HttpPost("RequirePaintSchedule")]
        public async Task<IActionResult> RequirePaintSchedule([FromBody] OptionRequirePaintSchedule Scehdule)
        {
            string Message = "";
            try
            {
                var QueryData = this.repository.GetAllAsQueryable()
                                               .Where(x => x.SendWorkItem != null)
                                               .Include(x => x.RequirePaintingMaster.ProjectCodeSub)
                                               .Include(x => x.RequirePaintingListOption)
                                               .AsQueryable();
                int TotalRow;

                if (Scehdule != null)
                {
                    if (!string.IsNullOrEmpty(Scehdule.Filter))
                    {
                        // Filter
                        var filters = string.IsNullOrEmpty(Scehdule.Filter) ? new string[] { "" }
                                            : Scehdule.Filter.ToLower().Split(null);
                        foreach (var keyword in filters)
                        {
                            QueryData = QueryData.Where(x => x.Description.ToLower().Contains(keyword) ||
                                                             x.MarkNo.ToLower().Contains(keyword) ||
                                                             x.PartNo.ToLower().Contains(keyword) ||
                                                             x.RequirePaintingMaster.ProjectCodeSub.Code.ToLower().Contains(keyword));
                        }
                    }

                    // Option ProjectCodeMaster
                    if (Scehdule.ProjectId.HasValue)
                        QueryData = QueryData.Where(x => x.RequirePaintingMaster.ProjectCodeSubId == Scehdule.ProjectId);

                    // Option Status
                    if (Scehdule.Status.HasValue)
                    {
                        if (Scehdule.Status == 1)
                        {
                            QueryData = QueryData.Where(
                                x => (x.RequirePaintingMaster.RequirePaintingStatus == RequirePaintingStatus.Waiting ||
                                      x.RequirePaintingMaster.RequirePaintingStatus == RequirePaintingStatus.Tasking) &&
                                      x.RequirePaintingListStatus == RequirePaintingListStatus.Waiting);
                        }
                        else if (Scehdule.Status == 2)
                            QueryData = QueryData.Where(x => x.RequirePaintingMaster.RequirePaintingStatus == RequirePaintingStatus.Tasking);
                        else
                            QueryData = QueryData.Where(x => x.RequirePaintingMaster.RequirePaintingStatus != RequirePaintingStatus.Cancel);
                    }
                    else
                        QueryData = QueryData.Where(x => x.RequirePaintingMaster.RequirePaintingStatus == RequirePaintingStatus.Waiting);

                    TotalRow = await QueryData.CountAsync();

                    // Option Skip and Task
                    if (Scehdule.Skip.HasValue && Scehdule.Take.HasValue)
                        QueryData = QueryData.Skip(Scehdule.Skip ?? 0).Take(Scehdule.Take ?? 10);
                    else
                        QueryData = QueryData.Skip(0).Take(10);
                }
                else
                    TotalRow = await QueryData.CountAsync();

                var GetData = await QueryData.ToListAsync();
                if (GetData.Any())
                {
                    List<string> Columns = new List<string>();

                    var MinDate = GetData.Min(x => x.SendWorkItem);
                    var MaxDate = GetData.Max(x => x.SendWorkItem);

                    if (MinDate == null && MaxDate == null)
                    {
                        return NotFound(new { Error = "Data not found" });
                    }

                    foreach (DateTime day in EachDay(MinDate.Value, MaxDate.Value))
                    {
                        if (GetData.Any(x => x.SendWorkItem.Value.Date == day.Date))
                            Columns.Add(day.Date.ToString("dd/MM/yy"));
                    }

                    var DataTable = new List<IDictionary<String, Object>>();

                    foreach (var Data in GetData.OrderBy(x => x.RequirePaintingMaster.ProjectCodeSub.ProjectCodeMasterId))
                    {
                        var ProjectMaster = await this.repositoryProMaster.GetAsync(Data.RequirePaintingMaster.ProjectCodeSub.ProjectCodeMasterId ?? 0);
                        var JobNumber = $"{ProjectMaster?.ProjectCode ?? "No-Data"}/{(Data.RequirePaintingMaster.ProjectCodeSub == null ? "No-Data" : Data.RequirePaintingMaster.ProjectCodeSub.Code)}";

                        IDictionary<String, Object> rowData;
                        bool update = false;
                        if (DataTable.Any(x => (string)x["JobNumber"] == JobNumber))
                        {
                            var FirstData = DataTable.FirstOrDefault(x => (string)x["JobNumber"] == JobNumber);
                            if (FirstData != null)
                            {
                                rowData = FirstData;
                                update = true;
                            }
                            else
                                rowData = new ExpandoObject();
                        }
                        else
                            rowData = new ExpandoObject();

                        if (Data.SendWorkItem != null)
                        {
                            //Get Employee Name
                            // var Employee = await this.repositoryEmp.GetAsync(Data.RequireEmp);
                            // var EmployeeReq = Employee != null ? $"คุณ{(Employee?.NameThai ?? "")}" : "No-Data";

                            var Key = Data.SendWorkItem.Value.ToString("dd/MM/yy");
                            // New Data
                            var Master = new RequirePaintingListViewModel()
                            {
                                RequirePaintingMasterId = Data.RequirePaintingMaster.RequirePaintingMasterId,
                                RequirePaintingListId = Data.RequirePaintingListId,
                                // RequireString = $"{EmployeeReq} | No.{Data.RequireNo}",
                                MarkNo = $"MarkNo:{Data.MarkNo}",
                                IsReceive = Data.RequirePaintingListOption != null
                            };

                            if (rowData.Any(x => x.Key == Key))
                            {
                                // New Value
                                var ListMaster = (List<RequirePaintingListViewModel>)rowData[Key];
                                ListMaster.Add(Master);
                                // add to row data
                                rowData[Key] = ListMaster;
                            }
                            else // add new
                                rowData.Add(Key, new List<RequirePaintingListViewModel>() { Master });
                        }

                        if (!update)
                        {
                            rowData.Add("JobNumber", JobNumber);
                            DataTable.Add(rowData);
                        }
                    }

                    return new JsonResult(new
                    {
                        TotalRow = TotalRow,
                        Columns = Columns,
                        DataTable = DataTable
                    }, this.DefaultJsonSettings);
                }
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }

            return NotFound(new { Error = Message });
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

                #region Mark not use

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

                #endregion

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

        #region ATTACH

        // GET: api/RequirePaintingList/GetAttach/5
        [HttpGet("GetAttach/{key}")]
        public async Task<IActionResult> GetAttach(int key)
        {
            var AttachIds = await this.repositoryHasAttach.GetAllAsQueryable()
                                  .Where(x => x.RequirePaintingListId == key)
                                  .Select(x => x.AttachFileId).ToListAsync();
            if (AttachIds != null)
            {
                var DataAttach = await this.repositoryAttach.GetAllAsQueryable()
                                       .Where(x => AttachIds.Contains(x.AttachFileId))
                                       .AsNoTracking()
                                       .ToListAsync();

                return new JsonResult(DataAttach, this.DefaultJsonSettings);
            }

            return NotFound(new { Error = "Attatch not been found." });
        }

        // POST: api/RequirePaintingList/PostAttach/5/Someone
        [HttpPost("PostAttach/{key}/{CreateBy}")]
        public async Task<IActionResult> PostAttac(int key, string CreateBy, IEnumerable<IFormFile> files)
        {
            string Message = "";
            try
            {
                long size = files.Sum(f => f.Length);

                // full path to file in temp location
                var filePath1 = Path.GetTempFileName();

                foreach (var formFile in files)
                {
                    string FileName = Path.GetFileName(formFile.FileName).ToLower();
                    // create file name for file
                    string FileNameForRef = $"{DateTime.Now.ToString("ddMMyyhhmmssfff")}{ Path.GetExtension(FileName).ToLower()}";
                    // full path to file in temp location
                    var filePath = Path.Combine(this.appEnvironment.WebRootPath + "\\files", FileNameForRef);

                    if (formFile.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                            await formFile.CopyToAsync(stream);
                    }

                    var returnData = await this.repositoryAttach.AddAsync(new AttachFile()
                    {
                        FileAddress = $"/paint/files/{FileNameForRef}",
                        FileName = FileName,
                        CreateDate = DateTime.Now,
                        Creator = CreateBy ?? "Someone"
                    });

                    await this.repositoryHasAttach.AddAsync(new RequirePaintingListHasAttach()
                    {
                        AttachFileId = returnData.AttachFileId,
                        CreateDate = DateTime.Now,
                        Creator = CreateBy ?? "Someone",
                        RequirePaintingListId = key
                    });
                }

                return Ok(new { count = 1, size, filePath1 });

            }
            catch (Exception ex)
            {
                Message = ex.ToString();
            }

            return NotFound(new { Error = "Not found " + Message });
        }

        // DELETE: api/RequirePaintingList/DeleteAttach/5
        [HttpDelete("DeleteAttach/{AttachFileId}")]
        public async Task<IActionResult> DeleteAttach(int AttachFileId)
        {
            if (AttachFileId > 0)
            {
                var AttachFile = await this.repositoryAttach.GetAsync(AttachFileId);
                if (AttachFile != null)
                {
                    var filePath = Path.Combine(this.appEnvironment.WebRootPath + AttachFile.FileAddress);
                    FileInfo delFile = new FileInfo(filePath);

                    if (delFile.Exists)
                        delFile.Delete();
                    // Condition
                    Expression<Func<RequirePaintingListHasAttach, bool>> condition = c => c.AttachFileId == AttachFile.AttachFileId;
                    var RequirePaintingList = this.repositoryHasAttach.FindAsync(condition).Result;
                    if (RequirePaintingList != null)
                        this.repositoryHasAttach.Delete(RequirePaintingList.RequirePaintingListHasAttachId);
                    // remove attach
                    return new JsonResult(await this.repositoryAttach.DeleteAsync(AttachFile.AttachFileId), this.DefaultJsonSettings);
                }
            }
            return NotFound(new { Error = "Not found attach file." });
        }

        #endregion
    }
}
