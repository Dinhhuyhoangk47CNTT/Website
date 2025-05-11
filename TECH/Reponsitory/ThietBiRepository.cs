using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using Website.Data.DatabaseEntity;

namespace Website.Reponsitory
{
    public interface IThietBiRepository : IRepository<ThietBi, int>
    {

    }

    public class ThietBiRepository : EFRepository<ThietBi, int>, IThietBiRepository
    {
        public ThietBiRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
