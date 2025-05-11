using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Controllers;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Service;

namespace Website.Areas.Admin.Controllers
{
    public class TienNuocController : BaseController
    {
        private readonly ITienNuocService _serrvice;

        public TienNuocController(
            ITienNuocService service
            )
        {
            _serrvice = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new TienNuocModelView();

            if (id > 0)
            {
                model = _serrvice.GetById(id);
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
        public JsonResult Add(TienNuocModelView vm)
        {
            _serrvice.Add(vm);
            _serrvice.Save();

            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public JsonResult Update(TienNuocModelView vm)
        {
            var result = _serrvice.Update(vm);
            _serrvice.Save();

            return Json(new
            {
                success = result
            });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _serrvice.Deleted(id);

            _serrvice.Save();

            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var data = _serrvice.GetAll();
            return Json(new { Data = data });
        }

        [HttpGet]
        public JsonResult GetAllPaging(TienNuocViewModelSearch search)
        {
            var data = _serrvice.GetAllPaging(search);
            return Json(new { data = data });
        }
    }
}
