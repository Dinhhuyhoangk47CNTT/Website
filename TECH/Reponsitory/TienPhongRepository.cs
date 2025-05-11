using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using Website.Data.DatabaseEntity;

namespace Website.Reponsitory
{
    public interface ITienPhongRepository : IRepository<TienPhong, int>
    {

    }

    public class TienPhongRepository : EFRepository<TienPhong, int>, ITienPhongRepository
    {
        public TienPhongRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
