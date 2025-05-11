
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.General;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IPhongService
    {
        PagedResult<PhongModelView> GetAllPaging(PhongViewModelSearch PhongModelViewSearch);
        PhongModelView GetByid(int id);
        List<PhongModelView> GetPhongByNha(int id);
        List<PhongModelView> GetPhongByNhaStatus(int id);
        void AddPhongFast(PhongModelView view);
        void Add(PhongModelView view);
        bool Update(PhongModelView view);
        bool UpdateTrangThai(int maPhong, int trangthai);
        bool Deleted(int id);
        void Save();
        int GetCount();
        List<PhongModelView> GetAll();
    }

    public class PhongService : IPhongService
    {
        private readonly IPhongRepository _phongRepository;
        private readonly IHopDongRepository _hopdongRepository;
        private readonly IThanhVienPhongRepository _thanhVienPhongRepository;
        private readonly List<ThanhVienPhong> _dsThanhVienPhong;
        private IUnitOfWork _unitOfWork;
        public PhongService(IPhongRepository phongRepository,
            IHopDongRepository hopdongRepository,
            IThanhVienPhongRepository thanhVienPhongRepository,
            IUnitOfWork unitOfWork)
        {
            _phongRepository = phongRepository;
            _hopdongRepository = hopdongRepository;
            _thanhVienPhongRepository = thanhVienPhongRepository;
            _dsThanhVienPhong = _thanhVienPhongRepository.FindAll().ToList();
            _unitOfWork = unitOfWork;
        }
        public bool UpdateTrangThai(int maPhong,int trangthai)
        {
            var dataServer = _phongRepository.FindById(maPhong);
            if (dataServer != null)
            {
                dataServer.TinhTrang = trangthai; 
                _phongRepository.Update(dataServer);
                Save();
                return true;
            }
            return false;
        }
        public List<PhongModelView> GetAll()
        {
            var data = _phongRepository.FindAll().Select(n => new PhongModelView()
            {
                Id = n.Id,
                TenPhong = n.TenPhong
            }).ToList();
            if (data != null)
            {
                return data;
            }
            return null;
        }
        public PhongModelView GetByid(int id)
        {
            
            var data = _phongRepository.FindAll(p => p.Id == id).FirstOrDefault();

            if (data != null)
            {
                var model = new PhongModelView()
                {
                    Id = data.Id,
                    MaNha = data.MaNha,
                    TenPhong = data.TenPhong,
                    DonGia = data.DonGia,
                    SLNguoiMax = data.SLNguoiMax,
                    ChieuDai = data.ChieuDai,
                    ChieuRong = data.ChieuRong,
                    MoTa = !string.IsNullOrEmpty(data.MoTa) ? Regex.Replace(data.MoTa, @"\r\n?|\n", "</br>") : "",
                    LoaiPhong = data.LoaiPhong,
                    HinhAnh = data.HinhAnh,
                    TinhTrang = data.TinhTrang,
                    DonGiaStr = data.DonGia.HasValue && data.DonGia.Value > 0 ? data.DonGia.Value.ToString("#,###") : "",
                    SucChua = data.SucChua.HasValue ? data.SucChua.Value : 0
                };
                
                model.SoSVHienTai = model.TinhTrang == 2 ? _dsThanhVienPhong
                        .FindAll(p => p.MaPhong == model.Id).Count() : 0;
                
                return model;
            }
            return null;
        }
        public List<PhongModelView> GetPhongByNha(int id)
        {
            if (id > 0)
            {
                var data = _phongRepository.FindAll(p => p.MaNha == id).Select(c => new PhongModelView()
                {
                    Id = c.Id,
                    MaNha = c.MaNha,
                    TenPhong = c.TenPhong,
                    DonGia = c.DonGia,
                    SLNguoiMax = c.SLNguoiMax,
                    ChieuDai = c.ChieuDai,
                    ChieuRong = c.ChieuRong,
                    MoTa = !string.IsNullOrEmpty(c.MoTa) ? Regex.Replace(c.MoTa, @"\r\n?|\n", "</br>") : "",
                    LoaiPhong = c.LoaiPhong,
                    HinhAnh = c.HinhAnh,
                    TinhTrang = c.TinhTrang,
                    DonGiaStr = c.DonGia.HasValue && c.DonGia.Value > 0 ? c.DonGia.Value.ToString("#,###"):"",
                    TinhTrangStr = Common.GetTinhTrangPhong(c.TinhTrang.Value),
                    SucChua = c.SucChua.HasValue ? c.SucChua.Value : 0
                }).ToList();

                foreach (var item in data)
                {
                    item.SoSVHienTai = item.TinhTrang == 2 ? _dsThanhVienPhong
                        .FindAll(p => p.MaPhong == item.Id).Count() : 0;
                }

                return data;
            }
            return null;
        }
        public List<PhongModelView> GetPhongByNhaStatus(int maNha)
        {
            if (maNha > 0)
            {
                var data = _phongRepository.FindAll(p => p.MaNha == maNha).Select(c => new PhongModelView()
                {
                    Id = c.Id,
                    MaNha = c.MaNha,
                    TenPhong = c.TenPhong,
                    DonGia = c.DonGia,
                    SLNguoiMax = c.SLNguoiMax,
                    ChieuDai = c.ChieuDai,
                    ChieuRong = c.ChieuRong,
                    HinhAnh = c.HinhAnh,
                    MoTa = !string.IsNullOrEmpty(c.MoTa) ? Regex.Replace(c.MoTa, @"\r\n?|\n", "</br>") : "",
                    LoaiPhong = c.LoaiPhong,
                    TinhTrang = c.TinhTrang,
                    DonGiaStr = c.DonGia.HasValue && c.DonGia.Value > 0 ? c.DonGia.Value.ToString("#,###") : "",
                    TinhTrangStr = Common.GetTinhTrangPhong(c.TinhTrang.Value),
                    SucChua = c.SucChua.HasValue ? c.SucChua.Value : 0
                }).ToList();

                foreach (var item in data)
                {
                    item.SoSVHienTai = item.TinhTrang == 2 ? _dsThanhVienPhong
                        .FindAll(p => p.MaPhong == item.Id).Count() : 0;
                }

                return data;
            }
            return null;
        }
        
        public int GetCount()
        {
            int count = 0;
            count = _phongRepository.FindAll().Count();
            return count;
        }
        public void Add(PhongModelView view)
        {
            try
            {
                if (view != null)
                {
                    var phong = new Phong
                    {
                        MaNha = view.MaNha,
                        TenPhong = view.TenPhong,
                        DonGia = view.DonGia,
                        SLNguoiMax = view.SLNguoiMax,
                        ChieuDai = view.ChieuDai,
                        ChieuRong = view.ChieuRong,
                        MoTa = view.MoTa,
                        LoaiPhong = view.LoaiPhong,
                        TinhTrang = view.TinhTrang,
                        HinhAnh = view.HinhAnh,
                        SucChua = view.SucChua
                    };
                    _phongRepository.Add(phong);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void AddPhongFast(PhongModelView view)
        {
            if (view != null && view.PhongTu > 0 && view.DenPhong > 0 && view.PhongTu < view.DenPhong)
            {
                for (int i = view.PhongTu.Value; i <= view.DenPhong.Value; i++)
                {
                    view.TenPhong = i.ToString();
                    Add(view);
                }
                //Save();
            }
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(PhongModelView view)
        {
            try
            {
                var dataServer = _phongRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    //dataServer.MaNha = view.MaNha;
                    dataServer.TenPhong = view.TenPhong;
                    dataServer.DonGia = view.DonGia;
                    dataServer.SLNguoiMax = view.SLNguoiMax;
                    dataServer.ChieuDai = view.ChieuDai;
                    dataServer.ChieuRong = view.ChieuRong;
                    dataServer.MoTa = view.MoTa;
                    dataServer.LoaiPhong = view.LoaiPhong;
                    dataServer.TinhTrang = view.TinhTrang;
                    dataServer.HinhAnh = view.HinhAnh;
                    dataServer.SucChua = view.SucChua;
                    _phongRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }      
        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _phongRepository.FindById(id);
                if (dataServer != null)
                {
                    _phongRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<PhongModelView> GetAllPaging(PhongViewModelSearch PhongModelViewSearch)
        {
            try
            {
                var query = _phongRepository.FindAll();

                if (!string.IsNullOrEmpty(PhongModelViewSearch.name))
                {
                    query = query.Where(c => c.TenPhong.ToLower().Trim().Contains(PhongModelViewSearch.name.ToLower().Trim()));
                }
                if (PhongModelViewSearch.categoryId.HasValue && PhongModelViewSearch.categoryId.Value > 0)
                {
                    query = query.Where(c => c.MaNha == PhongModelViewSearch.categoryId);
                }
                if (PhongModelViewSearch.status > 0)
                {
                    query = query.Where(c => c.TinhTrang == PhongModelViewSearch.status);
                }
                if (PhongModelViewSearch.loaiphong > 0)
                {
                    query = query.Where(c => c.LoaiPhong == PhongModelViewSearch.loaiphong);
                }

                int totalRow = query.Count();
                query = query.Skip((PhongModelViewSearch.PageIndex - 1) * PhongModelViewSearch.PageSize).Take(PhongModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.MaNha).ThenByDescending(p=>p.Id).Select(c => new PhongModelView()
                {
                    Id = c.Id,
                    MaNha = c.MaNha,
                    TenPhong = c.TenPhong,
                    DonGia = c.DonGia,                    
                    SLNguoiMax = c.SLNguoiMax,
                    ChieuDai = c.ChieuDai,
                    ChieuRong = c.ChieuRong,
                    MoTa = !string.IsNullOrEmpty(c.MoTa)? Regex.Replace(c.MoTa, @"\r\n?|\n", "</br>") : "",
                    LoaiPhong = c.LoaiPhong,
                    TinhTrang = c.TinhTrang,
                    HinhAnh = c.HinhAnh,
                    DonGiaStr = c.DonGia.HasValue && c.DonGia.Value > 0? c.DonGia.Value.ToString("#,###"):"",
                    SucChua = c.SucChua.HasValue ? c.SucChua.Value : 0
                }).ToList();

                foreach (var item in data)
                {
                    item.SoSVHienTai = item.TinhTrang == 2 ? _dsThanhVienPhong
                        .FindAll(p => p.MaPhong == item.Id).Count() : 0;
                }  

                var pagingData = new PagedResult<PhongModelView>
                {
                    Results = data,
                    CurrentPage = PhongModelViewSearch.PageIndex,
                    PageSize = PhongModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
