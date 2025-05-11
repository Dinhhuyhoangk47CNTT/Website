using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.General;
using TECH.Models;
using TECH.Service;
using TECH.Utilities;

namespace TECH.Controllers
{
    public class PhongController : Controller
    {
        private readonly IPhongService _phongService;
        private readonly INhaService _nhaService;
        //private readonly ICategoryService _categoryService;
        //private readonly ISizesService _sizesService;
        public IHttpContextAccessor _httpContextAccessor;
        public PhongController(IPhongService phongService,
            IHttpContextAccessor httpContextAccessor,
            INhaService nhaService
            )
        {
            _phongService = phongService;
            _nhaService = nhaService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult PhongCategory(int categoryId)
        {
            var productViewModelSearch = new PhongViewModelSearch();
            productViewModelSearch.PageIndex = 1;
            productViewModelSearch.PageSize = 250;
            productViewModelSearch.categoryId = categoryId;
            var data = _phongService.GetAllPaging(productViewModelSearch);
            if (data != null && data.Results != null && data.Results.Count > 0)
            {
                //data.Results = data.Results.Where(p => p.ishidden != 1).ToList();
                foreach (var item in data.Results)
                {
                    if (item.MaNha.HasValue && item.MaNha.Value > 0)
                    {
                        var category = _nhaService.GetByid(item.MaNha.Value);
                        if (category != null && !string.IsNullOrEmpty(category.TenNha))
                        {
                            item.TenNha = category.TenNha;
                        }
                        else
                        {
                            item.TenNha = "Chờ xử lý";
                        }
                    }
                    else
                    {
                        item.TenNha = "";
                    }
                    if (item.LoaiPhong.HasValue)
                    {
                        item.LoaiPhongStr = Common.GetLoaiPhong(item.LoaiPhong.Value);
                    }
                    if (item.TinhTrang.HasValue)
                    {
                        item.TinhTrangStr = Common.GetTinhTrangPhong(item.TinhTrang.Value);
                    }
                }
            }
            return View(data.Results.ToList());
        }

        public IActionResult PhongDetail(int phongId)
        {
            var model = new PhongModelView();
            if (phongId > 0)
            {
                model = _phongService.GetByid(phongId);
                if (model != null)
                {
                    if (model.MaNha.HasValue && model.MaNha.Value > 0)
                    {
                        var nha = _nhaService.GetByid(model.MaNha.Value);
                        if (nha != null && !string.IsNullOrEmpty(nha.TenNha))
                        {
                            model.TenNha = nha.TenNha;
                        }
                        if (model.LoaiPhong.HasValue)
                        {
                            model.LoaiPhongStr = Common.GetLoaiPhong(model.LoaiPhong.Value);
                        }
                        if (model.TinhTrang.HasValue)
                        {
                            model.TinhTrangStr = Common.GetTinhTrangPhong(model.TinhTrang.Value);
                        }
                    }
                    else
                    {
                        model.TenNha = "";
                    }
                }
            }
            return View(model);
        }


        public IActionResult ProductSearch(string textSearch)
        {
            //var data = new List<ProductModelView>();
            if (!string.IsNullOrEmpty(textSearch))
            {
                var ProductModelViewSearch = new PhongViewModelSearch();
                ProductModelViewSearch.name = textSearch.Replace("timkiem=", "");
                ProductModelViewSearch.PageIndex = 1;
                ProductModelViewSearch.PageSize = 20;
                //ProductModelViewSearch.status = 1;
                if (!string.IsNullOrEmpty(ProductModelViewSearch.name))
                {
                    ProductModelViewSearch.status = Common.GetTinhTrangPhongForText(ProductModelViewSearch.name);
                    ProductModelViewSearch.loaiphong = Common.GetLoaiPhongForText(ProductModelViewSearch.name);
                    if (ProductModelViewSearch.status > 0 || ProductModelViewSearch.loaiphong>0)
                    {
                        ProductModelViewSearch.name = "";
                    }
                }
                var data = _phongService.GetAllPaging(ProductModelViewSearch);
                if (data != null && data.Results != null && data.Results.Count > 0)
                {
                    //data.Results = data.Results.Where(p => p.ishidden != 1).ToList();
                    foreach (var item in data.Results)
                    {
                        if (item.MaNha.HasValue && item.MaNha.Value > 0)
                        {
                            var category = _nhaService.GetByid(item.MaNha.Value);
                            if (category != null && !string.IsNullOrEmpty(category.TenNha))
                            {
                                item.TenNha = category.TenNha;
                            }
                            else
                            {
                                item.TenNha = "Chờ xử lý";
                            }
                        }
                        else
                        {
                            item.TenNha = "";
                        }
                        if (item.LoaiPhong.HasValue)
                        {
                            item.LoaiPhongStr = Common.GetLoaiPhong(item.LoaiPhong.Value);
                        }
                        if (item.TinhTrang.HasValue)
                        {
                            item.TinhTrangStr = Common.GetTinhTrangPhong(item.TinhTrang.Value);
                        }
                    }
                }
                return View(data.Results.ToList());
            }
            return View();
        }


        [HttpGet]
        public JsonResult AddView()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserMapModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserMapModelView>(userString);
                if (user != null)
                    return Json(new
                    {
                        success = true,
                        data = user
                    });
            }
            return Json(new
            {
                success = false,
            });
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}