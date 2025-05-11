using System.ComponentModel.DataAnnotations.Schema;
using TECH.SharedKernel;

namespace Website.Data.DatabaseEntity
{
    [Table("VanBan")]
    public class VanBan : DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(250)")]
        public string? TieuDe { get; set; }
        [Column(TypeName = "nvarchar(2000)")]
        public string? NoiDung { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string? FileDinhKem { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? NgayHetHan { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? LoaiVanBan { get; set; }
    }
}
