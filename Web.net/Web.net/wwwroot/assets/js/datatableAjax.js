var notyf = new Notyf();
let addEle = 'Thêm';
let updateEle = 'Cập nhật';
let delEle = 'Xoá';

let vid;

let setActive = [];
let action;

let pid;
let specId;
let colorRowId;

let linkDes;

var editor = CKEDITOR.replace('des', {
    allowedContent: true,
    entities: false, // Tắt mã hóa ký tự đặc biệt
    basicEntities: false, // Tắt mã hóa ký tự cơ bản (như &)
    extraAllowedContent: '*[*]{*}(*);', // Cho phép mọi thẻ và thuộc tính
    disallowedContent: '', // Không loại bỏ bất kỳ nội dung nào
});

function isNullOrEmptyOrWhitespace(str) {
    return str === null || str === undefined || /^\s*$/.test(str) || str === "";
}

function openModal(modalSelector, inputSelector, bind) {
    MicroModal.show(modalSelector);
    $("#" + inputSelector).val(bind);
}

var specTable = null;
var colorTable = null;

var table = new DataTable('#example', {
    ajax: {
        url: '/Admin/GetProduct',
        dataSrc: function (e) {

            return e.products;
        }
    },
    fixedHeader: true,
    layout: {
        topStart: {
            buttons: [
                'colvis',
            ]
        },
    },
    select: true,
    columnDefs: [
        {
            targets: 0, // Cột hình ảnh
            data: 'thumbnail',
            render: function (data) {
                return `<td style="background-color: #f5f5f5;">
                            <img src="${data}" style="width:50px; height:auto;">
                        </td>`;
            },
            className: 'dt-center'
        },
        {
            targets: 1,
            data: 'name',
            className: 'dt-center'
        },
        {
            targets: 2, // Cột giá
            data: 'price',
            render: function (data) {
                return `<td style="color: red;">${data.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' })}</td>`;
            },
            className: 'dt-center  dt-nowrap'
        },
        {
            targets: 3, // Cột số lượng
            data: 'quanlityStock',
            render: function (data) {
                const style = data === 0 ? 'background-color: #ffe0e0;' : '';
                return `<td style="${style}">${data}</td>`;
            },
            className: 'dt-center'
        },
        {
            targets: 4, // Cột đánh giá
            data: 'subcategoryId',
            //render: function (data) {
            //    if(data) {
            //        return `<td> <div class="isActive"> Còn hàng </div> </td>`
            //    } else return `<td> <div class="deActive"> Hết hàng </div> </td>`
            //},
            className: 'dt-center'
        },
        {
            target: 5,
            data: 'productId',
            render: function (data) {
                return`<div class='table-icon-label'>  <a href="#" onclick="openUpdateProductModal('${data.toString()}')"><i class="fa-solid fa-note-sticky"></i></a><a href="#" onclick="openSpecModal('${data.toString()}')"><i class="fa-solid fa-eraser"></i></a> <a href="#" onclick="openColorModal('${data.toString()}')"><i class="fa-solid fa-eraser"></i></a> </div>`
            },
            className: 'dt-center'
        }
    ],
    'autoWidth': false,
    language: {
        lengthMenu: "Hiển thị _MENU_ mục mỗi trang",
        zeroRecords: "Không tìm thấy kết quả",
        info: "Hiển thị trang _PAGE_ của _PAGES_",
        infoEmpty: "Không có dữ liệu",
        infoFiltered: "(lọc từ _MAX_ tổng mục)"
    },

    createdRow: function (row, data, dataIndex) {
        // Thêm lớp CSS để căn giữa các ô
        $('td', row).css({
            'text-align': 'center',
            'vertical-align': 'middle'
        });
    }
});


$('table th').resizable({
    handles: 'e',
    stop: function(e, ui) {
        $(this).width(ui.size.width);
    }
});

table.on('select deselect', function () {
    var selectedRows = table.rows({ selected: true }).count();

    // table.button(1).enable(selectedRows === 1);
    // table.button(2).enable(selectedRows > 0);

    table.button(1).enable(selectedRows === 1);
    table.button(2).enable(selectedRows > 0);
});

$("#update_id_btn").on('click', function () {
    const active = $("#update_active_id").val();
    if((isNullOrEmptyOrWhitespace(active))) {
        notyf.error("Cần nhập đủ thông tin");
    } else{
        $.ajax({
            url: '/admin/get-product',
            type: 'POST',
            data: {
                active: active,
                action: action,
                id: JSON.stringify(setActive)
            },
            success: function (data) {
                if(data.success) {
                    notyf.success(action + " thành công");
                    MicroModal.close('update_id_modal');
                    table.ajax.reload();
                    table.rows().deselect();
                } else {
                    notyf.error(action + " thất bại");
                    table.rows().deselect();
                }
            },
            error: function () {
                notyf.error(action + " thất bại");
            }
        })
    }
});

function openUpdateProductModal(id) {
    let rowIndex = table.rows().data().toArray().findIndex(function (data) {
        return data.productId === id;
    });
    if (rowIndex !== -1) {
        MicroModal.show('update_product_modal');
        let row = table.row(rowIndex);
        let rowData = row.data();
        vid = rowData.productId;
        $("#productname").val(rowData.name);
        $("#productquantity").val(rowData.quanlityStock);
        $("#productprice").val(rowData.price);
        $("#sub_category").val(rowData.subcategoryId);
        $("#sell").val(rowData.quanlitySell);
        $("#price_promotion").val(rowData.discountDefault === 0 ? "" : rowData.discountDefault) ;
        $("#productimg").val(rowData.thumbnail);
        editor.setData(rowData.description);
    } else {
        notyf.error('Không tìm thấy dòng có id:', id);
    }
}

$("#update_product_btn").on('click', function () {
    action = updateEle;
    var p = {
        ProductId: vid,
        Name: $("#productname").val(),
        Price: $("#productprice").val(),
        DiscountDefault: $("#price_promotion").val(),
        SubcategoryId: $("#sub_category").val(),
        QuanlityStock: $("#productquantity").val(),
        QuanlitySell: $("#sell").val(),
        Description: editor.getData(),
        Thumbnail: $("#productimg").val()
    }
    //if (isNullOrEmptyOrWhitespace(p.ProductId) || isNullOrEmptyOrWhitespace(p.Name) || isNullOrEmptyOrWhitespace(p.Price)
    //    || isNullOrEmptyOrWhitespace(p.DiscountDefault) || isNullOrEmptyOrWhitespace(p.SubcategoryId) || isNullOrEmptyOrWhitespace(p.quanlityStock)
    //    || isNullOrEmptyOrWhitespace(p.quanlitySell) || isNullOrEmptyOrWhitespace(p.Description) || isNullOrEmptyOrWhitespace(p.Thumbnail)) {
    //    notyf.error("Cần nhập đủ thông tin");
    //} else{
        $.ajax({
            url: '/Admin/UpdateProduct',
            type: 'POST',
            data: {
                data: JSON.stringify({
                    ProductId: vid,
                    Name: $("#productname").val(),
                    Price: $("#productprice").val(),
                    DiscountDefault: $("#price_promotion").val(),
                    SubcategoryId: $("#sub_category").val(),
                    QuanlityStock: $("#productquantity").val(),
                    QuanlitySell: $("#sell").val(),
                    Description: editor.getData(),
                    Thumbnail: $("#productimg").val()
                })
            },
            success: function (data) {
                if(data.success) {
                    notyf.success(action + " thành công");
                    MicroModal.close('update_product_modal');
                    table.ajax.reload();
                    table.rows().deselect();
                } else {
                    notyf.error(action + " thất bại");
                    table.rows().deselect();
                }
            },
            error: function () {
                notyf.error(action + " thất bại");
            }
        });
    //}
});

$("#update_spec_btn").on('click', function () {
   const dimension = $("#update_dimension_val").val();
   const material = $("#update_material_val").val();
   const original= $("#update_original_val").val();
    const standard = $("#update_standard_val").val();
    if ((isNullOrEmptyOrWhitespace(dimension) || isNullOrEmptyOrWhitespace(material) || isNullOrEmptyOrWhitespace(original) || isNullOrEmptyOrWhitespace(standard))) {
        notyf.error("Cần nhập đủ thông tin");
    } else {
        $.ajax({
            url: '/Admin/UpdateSpec',
            type: 'POST',
            data: {
                data: JSON.stringify({
                   SpecificationId : pid,
                   Dimensions: $("#update_dimension_val").val(),
                    Material: $("#update_material_val").val(),
                    Original: $("#update_original_val").val(),
                    Standard: $("#update_standard_val").val()
                })
            },
            success: function (data) {
                if(data.success) {
                    notyf.success("thành công");
                    MicroModal.close('update_spec_modal');
                    table.rows().deselect();
                } else {
                    notyf.error("thất bại");
                }
            },
            error: function () {
                notyf.error("thất bại");
            }
        });
    }

});

function deleteSpec(specIdToDel) {
    $.ajax({
        url: '/admin/getspec',
        type: 'POST',
        data: {
            action: action,
            pid: pid,
            specIdToDel: JSON.stringify(specIdToDel),
        },
        success: function (data) {
            if(data.success) {
                notyf.success(action + " thành công");
                specTable.ajax.reload();
                specTable.rows().deselect();

            } else {
                notyf.error(action + " thất bại");
                specTable.rows().deselect();
            }
            $("#add_spec_type").empty();
        },
        error: function () {
            notyf.error(action + " thất bại");
        }
    });
}

function openSpecModal(productId) {
    pid = productId;

    $.ajax({
        url: '/Admin/GetProductSpec',
        type: 'POST',
        data: {
            specId: pid
        },
        success: function (data) {
            console.log(data);
            if (data.success) {
                $("#update_dimension_val").val(data.spec.dimensions);
                $("#update_material_val").val(data.spec.material);
                $("#update_original_val").val(data.spec.original);
                $("#update_standard_val").val(data.spec.standard);
                MicroModal.show('update_spec_modal');
             
            } else {
                notyf.error("Lấy thông tin thất bại");
            }
        },
        error: function () {
            notyf.error("Lấy thông tin thất bại");
        }
    });
   
    
    
}

function openColorModal(productForColorId) {
    pid = productForColorId;
    $("#add_color_type").empty();
    if (colorTable === null) {
        colorTable = new DataTable('#color', {
            ajax: {
                url: '/admin/GetImg',
                data: {
                    pid: productForColorId
                },
                dataSrc: function (json) {
                    console.log(json.subImg);
                    return json.subImg;
                }
            },
            fixedHeader: true,
            layout: {
                topStart: {
                    buttons: [
                        'colvis',
                        {
                            text: 'Thêm',
                            className: 'green',
                            action: function (e, dt, node, config) {
                                let data = dt.row({ selected: true }).data();
                                action = addEle;

                                    MicroModal.close('update_color_modal');
                                    MicroModal.show('add_color', {
                                        onClose: function() {
                                            openColorModal(pid);
                                        }
                                    });
                            },
                            enabled: true
                        },
                        {
                            text: 'Xoá',
                            className: 'red',
                            action: function (e, dt, node, config) {
                                var data = dt.rows({ selected: true }).data();
                                var colorRowIdToDel = [];
                                data.each(function(rowData, index) {
                                    colorRowIdToDel.push(rowData.subImagesId);
                                });
                                action = delEle;
                                deleteColor(colorRowIdToDel);
                            },
                            enabled: false
                        }
                    ]
                },
            },
            select: true,
            columnDefs: [
                {
                    targets: 0,
                    data: 'productId',
                    className: 'dt-center'
                },
                {
                    targets: 1,
                    data: 'image',
                    render: function (data) {
                        return `<td style="background-color: #f5f5f5;">
                            <img src="${data}" style="width:50px; height:auto;">
                        </td>`;
                    },
                    className: 'dt-center'
                },
            ],
            'autoWidth': false,
            language: {
                lengthMenu: "Hiển thị _MENU_ mục mỗi trang",
                zeroRecords: "Không tìm thấy kết quả",
                info: "Hiển thị trang _PAGE_ của _PAGES_",
                infoEmpty: "Không có dữ liệu",
                infoFiltered: "(lọc từ _MAX_ tổng mục)"
            },

            createdRow: function (row, data, dataIndex) {
                // Thêm lớp CSS để căn giữa các ô
                $('td', row).css({
                    'text-align': 'center',
                    'vertical-align': 'middle'
                });
            },
            paging: false,
            searching: false,
        });

        colorTable.on('select deselect', function () {
            var selectedRows = colorTable.rows({ selected: true }).count();

            //colorTable.button(1).enable(selectedRows === 1);
            colorTable.button(2).enable(selectedRows > 0);
        });
    } else {
        colorTable.settings()[0].ajax.data = function (d) {
            d.pid = productForColorId;
        };
        colorTable.ajax.reload();
    }
    MicroModal.show('update_color_modal');
}

$("#add_color_btn").on('click', function () {
    const value =  $("#add_color_val").val();
    if((isNullOrEmptyOrWhitespace(value))) {
        notyf.error("Cần nhập đủ thông tin");
    } else {
        $.ajax({
            url: '/admin/AddImg',
            type: 'POST',
            data: {
                data: JSON.stringify({
                    SubImagesId: 0,
                    Image: value,
                   ProductId : pid
                   
                  
                })
            },
            success: function (data) {
                if(data.success) {
                    notyf.success(action + " thành công");
                    MicroModal.close('add_color');
                    colorTable.ajax.reload();
                    colorTable.rows().deselect();
                    $("#add_color_val").val("");
                } else {
                    notyf.error(action + " thất bại");
                    colorTable.rows().deselect();
                }
            },
            error: function () {
                notyf.error(action + " thất bại");
            }
        });
    }

});

$("#update_color_btn").on('click', function () {
    const value =  $("#update_color_val").val();
    if((isNullOrEmptyOrWhitespace(value))) {
        notyf.error("Cần nhập đủ thông tin");
    } else {
        $.ajax({
            url: '/admin/productColorImg',
            type: 'POST',
            data: {
                action: action,
                value: value,
                colorRowId: colorRowId,
                vid: vid
            },
            success: function (data) {
                if(data.success) {
                    notyf.success(action + " thành công");
                    MicroModal.close('update_color');
                    colorTable.ajax.reload();
                    colorTable.rows().deselect();
                    $("#update_color_val").val("");
                } else {
                    notyf.error(action + " thất bại");
                    colorTable.rows().deselect();
                }
            },
            error: function () {
                notyf.error(action + " thất bại");
            }
        });
    }

});

function deleteColor(colorIdToDel) {
    $.ajax({
        url: '/Admin/DelImg',
        type: 'POST',
        data: JSON.stringify(colorIdToDel),
        
        success: function (data) {
            if(data.success) {
                notyf.success(action + " thành công");
                colorTable.ajax.reload();
                colorTable.rows().deselect();
                $("#add_color_type").empty();

            } else {
                notyf.error(action + " thất bại");
                colorTable.rows().deselect();
                $("#add_color_type").empty();
            }
        },
        error: function () {
            notyf.error(action + " thất bại");
        }
    });
}

function loadFileCKEditor(linkDescription, productId) {
    $.ajax({
        url: '/admin/des',
        type: 'GET',
        data: {
            pid: pid,
            linkDes: linkDescription
        },
        dataType: 'text',
        success: function (data) {
            console.log(data);
            editor.setData(data);
            MicroModal.show('update_des');

        },
        error: function () {
            alert('Không thể tải danh sách sản phẩm.');
        }
    });
}

$("#update_des_btn").on('click', function () {
    const value =  editor.getData();
    if((isNullOrEmptyOrWhitespace(value))) {
        notyf.error("Cần nhập đủ thông tin");
    } else {
        var decodedContent = $('<div/>').html(value).text();
        $.ajax({
            url: '/admin/des',
            type: 'POST',
            data: {
                action: action,
                value: decodedContent,
                linkDes: linkDes
            },
            success: function (data) {
                if(data.success) {
                    notyf.success(action + " thành công");
                    MicroModal.close('update_des');
                    table.ajax.reload();
                    table.rows().deselect();
                } else {
                    notyf.error(action + " thất bại");
                    table.rows().deselect();
                }
            },
            error: function () {
                notyf.error(action + " thất bại");
            }
        });
    }

});

