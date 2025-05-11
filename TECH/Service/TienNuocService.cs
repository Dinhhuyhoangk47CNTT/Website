using TECH.Reponsitory;
using TECH.Utilities;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Data.DatabaseEntity;
using Website.Reponsitory;

namespace Website.Service
{
    public class TienNuocService : ITienNuocService
    {
        private readonly ITienNuocRepository _tienNuocRepository;
        private IUnitOfWork _unitOfWork;

        public TienNuocService(
            IUnitOfWork unitOfWork
            , ITienNuocRepository tienNuocRepository)
        {
            _unitOfWork = unitOfWork;
            _tienNuocRepository = tienNuocRepository;
        }

        public PagedResult<TienNuocModelView> GetAllPaging(TienNuocViewModelSearch TienNuocModelViewSearch)
        {
            // viết code cho hàm GetAllPaging
            var query = _tienNuocRepository.FindAll(c => c.Nha, c => c.Phong);

            if (!string.IsNullOrEmpty(TienNuocModelViewSearch.name))
            {
                query = query.Where(x => x
                    .Phong.TenPhong
                    .ToLower()
                    .Contains(TienNuocModelViewSearch.name.ToLower()));
            }
            var totalRow = query.Count();
            var data = query
                .Skip((TienNuocModelViewSearch.PageIndex - 1) * TienNuocModelViewSearch.PageSize)
                .Take(TienNuocModelViewSearch.PageSize)
                .Select(x => new TienNuocModelView
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
                    SoCongToNuocTruoc = x.SoCongToNuocTruoc,
                    SoCongToNuocTruocStr = x.SoCongToNuocTruoc.HasValue ? x.SoCongToNuocTruoc.Value.ToString("#,###") : "",
                    SoCongToNuocHienTai = x.SoCongToNuocHienTai,
                    SoCongToNuocHienTaiStr = x.SoCongToNuocHienTai.HasValue ? x.SoCongToNuocHienTai.Value.ToString("#,###") : "",
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

            var pagedResult = new PagedResult<TienNuocModelView>
            {
                Results = data,
                CurrentPage = TienNuocModelViewSearch.PageIndex,
                PageSize = TienNuocModelViewSearch.PageSize,
                RowCount = totalRow
            };
            return pagedResult;
        }

        public TienNuocModelView GetById(int id)
        {
            // viết code cho hàm GetById
            var x = _tienNuocRepository.FindById(id, c => c.Nha, c => c.Phong);
            // convert sang TienNuocModelView rồi return
            return new TienNuocModelView()
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
                SoCongToNuocTruoc = x.SoCongToNuocTruoc,
                SoCongToNuocTruocStr = x.SoCongToNuocTruoc.HasValue ? x.SoCongToNuocTruoc.Value.ToString("#,###") : "",
                SoCongToNuocHienTai = x.SoCongToNuocHienTai,
                SoCongToNuocHienTaiStr = x.SoCongToNuocHienTai.HasValue ? x.SoCongToNuocHienTai.Value.ToString("#,###") : "",
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

        public List<TienNuocModelView> GetAll()
        {
            // viết code cho hàm GetAll
            var data = _tienNuocRepository.FindAll(c => c.Nha, c => c.Phong).Select(x => new TienNuocModelView
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
                SoCongToNuocTruoc = x.SoCongToNuocTruoc,
                SoCongToNuocTruocStr = x.SoCongToNuocTruoc.HasValue ? x.SoCongToNuocTruoc.Value.ToString("#,###") : "",
                SoCongToNuocHienTai = x.SoCongToNuocHienTai,
                SoCongToNuocHienTaiStr = x.SoCongToNuocHienTai.HasValue ? x.SoCongToNuocHienTai.Value.ToString("#,###") : "",
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

        public void Add(TienNuocModelView view)
        {
            if (view == null)
                return;

            try
            {
                // viết code cho hàm Add
                var entity = new TienNuoc()
                {
                    MaNha = view.MaNha,
                    MaPhong = view.MaPhong,
                    LoaiPHong = view.LoaiPHong,
                    TrangThai = view.TrangThai,
                    SoCongToNuocTruoc = view.SoCongToNuocTruoc,
                    SoCongToNuocHienTai = view.SoCongToNuocHienTai,
                    SoTienCanPhaiNop = view.SoTienCanPhaiNop,
                    SoTienDaNop = view.SoTienDaNop,
                    HanNop = view.HanNop,
                    NgayNop = view.NgayNop,
                    Thang = view.Thang
                };
                _tienNuocRepository.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool Update(TienNuocModelView view)
        {
            // viết code cho hàm Update
            var entity = _tienNuocRepository.FindById(view.Id);
            if (entity == null)
                return false;
            try
            {
                entity.MaNha = view.MaNha;
                entity.MaPhong = view.MaPhong;
                entity.LoaiPHong = view.LoaiPHong;
                entity.TrangThai = view.TrangThai;
                entity.SoCongToNuocTruoc = view.SoCongToNuocTruoc;
                entity.SoCongToNuocHienTai = view.SoCongToNuocHienTai;
                entity.SoTienCanPhaiNop = view.SoTienCanPhaiNop;
                entity.SoTienDaNop = view.SoTienDaNop;
                entity.HanNop = view.HanNop;
                entity.NgayNop = view.NgayNop;
                entity.Thang = view.Thang;
                _tienNuocRepository.Update(entity);
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
                var item = _tienNuocRepository.FindById(id);
                if (item == null)
                    return false;
                _tienNuocRepository.Remove(item);
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
            return _tienNuocRepository.FindAll().Count();
        }
    }

    public interface ITienNuocService
    {
        PagedResult<TienNuocModelView> GetAllPaging(TienNuocViewModelSearch TienNuocModelViewSearch);
        TienNuocModelView GetById(int id);
        List<TienNuocModelView> GetAll();
        void Add(TienNuocModelView view);
        bool Update(TienNuocModelView view);
        bool Deleted(int id);
        void Save();
        int GetCount();
    }
}
