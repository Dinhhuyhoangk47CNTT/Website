using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Controllers;

namespace Website.Areas.Admin.Controllers
{
    public class MauDangKyController : BaseController
    {

        public MauDangKyController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
