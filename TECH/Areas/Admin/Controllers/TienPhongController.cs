using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Controllers;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Service;

namespace Website.Areas.Admin.Controllers
{
    public class TienPhongController : BaseController
    {
        private readonly ITienPhongService _service;

        public TienPhongController(
            ITienPhongService service
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
            var model = new TienPhongModelView();

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
        public JsonResult Add(TienPhongModelView vm)
        {
            _service.Add(vm);
            _service.Save();

            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public JsonResult Update(TienPhongModelView vm)
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
        public JsonResult GetAllPaging(TienPhongViewModelSearch search)
        {
            var data = _service.GetAllPaging(search);
            return Json(new { data = data });
        }
    }
}
