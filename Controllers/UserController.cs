using AutoMapper;
using Newtonsoft.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.IO;
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
    [Route("api/User")]
    public class UserController : Controller
    {
        #region PrivateMenbers

        private IRepositoryMachine<User> repository;
        private IMapper mapper;
        private JsonSerializerSettings DefaultJsonSettings;
        private ConverterTableToVM ConvertTable;

        #endregion PrivateMenbers

        #region Constructor

        public UserController(IRepositoryMachine<User> repo, IMapper map)
        {
            this.repository = repo;
            this.mapper = map;
            // Json
            this.DefaultJsonSettings = new Helpers.JsonSerializer().DefaultJsonSettings;
            // ConvertTable
            this.ConvertTable = new ConverterTableToVM(map);
        }

        #endregion

        #region GET

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(await this.repository.GetAllAsync(), this.DefaultJsonSettings);
        }

        // GET: api/User/5
        [HttpGet("{key}")]
        public async Task<IActionResult> Get(int key)
        {
            return new JsonResult(await this.repository.GetAsync(key), this.DefaultJsonSettings);
        }

        // GET: api/User/EmployeeAlready
        [HttpGet("EmployeeAlready/{EmpCode}")]
        public async Task<IActionResult> EmployeeAlready(string EmpCode)
        {
            Expression<Func<User, bool>> condition = u => u.EmpCode == EmpCode;
            if (await this.repository.AnyDataAsync(condition))
                return NotFound(new { Error = " this employee was already in system." });

            return new JsonResult(new { Result = true }, this.DefaultJsonSettings);
        }

        #endregion

        #region POST
        // POST: api/LoginName/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {

            var Message = "Login has error.";
            try
            {
                var HasData = await this.repository.GetAllAsQueryable()
                                               .Include(x => x.EmpCodeNavigation)
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(m => m.UserName.ToLower() == login.UserName.ToLower() &&
                                                                         m.PassWord.ToLower() == login.PassWord.ToLower());
                if (HasData != null)
                    return new JsonResult(this.mapper.Map<User, UserViewModel>(HasData), this.DefaultJsonSettings);
                else
                    return NotFound(new { Error = "user or password not match" });
            }
            catch(Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }
            return NotFound(new { Error = Message });
        }


        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User nUser)
        {
            if (nUser != null)
            {
                Expression<Func<User, bool>> condition = u => u.UserName.ToLower() == nUser.UserName.ToLower();
                if (await this.repository.AnyDataAsync(condition))
                {
                    return NotFound(new { Error = " this username was already in system." });
                }

                nUser.CreateDate = DateTime.Now;
                nUser.Creator = nUser.Creator ?? "Someone";

                return new JsonResult(await this.repository.AddAsync(nUser), this.DefaultJsonSettings);
            }
            return NotFound(new { Error = "Not found user data !!!" });
        }
        #endregion

        #region PUT
        // PUT: api/User/5
        [HttpPut("{key}")]
        public async Task<IActionResult> PutByNumber(int key, [FromBody]User uUser)
        {
            var Message = "Not found user data.";

            try
            {
                if (uUser != null)
                {
                    uUser.ModifyDate = DateTime.Now;
                    uUser.Modifyer = uUser.Modifyer ?? "Someone";

                    var UpdateData = await this.repository.UpdateAsync(uUser, key);
                    if (UpdateData != null)
                    {
                        var Includes = new List<string> { "EmpCodeNavigation" };
                        return new JsonResult(
                           this.mapper.Map<User, UserViewModel>(await this.repository.GetAsynvWithIncludes(key, "UserId", Includes)),
                           this.DefaultJsonSettings);
                    }
                }
            }
            catch (Exception ex)
            {
                Message = $"Has error {ex.ToString()}";
            }

            return NotFound(new { Error = Message });
        }
        #endregion

        #region DELETE
        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return new JsonResult(await this.repository.DeleteAsync(id), this.DefaultJsonSettings);
        }
        #endregion
    }
}
