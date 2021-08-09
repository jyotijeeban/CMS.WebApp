using CMS.Repositories;
using CMS.Repositories.Models;
using CMS.WebApp.Helpers;
using CMS.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApp.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        IUnitOfWork _uow;
        public DashboardController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult Index(int page = 1, int pageSize = 2)
        {
            PagingModel<UserModel> model = _uow.UserRepo.GetUsers(page, pageSize);
            return View(model);
        }
    }
}
