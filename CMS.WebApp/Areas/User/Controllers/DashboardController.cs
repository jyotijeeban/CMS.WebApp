using CMS.Repositories;
using CMS.Repositories.Models;
using CMS.Services.Interfaces;
using CMS.WebApp.Helpers;
using CMS.WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;


namespace CMS.WebApp.Areas.User.Controllers
{
    [CustomAuthorize(Roles = "User")]
    [Area("User")]
    public class DashboardController : Controller
    {
        IAuthService _authService;
        IUnitOfWork _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DashboardController(IAuthService authService, IUnitOfWork uow, IWebHostEnvironment webHostEnvironment)
        {
            _authService = authService;
            _uow = uow;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            string UserId = HttpContext.Session.GetString("UserId");
            var user = _authService.FindById(UserId);
            UserViewModel model = new UserViewModel
            {
                Name = user.Name,
                Email = user.Email,
                Image = user.Image,
                MobileNo = user.MobileNo,
                Designation = user.Designation,
                CountryId = user.CountryId,
                Country = user.CountryId.ToString(),
                Gender = user.Gender
            };

            //var countryList = _uow.CountryRepo.GetAll();
            //model.Country = countryList.Where(x => x.CountryId == model.CountryId).SingleOrDefault().CountryName;

            ViewBag.CountryList = _uow.CountryRepo.GetAll();
            return View(model);
        }

        private string UploadedFile(UserViewModel model)
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

        [HttpPost]
        public IActionResult Profile(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = string.Empty;

                if (model.ImageFile != null)
                    uniqueFileName = UploadedFile(model);
                else
                    uniqueFileName = model.Image;

                string UserId = HttpContext.Session.GetString("UserId");
                var user = _authService.FindById(UserId);

                user.Id = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                user.Name = model.Name;
                user.Image = uniqueFileName;
                user.MobileNo = model.MobileNo;
                user.PhoneNumber = model.MobileNo;
                user.Designation = model.Designation;
                user.CountryId = Convert.ToInt32(model.Country);
                user.Gender = model.Gender;
                user.UserName = HttpContext.Session.GetString("UserName").ToString();
                user.SecurityStamp = Guid.NewGuid().ToString();

                _uow.UserRepo.AutoDetectChangesEnabled();
                bool result = _authService.UpdateUser(user);
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
    }
}