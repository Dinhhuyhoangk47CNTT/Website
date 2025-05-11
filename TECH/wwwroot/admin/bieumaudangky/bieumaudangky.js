(function ($) {
    var self = this;
    self.Data = [];
    self.UserImages = {};
    self.IsUpdate = false;
    self.FormDangKy = {
        Id: 0,
        TieuDe: "",
        NoiDung: "",
        NgayHetHan: null,
        NgayHetHanStr: ""
    }

    self.Search = {
        name: "",
        PageIndex: tedu.configs.pageIndex,
        PageSize: tedu.configs.pageSize
    }

    self.lstRole = [];

    self.UserSearch = {
        name: "",
        role: null,
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
                html += "<td>" + item.TieuDe + "</td>";
               /* html += "<td>" + item.NoiDung + "</td>";*/
                html += "<td>" + item.NgayHetHanStr + "</td>";

                html += "<td style=\"text-align: center;\">" +
                    "<button  class=\"btn btn-primary custom-button\" onClick=\"Update(" + item.Id + ")\"><i  class=\"bi bi-pencil-square\"></i></button>" +
                    "<button  class=\"btn btn-danger custom-button\" onClick=\"Deleted(" + item.Id + ")\"><i  class=\"bi bi-trash\"></i></button>" +
                    "</td>";
                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"10\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };

    self.Update = function (id) {
        if (id != null && id != "") {
            $(".txtPassword").hide();
            $("#titleModal").text("Cập nhật form đăng ký");
            $(".btn-submit-format").text("Cập nhật");
            /*$(".custom-format").attr("disabled", "disabled");*/
            self.GetById(id, self.RenderHtmlByObject);
            self.FormDangKy.Id = id;
            $('#AddOrUpdateModal').modal('show');

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/BieuMauDangKy/GetById',
                type: 'GET',
                dataType: 'json',
                data: {
                    id: id
                },
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.Data != null) {
                        setTimeout(function () {
                            renderCallBack(response.Data);
                        }, 1000);

                    }
                }
            })
        }
    }

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

    self.Deleted = function (id) {
        if (id != null && id != "") {
            tedu.confirm('Bạn có chắc muốn xóa form đăng ký này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/BieuMauDangKy/Delete",
                    data: { id: id },
                    beforeSend: function () {
                    },
                    success: function () {
                        tedu.notify('Đã xóa thành công', 'success');
                        self.GetDataPaging(true);
                    },
                    error: function () {
                        tedu.notify('Has an error', 'error');
                    }
                });
            });
        }
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
            url: '/Admin/BieuMauDangKy/GetAllPaging',
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

        $(".btn-add").click(function () {
            self.SetValueDefault();
            self.FormDangKy.Id = 0;
            $('#AddOrUpdateModal').modal('show')
        })

        // hủy add và edit
        $(".cs-close-addedit,.btn-cancel-addedit").click(function () {
            $("#CreateEdit").css("display", "none");
        })

        $('.add-role').click(function () {
            $('#AddRole').modal('show');
        })

    }
    self.AddUser = function (userView) {
        $.ajax({
            url: '/Admin/BieuMauDangKy/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                vanBanViewModel: userView
            },
            beforeSend: function () {
                Loading('show');
            },
            complete: function () {
                Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Thêm mới dữ liệu thành công', 'success');
                    self.GetDataPaging(true);
                    $('#AddOrUpdateModal').modal('hide');
                } else {
                    tedu.notify('Tiêu đề form đăng ký đã tồn tại', 'error');
                }
            }
        })
    }

    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/BieuMauDangKy/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                vanBanViewModel: userView
            },
            beforeSend: function () {
                //Loading('show');
            },
            complete: function () {
                //Loading('hiden');
            },
            success: function (response) {
                if (response.success) {
                    tedu.notify('Cập nhật dữ liệu thành công', 'success');
                    self.GetDataPaging(true);
                    $('#AddOrUpdateModal').modal('hide');
                } else {
                    tedu.notify('Tiêu đề form đăng ký đã tồn tại', 'error');
                }

            }
        })
    }

    self.ValidateUser = function () {
        $("#form-submit").validate({
            rules:
            {
                TieuDe: {
                    required: true,
                },
                NoiDung: {
                    required: true,
                },
                NgayHetHan: {
                    required: false,
                }
            },
            messages:
            {
                TieuDe: {
                    required: "Tiêu đề form đăng ký không được để trống",
                },
                NoiDung: {
                    required: "Nội dung không được để trống !"
                }
            },
            submitHandler: function (form) {
                self.GetValue();
                if (self.IsUpdate) {
                    self.UpdateUser(self.FormDangKy);
                }
                else {
                    self.AddUser(self.FormDangKy);
                }
            }
        });
    }

    self.GetValue = function () {
        self.FormDangKy.TieuDe = $("#TieuDe").val();
        self.FormDangKy.NoiDung = $("#NoiDung").val();
        self.FormDangKy.NgayHetHan = $.datepicker.formatDate("yy-mm-dd", $("#NgayHetHan").datepicker("getDate"));
    }

    self.SetValue = function () {
        $("#TieuDe").val(self.FormDangKy.TieuDe);
        $("#NoiDung").val(self.FormDangKy.NoiDung);
        tinymce.activeEditor.setContent(self.FormDangKy.NoiDung);
        $("#NgayHetHan").val(self.FormDangKy.NgayHetHan);
    }

    self.RenderHtmlByObject = function (view) {
        $("#TieuDe").val(view.TieuDe);
        $("#NoiDung").val(view.NoiDung);
        tinymce.activeEditor.setContent(view.NoiDung);
        $("#NgayHetHan").val(view.NgayHetHan);
    }

    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();

        $(".formatdate").datepicker({
            dateFormat: 'dd/mm/yy'
        });

        $(".modal").on("hidden.bs.modal", function () {
            $(this).find('form').trigger('reset');
            $("form").validate().resetForm();
            $("label.error").hide();
            $(".error").removeClass("error");
        });

        $(".btn-addorupdate").click(function () {
            //$(".custom-format").removeAttr("disabled");
            $("#titleModal").text("Cập nhật form đăng ký");
            //$(".txtPassword").show();
            $(".btn-submit-format").text("Thêm mới");
            self.IsUpdate = false;
            $('#AddOrUpdateModal').modal('show');
        })

        $('#select-right').on('change', function () {
            $('input.form-search').val("");
            /*self.FormDangKy.name = null;*/
            self.Search.loaiDV = $(this).val();
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

    })
})(jQuery);