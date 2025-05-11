using System.ComponentModel.DataAnnotations.Schema;
using TECH.SharedKernel;

namespace Website.Areas.Admin.Models
{
    public class VanBanViewModel
    {
        public int Id { get; set; }
        public string? TieuDe { get; set; }
        public string? NoiDung { get; set; }
        public string? FileDinhKem { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public string? NgayHetHanStr { get; set; }
        public string? LoaiVanBan { get; set; }
    }
}
