(function ($) {
    var self = this;
    self.Data = [];
    self.UserImages = {};
    self.IsUpdate = false;   
    self.HopDong = {
        Id:0,
        MaPhong:0,
        MaNha:0,
        MaNV:0,
        MaKH:0,
        //NgayBatDau:"",
        NgayKetThuc:"",
        //TienCoc:0,
        TrangThai: "",
        ChucVuPhong: ""
        //GhiChu:""
    }
    self.UserSearch = {
        name: "",
        status: 2,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }
    self.lstRole = [];

    self.addSerialNumber = function () {
        var index = 0;
        $("table tbody tr").each(function (index) {
            $(this).find('td:nth-child(1)').html(index + 1);
        });
    };
    self.Files = {};

    self.RenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                //html += "<td>" + item.Id + "</td>";
                html += "<td>" + item.TenKhachHang + "</td>";
                //html += "<td>" + (item.TenNhanVien != "" && item.TenNhanVien != null ? item.TenNhanVien:"") + "</td>";
                html += "<td>" + (item.ChucVuPhong == "TRUONG_PHONG" ? "Trưởng Phòng" : "Thành Viên") + "</td>";
                html += "<td>" + item.TenNha + "</td>";
                html += "<td>" + item.TenPhong + "</td>";
                //html += "<td>" + item.NgayBatDauStr + "</td>";
                html += "<td>" + item.NgayKetThucStr + "</td>";
                //html += "<td>" + item.TienCoc + "</td>";
                 
                html += "<td>Đã trả phòng</td>";  
                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"11\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };
   
    self.WrapPaging = function (recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / tedu.configs.pageSize);
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: '<<',
            prev: '<',
            next: '>',
            last: '>>',
            onPageClick: function (event, p) {
                tedu.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }

    self.GetDataPaging = function (isPageChanged) {
        var _data = {
            Name: $(".name-search").val() != "" ? $(".name-search").val() : null,
            Code: $(".code-search").val() != "" ? $(".code-search").val() : null,
            PageIndex: tedu.configs.pageIndex,
            PageSize: tedu.configs.pageSize
        };

        self.UserSearch.PageIndex = tedu.configs.pageIndex;
        self.UserSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/ThongKe/ThongKeSinhVienDaTraPhongPaging',
            type: 'GET',
            data: self.UserSearch,
            dataType: 'json',
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                self.RenderTableHtml(response.data.Results);
                $('#lblTotalRecords').text(response.data.RowCount);
                if (response.data.RowCount != null && response.data.RowCount > 0) {
                    self.WrapPaging(response.data.RowCount, function () {
                        GetDataPaging();
                    }, isPageChanged);
                }

            }
        })
    };

    self.Init = function () {
       
    }

  

    self.RenderHtmlByObject = function (view) {
        $("#MaPhong").val(view.MaPhong);
        $("#MaNha").val(view.MaNha);
        //$("#MaNV").val(view.MaNV);
        $("#ChucVuPhong").val(view.ChucVuPhong);
        $("#MaKH").val(view.MaKH);

        //$("#TienCoc").val(view.TienCoc);
        $("#TrangThai").val(view.TrangThai);
        //$("#GhiChu").val(view.GhiChu); 
        //$("#NgayBatDau").datepicker("setDate", new Date(view.NgayBatDauStr));
        if (view.NgayKetThucStr != "") {
            $("#NgayKetThuc").datepicker("setDate", new Date(view.NgayKetThucStr));
        }
       

    }
    self.BindRoleHtml = function (data) {
        if (data !== null && data.length > 0) {
            var html = "<option value='0'>Chọn quyền</option>";
            $.each(data, function (key, item) {
                html += "<option value=" + item.Id + ">" + item.Name+"</option>";
            })
            $(".data-select2").html(html);
        }
    }

    $(document).ready(function () {
        self.GetDataPaging();


        $('#select-right').on('change', function () {
            $('input.form-search').val("");
            self.UserSearch.name = null;
            self.UserSearch.status = $(this).val();
            self.GetDataPaging(true);
        });
        $('#TrangThaiSearch').on('change', function () {
            $('input.form-search').val("");
            self.UserSearch.name = null;
            self.UserSearch.status = $(this).val();
            self.GetDataPaging(true);
        });

        

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.UserSearch.name = $(this).val();
            self.GetDataPaging(true);
        });

        $(".btn-export-excel").click(function () {
            window.location.href = "/admin/thongke/ThongKeSinhVienDaTraPhongExcel";
        });
    })
})(jQuery);