using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bugeto_Store.Application.Services.Users.Commands.EditUser;
using Bugeto_Store.Application.Services.Users.Commands.RemoveUser;
using Bugeto_Store.Application.Services.Users.Commands.RgegisterUser;
using Bugeto_Store.Application.Services.Users.Commands.UserSatusChange;
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
        private readonly IRemoveUserService _RemoveUserService;
        private readonly IUserSatusChangeService _userSatusChangeService;
        private readonly IEditUserService _editUserService;


        public UsersController(
            IGetUsersService getUsersService,
            IGetRolesService getRolesService, 
            IRegisterUserService registeredServices,
            IRemoveUserService removeUserService,
            IUserSatusChangeService userSatusChangeService,
            IEditUserService editUserService)
        {
            _getUsersService = getUsersService;
            _getRolesService = getRolesService;
            _RegisterUserService = registeredServices;
            _RemoveUserService = removeUserService;
            _userSatusChangeService = userSatusChangeService;
            _editUserService = editUserService;

        }


        public IActionResult Index( string searchkey, int page=1)
        {
            return View(_getUsersService.Execute(new RequestGetUserDto
            {
                Page=page,
                SearchKey= searchkey
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
        [HttpPost]
        public IActionResult Delete(long UserId)
        {
            return Json(_RemoveUserService.Execute(UserId));
        }

        [HttpPost]
        public IActionResult UserSatusChange(long UserId)
        {
            return Json(_userSatusChangeService.Execute(UserId));
        }

        [HttpPost]
        public IActionResult Edit(long UserId, string Fullname)
        {
            return Json(_editUserService.Execute(new RequestEdituserDto
            {
                Fullname = Fullname,
                UserId = UserId,
            }));
        }
    }
}

