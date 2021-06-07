using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiRoleAuthentication.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiRoleAuthentication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private DBContext _dbContext;
        private UserManager<MultiRoleAuthUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AdminController(DBContext dbContext, 
                                RoleManager<IdentityRole> roleManager,
                                UserManager<MultiRoleAuthUser> userManager)
        {
            this._dbContext = dbContext;
            this._roleManager = roleManager;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AllUsers()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> ApproveUser(string id, int btnAction)
        {
            if (btnAction == 0)
            {
                var user = await _userManager.FindByIdAsync(id);
                var logins = await _userManager.GetLoginsAsync(user);
                var rolesForUser = await _userManager.GetRolesAsync(user);

                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    IdentityResult result = IdentityResult.Success;
                    foreach (var login in logins)
                    {
                        result = await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                        if (result != IdentityResult.Success)
                            break;
                    }
                    if (result == IdentityResult.Success)
                    {
                        foreach (var item in rolesForUser)
                        {
                            result = await _userManager.RemoveFromRoleAsync(user, item);
                            if (result != IdentityResult.Success)
                                break;
                        }
                    }
                    if (result == IdentityResult.Success)
                    {
                        result = await _userManager.DeleteAsync(user);
                        if (result == IdentityResult.Success)
                            transaction.Commit(); //only commit if user and all his logins/roles have been deleted  
                    }
                }
            }
            else
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
                user.LockoutEnd = null;
                _dbContext.SaveChanges();
            }

            return RedirectToAction("AllUsers");
        }

        public IActionResult UserRole()
        {
            var roles = _roleManager.Roles.ToList().OrderBy(x => x.Name);
            return View(roles);
        }

        public IActionResult CreateUserRole()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserRole(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
            return RedirectToAction("UserRole");
        }

        public async Task<ActionResult> DeleteRole(string id)
        {
            if (id != "Admin")
            {
                var role = await _roleManager.FindByNameAsync(id);
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("UserRole");
        }
    }
}
