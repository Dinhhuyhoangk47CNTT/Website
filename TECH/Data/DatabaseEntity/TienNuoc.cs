using System.ComponentModel.DataAnnotations.Schema;
using TECH.Data.DatabaseEntity;
using TECH.SharedKernel;

namespace Website.Data.DatabaseEntity
{
    [Table("TienNuoc")]
    public class TienNuoc : DomainEntity<int>
    {
        public int? MaNha { get; set; }
        [ForeignKey("MaNha")]
        public Nha? Nha { get; set; }

        public int? MaPhong { get; set; }

        [ForeignKey("MaPhong")]
        public Phong? Phong { get; set; }

        public int? LoaiPHong { get; set; }
        public decimal? SoCongToNuocTruoc { get; set; }
        public decimal? SoCongToNuocHienTai { get; set; }
        public decimal? SoTienCanPhaiNop { get; set; }
        public decimal? SoTienDaNop { get; set; }
        public DateTime? HanNop { get; set; }
        public DateTime? NgayNop { get; set; }
        public int? Thang { get; set; }
        public string? TrangThai { get; set; }
    }
}
