namespace Website.Areas.Admin.Models
{
    public class TienNuocModelView
    {
        public int Id { get; set; }
        public int? MaNha { get; set; }
        public string? TenNha { get; set; }
        public int? MaPhong { get; set; }
        public string? TenPhong { get; set; }
        public int? LoaiPHong { get; set; }
        public string? LoaiPhongStr { get; set; }
        public decimal? SoCongToNuocTruoc { get; set; }
        public string? SoCongToNuocTruocStr { get; set; }
        public decimal? SoCongToNuocHienTai { get; set; }
        public string? SoCongToNuocHienTaiStr { get; set; }
        public decimal? SoTienCanPhaiNop { get; set; }
        public string? SoTienCanPhaiNopStr { get; set; }
        public decimal? SoTienDaNop { get; set; }
        public string? SoTienDaNopStr { get; set; }
        public DateTime? HanNop { get; set; }
        public string? HanNopStr { get; set; }
        public DateTime? NgayNop { get; set; }
        public string? NgayNopStr { get; set; }
        public int? Thang { get; set; }
        public string? TrangThai { get; set; }
        public string? TrangThaiStr { get; set; }
    }
}
