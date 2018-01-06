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
    [Route("api/PaintTeam")]
    public class PaintTeamController : Controller
    {
        #region PrivateMembers
        // Repository
        private IRepositoryPainting<PaintTeam> repository;
        // Mapper
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;
        #endregion PrivateMenbers

        #region Constructor
        public PaintTeamController(IRepositoryPainting<PaintTeam> repo, IMapper map)
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

        // GET: api/PaintTeam
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);

            //var Includes = new List<string> { "PaintTeam" };
            //return new JsonResult(this.ConvertTable.ConverterTableToViewModel<BlastRoomViewModel, PaintTeam>
            //                     (await this.repository.GetAllWithInclude2Async(Includes)),
            //                      this.DefaultJsonSettings);
        }

        // GET: api/PaintTeam/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);

            //var Includes = new List<string> { "PaintTeam" };
            //return new JsonResult(this.mapper.Map<PaintTeam, BlastRoomViewModel>
            //                     (await this.repository.GetAsynvWithIncludes(key, "BlastRoomId", Includes)),
            //                      this.DefaultJsonSettings);
        }

        #endregion GET

        #region POST
        // POST: api/PaintTeam/GetScroll
        [HttpPost("GetScroll")]
        public async Task<IActionResult> GetScroll([FromBody] ScrollViewModel Scroll)
        {
            var QueryData = this.repository.GetAllAsQueryable().AsQueryable();

            // Filter
            var filters = string.IsNullOrEmpty(Scroll.Filter) ? new string[] { "" }
                                : Scroll.Filter.ToLower().Split(null);

            foreach (var keyword in filters)
            {
                QueryData = QueryData.Where(x => x.TeamName.ToLower().Contains(keyword));
            }

            // Order
            switch (Scroll.SortField)
            {
                case "TeamName":
                    if (Scroll.SortOrder == -1)
                        QueryData = QueryData.OrderByDescending(e => e.TeamName);
                    else
                        QueryData = QueryData.OrderBy(e => e.TeamName);
                    break;

                default:
                    QueryData = QueryData.OrderByDescending(e => e.CreateDate);
                    break;
            }
            // Skip Take
            QueryData = QueryData.Skip(Scroll.Skip ?? 0).Take(Scroll.Take ?? 50);

            return new JsonResult(new ScrollDataViewModel<PaintTeam>(Scroll,
                await QueryData.AsNoTracking().ToListAsync()), this.DefaultJsonSettings);
        }

        // POST: api/PaintTeam
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PaintTeam nBlastWorkItem)
        {
            if (nBlastWorkItem != null)
            {
                nBlastWorkItem.CreateDate = DateTime.Now;
                nBlastWorkItem.Creator = nBlastWorkItem.Creator ?? "Someone";

                return new JsonResult(await this.repository.AddAsync(nBlastWorkItem), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found blast room data !!!" });
        }

        #endregion POST

        #region PUT

        // PUT: api/PaintTeam/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]PaintTeam uBlastWorkItem)
        {
            var Message = "PaintTeam not been found.";

            if (uBlastWorkItem != null)
            {
                // set modified
                uBlastWorkItem.ModifyDate = DateTime.Now;
                uBlastWorkItem.Modifyer = uBlastWorkItem.Modifyer ?? "Someone";

                return new JsonResult(await this.repository.UpdateAsync(uBlastWorkItem, key), this.DefaultJsonSettings);
            }

            return NotFound(new { Error = Message });
        }

        #endregion PUT

        #region DELETE

        // DELETE: api/PaintTeam/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }

        #endregion DELETE
    }
}
