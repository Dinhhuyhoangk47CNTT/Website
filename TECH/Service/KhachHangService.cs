
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TECH.Service
{
    public interface IKhachHangService
    {
        PagedResult<KhachHangModelView> GetAllPaging(KhachHangViewModelSearch KhachHangModelViewSearch);
        KhachHangModelView GetByid(int id);
        void Add(KhachHangModelView view);
        bool Update(KhachHangModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
        List<KhachHangModelView> GetAll();
        bool IsMailExist(string mail);
        bool IsPhoneExist(string phone);
        bool UpdateDetailView(UserMapModelView view);
        KhachHangModelView GetByUser(string emailOrPhone);
        KhachHangModelView AppUserLogin(string userName, string passWord);
        bool ChangePassWord(int userId, string current_password, string new_password, bool isCofirm = false);
    }

    public class KhachHangService : IKhachHangService
    {
        private readonly IKhachHangRepository _khachHangRepository;
        private readonly IHopDongRepository _hopDongRepository;
        private IUnitOfWork _unitOfWork;
        public KhachHangService(IKhachHangRepository khachHangRepository,
            IHopDongRepository hopDongRepository,
            IUnitOfWork unitOfWork)
        {
            _khachHangRepository = khachHangRepository;
            _hopDongRepository = hopDongRepository;
            _unitOfWork = unitOfWork;
        }
        public bool ChangePassWord(int userId, string current_password, string new_password, bool isCofirm = false)
        {
            var modelUser = _khachHangRepository.FindAll().Where(u => u.Id == userId).FirstOrDefault();
            bool status = false;
            if (modelUser != null)
            {
                if (modelUser.MatKhau == current_password)
                {
                    modelUser.MatKhau = new_password;
                    //if (isCofirm == true)
                    //{
                    //    modelUser.code = "";
                    //}
                    _khachHangRepository.Update(modelUser);
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }
            return status;
        }
        public KhachHangModelView GetByUser(string emailOrPhone)
        {
            var data = _khachHangRepository.FindAll(p => p.Email == emailOrPhone || p.SoDienThoai == emailOrPhone).FirstOrDefault();
            
            if (data != null)
            {
                var model = new KhachHangModelView()
                {
                    Id = data.Id,
                    TenKH = data.TenKH,
                    SoDienThoai = data.SoDienThoai,
                    Email = data.Email,
                    CMND = data.CMND,
                    GioiTinh = data.GioiTinh,
                    DiaChi = data.DiaChi,
                    MatKhau = data.MatKhau,
                    MaSV = data.MaSV,
                    Nganh = data.Nganh,
                    Khoa = data.Khoa,
                    SDTGiaDinh = data.SDTGiaDinh,
                    TrangThai = data.TrangThai,
                    ChucVu = data.ChucVu,
                    TenPhong = data.TenPhong
                };

                // Lấy ra hợp đồng mới nhất của khách hàng hiện tại
                var hopDong = _hopDongRepository.FindAll(x => x.MaKH == data.Id 
                    && x.IsDeteled == false, c => c.Phong)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (hopDong != null)
                {
                    if (hopDong.TrangThai == 1)
                        model.TrangThai = "THUE";

                    if (hopDong.TrangThai == 2)
                        model.TrangThai = "TRA";

                    model.ChucVu = hopDong.ChucVuPhong;

                    model.TenPhong = hopDong.Phong.TenPhong;
                }


                return model;
            }

            
            return null;
        }
        public bool IsMailExist(string mail)
        {
            var data = _khachHangRepository.FindAll().Any(p => p.Email == mail);
            return data;
        }
        public bool IsPhoneExist(string phone)
        {
            var data = _khachHangRepository.FindAll().Any(p => p.SoDienThoai == phone);
            return data;
        }
        public List<KhachHangModelView> GetAll()
        {
            var data = _khachHangRepository.FindAll().Select(n => new KhachHangModelView()
            {
                Id = n.Id,
                TenKH = n.TenKH,
                SoDienThoai = n.SoDienThoai,
                DiaChi = n.DiaChi,
                Email = n.Email,
                GioiTinh = n.GioiTinh,
                GioiTinhStr = !string.IsNullOrEmpty(n.GioiTinh) ? (n.GioiTinh == "nam" ? "Nam" : "Nữ") : "",
                NgaySinh = n.NgaySinh,               
                NgaySinhStr = n.NgaySinh.HasValue ? n.NgaySinh.Value.ToString("yyyy-MM-dd") : "",
                MaSV = n.MaSV,
                Nganh = n.Nganh,
                Khoa = n.Khoa,
                SDTGiaDinh = n.SDTGiaDinh,
                TrangThai = n.TrangThai,
                ChucVu = n.ChucVu,
                TenPhong = n.TenPhong
            }).ToList();

            if (data != null)
            {
                foreach (var item in data)
                {
                    // Lấy ra hợp đồng mới nhất của khách hàng hiện tại
                    var hopDong = _hopDongRepository.FindAll(x => x.MaKH == item.Id 
                    && x.IsDeteled == false, c => c.Phong)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();

                    if (hopDong != null)
                    {
                        if (hopDong.TrangThai == 1)
                            item.TrangThai = "THUE";

                        if (hopDong.TrangThai == 2)
                            item.TrangThai = "TRA";

                        item.ChucVu = hopDong.ChucVuPhong;

                        item.TenPhong = hopDong.Phong.TenPhong;
                    }

                }

                return data;
            }
            return null;
        }

        public KhachHangModelView GetByid(int id)
        {
            var data = _khachHangRepository
                .FindAll(p => p.Id == id)
                .FirstOrDefault();
            
            if (data != null)
            {
                var model = new KhachHangModelView()
                {
                    Id = data.Id,
                    TenKH = data.TenKH,
                    SoDienThoai = data.SoDienThoai,
                    Email = data.Email,
                    CMND = data.CMND,
                    GioiTinh = data.GioiTinh,
                    DiaChi = data.DiaChi,
                    //TenDangNhap = data.TenDangNhap,
                    //MatKhau = data.MatKhau,
                    NgaySinh = data.NgaySinh,
                    GioiTinhStr = !string.IsNullOrEmpty(data.GioiTinh) ? (data.GioiTinh == "nam" ? "Nam" : "Nữ") : "",
                    NgaySinhStr = data.NgaySinh.HasValue ? data.NgaySinh.Value.ToString("yyyy-MM-dd") : "",
                    //Role = data.Role,
                    //RoleStr = data.Role.HasValue && data.Role.Value == 0 ? "Quản trị viên" : "Nhân viên"
                    MaSV = data.MaSV,
                    Nganh = data.Nganh,
                    Khoa = data.Khoa,
                    SDTGiaDinh = data.SDTGiaDinh,
                    TrangThai = data.TrangThai,
                    ChucVu = data.ChucVu,
                    TenPhong = data.TenPhong
                };

                // Lấy ra hợp đồng mới nhất của khách hàng hiện tại
                var hopDong = _hopDongRepository.FindAll(x => x.MaKH == data.Id 
                && x.IsDeteled == false, c => c.Phong)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (hopDong != null)
                {
                    if (hopDong.TrangThai == 1)
                        model.TrangThai = "THUE";

                    if (hopDong.TrangThai == 2)
                        model.TrangThai = "TRA";

                    model.ChucVu = hopDong.ChucVuPhong;

                    model.TenPhong = hopDong.Phong.TenPhong;
                }

                return model;
            }
            return null;
        }
        public int GetCount()
        {
            int count = 0;
            count = _khachHangRepository.FindAll().Count();
            return count;
        }
        public KhachHangModelView AppUserLogin(string userName, string passWord)
        {
            var data = _khachHangRepository.FindAll()
                .Where(u => u.Email == userName || u.SoDienThoai == userName)
                .FirstOrDefault();
            if (data != null && data.MatKhau == passWord)
            {
                var model = GetByid(data.Id);
                if (model != null)
                {
                    return model;
                }
            }
            return null;
        }
        public void Add(KhachHangModelView view)
        {
            try
            {
                if (view != null)
                {
                    var nhanvien = new KhachHang
                    {
                        TenKH = view.TenKH,
                        SoDienThoai = view.SoDienThoai,
                        Email = view.Email,
                        CMND = view.CMND,
                        GioiTinh = view.GioiTinh,
                        DiaChi = view.DiaChi,
                        //TenDangNhap = view.TenDangNhap,
                        MatKhau = view.MatKhau,
                        NgaySinh = view.NgaySinh,
                        //Role = view.Role
                        MaSV = view.MaSV,
                        Nganh = view.Nganh,
                        Khoa = view.Khoa,
                        SDTGiaDinh = view.SDTGiaDinh,
                        TrangThai = view.TrangThai,
                        ChucVu = view.ChucVu,
                        TenPhong = view.TenPhong
                    };
                    _khachHangRepository.Add(nhanvien);
                }
            }
            catch (Exception ex)
            {
            }

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(KhachHangModelView view)
        {
            try
            {
                var dataServer = _khachHangRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.TenKH = view.TenKH;
                    dataServer.SoDienThoai = view.SoDienThoai;
                    dataServer.Email = view.Email;
                    dataServer.CMND = view.CMND;
                    dataServer.GioiTinh = view.GioiTinh;
                    dataServer.DiaChi = view.DiaChi;
                    //dataServer.TenDangNhap = view.TenDangNhap;
                    //dataServer.MatKhau = view.MatKhau;
                    //dataServer.Role = view.Role;
                    dataServer.NgaySinh = view.NgaySinh;
                    dataServer.MaSV = view.MaSV;
                    dataServer.Nganh = view.Nganh;
                    dataServer.Khoa = view.Khoa;
                    dataServer.SDTGiaDinh = view.SDTGiaDinh;
                    dataServer.TrangThai = view.TrangThai;
                    dataServer.ChucVu = view.ChucVu;
                    dataServer.TenPhong = view.TenPhong;

                    _khachHangRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        //public bool UpdateStatus(int id, int status)
        //{
        //    try
        //    {
        //        var dataServer = _khachHangRepository.FindById(id);
        //        if (dataServer != null)
        //        {
        //            dataServer.status = status;
        //            _khachHangRepository.Update(dataServer);
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //    return false;
        //}

        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _khachHangRepository.FindById(id);
                if (dataServer != null)
                {
                    _khachHangRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<KhachHangModelView> GetAllPaging(KhachHangViewModelSearch KhachHangModelViewSearch)
        {
            try
            {
                var query = _khachHangRepository.FindAll();

                
                if (!string.IsNullOrEmpty(KhachHangModelViewSearch.name))
                {
                    if (KhachHangModelViewSearch.name.ToLower().Trim() == "nữ")
                        KhachHangModelViewSearch.name = "nu";

                    query = query.Where(c => c.TenKH.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    || c.CMND.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    || c.Email.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    || c.SoDienThoai.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    || c.DiaChi.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    || c.MaSV.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    || c.Nganh.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    || c.Khoa.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    || c.SDTGiaDinh.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    || c.ChucVu.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    || c.TrangThai.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    || c.GioiTinh.ToLower().Trim().Contains(KhachHangModelViewSearch.name.ToLower().Trim())
                    );
                }

                int totalRow = query.Count();
                query = query.Skip((KhachHangModelViewSearch.PageIndex - 1) * KhachHangModelViewSearch.PageSize).Take(KhachHangModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.Id).Select(c => new KhachHangModelView()
                {
                    Id = c.Id,
                    TenKH = !string.IsNullOrEmpty(c.TenKH) ? c.TenKH : "",
                    SoDienThoai = !string.IsNullOrEmpty(c.SoDienThoai) ? c.SoDienThoai : "",
                    Email = !string.IsNullOrEmpty(c.Email) ? c.Email : "",
                    CMND = !string.IsNullOrEmpty(c.CMND) ? c.CMND : "",
                    GioiTinh = c.GioiTinh,
                    DiaChi = !string.IsNullOrEmpty(c.DiaChi) ? c.DiaChi : "",
                    //TenDangNhap = !string.IsNullOrEmpty(c.TenDangNhap)? c.TenDangNhap:"",
                    //MatKhau = c.MatKhau,
                    NgaySinh = c.NgaySinh,
                    NgaySinhStr = c.NgaySinh.HasValue ? c.NgaySinh.Value.ToString("dd/MM/yyyy"):"",
                    GioiTinhStr = !string.IsNullOrEmpty(c.GioiTinh)?(c.GioiTinh == "nam"?"Nam":"Nữ"):"",
                    //Role = c.Role,
                    //RoleStr = c.Role.HasValue && c.Role.Value == 0 ? "Quản trị viên" : "Nhân viên"
                    MaSV = c.MaSV,
                    Nganh = c.Nganh,
                    Khoa = c.Khoa,
                    SDTGiaDinh = c.SDTGiaDinh,
                    TrangThai = c.TrangThai,
                    ChucVu = c.ChucVu,
                    TenPhong = c.TenPhong
                }).ToList();

                foreach (var item in data)
                {
                    // Lấy ra hợp đồng mới nhất của khách hàng hiện tại
                    var hopDong = _hopDongRepository.FindAll(x => x.MaKH == item.Id 
                    && x.IsDeteled == false, c => c.Phong)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();

                    if (hopDong != null)
                    {
                        if (hopDong.TrangThai == 1)
                            item.TrangThai = "THUE";
                        if (hopDong.TrangThai == 2)
                            item.TrangThai = "TRA";
                        item.ChucVu = hopDong.ChucVuPhong;
                        item.TenPhong = hopDong.Phong.TenPhong;
                    }
                }

                var pagingData = new PagedResult<KhachHangModelView>
                {
                    Results = data,
                    CurrentPage = KhachHangModelViewSearch.PageIndex,
                    PageSize = KhachHangModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool UpdateDetailView(UserMapModelView view)
        {
            try
            {
                var dataServer = _khachHangRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.DiaChi = view.Address;
                    dataServer.TenKH = view.Ten;
                    dataServer.Email = view.Email;
                    dataServer.SoDienThoai = view.SDT;
                    _khachHangRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
    }
}
