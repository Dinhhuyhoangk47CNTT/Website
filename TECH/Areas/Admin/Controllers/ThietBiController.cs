using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Controllers;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Service;

namespace Website.Areas.Admin.Controllers
{
    public class ThietBiController : BaseController
    {
        private readonly IThietBiService _thietBiService;

        public ThietBiController(
            IThietBiService thietBiService
            )
        {
            _thietBiService = thietBiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new ThietBiModelView();

            if (id > 0)
            {
                model = _thietBiService.GetById(id);
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
        public JsonResult Add(ThietBiModelView thietBiModelView)
        {
            if (_thietBiService.IsExist(thietBiModelView.TenThietBi))
            {
                return Json(new
                {
                    success = false
                });
            }

            _thietBiService.Add(thietBiModelView);
            _thietBiService.Save();

            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public JsonResult Update(ThietBiModelView thietBiModelView)
        {
            var result = _thietBiService.Update(thietBiModelView);
            _thietBiService.Save();

            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _thietBiService.Deleted(id);

            _thietBiService.Save();

            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var data = _thietBiService.GetAll();
            return Json(new { Data = data });
        }

        [HttpGet]
        public JsonResult GetAllPaging(ThietBiViewModelSearch Search)
        {
            var data = _thietBiService.GetAllPaging(Search);
            return Json(new { data = data });
        }
    }
}
