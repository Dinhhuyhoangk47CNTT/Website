﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Website.Data.DatabaseEntity;

namespace TECH.Data.DatabaseEntity
{
    public class DataBaseEntityContext : DbContext
    {
        public DataBaseEntityContext(DbContextOptions<DataBaseEntityContext> options) : base(options) { }

        public DbSet<ChiTietHoaDon> chiTietHoaDons { set; get; }
        public DbSet<Nha> Nhas { set; get; }
        public DbSet<DichVu> dichVus { set; get; }
        public DbSet<HoaDon> hoaDons { set; get; }
        public DbSet<KhachHang> khachHangs { set; get; }
        public DbSet<NhanVien> nhanViens { set; get; }
        public DbSet<HopDong> HopDongs { set; get; }
        public DbSet<Phong> Phongs { set; get; }
        public DbSet<ThanhPho> ThanhPhos { set; get; }
        public DbSet<QuanHuyen> quanHuyens { set; get; }
        public DbSet<PhuongXa> PhuongXas { set; get; }
        public DbSet<DichVuPhong> DichVuPhongs { set; get; }
        public DbSet<ThanhVienPhong> ThanhVienPhongs { set; get; }
        public DbSet<LoiPham> LoiPhams { set; get; }
        public DbSet<TheKiTucXa> TheKiTucXas { set; get; }
        public DbSet<ThietBi> ThietBis { set; get; }
        public DbSet<TienPhong> TienPhongs { set; get; }
        public DbSet<TienDien> TienDiens { set; get; }
        public DbSet<TienNuoc> TienNuocs { set; get; }
        public DbSet<VanBan> VanBans { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);            
        }
    }
}
