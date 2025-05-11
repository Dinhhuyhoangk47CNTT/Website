(function ($) {
    var self = this;
    self.Data = [];
    self.UserImages = {};
    self.IsUpdate = false;
    self.TienPhong = {
        Id: 0,
        MaNha: 0,
        TenNha: "",
        MaPhong: 0,
        TenPhong: "",
        LoaiPhong: 2,
        LoaiPhongStr: "",
        GiaPhong: 0,
        GiaPhongStr: "",
        SoTienCanNop: 0,
        SoTienCanNopStr: "",
        SoTienDaNop: 0,
        SoTienDaNopStr: "",
        HanNop: null,
        HanNopStr: "",
        NgayNop: null,
        NgayNopStr: "",
        NamHoc: "",
        HocKy: "",
        TrangThai: "", // DA_NOP, CHUA_NOP
        TrangThaiStr: "",
    }

    self.Search = {
        thang: 0,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }

    self.lstRole = [];

    self.UserSearch = {
        thang: 0,
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }

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
                html += "<td>" + item.TenNha + "</td>";
                html += "<td>" + item.TenPhong + "</td>";
                html += "<td>" + item.LoaiPhongStr + "</td>";
                html += "<td>" + item.GiaPhongStr + "</td>";
                html += "<td>" + item.SoTienCanNopStr + "</td>";
                html += "<td>" + item.SoTienDaNopStr + "</td>";
                html += "<td>" + item.HocKy + "</td>";
                html += "<td>" + item.NgayNopStr + "</td>";
                html += "<td>" + item.HanNopStr + "</td>";
                html += "<td>" + item.NamHoc + "</td>";
                html += "<td>" + item.TrangThaiStr + "</td>";

                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
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
        //var _data = {
        //    Name: $(".name-search").val() != "" ? $(".name-search").val() : null,
        //    loaiDV: $("#LoaiDV").val() != "" ? $("#LoaiDV").val() : null,
        //    PageIndex: tedu.configs.pageIndex,
        //    PageSize: tedu.configs.pageSize
        //};

        self.UserSearch.PageIndex = tedu.configs.pageIndex;
        self.UserSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/ThongKe/ThongKeTienPhongPaging',
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

    

    $(document).ready(function () {
        self.GetDataPaging();

        $('#TrangThaiSearch').on('change', function () {
            $('input.form-search').val("");
            self.UserSearch.thang = $(this).val();
            self.GetDataPaging(true);
        });

        $('#ddlShowPage').on('change', function () {
            tedu.configs.pageSize = $(this).val();
            tedu.configs.pageIndex = 1;
            self.GetDataPaging(true);
        });

        $('input.form-search').on('input', function (e) {
            self.UserSearch.thang = $(this).val();
            self.GetDataPaging(true);
        });

        $(".btn-export-excel").click(function () {
            window.location.href = "/admin/thongke/ThongKeTienPhongExcel";
        });
    })
})(jQuery);