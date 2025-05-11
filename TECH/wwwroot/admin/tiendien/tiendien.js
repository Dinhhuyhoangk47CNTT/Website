(function ($) {
    var self = this;
    self.Data = [];
    self.UserImages = {};
    self.IsUpdate = false;  
    self.TienDien = {
        Id: 0,
        MaNha: 0,
        TenNha: "",
        MaPhong: 0,
        TenPhong: "",
        LoaiPhong: 2,
        LoaiPhongStr: "",
        TrangThai: "",
        TrangThaiStr: "",
        SoCongToDienTruoc: 0,
        SoCongToDienTruocStr: "",
        SoCongToDienHienTai: 0,
        SoCongToDienHienTaiStr: "",
        SoTienCanPhaiNop: 0,
        SoTienCanPhaiNopStr: "",
        SoTienDaNop: 0,
        SoTienDaNopStr: "",
        HanNop: null,
        HanNopStr: "",
        NgayNop: null,
        NgayNopStr: "",
        Thang: 0
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
                html += "<td>" + item.TenNha + "</td>";
                html += "<td>" + item.TenPhong + "</td>";
                html += "<td>" + item.LoaiPhongStr + "</td>";
                html += "<td>" + item.SoCongToDienTruocStr + "</td>";
                html += "<td>" + item.SoCongToDienHienTaiStr + "</td>";
                html += "<td>" + item.SoTienCanPhaiNopStr + "</td>";
                html += "<td>" + item.SoTienDaNopStr + "</td>";
                html += "<td>" + item.HanNopStr + "</td>";
                html += "<td>" + item.NgayNopStr + "</td>";
                html += "<td>" + item.Thang + "</td>";
                html += "<td>" + item.TrangThaiStr + "</td>";
                     
                html += "<td style=\"text-align: center;\">" +                    
                    "<button  class=\"btn btn-primary custom-button\" onClick=\"Update(" + item.Id +")\"><i  class=\"bi bi-pencil-square\"></i></button>" +
                    "<button  class=\"btn btn-danger custom-button\" onClick=\"Deleted(" + item.Id +")\"><i  class=\"bi bi-trash\"></i></button>" +
                    "</td>";                
                html += "</tr>";
            }
        }
        else {
            html += "<tr><td colspan=\"13\" style=\"text-align:center\">Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    };
    
    self.Update = function (id) {
        if (id != null && id != "") {
            $(".txtPassword").hide();
            $("#titleModal").text("Cập nhật tiền điện");
            $(".btn-submit-format").text("Cập nhật");
            /*$(".custom-format").attr("disabled", "disabled");*/
            self.GetById(id, self.RenderHtmlByObject);
            self.TienDien.Id = id;
            $('#AddOrUpdateModal').modal('show');

            self.IsUpdate = true;
        }
    }

    self.GetById = function (id, renderCallBack) {
        if (id != null && id != "") {
            $.ajax({
                url: '/Admin/TienDien/GetById',
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
                        self.GetPhongByNhaUpdate(response.Data.MaNha, response.Data.MaPhong);
                        setTimeout(function () {
                            renderCallBack(response.Data);
                            //self.Id = id;
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
            tedu.confirm('Bạn có chắc muốn xóa nhà này?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/TienDien/Delete",
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
            url: '/Admin/TienDien/GetAllPaging',
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

    self.TinhTienDienTieuThu = function (previousMeter, currentMeter) {
        // Kiểm tra dữ liệu đầu vào
        if (currentMeter < previousMeter || previousMeter < 0 || currentMeter < 0) {
            return "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại số công tơ.";
        }

        // Tính số kWh tiêu thụ
        const consumption = currentMeter - previousMeter;

        // Khai báo các bậc và đơn giá (đồng/kWh)
        const tiers = [
            { max: 50, price: 1728 },    // Bậc 1: 0-50 kWh
            { max: 100, price: 1236 },   // Bậc 2: 51-100 kWh
            { max: 200, price: 2074 },   // Bậc 3: 101-200 kWh
            { max: 300, price: 2612 },   // Bậc 4: 201-300 kWh
            { max: 400, price: 2919 },   // Bậc 5: 301-400 kWh
            { max: Infinity, price: 3015 } // Bậc 6: trên 400 kWh
        ];

        let totalCost = 0;
        let remaining = consumption;
        let prevLimit = 0;

        // Tính tiền theo từng bậc
        for (let tier of tiers) {
            if (remaining <= 0) break;

            // Tính số kWh trong bậc này
            let kWhInTier = Math.min(remaining, tier.max - prevLimit);
            totalCost += kWhInTier * tier.price;
            remaining -= kWhInTier;
            prevLimit = tier.max;
        }

        return totalCost;
    }

    self.Init = function () {

        $(".btn-add").click(function () {
            self.SetValueDefault();
            self.TienDien.Id = 0;
            $('#AddOrUpdateModal').modal('show')
        })

        // hủy add và edit
        $(".cs-close-addedit,.btn-cancel-addedit").click(function () {
            $("#CreateEdit").css("display", "none");
        })

        $('.add-role').click(function () {
            $('#AddRole').modal('show');
        })

        //$('body').on('click', '.btn-delete', function () {
        //    var id = $(this).attr('data-id');
        //    var fullname = $(this).attr('data-fullname');
        //    if (id !== null && id !== '') {
        //        self.confirmUser(fullname, id);
        //    }
        //})
        //$(".add-image").click(function () {
        //    $("#file-input").click();
        //})

        //$('body').on('click', '.btn-role-user', function () {
        //    var id = $(this).attr('data-id');
        //    $("#user_id").val(id);
        //    //self.GetAllRoles(id);           
        //})

        //$('body').on('click', '.btn-set-role', function () {
        //    var userId = parseInt($("#user_id").val());
        //    $.each($("#lst-role tr"), function (key, item) {
        //        var check = $(item).find('.ckRole').prop('checked');
        //        if (check == true) {
        //            var id = parseInt($(item).find('.ckRole').val());
        //            self.lstRole.push({
        //                UserId: userId,
        //                RoleId: id
        //            });
        //        }
        //    })
        //    if (self.lstRole.length > 0) {
        //        self.SaveRoleForUser(self.lstRole, userId);
        //    }

        //})

        //$('.filesImages').on('change', function () {
        //    var fileUpload = $(this).get(0);
        //    var files = fileUpload.files;
        //    if (files != null && files.length > 0) {
        //        var fileExtension = ['jpeg', 'jpg', 'png'];
        //        var html = "";
        //        for (var i = 0; i < files.length; i++) {
        //            if ($.inArray(files[i].type.split('/')[1].toLowerCase(), fileExtension) == -1) {
        //                alert("Only formats are allowed : " + fileExtension.join(', '));
        //            }
        //            else {
        //                var src = URL.createObjectURL(files[i]);
        //                html += "<div class=\"box-item-image\"> <div class=\"image-upload item-image\" style=\"background-image:url(" + src + ")\"></div></div>";
        //            }
        //        }
        //        if (html != "") {
        //            $(".image-default").hide();
        //            $(".box-images").html(html);
        //        }
        //    }

        //});
    }


    self.AddUser = function (userView) {
        $.ajax({
            url: '/Admin/TienDien/Add',
            type: 'POST',
            dataType: 'json',
            data: {
                vm: userView
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
                    tedu.notify('Tên nhà đã tồn tại', 'error');
                }
                //else {
                //    if (response.isPhoneExist) {
                //        $(".phone_number-exist").show().text("Phone đã tồn tại");
                //    }
                //    if (response.isMailExist) {
                //        $(".email-exist").show().text("Email đã tồn tại");
                //    }
                //}
            }
        })
    }

    self.UpdateUser = function (userView) {
        $.ajax({
            url: '/Admin/TienDien/Update',
            type: 'POST',
            dataType: 'json',
            data: {
                vm: userView
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
                    tedu.notify('Tên nhà đã tồn tại', 'error');
                }
               
            }
        })
    }

    self.ValidateUser = function () {                
        $("#form-submit").validate({
            rules:
            {
                MaPhong: {
                    required: true,
                },
            },
            messages:
            {
                MaPhong: {
                    required: "Vui lòng chọn phòng",
                },
            },
            submitHandler: function (form) {   
                self.GetValue();
                if (self.IsUpdate) {
                    self.UpdateUser(self.TienDien);
                }
                else {
                    self.AddUser(self.TienDien);
                }
            }
        });
    }

    self.GetValue = function () {
        self.TienDien.MaNha = $("#MaNha").val();
        self.TienDien.TenNha = $("#TenNha").val();
        self.TienDien.MaPhong = $("#MaPhong").val();
        self.TienDien.TenPhong = $("#TenPhong").val();
        self.TienDien.LoaiPhong = $("#LoaiPhong").val();
        self.TienDien.LoaiPhongStr = $("#LoaiPhong option:selected").text();
        self.TienDien.TrangThai = $("#TrangThai").val();
        self.TienDien.TrangThaiStr = $("#TrangThai option:selected").text();
        self.TienDien.SoCongToDienTruoc = $("#SoCongToDienTruoc").val();
        self.TienDien.SoCongToDienTruocStr = $("#SoCongToDienTruoc").val();
        self.TienDien.SoCongToDienHienTai = $("#SoCongToDienHienTai").val();
        self.TienDien.SoCongToDienHienTaiStr = $("#SoCongToDienHienTai").val();
        self.TienDien.SoTienCanPhaiNop = $("#SoTienCanPhaiNop").val();
        self.TienDien.SoTienCanPhaiNopStr = $("#SoTienCanPhaiNop").val();
        self.TienDien.SoTienDaNop = $("#SoTienDaNop").val();
        self.TienDien.SoTienDaNopStr = $("#SoTienDaNop").val();
        self.TienDien.HanNop = $.datepicker.formatDate("yy-mm-dd", $("#HanNop").datepicker("getDate"));
        self.TienDien.NgayNop = $.datepicker.formatDate("yy-mm-dd", $("#NgayNop").datepicker("getDate"));
        self.TienDien.Thang = $("#Thang").val();
    }

    Set.SetValue = function () {
        $("#MaNha").val(self.TienDien.MaNha);
        $("#TenNha").val(self.TienDien.TenNha);
        $("#MaPhong").val(self.TienDien.MaPhong);
        $("#TenPhong").val(self.TienDien.TenPhong);
        $("#LoaiPhong").val(self.TienDien.LoaiPhong);
        $("#TrangThai").val(self.TienDien.TrangThai);
        $("#SoCongToDienTruoc").val(self.TienDien.SoCongToDienTruoc);
        $("#SoCongToDienHienTai").val(self.TienDien.SoCongToDienHienTai);
        $("#SoTienCanPhaiNop").val(self.TienDien.SoTienCanPhaiNop);
        $("#SoTienDaNop").val(self.TienDien.SoTienDaNop);
        $("#HanNop").val(self.TienDien.HanNop);
        $("#NgayNop").val(self.TienDien.NgayNop);
        $("#Thang").val(self.TienDien.Thang);
    }

    self.RenderHtmlByObject = function (view) {
        $("#MaNha").val(view.MaNha);
        $("#TenNha").val(view.TenNha);
        $("#MaPhong").val(view.MaPhong);
        $("#TenPhong").val(view.TenPhong);
        $("#LoaiPhong").val(view.LoaiPhong);
        $("#TrangThai").val(view.TrangThai);
        $("#SoCongToDienTruoc").val(view.SoCongToDienTruoc);
        $("#SoCongToDienHienTai").val(view.SoCongToDienHienTai);
        $("#SoTienCanPhaiNop").val(view.SoTienCanPhaiNop);
        $("#SoTienDaNop").val(view.SoTienDaNop);
        $("#HanNop").val(view.HanNop);
        $("#NgayNop").val(view.NgayNop);
        $("#Thang").val(view.Thang);
    }

    self.GetAllNha = function () {
        $.ajax({
            url: '/Admin/Nha/GetAll',
            type: 'GET',
            dataType: 'json',
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn nhà</option>";
                if (response.Data != null && response.Data.length > 0) {
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.TenNha + "</option>";
                    }
                }
                $("#MaNha").html(html);
                $("#MaPhong").val($("#MaPhong option:first").val());
            }
        })
    }

    self.GetPhongByNha = function (tag) {
        var maNha = $(tag).val();
        $.ajax({
            url: '/Admin/Phong/GetPhongByMaNha',
            type: 'GET',
            dataType: 'json',
            data: {
                maNha: maNha
            },
            beforeSend: function () {
                /*Loading('show');*/
            },
            complete: function () {
                /*Loading('show');*/
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn phòng</option>";
                if (response.Data != null && response.Data.length > 0) {
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        html += "<option value =" + item.Id + ">" + item.TenPhong + "</option>";
                    }
                }
                $("#MaPhong").html(html);
            }
        })
    }

    self.GetThongTinPhong = function (tag) {
        var maPhong = $(tag).val();
        $.ajax({
            url: '/Admin/Phong/GetById',
            type: 'GET',
            dataType: 'json',
            data: {
                id: maPhong
            },
            beforeSend: function () {
                /*Loading('show');*/
            },
            complete: function () {
                /*Loading('show');*/
            },
            success: function (response) {
                if (response.Data != null) {
                    $("#LoaiPhong").val(response.Data.LoaiPhong);
                }
            }
        })
    }

    self.GetPhongByNhaUpdate = function (maNha, maPhong) {
        $.ajax({
            url: '/Admin/Phong/GetPhongByMaNha',
            type: 'GET',
            dataType: 'json',
            data: {
                maNha: maNha
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            success: function (response) {
                var html = "<option value =\"\">Chọn phòng</option>";
                if (response.Data != null && response.Data.length > 0) {
                    for (var i = 0; i < response.Data.length; i++) {
                        var item = response.Data[i];
                        if (item.Id == maPhong) {
                            html += "<option value =" + item.Id + " selected>" + item.TenPhong + "</option>";
                        }
                        else {
                            html += "<option value =" + item.Id + ">" + item.TenPhong + "</option>";
                        }

                    }
                }
                $("#MaPhong").html(html);

                setTimeout(function () {
                    self.GetThongTinPhong($("#MaPhong"));
                }, 1000);
            }
        })
    }

    $(document).ready(function () {
        self.GetDataPaging();
        self.ValidateUser();    
        self.GetAllNha();

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
            $("#titleModal").text("Cập nhật tiền điện");
            //$(".txtPassword").show();
            $(".btn-submit-format").text("Thêm mới");
            self.IsUpdate = false;
            $('#AddOrUpdateModal').modal('show');
        })
        
        $('#select-right').on('change', function () {
            $('input.form-search').val("");
            /*self.TienDien.name = null;*/
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

        $("#SoCongToDienHienTai").on('input', function () {
            var previousMeter = parseInt($("#SoCongToDienTruoc").val());
            var currentMeter = parseInt($("#SoCongToDienHienTai").val());
            var totalCost = self.TinhTienDienTieuThu(previousMeter, currentMeter);
            $("#SoTienCanPhaiNop").val(totalCost);
        });
       
    })
})(jQuery);