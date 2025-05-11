using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using Website.Data.DatabaseEntity;

namespace Website.Reponsitory
{
    public interface IVanBanRepository : IRepository<VanBan, int>
    {

    }

    public class VanBanRepository : EFRepository<VanBan, int>, IVanBanRepository
    {
        public VanBanRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
