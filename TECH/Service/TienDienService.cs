using TECH.Reponsitory;
using TECH.Utilities;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Data.DatabaseEntity;
using Website.Reponsitory;

namespace Website.Service
{
    public class TienDienService : ITienDienService
    {
        private IUnitOfWork _unitOfWork;
        private readonly ITienDienRepository _tienDienRepository;

        public TienDienService(
            IUnitOfWork unitOfWork
            , ITienDienRepository tienDienRepository)
        {
            _unitOfWork = unitOfWork;
            _tienDienRepository = tienDienRepository;
        }

        public PagedResult<TienDienModelView> GetAllPaging(TienDienViewModelSearch TienDienModelViewSearch)
        {
            // viết code cho hàm GetAllPaging
            var query = _tienDienRepository.FindAll(c => c.Nha, c => c.Phong);

            if (!string.IsNullOrEmpty(TienDienModelViewSearch.name))
            {
                query = query.Where(x => x
                    .Phong.TenPhong
                    .ToLower()
                    .Contains(TienDienModelViewSearch.name.ToLower()));
            }
            var totalRow = query.Count();
            var data = query
                .Skip((TienDienModelViewSearch.PageIndex - 1) * TienDienModelViewSearch.PageSize)
                .Take(TienDienModelViewSearch.PageSize)
                .Select(x => new TienDienModelView
                {
                    Id = x.Id,
                    MaNha = x.MaNha,
                    TenNha = x.Nha.TenNha,
                    MaPhong = x.MaPhong,
                    TenPhong = x.Phong.TenPhong,
                    LoaiPHong = x.LoaiPHong,
                    LoaiPhongStr = x.LoaiPHong.HasValue ? x.LoaiPHong == 2 ? "Nam" : "Nữ" : "Khác",
                    TrangThai = x.TrangThai,
                    TrangThaiStr = x.TrangThai == "DA_NOP" ? "Đã nộp" : "Chưa nộp",
                    SoCongToDienTruoc = x.SoCongToDienTruoc,
                    SoCongToDienTruocStr = x.SoCongToDienTruoc.HasValue ? x.SoCongToDienTruoc.Value.ToString("#,###") : "",
                    SoCongToDienHienTai = x.SoCongToDienHienTai,
                    SoCongToDienHienTaiStr = x.SoCongToDienHienTai.HasValue ? x.SoCongToDienHienTai.Value.ToString("#,###") : "",
                    SoTienCanPhaiNop = x.SoTienCanPhaiNop,
                    SoTienCanPhaiNopStr = x.SoTienCanPhaiNop.HasValue ? x.SoTienCanPhaiNop.Value.ToString("#,###") : "",
                    SoTienDaNop = x.SoTienDaNop,
                    SoTienDaNopStr = x.SoTienDaNop.HasValue ? x.SoTienDaNop.Value.ToString("#,###") : "",
                    HanNop = x.HanNop,
                    HanNopStr = x.HanNop.HasValue ? x.HanNop.Value.ToString("dd/MM/yyyy") : "",
                    NgayNop = x.NgayNop,
                    NgayNopStr = x.NgayNop.HasValue ? x.NgayNop.Value.ToString("dd/MM/yyyy") : "",
                    Thang = x.Thang
                }).ToList();

            var pagedResult = new PagedResult<TienDienModelView>
            {
                Results = data,
                CurrentPage = TienDienModelViewSearch.PageIndex,
                PageSize = TienDienModelViewSearch.PageSize,
                RowCount = totalRow
            };
            return pagedResult;
        }

        public TienDienModelView GetById(int id)
        {
            // viết code cho hàm GetById
            var x = _tienDienRepository.FindById(id, c => c.Nha, c => c.Phong);
            // convert sang TienDienModelView rồi return
            return new TienDienModelView()
            {
                Id = x.Id,
                MaNha = x.MaNha,
                TenNha = x.Nha.TenNha,
                MaPhong = x.MaPhong,
                TenPhong = x.Phong.TenPhong,
                LoaiPHong = x.LoaiPHong,
                LoaiPhongStr = x.LoaiPHong.HasValue ? x.LoaiPHong == 2 ? "Nam" : "Nữ" : "Khác",
                TrangThai = x.TrangThai,
                TrangThaiStr = x.TrangThai == "DA_NOP" ? "Đã nộp" : "Chưa nộp",
                SoCongToDienTruoc = x.SoCongToDienTruoc,
                SoCongToDienTruocStr = x.SoCongToDienTruoc.HasValue ? x.SoCongToDienTruoc.Value.ToString("#,###") : "",
                SoCongToDienHienTai = x.SoCongToDienHienTai,
                SoCongToDienHienTaiStr = x.SoCongToDienHienTai.HasValue ? x.SoCongToDienHienTai.Value.ToString("#,###") : "",
                SoTienCanPhaiNop = x.SoTienCanPhaiNop,
                SoTienCanPhaiNopStr = x.SoTienCanPhaiNop.HasValue ? x.SoTienCanPhaiNop.Value.ToString("#,###") : "",
                SoTienDaNop = x.SoTienDaNop,
                SoTienDaNopStr = x.SoTienDaNop.HasValue ? x.SoTienDaNop.Value.ToString("#,###") : "",
                HanNop = x.HanNop,
                HanNopStr = x.HanNop.HasValue ? x.HanNop.Value.ToString("dd/MM/yyyy") : "",
                NgayNop = x.NgayNop,
                NgayNopStr = x.NgayNop.HasValue ? x.NgayNop.Value.ToString("dd/MM/yyyy") : "",
                Thang = x.Thang
            };
        }

        public List<TienDienModelView> GetAll()
        {
            // viết code cho hàm GetAll
            var data = _tienDienRepository.FindAll(c => c.Nha, c => c.Phong).Select(x => new TienDienModelView
            {
                Id = x.Id,
                MaNha = x.MaNha,
                TenNha = x.Nha.TenNha,
                MaPhong = x.MaPhong,
                TenPhong = x.Phong.TenPhong,
                LoaiPHong = x.LoaiPHong,
                LoaiPhongStr = x.LoaiPHong.HasValue ? x.LoaiPHong == 2 ? "Nam" : "Nữ" : "Khác",
                TrangThai = x.TrangThai,
                TrangThaiStr = x.TrangThai == "DA_NOP" ? "Đã nộp" : "Chưa nộp",
                SoCongToDienTruoc = x.SoCongToDienTruoc,
                SoCongToDienTruocStr = x.SoCongToDienTruoc.HasValue ? x.SoCongToDienTruoc.Value.ToString("#,###") : "",
                SoCongToDienHienTai = x.SoCongToDienHienTai,
                SoCongToDienHienTaiStr = x.SoCongToDienHienTai.HasValue ? x.SoCongToDienHienTai.Value.ToString("#,###") : "",
                SoTienCanPhaiNop = x.SoTienCanPhaiNop,
                SoTienCanPhaiNopStr = x.SoTienCanPhaiNop.HasValue ? x.SoTienCanPhaiNop.Value.ToString("#,###") : "",
                SoTienDaNop = x.SoTienDaNop,
                SoTienDaNopStr = x.SoTienDaNop.HasValue ? x.SoTienDaNop.Value.ToString("#,###") : "",
                HanNop = x.HanNop,
                HanNopStr = x.HanNop.HasValue ? x.HanNop.Value.ToString("dd/MM/yyyy") : "",
                NgayNop = x.NgayNop,
                NgayNopStr = x.NgayNop.HasValue ? x.NgayNop.Value.ToString("dd/MM/yyyy") : "",
                Thang = x.Thang
            }).ToList();
            return data;
        }

        public void Add(TienDienModelView view)
        {
            if (view == null)
                return;

            try
            {
                // viết code cho hàm Add
                var entity = new TienDien()
                {
                    MaNha = view.MaNha,
                    MaPhong = view.MaPhong,
                    LoaiPHong = view.LoaiPHong,
                    TrangThai = view.TrangThai,
                    SoCongToDienTruoc = view.SoCongToDienTruoc,
                    SoCongToDienHienTai = view.SoCongToDienHienTai,
                    SoTienCanPhaiNop = view.SoTienCanPhaiNop,
                    SoTienDaNop = view.SoTienDaNop,
                    HanNop = view.HanNop,
                    NgayNop = view.NgayNop,
                    Thang = view.Thang
                };
                _tienDienRepository.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public bool Update(TienDienModelView view)
        {
            // viết code cho hàm Update
            var entity = _tienDienRepository.FindById(view.Id);
            if (entity == null)
                return false;
            try
            {
                entity.MaNha = view.MaNha;
                entity.MaPhong = view.MaPhong;
                entity.LoaiPHong = view.LoaiPHong;
                entity.TrangThai = view.TrangThai;
                entity.SoCongToDienTruoc = view.SoCongToDienTruoc;
                entity.SoCongToDienHienTai = view.SoCongToDienHienTai;
                entity.SoTienCanPhaiNop = view.SoTienCanPhaiNop;
                entity.SoTienDaNop = view.SoTienDaNop;
                entity.HanNop = view.HanNop;
                entity.NgayNop = view.NgayNop;
                entity.Thang = view.Thang;
                _tienDienRepository.Update(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Deleted(int id)
        {
            // viết code cho hàm Deleted
            try
            {
                var item = _tienDienRepository.FindById(id);
                if (item == null)
                    return false;
                _tienDienRepository.Remove(item);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Save()
        {
            // viết code cho hàm Save
            _unitOfWork.Commit();
        }

        public int GetCount()
        {
            // viết code cho hàm GetCount
            return _tienDienRepository.FindAll().Count();
        }
    }

    public interface ITienDienService
    {
        PagedResult<TienDienModelView> GetAllPaging(TienDienViewModelSearch TienDienModelViewSearch);
        TienDienModelView GetById(int id);
        List<TienDienModelView> GetAll();
        void Add(TienDienModelView view);
        bool Update(TienDienModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
    }
}
