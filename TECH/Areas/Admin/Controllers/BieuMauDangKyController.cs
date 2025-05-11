using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Controllers;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Service;

namespace Website.Areas.Admin.Controllers
{
    public class BieuMauDangKyController : BaseController
    {
        private readonly IVanBanService _vanBanService;

        public BieuMauDangKyController(IVanBanService vanBanService)
        {
            _vanBanService = vanBanService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new VanBanViewModel();

            if (id > 0)
            {
                model = _vanBanService.GetById(id);
            }

            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(VanBanViewModel vanBanViewModel)
        {
            if (_vanBanService.IsExist(vanBanViewModel.TieuDe))
            {
                return Json(new
                {
                    success = false
                });
            }

            vanBanViewModel.LoaiVanBan = "BIEU_MAU";
            _vanBanService.Add(vanBanViewModel);
            _vanBanService.Save();

            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public JsonResult Update(VanBanViewModel vanBanViewModel)
        {
            vanBanViewModel.LoaiVanBan = "BIEU_MAU";
            var result = _vanBanService.Update(vanBanViewModel);
            _vanBanService.Save();

            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _vanBanService.Deleted(id);

            _vanBanService.Save();

            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var data = _vanBanService.GetAll()
                .Where(x => x.LoaiVanBan == "BIEU_MAU");

            return Json(new { Data = data });
        }

        [HttpGet]
        public JsonResult GetAllPaging(VanBanViewModelSearch search)
        {
            search.LoaiVanBan = "BIEU_MAU";
            var data = _vanBanService.GetAllPaging(search);
            return Json(new { data = data });
        }
    }
}
