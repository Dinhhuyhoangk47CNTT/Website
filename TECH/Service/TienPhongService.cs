using TECH.Reponsitory;
using TECH.Utilities;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Data.DatabaseEntity;
using Website.Reponsitory;

namespace Website.Service
{
    public class TienPhongService : ITienPhongService
    {
        private readonly ITienPhongRepository _tienPhongRepository;
        private IUnitOfWork _unitOfWork;

        public TienPhongService(
            IUnitOfWork unitOfWork
            , ITienPhongRepository tienPhongRepository)
        {
            _unitOfWork = unitOfWork;
            _tienPhongRepository = tienPhongRepository;
        }

        public void Add(TienPhongModelView view)
        {
            if (view == null)
                return;
            try
            {
                // viết code cho hàm Add
                var entity = new TienPhong()
                {
                    MaNha = view.MaNha,
                    MaPhong = view.MaPhong,
                    LoaiPHong = view.LoaiPHong,
                    GiaPhong = view.GiaPhong,
                    SoTienCanNop = view.SoTienCanNop,
                    SoTienDaNop = view.SoTienDaNop,
                    HanNop = view.HanNop,
                    NgayNop = view.NgayNop,
                    NamHoc = view.NamHoc,
                    HocKy = view.HocKy,
                    TrangThai = view.TrangThai,
                };
                _tienPhongRepository.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Deleted(int id)
        {
            // viết code cho hàm Deleted
            try
            {
                var item = _tienPhongRepository
                    .FindById(id, c => c.Nha, c => c.Phong);
                
                if (item == null)
                    return false;
                
                _tienPhongRepository.Remove(item);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<TienPhongModelView> GetAll()
        {
            // viết code cho hàm GetAll
            var data = _tienPhongRepository
                .FindAll(c => c.Nha, c => c.Phong);
            
            return data
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
        }

        public PagedResult<TienPhongModelView> GetAllPaging(TienPhongViewModelSearch TienPhongModelViewSearch)
        {
            // viết code cho hàm GetAllPaging
            var query = _tienPhongRepository
                .FindAll(c => c.Nha, c => c.Phong);
            
            if (!string.IsNullOrEmpty(TienPhongModelViewSearch.name))
            {
                query = query.Where(x => x
                    .Phong.TenPhong
                    .ToLower()
                    .Contains(TienPhongModelViewSearch.name.ToLower()));
            }

            var totalRow = query.Count();
            var data = query
                .Skip((TienPhongModelViewSearch.PageIndex - 1) * TienPhongModelViewSearch.PageSize)
                .Take(TienPhongModelViewSearch.PageSize)
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
                CurrentPage = TienPhongModelViewSearch.PageIndex,
                PageSize = TienPhongModelViewSearch.PageSize,
                RowCount = totalRow
            };

            return pagedResult;
        }

        public TienPhongModelView GetById(int id)
        {
            // viết code cho hàm GetById
            var item = _tienPhongRepository
                .FindById(id, c => c.Phong, c => c.Nha);
            
            if (item == null)
                return null;

            return new TienPhongModelView
            {
                Id = item.Id,
                MaNha = item.MaNha,
                TenNha = item.Nha.TenNha,
                MaPhong = item.MaPhong,
                TenPhong = item.Phong.TenPhong,
                LoaiPHong = item.LoaiPHong,
                LoaiPhongStr = item.LoaiPHong.HasValue ? item.LoaiPHong == 2 ? "Nam" : "Nữ" : "Khác",
                GiaPhong = item.GiaPhong,
                GiaPhongStr = item.GiaPhong.HasValue ? item.GiaPhong.Value.ToString("#,###") : "",
                SoTienCanNop = item.SoTienCanNop,
                SoTienCanNopStr = item.SoTienCanNop.HasValue ? item.SoTienCanNop.Value.ToString("#,###") : "",
                SoTienDaNop = item.SoTienDaNop,
                SoTienDaNopStr = item.SoTienDaNop.HasValue ? item.SoTienDaNop.Value.ToString("#,###") : "",
                HanNop = item.HanNop,
                HanNopStr = item.HanNop.HasValue ? item.HanNop.Value.ToString("dd/MM/yyyy") : "",
                NgayNop = item.NgayNop,
                NgayNopStr = item.NgayNop.HasValue ? item.NgayNop.Value.ToString("dd/MM/yyyy") : "",
                NamHoc = item.NamHoc,
                HocKy = item.HocKy,
                TrangThai = item.TrangThai,
                TrangThaiStr = item.TrangThai == "DA_NOP" ? "Đã nộp" : "Chưa nộp"
            };
        }

        public int GetCount()
        {
            // viết code cho hàm GetCount
            return _tienPhongRepository.FindAll().Count();
        }

        public void Save()
        {
            // viết code cho hàm Save
            _unitOfWork.Commit();
        }

        public bool Update(TienPhongModelView view)
        {
            // viết code cho hàm Update
            try
            {
                var item = _tienPhongRepository.FindById(view.Id);

                if (item == null)
                    return false;
                
                item.MaNha = view.MaNha;
                item.MaPhong = view.MaPhong;
                item.LoaiPHong = view.LoaiPHong;
                item.GiaPhong = view.GiaPhong;
                item.SoTienCanNop = view.SoTienCanNop;
                item.SoTienDaNop = view.SoTienDaNop;
                item.HanNop = view.HanNop;
                item.NgayNop = view.NgayNop;
                item.NamHoc = view.NamHoc;
                item.HocKy = view.HocKy;
                item.TrangThai = view.TrangThai;
                _tienPhongRepository.Update(item);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
    public interface ITienPhongService
    {
        PagedResult<TienPhongModelView> GetAllPaging(TienPhongViewModelSearch TienPhongModelViewSearch);
        TienPhongModelView GetById(int id);
        List<TienPhongModelView> GetAll();
        void Add(TienPhongModelView view);
        bool Update(TienPhongModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
    }
}
