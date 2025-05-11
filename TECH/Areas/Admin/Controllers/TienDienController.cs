using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Controllers;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Service;

namespace Website.Areas.Admin.Controllers
{
    public class TienDienController : BaseController
    {
        private readonly ITienDienService _service;

        public TienDienController(
            ITienDienService service
            )
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new TienDienModelView();

            if (id > 0)
            {
                model = _service.GetById(id);
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
        public JsonResult Add(TienDienModelView vm)
        {
            _service.Add(vm);
            _service.Save();

            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public JsonResult Update(TienDienModelView vm)
        {
            var result = _service.Update(vm);
            _service.Save();

            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _service.Deleted(id);

            _service.Save();

            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var data = _service.GetAll();
            return Json(new { Data = data });
        }

        [HttpGet]
        public JsonResult GetAllPaging(TienDienViewModelSearch search)
        {
            var data = _service.GetAllPaging(search);
            return Json(new { data = data });
        }
    }
}
