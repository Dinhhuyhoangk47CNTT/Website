using TECH.Reponsitory;
using TECH.Utilities;
using Website.Areas.Admin.Models;
using Website.Areas.Admin.Models.Search;
using Website.Data.DatabaseEntity;
using Website.Reponsitory;
using Website.Areas.Admin.Models;

namespace Website.Service
{
    public class VanBanService : IVanBanService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IVanBanRepository _vanBanRepository;
        public VanBanService(IUnitOfWork unitOfWork
            , IVanBanRepository VanBanRepository)
        {
            _unitOfWork = unitOfWork;
            _vanBanRepository = VanBanRepository;
        }

        public PagedResult<VanBanViewModel> GetAllPaging(VanBanViewModelSearch vanBanViewModelSearch)
        {
            // viết code cho hàm GetAllPaging
            var query = _vanBanRepository.FindAll();

            if (!string.IsNullOrEmpty(vanBanViewModelSearch.LoaiVanBan))
            {
                query = query
                    .Where(x => x.LoaiVanBan == vanBanViewModelSearch.LoaiVanBan);
            }

            if (!string.IsNullOrEmpty(vanBanViewModelSearch.name))
            {
                query = query.Where(x => x
                    .TieuDe
                    .ToLower()
                    .Contains(vanBanViewModelSearch.name.ToLower()));
            }

            var totalRow = query.Count();
            var data = query.OrderBy(x => x.TieuDe)
                .Skip((vanBanViewModelSearch.PageIndex - 1) * vanBanViewModelSearch.PageSize)
                .Take(vanBanViewModelSearch.PageSize)
                .Select(x => new VanBanViewModel
                {
                    Id = x.Id,
                    TieuDe = x.TieuDe,
                    NoiDung = x.NoiDung,
                    FileDinhKem = x.FileDinhKem,
                    NgayHetHan = x.NgayHetHan,
                    NgayHetHanStr = x.NgayHetHan.HasValue ? x.NgayHetHan.Value.ToString("dd/MM/yyyy") : "",
                    LoaiVanBan = x.LoaiVanBan
                }).ToList();

            var pagedResult = new PagedResult<VanBanViewModel>
            {
                Results = data,
                CurrentPage = vanBanViewModelSearch.PageIndex,
                PageSize = vanBanViewModelSearch.PageSize,
                RowCount = totalRow
            };
            return pagedResult;
        }

        public VanBanViewModel GetById(int id)
        {
            // viết code cho hàm GetById
            var data = _vanBanRepository
                .FindAll(p => p.Id == id)
                .FirstOrDefault();

            if (data != null)
            {
                var model = new VanBanViewModel()
                {
                    Id = data.Id,
                    TieuDe = data.TieuDe,
                    NoiDung = data.NoiDung,
                    FileDinhKem = data.FileDinhKem,
                    NgayHetHan = data.NgayHetHan,
                    NgayHetHanStr = data.NgayHetHan.HasValue ? data.NgayHetHan.Value.ToString("dd/MM/yyyy") : "",
                    LoaiVanBan = data.LoaiVanBan
                };
                return model;
            }
            return null;
        }

        public List<VanBanViewModel> GetAll()
        {
            // viết code cho hàm GetAll
            var data = _vanBanRepository.FindAll()
                .Select(x => new VanBanViewModel
            {
                Id = x.Id,
                TieuDe = x.TieuDe,
                NoiDung = x.NoiDung,
                FileDinhKem = x.FileDinhKem,
                NgayHetHan = x.NgayHetHan,
                NgayHetHanStr = x.NgayHetHan.HasValue ? x.NgayHetHan.Value.ToString("dd/MM/yyyy") : "",
                    LoaiVanBan = x.LoaiVanBan
                }).ToList();

            return data;

        }

        public void Add(VanBanViewModel view)
        {
            if (view == null)
                return;

            try
            {
                var data = new VanBan()
                {
                    TieuDe = view.TieuDe,
                    NoiDung = view.NoiDung,
                    FileDinhKem =  view.FileDinhKem,
                    NgayHetHan = view.NgayHetHan,
                    LoaiVanBan = view.LoaiVanBan
                };
                _vanBanRepository.Add(data);
            }
            catch (Exception ex) { }


        }

        public bool Update(VanBanViewModel view)
        {
            if (view == null)
                return false;

            try
            {
                var item = _vanBanRepository.FindById(view.Id);

                if (item == null)
                    return false;

                item.TieuDe = view.TieuDe;
                item.NoiDung = view.NoiDung;
                item.FileDinhKem = view.FileDinhKem;
                item.NgayHetHan = view.NgayHetHan;
                item.LoaiVanBan = view.LoaiVanBan;
                _vanBanRepository.Update(item);
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
                var item = _vanBanRepository.FindById(id);
                if (item == null)
                    return false;
                _vanBanRepository.Remove(item);
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
            return _vanBanRepository.FindAll().Count();
        }

        public bool IsExist(string tieuDe)
        {
            // viết code cho hàm IsExist
            return _vanBanRepository
                .FindAll()
                .Any(p => p.TieuDe.ToLower() == tieuDe.ToLower());
        }
    }

    public interface IVanBanService
    {
        PagedResult<VanBanViewModel> GetAllPaging(VanBanViewModelSearch VanBanViewModelSearch);
        VanBanViewModel GetById(int id);
        List<VanBanViewModel> GetAll();
        void Add(VanBanViewModel view);
        bool Update(VanBanViewModel view);
        bool Deleted(int id);
        void Save();
        int GetCount();
        bool IsExist(string tenVanBan);
    }
}
