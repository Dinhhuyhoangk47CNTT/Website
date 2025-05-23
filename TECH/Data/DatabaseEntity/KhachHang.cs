﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
using static TECH.General.General;
namespace TECH.Data.DatabaseEntity
{

    [Table("KhachHang")]
    public class KhachHang : DomainEntity<int>
    {
        [Column(TypeName = "nvarchar(250)")]
        public string? TenKH { get; set; }

        [Column(TypeName = "varchar(11)")]
        public string? SoDienThoai { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Email { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? CMND { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string? GioiTinh { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string? DiaChi { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string? MatKhau { get; set; }
        public DateTime? NgaySinh { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string MaSV { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Nganh { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Khoa { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string SDTGiaDinh { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? TrangThai { get; set; } // TRA/MUON
        [Column(TypeName = "nvarchar(50)")]
        public string? ChucVu { get; set; } // TRUONG_PHONG/NGUOI_THUE
        [Column(TypeName = "nvarchar(100)")]
        public string? TenPhong { get; set; }
    }
}
