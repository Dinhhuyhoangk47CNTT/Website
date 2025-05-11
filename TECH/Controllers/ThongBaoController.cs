using Microsoft.AspNetCore.Mvc;
using Website.Service;

namespace Website.Controllers
{
    public class ThongBaoController : Controller
    {
        private readonly IVanBanService _vanBanService;
        public ThongBaoController(IVanBanService vanBanService)
        {
            _vanBanService = vanBanService;
        }

        public IActionResult Index()
        {
            var data = _vanBanService.GetAll()
                .Where(x => x.LoaiVanBan == "THONG_BAO")
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
