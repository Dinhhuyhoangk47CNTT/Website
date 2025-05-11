using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using TECH.Areas.Admin.Controllers;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Service;

namespace Website.Areas.Admin.Controllers
{
    public class ThongBaoController : BaseController
    {
        private readonly IVanBanService _vanBanService;

        public ThongBaoController(IVanBanService vanBanService)
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

            vanBanViewModel.LoaiVanBan = "THONG_BAO";
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
            vanBanViewModel.LoaiVanBan = "THONG_BAO";
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
                .Where(x => x.LoaiVanBan == "THONG_BAO");

            return Json(new { Data = data });
        }

        [HttpGet]
        public JsonResult GetAllPaging(VanBanViewModelSearch search)
        {
            search.LoaiVanBan = "THONG_BAO";
            var data = _vanBanService.GetAllPaging(search);
            return Json(new { data = data });
        }
    }
}
