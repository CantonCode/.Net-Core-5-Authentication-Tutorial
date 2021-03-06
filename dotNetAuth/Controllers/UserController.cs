using dotNetAuth.BindingModel;
using dotNetAuth.Data.Entities;
using dotNetAuth.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetAuth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
       

        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signManager;


        public UserController(ILogger<UserController> logger,UserManager<AppUser> userManger, SignInManager<AppUser> signManager)
        {
            _logger = logger;
            _userManager = userManger;
            _signManager = signManager;

        }


        [HttpPost("RegisterUser")]
        public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
        {
            try
            {
                var user = new AppUser()
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    UserName = model.Email,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return await Task.FromResult("User has been Registered");
                }

                return await Task.FromResult(result.Errors);

            }
            catch(Exception ex)
            {
                return await Task.FromResult(ex);
            }
        }
           
        [HttpGet("GetAllUser")]
        public async Task<object> GetAllUser()
        {
            try
            {
                var users = _userManager.Users.Select(x => new UserDTO(x.FullName, x.Email, x.UserName,x.DateCreated)); ;

                
                return await Task.FromResult(users);
            }
            catch(Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<object> Login([FromBody] LoginBindingModel model)
        {
            try
            {
                var email = model.Email;
                var password = model.Password;

                if (ModelState.IsValid)
                {
                    var result = await _signManager.PasswordSignInAsync(email, password, false, false);

                    if (result.Succeeded)
                    {
                        return await Task.FromResult("Login Success");
                    }

                }

                return await Task.FromResult("Invalid Email or Passowrd");
                    

            }
            catch(Exception ex)
            {
                return await Task.FromResult(ex.Message);
            }
        }

     
    }
}
