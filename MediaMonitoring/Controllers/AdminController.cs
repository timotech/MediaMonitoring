using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaMonitoring.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediaMonitoring.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment hostingEnvironment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            this.hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Customers()
        {
            ViewBag.AllCustomers = db.Users.ToList();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateRoles(string Id = "")
        {
            ListRoles();

            if (Id != "")
            {
                var role = await roleManager.FindByIdAsync(Id);
                if (role != null)
                {
                    var model = new RolesViewModel
                    {
                        ID = role.Id,
                        RoleName = role.Name
                    };

                    return View(model);
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoles(RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("CreateRoles");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        public void ListRoles()
        {
            var roles = roleManager.Roles.ToList();
            ViewBag.AllRoles = roles;
        }

        [HttpGet]
        public IActionResult EditRoles(string Id)
        {
            return RedirectToAction("CreateRoles", Id);
        }

        public async Task<IActionResult> DeleteRoles(string Id)
        {
            if (Id != "")
            {
                var role = await roleManager.FindByIdAsync(Id);
                if (role != null)
                {
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("CreateRoles");
                    }
                }
            }

            return RedirectToAction("CreateRoles");
        }

        [HttpGet]
        public IActionResult GetBrands(string companyId)
        {
            if (!string.IsNullOrEmpty(companyId))
            {
                companyId = companyId.Substring(1);
                IEnumerable<SelectListItem> depts = db.Brands.AsNoTracking()
                    .OrderBy(x => x.Description)
                    .Where(x => x.FK_AdvertizerId == companyId).Select(n =>
                           new SelectListItem
                           {
                               Value = n.BrandId,
                               Text = n.Description
                           }).ToList();
                return Json(depts);
            }
            return null;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ListRoles();

            var AllCategories = db.Products.OrderBy(x => x.Description).ToList();
            AllCategories.Insert(0, new Product { ProductId = "0" , Description = "Select Category" });
            ViewBag.AllCategories = AllCategories;

            var AllCompany = db.CodeFiles.Where(x => x.codefileid.StartsWith("G")).OrderBy(x => x.Description).ToList();
            AllCompany.Insert(0, new CodeFile { codefileid = "0", Description = "Select Company" });
            ViewBag.AllCompany = AllCompany;

            ViewBag.AllBrands = new List<SelectListItem>()
                {
                    new SelectListItem
                    {
                        Text = "",
                        Value = null
                    }
                };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RolesRegisterViewModel model, IFormCollection formCollection)
        {
            if (ModelState.IsValid)
            {
                string accessLevels = formCollection["AccessLevel"];
                string[] userRoles = accessLevels.Split(",");

                string brands = formCollection["brand"];
                //string[] brandsSplit = brands.Split(" ");

                model.Brand = brands;

                model.CompanyId = model.Company.Substring(1);

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, CategoryId = model.CategoryId, Company = model.Company, Brand = model.Brand, CompanyId = model.CompanyId, FirstName = model.FirstName, LastName = model.LastName };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //await signInManager.SignInAsync(user, isPersistent: false);  //Sign In new User

                    //Add user to selected role
                    foreach(var item in userRoles)
                    {
                        var role = await roleManager.FindByNameAsync(item);

                        IdentityResult roles = await userManager.AddToRoleAsync(user, role.Name);
                        if (!roles.Succeeded)
                        {
                            throw new ApplicationException("Adding user '" + user.UserName + "' to '" + model.UserRole + "' role failed with error(s): " + (object)roles.Errors);
                        }
                    }
                    

                    //Confirm user's email automatically
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var result2 = await userManager.ConfirmEmailAsync(user, token);

                    if (result2.Succeeded == false)
                    {
                        ModelState.AddModelError("ConfirmationError", "Could not confirm users email, pls try again by recreating the user");
                    }

                    return RedirectToAction("Register");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            ListRoles();

            var AllCategories = db.Products.OrderBy(x => x.Description).ToList();
            AllCategories.Insert(0, new Product { ProductId = "0", Description = "Select Category" });
            ViewBag.AllCategories = AllCategories;

            var AllCompany = db.CodeFiles.Where(x => x.codefileid.StartsWith("G")).OrderBy(x => x.Description).ToList();
            AllCompany.Insert(0, new CodeFile { codefileid = "0", Description = "Select Company" });
            ViewBag.AllCompany = AllCompany;

            ViewBag.AllBrands = new List<SelectListItem>()
                {
                    new SelectListItem
                    {
                        Text = "",
                        Value = null
                    }
                };

            return View(model);
        }
    }
}
