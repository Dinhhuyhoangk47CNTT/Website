using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using System.Text.Json;
using TECH.Areas.Admin.Controllers;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.General;
using TECH.Reponsitory;
using TECH.Service;
using TECH.Utilities;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Reponsitory;

namespace Website.Areas.Admin.Controllers
{
    public class ThongKeController : BaseController
    {
        private readonly DataBaseEntityContext _context;
        private readonly IHopDongService _hopDongService;
        private readonly INhaService _nhaService;
        private readonly IPhongService _phongService;
        private readonly IDichVuPhongService _dichVuPhongService;
        private readonly IKhachHangService _khachHangService;
        private readonly INhanVienService _nhanVienService;
        private readonly IThanhVienPhongService _thanhVienPhongService;
        private readonly ITienPhongRepository _tienPhongRepository;
        private readonly IHopDongRepository _hopDongRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ThongKeController(DataBaseEntityContext context,
            IHopDongService hopDongService,
             INhaService nhaService,
            IPhongService phongService,
            IKhachHangService khachHangService,
            INhanVienService nhanVienService,
            IDichVuPhongService dichVuPhongService,
            IThanhVienPhongService thanhVienPhongService,
            ITienPhongRepository tienPhongRepository,
            IHopDongRepository hopDongRepository
            )
        {
            _hopDongService = hopDongService;
            _nhaService = nhaService;
            _phongService = phongService;
            _khachHangService = khachHangService;
            _nhanVienService = nhanVienService;
            _dichVuPhongService = dichVuPhongService;
            _thanhVienPhongService = thanhVienPhongService;
            _tienPhongRepository = tienPhongRepository;
            _hopDongRepository = hopDongRepository;
            _context = context;
        }

        [HttpGet]
        public IActionResult ThongKeTienDien()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ThongKeTienDienPaging(ThongKeTienDienViewModelSearch search)
        {
            var data = _context.TienDiens
                .GroupBy(x => x.Thang)
                .Select(x => new ThongKeTienDienViewModel()
                {
                    Thang = x.Key.Value,
                    TongLuongDienTieuThu = x.Sum(c => c.SoCongToDienHienTai - c.SoCongToDienTruoc) ?? 0,
                    TongTienCanNop = x.Sum(c => c.SoTienCanPhaiNop) ?? 0,
                    TongTienDaNop = x.Sum(c => c.SoTienDaNop) ?? 0
                }).ToList();

            if (search.thang.HasValue && search.thang.Value >= 1 && search.thang.Value <= 12)
            {
                data = data.Where(x => x.Thang == search.thang)
                    .ToList();
            }

            foreach (var item in data)
            {
                item.TongTienCanNopStr = item.TongTienCanNop.Value.ToString("#,###");
                item.TongTienDaNopStr = item.TongTienDaNop.Value.ToString("#,###");
            } 

            var pagingData = new PagedResult<ThongKeTienDienViewModel>
            {
                Results = data,
                CurrentPage = search.PageIndex,
                PageSize = search.PageSize,
                RowCount = data.Count,
            };

            return Json(new { data = pagingData });
        }

        [HttpGet]
        public IActionResult ThongKeTienDienExcel()
        {
            var data = _context.TienDiens
                .GroupBy(x => x.Thang)
                .Select(x => new ThongKeTienDienViewModel()
                {
                    Thang = x.Key.Value,
                    TongLuongDienTieuThu = x.Sum(c => c.SoCongToDienHienTai - c.SoCongToDienTruoc) ?? 0,
                    TongTienCanNop = x.Sum(c => c.SoTienCanPhaiNop) ?? 0,
                    TongTienDaNop = x.Sum(c => c.SoTienDaNop) ?? 0
                }).ToList();

            using (var package = new ExcelPackage())
            {
                // Create a worksheet.
                var worksheet = package.Workbook.Worksheets.Add("TienDien");
                // Set up header row.
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Tháng";
                worksheet.Cells[1, 3].Value = "Tổng điện tiêu thụ";
                worksheet.Cells[1, 4].Value = "Tổng tiền cần nộp";
                worksheet.Cells[1, 5].Value = "Tổng tiền đã nộp";

                int row = 2;
                foreach (var e in data)
                {
                    worksheet.Cells[row, 1].Value = (row - 1).ToString();
                    worksheet.Cells[row, 2].Value = string.Format("Tháng {0}", e.Thang);
                    worksheet.Cells[row, 3].Value = e.TongLuongDienTieuThu.Value.ToString("#,###");
                    worksheet.Cells[row, 4].Value = e.TongTienCanNop.Value.ToString("#,###");
                    worksheet.Cells[row, 5].Value = e.TongTienDaNop.Value.ToString("#,###");
                    row++;
                }
                // Auto-fit columns for better readability
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // Convert the package to a byte array
                var fileBytes = package.GetAsByteArray();
                // Return the Excel file for download
                // 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' is the MIME type for Excel files
                return File(
                    fileBytes, //Excel File data in Byte Array
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", //Excel Sheet Mime Type
                    "ThongKeTienDien.xlsx" //File Name
                );
            }
        }

        public IActionResult ThongKeTienNuoc()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ThongKeTienNuocPaging(ThongKeTienNuocViewModelSearch search)
        {
            var data = _context.TienNuocs
                .GroupBy(x => x.Thang)
                .Select(x => new ThongKeTienNuocViewModel()
                {
                    Thang = x.Key.Value,
                    TongLuongNuocTieuThu = x.Sum(c => c.SoCongToNuocHienTai - c.SoCongToNuocTruoc) ?? 0,
                    TongTienCanNop = x.Sum(c => c.SoTienCanPhaiNop) ?? 0,
                    TongTienDaNop = x.Sum(c => c.SoTienDaNop) ?? 0
                }).ToList();

            if (search.thang.HasValue && search.thang.Value >= 1 && search.thang.Value <= 12)
            {
                data = data.Where(x => x.Thang == search.thang)
                    .ToList();
            }

            foreach (var item in data)
            {
                item.TongTienCanNopStr = item.TongTienCanNop.Value.ToString("#,###");
                item.TongTienDaNopStr = item.TongTienDaNop.Value.ToString("#,###");
            }

            var pagingData = new PagedResult<ThongKeTienNuocViewModel>
            {
                Results = data,
                CurrentPage = search.PageIndex,
                PageSize = search.PageSize,
                RowCount = data.Count,
            };

            return Json(new { data = pagingData });
        }

        [HttpGet]
        public IActionResult ThongKeTienNuocExcel()
        {
            var data = _context.TienNuocs
               .GroupBy(x => x.Thang)
               .Select(x => new ThongKeTienNuocViewModel()
               {
                   Thang = x.Key.Value,
                   TongLuongNuocTieuThu = x.Sum(c => c.SoCongToNuocHienTai - c.SoCongToNuocTruoc) ?? 0,
                   TongTienCanNop = x.Sum(c => c.SoTienCanPhaiNop) ?? 0,
                   TongTienDaNop = x.Sum(c => c.SoTienDaNop) ?? 0
               }).ToList();

            using (var package = new ExcelPackage())
            {
                // Create a worksheet.
                var worksheet = package.Workbook.Worksheets.Add("TienNuoc");
                // Set up header row.
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Tháng";
                worksheet.Cells[1, 3].Value = "Tổng nước tiêu thụ";
                worksheet.Cells[1, 4].Value = "Tổng tiền cần nộp";
                worksheet.Cells[1, 5].Value = "Tổng tiền đã nộp";

                int row = 2;
                foreach (var e in data)
                {
                    worksheet.Cells[row, 1].Value = (row - 1).ToString();
                    worksheet.Cells[row, 2].Value = string.Format("Tháng {0}", e.Thang);
                    worksheet.Cells[row, 3].Value = e.TongLuongNuocTieuThu.Value.ToString("#,###");
                    worksheet.Cells[row, 4].Value = e.TongTienCanNop.Value.ToString("#,###");
                    worksheet.Cells[row, 5].Value = e.TongTienDaNop.Value.ToString("#,###");
                    row++;
                }
                // Auto-fit columns for better readability
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // Convert the package to a byte array
                var fileBytes = package.GetAsByteArray();
                // Return the Excel file for download
                // 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' is the MIME type for Excel files
                return File(
                    fileBytes, //Excel File data in Byte Array
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", //Excel Sheet Mime Type
                    "ThongKeTienNuoc.xlsx" //File Name
                );
            }
        }

        [HttpGet]
        public IActionResult ThongKeSinhVienDaTraPhong()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ThongKeSinhVienDaTraPhongPaging(HopDongViewModelSearch phongViewModelSearch)
        {
            phongViewModelSearch.status = 2; // đã trả phòng

            var data = _hopDongService.GetAllPaging(phongViewModelSearch);
            
            foreach (var item in data.Results)
            {
                if (item.MaNha.HasValue && item.MaNha.Value > 0)
                {
                    item.TenNha = _nhaService.GetByid(item.MaNha.Value)?.TenNha;
                }
                if (item.MaPhong.HasValue && item.MaPhong.Value > 0)
                {
                    item.TenPhong = _phongService.GetByid(item.MaPhong.Value)?.TenPhong;
                }
                if (item.MaKH.HasValue && item.MaKH.Value > 0)
                {
                    item.TenKhachHang = _khachHangService.GetByid(item.MaKH.Value)?.TenKH;
                }
                if (item.MaNV.HasValue && item.MaNV.Value > 0)
                {
                    item.TenNhanVien = _nhanVienService.GetByid(item.MaNV.Value)?.TenNV;
                }
                if (item.TrangThai.HasValue && item.TrangThai.Value > 0)
                {
                    item.TrangThaiStr = Common.GetTinhTrangHoaDon(item.TrangThai.Value);
                }
            }
            if (phongViewModelSearch != null && !string.IsNullOrEmpty(phongViewModelSearch.name))
            {
                data.Results = data.Results.Where(p => !string.IsNullOrEmpty(p.TenNha) && p.TenNha.ToLower().Contains(phongViewModelSearch.name.ToLower()) ||
               (!string.IsNullOrEmpty(p.TenPhong) && p.TenPhong.ToLower().Contains(phongViewModelSearch.name.ToLower())) ||
               (!string.IsNullOrEmpty(p.TenKhachHang) && p.TenKhachHang.ToLower().Contains(phongViewModelSearch.name.ToLower())) ||
               (!string.IsNullOrEmpty(p.TenNhanVien) && p.TenNhanVien.ToLower().Contains(phongViewModelSearch.name.ToLower()))).ToList();
            }
            if (phongViewModelSearch.status > 0)
            {
                data.Results = data.Results.Where(p => p.TrangThai == phongViewModelSearch.status).ToList();
            }
            return Json(new { data = data });
        }

        [HttpGet]
        public IActionResult ThongKeSinhVienDaTraPhongExcel()
        {
            var data = _hopDongRepository.FindAll(x => x.TrangThai == 2)
                .Select(c => new HopDongModelView()
                {
                    Id = c.Id,
                    MaPhong = c.MaPhong,
                    MaNV = c.MaNV,
                    MaKH = c.MaKH,
                    MaNha = c.MaNha,
                    //NgayBatDau=c.NgayBatDau,
                    NgayKetThuc = c.NgayKetThuc,
                    //TienCoc=c.TienCoc,
                    TrangThai = c.TrangThai,
                    IsDeteled = c.IsDeteled,
                    ChucVuPhong = c.ChucVuPhong,
                    //NgayBatDauStr = c.NgayBatDau.HasValue ? c.NgayBatDau.Value.ToString("dd/MM/yyyy") : "",
                    NgayKetThucStr = c.NgayKetThuc.HasValue ? c.NgayKetThuc.Value.ToString("dd/MM/yyyy") : ""
                }).ToList();

            foreach (var item in data)
            {
                if (item.MaNha.HasValue && item.MaNha.Value > 0)
                {
                    item.TenNha = _nhaService.GetByid(item.MaNha.Value)?.TenNha;
                }
                if (item.MaPhong.HasValue && item.MaPhong.Value > 0)
                {
                    item.TenPhong = _phongService.GetByid(item.MaPhong.Value)?.TenPhong;
                }
                if (item.MaKH.HasValue && item.MaKH.Value > 0)
                {
                    item.TenKhachHang = _khachHangService.GetByid(item.MaKH.Value)?.TenKH;
                }
                if (item.MaNV.HasValue && item.MaNV.Value > 0)
                {
                    item.TenNhanVien = _nhanVienService.GetByid(item.MaNV.Value)?.TenNV;
                }
                if (item.TrangThai.HasValue && item.TrangThai.Value > 0)
                {
                    item.TrangThaiStr = Common.GetTinhTrangHoaDon(item.TrangThai.Value);
                }
            }

            using (var package = new ExcelPackage())
            {
                // Create a worksheet.
                var worksheet = package.Workbook.Worksheets.Add("SinhVienDaTraPhong");
                // Set up header row.
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Họ Tên";
                worksheet.Cells[1, 3].Value = "Chức vụ";
                worksheet.Cells[1, 4].Value = "Tên nhà";
                worksheet.Cells[1, 5].Value = "Tên phòng";
                worksheet.Cells[1, 6].Value = "Ngày trả phòng";

                int row = 2;
                foreach (var e in data)
                {
                    worksheet.Cells[row, 1].Value = (row - 1).ToString();
                    worksheet.Cells[row, 2].Value = e.TenKhachHang;
                    worksheet.Cells[row, 3].Value = (e.ChucVuPhong == "TRUONG_PHONG" ? "Trưởng Phòng" : "Thành Viên");
                    worksheet.Cells[row, 4].Value = e.TenNha;
                    worksheet.Cells[row, 5].Value = e.TenPhong;
                    worksheet.Cells[row, 6].Value = e.NgayKetThucStr;
                    row++;
                }
                // Auto-fit columns for better readability
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // Convert the package to a byte array
                var fileBytes = package.GetAsByteArray();
                // Return the Excel file for download
                // 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' is the MIME type for Excel files
                return File(
                    fileBytes, //Excel File data in Byte Array
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", //Excel Sheet Mime Type
                    "ThongKeSinhVienDaTraPhong.xlsx" //File Name
                );
            }
        }

        [HttpGet]
        public IActionResult ThongKeTienPhong()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ThongKeTienPhongPaging(ThongKeTienPhongViewModelSearch search)
        {
            // viết code cho hàm GetAllPaging
            var query = _tienPhongRepository
                .FindAll(c => c.Nha, c => c.Phong);

            if (search.thang.HasValue && search.thang >= 1 && search.thang <= 12)
            {
                query = query.Where(x => x.NgayNop.Value.Month == search.thang);
            }

            var totalRow = query.Count();
            var data = query
                .Skip((search.PageIndex - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(x => new TienPhongModelView
                {
                    Id = x.Id,
                    MaNha = x.MaNha,
                    TenNha = x.Nha.TenNha,
                    MaPhong = x.MaPhong,
                    TenPhong = x.Phong.TenPhong,
                    LoaiPHong = x.LoaiPHong,
                    LoaiPhongStr = x.LoaiPHong.HasValue ? x.LoaiPHong == 2 ? "Nam" : "Nữ" : "Khác",
                    GiaPhong = x.GiaPhong,
                    GiaPhongStr = x.GiaPhong.HasValue ? x.GiaPhong.Value.ToString("#,###") : "",
                    SoTienCanNop = x.SoTienCanNop,
                    SoTienCanNopStr = x.SoTienCanNop.HasValue ? x.SoTienCanNop.Value.ToString("#,###") : "",
                    SoTienDaNop = x.SoTienDaNop,
                    SoTienDaNopStr = x.SoTienDaNop.HasValue ? x.SoTienDaNop.Value.ToString("#,###") : "",
                    HanNop = x.HanNop,
                    HanNopStr = x.HanNop.HasValue ? x.HanNop.Value.ToString("dd/MM/yyyy") : "",
                    NgayNop = x.NgayNop,
                    NgayNopStr = x.NgayNop.HasValue ? x.NgayNop.Value.ToString("dd/MM/yyyy") : "",
                    NamHoc = x.NamHoc,
                    HocKy = x.HocKy,
                    TrangThai = x.TrangThai,
                    TrangThaiStr = x.TrangThai == "DA_NOP" ? "Đã nộp" : "Chưa nộp"
                }).ToList();

            var pagedResult = new PagedResult<TienPhongModelView>
            {
                Results = data,
                CurrentPage = search.PageIndex,
                PageSize = search.PageSize,
                RowCount = totalRow
            };

            return Json(new { data = pagedResult });
        }

        [HttpGet]
        public IActionResult ThongKeTienPhongExcel()
        {
            // viết code cho hàm GetAllPaging
            var query = _tienPhongRepository
                .FindAll(c => c.Nha, c => c.Phong);

            var data = query
                .Select(x => new TienPhongModelView
                {
                    Id = x.Id,
                    MaNha = x.MaNha,
                    TenNha = x.Nha.TenNha,
                    MaPhong = x.MaPhong,
                    TenPhong = x.Phong.TenPhong,
                    LoaiPHong = x.LoaiPHong,
                    LoaiPhongStr = x.LoaiPHong.HasValue ? x.LoaiPHong == 2 ? "Nam" : "Nữ" : "Khác",
                    GiaPhong = x.GiaPhong,
                    GiaPhongStr = x.GiaPhong.HasValue ? x.GiaPhong.Value.ToString("#,###") : "",
                    SoTienCanNop = x.SoTienCanNop,
                    SoTienCanNopStr = x.SoTienCanNop.HasValue ? x.SoTienCanNop.Value.ToString("#,###") : "",
                    SoTienDaNop = x.SoTienDaNop,
                    SoTienDaNopStr = x.SoTienDaNop.HasValue ? x.SoTienDaNop.Value.ToString("#,###") : "",
                    HanNop = x.HanNop,
                    HanNopStr = x.HanNop.HasValue ? x.HanNop.Value.ToString("dd/MM/yyyy") : "",
                    NgayNop = x.NgayNop,
                    NgayNopStr = x.NgayNop.HasValue ? x.NgayNop.Value.ToString("dd/MM/yyyy") : "",
                    NamHoc = x.NamHoc,
                    HocKy = x.HocKy,
                    TrangThai = x.TrangThai,
                    TrangThaiStr = x.TrangThai == "DA_NOP" ? "Đã nộp" : "Chưa nộp"
                }).ToList();

            using (var package = new ExcelPackage())
            {
                // Create a worksheet.
                var worksheet = package.Workbook.Worksheets.Add("ThongKeTienPhong");
                // Set up header row.
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Phòng";
                worksheet.Cells[1, 3].Value = "Nhà";
                worksheet.Cells[1, 4].Value = "Loại phòng";
                worksheet.Cells[1, 5].Value = "Giá phòng";
                worksheet.Cells[1, 6].Value = "Số tiền cần nộp";
                worksheet.Cells[1, 7].Value = "Số tiền đã nộp";
                worksheet.Cells[1, 8].Value = "Hạn nộp";
                worksheet.Cells[1, 9].Value = "Ngày nộp";
                worksheet.Cells[1, 10].Value = "Năm học";
                worksheet.Cells[1, 11].Value = "Học kỳ";
                worksheet.Cells[1, 12].Value = "Trạng thái";

                int row = 2;
                foreach (var e in data)
                {
                    worksheet.Cells[row, 1].Value = (row - 1).ToString();
                    worksheet.Cells[row, 2].Value = e.TenPhong;
                    worksheet.Cells[row, 3].Value = e.TenNha;
                    worksheet.Cells[row, 4].Value = e.LoaiPhongStr;
                    worksheet.Cells[row, 5].Value = e.GiaPhongStr;
                    worksheet.Cells[row, 6].Value = e.SoTienCanNopStr;
                    worksheet.Cells[row, 7].Value = e.SoTienDaNopStr;
                    worksheet.Cells[row, 8].Value = e.HanNopStr;
                    worksheet.Cells[row, 9].Value = e.NgayNopStr;
                    worksheet.Cells[row, 10].Value = e.NamHoc;
                    worksheet.Cells[row, 11].Value = e.HocKy;
                    worksheet.Cells[row, 12].Value = e.TrangThaiStr;
                    row++;
                }
                // Auto-fit columns for better readability
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // Convert the package to a byte array
                var fileBytes = package.GetAsByteArray();
                // Return the Excel file for download
                // 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' is the MIME type for Excel files
                return File(
                    fileBytes, //Excel File data in Byte Array
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", //Excel Sheet Mime Type
                    "ThongKeTienPhong.xlsx" //File Name
                );
            }
        }

        [HttpGet]
        public IActionResult BieuDoTienDien()
        {
            var data = _context
                .TienDiens.Where(x => x.HanNop.Value.Year == DateTime.Now.Year)
                .GroupBy(x => x.NgayNop.Value.Month)
                .Select(x => new BieuDoViewModel()
                {
                    Thang = x.Key,
                    Value = x.Sum(c => c.SoTienDaNop) ?? 0
                })
                .OrderBy(x => x.Thang)
                .ToList();
            
            var series = new List<BieuDoViewModel>();
            
            for (int i = 1; i <= 12; i++)
            {
                var item = data.Where(x => x.Thang == i)
                    .FirstOrDefault();

                if (item != null) series.Add(item);
                else series.Add(new BieuDoViewModel() { Thang = i, Value = 0 });
            } 

            ViewData["BieuDo"] = JsonSerializer.Serialize(series);
            return View();
        }

        [HttpGet]
        public IActionResult BieuDoTienNuoc()
        {
            var data = _context
                .TienNuocs.Where(x => x.HanNop.Value.Year == DateTime.Now.Year)
                .GroupBy(x => x.NgayNop.Value.Month)
                .Select(x => new BieuDoViewModel()
                {
                    Thang = x.Key,
                    Value = x.Sum(c => c.SoTienDaNop) ?? 0
                })
                .OrderBy(x => x.Thang)
                .ToList();

            var series = new List<BieuDoViewModel>();

            for (int i = 1; i <= 12; i++)
            {
                var item = data.Where(x => x.Thang == i)
                    .FirstOrDefault();

                if (item != null)
                    series.Add(item);
                else
                    series.Add(new BieuDoViewModel() { Thang = i, Value = 0 });
            }

            ViewData["BieuDo"] = JsonSerializer.Serialize(series);
            return View();
        }

        [HttpGet]
        public IActionResult BieuDoTienPhong()
        {
            var data = _context
                .TienPhongs.Where(x => x.HanNop.Value.Year == DateTime.Now.Year)
                .GroupBy(x => x.NgayNop.Value.Month)
                .Select(x => new BieuDoViewModel()
                {
                    Thang = x.Key,
                    Value = x.Sum(c => c.SoTienDaNop) ?? 0
                })
                .OrderBy(x => x.Thang)
                .ToList();

            var series = new List<BieuDoViewModel>();

            for (int i = 1; i <= 12; i++)
            {
                var item = data.Where(x => x.Thang == i)
                    .FirstOrDefault();

                if (item != null)
                    series.Add(item);
                else
                    series.Add(new BieuDoViewModel() { Thang = i, Value = 0 });
            }

            ViewData["BieuDo"] = JsonSerializer.Serialize(series);
            return View();
        }

    }
}
