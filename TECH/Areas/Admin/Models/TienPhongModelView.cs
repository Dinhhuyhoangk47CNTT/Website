namespace Website.Areas.Admin.Models
{
    public class TienPhongModelView
    {
        public int Id { get; set; }
        public int? MaNha { get; set; }
        public string? TenNha { get; set; }
        public int? MaPhong { get; set; }
        public string? TenPhong { get; set; }
        public int? LoaiPHong { get; set; }
        public string? LoaiPhongStr { get; set; }
        public decimal? GiaPhong { get; set; }
        public string? GiaPhongStr { get; set; }
        public decimal? SoTienCanNop { get; set; }
        public string? SoTienCanNopStr { get; set; }
        public decimal? SoTienDaNop { get; set; }
        public string? SoTienDaNopStr { get; set; }
        public DateTime? HanNop { get; set; }
        public string? HanNopStr { get; set; }
        public DateTime? NgayNop { get; set; }
        public string? NgayNopStr { get; set; }
        public string? NamHoc { get; set; }
        public string? HocKy { get; set; }
        public string? TrangThai { get; set; }
        public string? TrangThaiStr { get; set; }
    }
}
