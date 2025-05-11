using System.ComponentModel.DataAnnotations.Schema;
using TECH.Data.DatabaseEntity;
using TECH.SharedKernel;

namespace Website.Areas.Admin.Models
{
    public class ThietBiModelView
    {
        public int Id { get; set; }
        public int? MaNha { get; set; }
        public string? TenNha { get; set; }
        public int? MaPhong { get; set; }
        public string? TenPhong { get; set; }
        public int? LoaiPHong { get; set; }
        public string? LoaiPhongStr { get; set; }
        public string? TrangThai { get; set; }
        public string? TrangThaiStr { get; set; }
        public string? TenThietBi { get; set; }
        public decimal? GiaThietBi { get; set; }
        public string? GiaThietBiStr { get; set; }
        public int? SoLuong { get; set; }
        public string? GhiChu { get; set; }
    }
}
