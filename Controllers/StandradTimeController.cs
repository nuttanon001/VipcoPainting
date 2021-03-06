﻿using AutoMapper;
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
    [Route("api/StandradTime")]
    public class StandradTimeController : Controller
    {
        #region PrivateMenbers

        private IRepositoryPainting<StandradTime> repository;
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        private HelpersClass<StandradTime> helpers;

        #endregion PrivateMenbers

        #region Constructor

        public StandradTimeController(IRepositoryPainting<StandradTime> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
            this.helpers = new HelpersClass<StandradTime>();
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion Constructor

        #region GET

        // GET: api/StandradTime
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(this.ConvertTable.ConverterTableToViewModel<StandardTimeViewModel,StandradTime>( await this.repository.GetAllAsync()), this.DefaultJsonSettings);
            // var Includes = new List<string> { "" };
            // return new JsonResult(await this.repository.GetAllWithInclude2Async(Includes),
            //                            this.DefaultJsonSettings);
        }

        // GET: api/StandradTime/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            // return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
            var Includes = new List<string> { "LinkStandradTime" };
            var HasData = this.mapper.Map<StandradTime, StandardTimeViewModel>(
                            await this.repository.GetAsynvWithIncludes(key, "StandradTimeId",Includes));
            return new JsonResult(HasData, this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST

        // POST: api/StandradTime/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable().AsQueryable();
            // Where
            if (!string.IsNullOrEmpty(Scroll.Where))
            {
                QueryData = QueryData.Where(x => x.TypeStandardTime == (Scroll.Where == "1" ? TypeStandardTime.Paint : TypeStandardTime.Blast));
            }

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.Code.ToLower().Contains(keyword) ||
                                                 x.Description.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "Code":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Code);
                    else
                        QueryData = QueryData.OrderBy(e => e.Code);
                    break;

                case "Description":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.Description);
                    else
                        QueryData = QueryData.OrderBy(e => e.Description);
                    break;

                case "TypeStandardTimeString":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.TypeStandardTime);
                    else
                        QueryData = QueryData.OrderBy(e => e.TypeStandardTime);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.CreateDate);
                    break;
            }

            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<StandradTime>(Scroll, 
                                    this.ConvertTable.ConverterTableToViewModel<StandardTimeViewModel,StandradTime>(
                                        await QueryData.AsNoTracking().ToListAsync())), this.DefaultJsonSettings);
        }

        // POST: api/StandradTime
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StandradTime nStandradTime)
        {
            if (nStandradTime != null)
            {
                nStandradTime = helpers.AddHourMethod(nStandradTime);

                nStandradTime.CreateDate = DateTime.Now;
                nStandradTime.Creator = nStandradTime.Creator ?? "Someone";

                nStandradTime.LinkStandradTime = null;
                var complate = await this.repository.AddAsync(nStandradTime);

                if (complate != null && complate.LinkStandardTimeId.HasValue)
                {
                    var update = await this.repository.GetAsync(complate.LinkStandardTimeId.Value);
                    if (update != null)
                    {
                        update.LinkStandardTimeId = complate.StandradTimeId;
                        await this.repository.UpdateAsync(update, update.StandradTimeId);
                    }
                }

                return new JsonResult(complate, this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found StandradTime data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/StandradTime/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]StandradTime uStandradTime)
        {
            var Message = "Not found StandradTime data.";
            try
            {
                if (uStandradTime != null)
                {
                    // add hour to DateTime to set Asia/Bangkok
                    uStandradTime = helpers.AddHourMethod(uStandradTime);

                    uStandradTime.ModifyDate = DateTime.Now;
                    uStandradTime.Modifyer = uStandradTime.Modifyer ?? "Someone";
                    uStandradTime.LinkStandradTime = null;

                    var UpdateData = await this.repository.UpdateAsync(uStandradTime, key);
                    if (UpdateData != null)
                    {
                        if (UpdateData.LinkStandardTimeId.HasValue)
                        {
                            var update = await this.repository.GetAsync(UpdateData.LinkStandardTimeId.Value);
                            if (update != null)
                            {
                                update.LinkStandardTimeId = UpdateData.StandradTimeId;
                                await this.repository.UpdateAsync(update, update.StandradTimeId);
                            }
                        }

                        return new JsonResult(UpdateData, this.DefaultJsonSettings);
                    }
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

        // DELETE: api/StandradTime/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
