(function ($) {
    var self = this;
    self.Data = [];
    self.ThongKe = {
        Id: null,
        Thang: 0,
        TongLuongDienTieuThu: 0,
        TongTienCanNop: 0,
        TongTienDaNop: 0
    }
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
    self.RenderTableHtml = function (data) {
        var html = "";
        if (data != "" && data.length > 0) {
            var index = 0;
            for (var i = 0; i < data.length; i++) {
                var item = data[i];
                html += "<tr>";
                html += "<td>" + (++index) + "</td>";
                html += "<td>" + item.Thang + "</td>";
                html += "<td>" + item.TongLuongDienTieuThu + "</td>";
                html += "<td>" + item.TongTienCanNopStr + "</td>";
                html += "<td>" + item.TongTienDaNopStr + "</td>";
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
       
        self.UserSearch.PageIndex = tedu.configs.pageIndex;
        self.UserSearch.PageSize = tedu.configs.pageSize;

        $.ajax({
            url: '/Admin/ThongKe/ThongKeTienDienPaging',
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
            window.location.href = "/admin/thongke/ThongKeTienDienExcel";
        });

    })
})(jQuery);