using System.Net.WebSockets;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Reponsitory;
using TECH.Utilities;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Data.DatabaseEntity;
using Website.Reponsitory;

namespace Website.Service
{
    public class ThietBiService : IThietBiService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IThietBiRepository _thietBiRepository;
        public ThietBiService(IUnitOfWork unitOfWork
            , IThietBiRepository thietBiRepository)
        {
            _unitOfWork = unitOfWork;
            _thietBiRepository = thietBiRepository;
        }

        public PagedResult<ThietBiModelView> GetAllPaging(ThietBiViewModelSearch ThietBiModelViewSearch)
        {
            // viết code cho hàm GetAllPaging
            var query = _thietBiRepository.FindAll(c => c.Nha, c => c.Phong);
            if (!string.IsNullOrEmpty(ThietBiModelViewSearch.name))
            {
                query = query.Where(x => x
                    .TenThietBi
                    .ToLower()
                    .Contains(ThietBiModelViewSearch.name.ToLower()));
            }
            var totalRow = query.Count();
            var data = query.OrderBy(x => x.TenThietBi)
                .Skip((ThietBiModelViewSearch.PageIndex - 1) * ThietBiModelViewSearch.PageSize)
                .Take(ThietBiModelViewSearch.PageSize)
                .Select(x => new ThietBiModelView
                {
                    Id = x.Id,
                    MaNha = x.MaNha,
                    TenNha = x.Nha.TenNha,
                    MaPhong = x.MaPhong,
                    TenPhong = x.Phong.TenPhong,
                    LoaiPHong = x.LoaiPHong,
                    LoaiPhongStr = x.LoaiPHong.HasValue ? x.LoaiPHong == 2 ? "Nam" : "Nữ" : "Khác",
                    TrangThai = x.TrangThai,
                    TrangThaiStr = x.TrangThai == "BINH_THUONG" ? "Bình thường" : "Bị hỏng",
                    TenThietBi = x.TenThietBi,
                    GiaThietBi = x.GiaThietBi,
                    GiaThietBiStr = x.GiaThietBi.HasValue ? x.GiaThietBi.Value.ToString("#,###") : "0",
                    SoLuong = x.SoLuong,
                    GhiChu = x.GhiChu
                }).ToList();

            var pagedResult = new PagedResult<ThietBiModelView>
            {
                Results = data,
                CurrentPage = ThietBiModelViewSearch.PageIndex,
                PageSize = ThietBiModelViewSearch.PageSize,
                RowCount = totalRow
            };
            return pagedResult;
        }

        public ThietBiModelView GetById(int id)
        {
            // viết code cho hàm GetById
            var data = _thietBiRepository
                .FindAll(p => p.Id == id, c => c.Nha, c => c.Phong)
                .FirstOrDefault();
            
            if (data != null)
            {
                var model = new ThietBiModelView()
                {
                    Id = data.Id,
                    MaNha = data.MaNha,
                    TenNha = data.Nha.TenNha,
                    MaPhong = data.MaPhong,
                    TenPhong = data.Phong.TenPhong,
                    LoaiPHong = data.LoaiPHong,
                    LoaiPhongStr = data.LoaiPHong.HasValue ? data.LoaiPHong == 2 ? "Nam" : "Nữ" : "Khác",
                    TrangThai = data.TrangThai,
                    TrangThaiStr = data.TrangThai == "BINH_THUONG" ? "Bình thường" : "Bị hỏng",
                    TenThietBi = data.TenThietBi,
                    GiaThietBi = data.GiaThietBi,
                    GiaThietBiStr = data.GiaThietBi.HasValue ? data.GiaThietBi.Value.ToString("#,###") : "0",
                    SoLuong = data.SoLuong,
                    GhiChu = data.GhiChu
                };
                return model;
            }
            return null;
        }

        public List<ThietBiModelView> GetAll()
        {
            // viết code cho hàm GetAll
            var data = _thietBiRepository.FindAll(c => c.Nha, c => c.Phong).Select(x => new ThietBiModelView
            {
                Id = x.Id,
                MaNha = x.MaNha,
                TenNha = x.Nha.TenNha,
                MaPhong = x.MaPhong,
                TenPhong = x.Phong.TenPhong,
                LoaiPHong = x.LoaiPHong,
                LoaiPhongStr = x.LoaiPHong.HasValue ? x.LoaiPHong == 2 ? "Nam" : "Nữ" : "Khác",
                TrangThai = x.TrangThai,
                TrangThaiStr = x.TrangThai == "BINH_THUONG" ? "Bình thường" : "Bị hỏng",
                TenThietBi = x.TenThietBi,
                GiaThietBi = x.GiaThietBi,
                GiaThietBiStr = x.GiaThietBi.HasValue ? x.GiaThietBi.Value.ToString("#,###") : "0",
                SoLuong = x.SoLuong,
                GhiChu = x.GhiChu
            }).ToList();
            
            return data;

        }

        public void Add(ThietBiModelView view)
        {
            if (view == null)
                return;

            try
            {
                var data = new ThietBi()
                {
                    MaNha = view.MaNha,
                    MaPhong = view.MaPhong,
                    LoaiPHong = view.LoaiPHong,
                    TrangThai = view.TrangThai,
                    TenThietBi = view.TenThietBi,
                    GiaThietBi = view.GiaThietBi,
                    SoLuong = view.SoLuong,
                    GhiChu = view.GhiChu
                };
                _thietBiRepository.Add(data);
            } 
            catch (Exception ex) { }
            

        }

        public bool Update(ThietBiModelView view)
        {
            if (view == null)
                return false;
            
            try
            {
                var item = _thietBiRepository.FindById(view.Id);
                
                if (item == null)
                    return false;
                
                item.MaNha = view.MaNha;
                item.MaPhong = view.MaPhong;
                item.LoaiPHong = view.LoaiPHong;
                item.TrangThai = view.TrangThai;
                item.TenThietBi = view.TenThietBi;
                item.GiaThietBi = view.GiaThietBi;
                item.SoLuong = view.SoLuong;
                item.GhiChu = view.GhiChu;
                _thietBiRepository.Update(item);
                return true;
                
            } 
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Deleted(int id)
        {
            try
            {
                var item = _thietBiRepository.FindById(id);
                if (item == null)
                    return false;
                _thietBiRepository.Remove(item);
                return true;
            } 
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public int GetCount()
        {
            // viết code cho hàm GetCount
            return _thietBiRepository.FindAll().Count();
        }

        public bool IsExist(string tenThietBi)
        {
            // viết code cho hàm IsExist
            return _thietBiRepository
                .FindAll()
                .Any(p => p.TenThietBi.ToLower() == tenThietBi.ToLower());
        }
    }

    public interface IThietBiService
    {
        PagedResult<ThietBiModelView> GetAllPaging(ThietBiViewModelSearch ThietBiModelViewSearch);
        ThietBiModelView GetById(int id);
        List<ThietBiModelView> GetAll();
        void Add(ThietBiModelView view);
        bool Update(ThietBiModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
        bool IsExist(string tenThietBi);
    }
}
