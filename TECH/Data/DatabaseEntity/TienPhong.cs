using System.ComponentModel.DataAnnotations.Schema;
using TECH.Data.DatabaseEntity;
using TECH.SharedKernel;

namespace Website.Data.DatabaseEntity
{
    [Table("TienPhong")]
    public class TienPhong : DomainEntity<int>
    {
        public int? MaNha { get; set; }
        [ForeignKey("MaNha")]
        public Nha? Nha { get; set; }

        public int? MaPhong { get; set; }

        [ForeignKey("MaPhong")]
        public Phong? Phong { get; set; }

        public int? LoaiPHong { get; set; }
        public decimal? GiaPhong { get; set; }
        public decimal? SoTienCanNop { get; set; }
        public decimal? SoTienDaNop { get; set; }
        public DateTime? HanNop { get; set; }
        public DateTime? NgayNop { get; set; }
        public string? NamHoc { get; set; }
        public string? HocKy { get; set; }
        public string? TrangThai { get; set; }
    }
}
