using Microsoft.AspNetCore.Mvc;
using Website.Service;

namespace Website.Controllers
{
    public class FormLinkDangKyController : Controller
    {
        private readonly IVanBanService _vanBanService;
        public FormLinkDangKyController(IVanBanService vanBanService)
        {
            _vanBanService = vanBanService;
        }

        public IActionResult Index()
        {
            var data = _vanBanService.GetAll()
                .Where(x => x.LoaiVanBan == "BIEU_MAU")
                .OrderByDescending(x => x.NgayHetHan)
                .ToList();

            return View(data);
        }

        public IActionResult View(int Id)
        {
            var data = _vanBanService.GetById(Id);
            return View(data);
        }
    }
}
