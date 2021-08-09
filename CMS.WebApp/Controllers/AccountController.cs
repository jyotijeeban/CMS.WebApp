using CMS.DataAccess;
using CMS.Repositories;
using CMS.Repositories.Models;
using CMS.Services.Interfaces;
using CMS.WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.WebApp.Controllers
{
    public class AccountController : Controller
    {
        IAuthService _authService;
        IUnitOfWork _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(IAuthService authService, IUnitOfWork uow, IWebHostEnvironment webHostEnvironment)
        {
            _authService = authService;
            _uow = uow;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            var user = _authService.AuthenticateUser(model.Email, model.Password);
            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserName", user.UserName.ToString());
                if (user.Roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                }
                else if (user.Roles.Contains("User"))
                {
                    return RedirectToAction("Index", "Dashboard", new { area = "User" });
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            ViewBag.CountryList = _uow.CountryRepo.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var userCheck = _authService.FindByMail(model.Email);
                if (userCheck == null)
                {
                    string uniqueFileName = UploadedFile(model);

                    User user = new User
                    {
                        Name = model.Name,
                        UserName = model.Email,
                        Email = model.Email,
                        Image = uniqueFileName,
                        MobileNo = model.MobileNo,
                        PhoneNumber = model.MobileNo,
                        Designation = model.Designation,
                        CountryId = Convert.ToInt32(model.Country),
                        Gender = model.Gender
                    };

                    bool result = _authService.CreateUser(user, model.Password);
                    if (result)
                    {                        
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    ViewBag.Error = "Email already exists.";
                }
            }
            else
            {
                ModelState.AddModelError("message", string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage)));
            }

            ViewBag.CountryList = _uow.CountryRepo.GetAll();
            return View(model);
        }

        private string UploadedFile(UserModel model)
        {
            string uniqueFileName = null;

            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public IActionResult Unauthorize()
        {
            return View();
        }

        public IActionResult LogOutComplete()
        {
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> SignOutResult()
        {
            await _authService.SignOut();
            return RedirectToAction("LogOutComplete");
        }
    }
}
