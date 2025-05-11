using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using Website.Data.DatabaseEntity;

namespace Website.Reponsitory
{
    public interface ITienNuocRepository : IRepository<TienNuoc, int>
    {

    }

    public class TienNuocRepository : EFRepository<TienNuoc, int>, ITienNuocRepository
    {
        public TienNuocRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
