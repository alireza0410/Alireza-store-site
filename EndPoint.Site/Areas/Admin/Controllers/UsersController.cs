using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bugeto_Store.Application.Services.Users.Commands.RgegisterUser;
using Bugeto_Store.Application.Services.Users.Queries.GetRoles;
using Bugeto_Store.Application.Services.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;

namespace EndPoint.Site.Areas.Admin.Controllers
{   [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IGetUsersService _getUsersService;
        private readonly IGetRolesService _getRolesService;
        private readonly IRegisterUserService _RegisterUserService;
      
        public UsersController(
            IGetUsersService getUsersService,
            IGetRolesService getRolesService, 
            IRegisterUserService registeredServices)
        {
            _getUsersService = getUsersService;
            _getRolesService = getRolesService;
            _RegisterUserService = registeredServices;
        }

      
        public IActionResult Index( string searchkey,int page=1)
        {
            return View(_getUsersService.Execute(new RequestGetUserDto
            {
                Page=page,
                SearchKey=searchkey
            }));
        }
        [HttpGet ]
        public IActionResult Creat()
        {
            ViewBag.Roles = new SelectList(_getRolesService.Execute().Data, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Creat(string Email, string FullName, long RoleId, string Password, string RePassword)
        {
            var result = _RegisterUserService.Execute(new RequestRgegisterUserDto
            {
                Email = Email,
                FullName = FullName,
                roles = new List<RolesInRgegisterUserDto>()
                   {
                        new RolesInRgegisterUserDto
                        {
                             Id= RoleId
                        }
                   },
                Password = Password,
                RePasword = RePassword,
            });
            return Json(result);
            
            
        }
    }
}
