using Bugeto_Store.Application.Interfaces.Contexts;
using Bugeto_Store.Common.Dto;
using Bugeto_Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;

namespace Bugeto_Store.Application.Services.Users.Commands.RgegisterUser
{
    public interface IRegisterUserService
    {
        ResultDto<ResultRgegisterUserDto> Execute(RequestRgegisterUserDto request);
    }

    public class RgegisterUserService : IRegisterUserService
    {
        private readonly IDataBaseContext _context;

        public RgegisterUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultRgegisterUserDto> Execute(RequestRgegisterUserDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return new ResultDto<ResultRgegisterUserDto>()
                    {
                        Data = new ResultRgegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "پست الکترونیک را وارد نمایید"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.FullName))
                {
                    return new ResultDto<ResultRgegisterUserDto>()
                    {
                        Data = new ResultRgegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام را وارد نمایید"
                    };
                }
                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return new ResultDto<ResultRgegisterUserDto>()
                    {
                        Data = new ResultRgegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "رمز عبور را وارد نمایید"
                    };
                }
                if (request.Password != request.RePasword)
                {
                    return new ResultDto<ResultRgegisterUserDto>()
                    {
                        Data = new ResultRgegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "رمز عبور و تکرار آن برابر نیست"
                    };
                }

                User user = new User()
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    //Password = HashPassword.Execute(request.Password),
                };

                List<UserInRole> userInRoles = new List<UserInRole>();

                foreach (var item in request.roles)
                {
                    var roles = _context.Roles.Find(item.Id);
                    userInRoles.Add(new UserInRole
                    {
                        Role = roles,
                        RoleId = roles.Id,
                        User = user,
                        UserId = user.Id,
                    });
                }
                user.UserInRoles = userInRoles;

                _context.Users.Add(user);

                _context.SaveChanges();

                return new ResultDto<ResultRgegisterUserDto>()
                {
                    Data = new ResultRgegisterUserDto()
                    {
                        UserId = user.Id,

                    },
                    IsSuccess = true,
                    Message = "ثبت نام کاربر انجام شد",
                };
            }
            catch (Exception)
            {
                return new ResultDto<ResultRgegisterUserDto>()
                {
                    Data = new ResultRgegisterUserDto()
                    {
                        UserId = 0,
                    },
                    IsSuccess = false,
                    Message = "ثبت نام انجام نشد !"
                };
            }
        }
    }
    public class RequestRgegisterUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePasword { get; set; }

        public List<RolesInRgegisterUserDto> roles { get; set; }
    }

    public class RolesInRgegisterUserDto
    {
        public long Id { get; set; }
    }

    public class ResultRgegisterUserDto
    {
        public long UserId { get; set; }

    }
   
}
