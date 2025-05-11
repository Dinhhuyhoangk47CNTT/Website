using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using Website.Data.DatabaseEntity;

namespace Website.Reponsitory
{
    public interface ITienDienRepository : IRepository<TienDien, int>
    {

    }

    public class TienDienRepository : EFRepository<TienDien, int>, ITienDienRepository
    {
        public TienDienRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
