using System.ComponentModel.DataAnnotations.Schema;
using TECH.Data.DatabaseEntity;
using TECH.SharedKernel;

namespace Website.Data.DatabaseEntity
{
    [Table("ThietBi")]
    public class ThietBi : DomainEntity<int>
    {
        public int? MaNha { get; set; }
        [ForeignKey("MaNha")]
        public Nha? Nha { get; set; }

        public int? MaPhong { get; set; }

        [ForeignKey("MaPhong")]
        public Phong? Phong { get; set; }

        public int? LoaiPHong { get; set; }
        public string? TrangThai { get; set; }
        public string? TenThietBi { get; set; }
        public decimal? GiaThietBi { get; set; }
        public int? SoLuong { get; set; }
        public string? GhiChu { get; set; }
    }
}
