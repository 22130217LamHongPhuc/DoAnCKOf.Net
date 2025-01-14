let orderDetail = null;

var notyf = new Notyf();
let addEle = 'Thêm';
let updateEle = 'Cập nhật';
let delEle = 'Xoá';

let vid;

let setActive = [];

let action;

function isNullOrEmptyOrWhitespace(str) {
    return str === null || str === undefined || /^\s*$/.test(str) || str === "";
}

var table = new DataTable('#orders', {
    ajax: {
        url: '/Admin/GetOrder',
        dataSrc: 'orders'
    },
    fixedHeader: true,
    layout: {
        topStart: {
            buttons: [
                'colvis',
                {
                    text: 'Trạng thái',
                    className: 'green',
                    action: function (e, dt, node, config) {
                        var data = dt.rows({ selected: true }).data();
                        setActive = [];
                        data.each(function(rowData, index) {
                            setActive.push(rowData.orderId);
                        });
                        action = delEle;
                        MicroModal.show('update_status_modal');
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
            data: 'userId',
            className: 'dt-center'
        },
        {
            targets: 1,
            data: 'fullname',
            className: 'dt-center'
        },
        {
            targets: 2, // Cột giá
            data: 'deliveredDate',
            // render: function (data) {
            //     return `<td style="color: red;">${data.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' })}</td>`;
            // },
            className: 'dt-center  dt-nowrap'
        },
        {
            targets: 3, // Cột số lượng
            data: 'address',
            // render: function (data) {
            //     const style = data === 0 ? 'background-color: #ffe0e0;' : '';
            //     return `<td style="${style}">${data}</td>`;
            // },
            className: 'dt-center'
        },
        {
            targets: 4, // Cột đánh giá
            data: 'orderStatusId',
            render: function (data) {
                if(data === 1) {
                    return `<td> <div class="isActive"> Đang xử lý </div> </td>`;
                } else if (data === 2) return `<td> <div class="isActive"> Đang vận chuyển </div> </td>`;
                else if (data === 3) return `<td> <div class="isActive"> Đã giao </div> </td>`;
                else if (data === 4) return `<td> <div class="deActive"> Đã huỷ </div> </td>`;
            },
            className: 'dt-center'
        },
        {
            target: 5,
            data: 'totalPrice',
            // render: function (data) {
            //     return`<div class='table-icon-label'>  <a href="#" onclick="openUpdateProductModal(${data})"><i class="fa-solid fa-note-sticky"></i></a><a href="#" onclick="openSpecModal(${data})"><i class="fa-solid fa-eraser"></i></a> <a href="#" onclick="openColorModal(${data})"><i class="fa-solid fa-eraser"></i></a> </div>`
            // },
            render: function (data) {
                return `<td style="color: red;">${data.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' })}</td>`;
            },
            className: 'dt-center'
        },
        {
            target: 6,
            data: 'fee',
            render: function (data) {
                return `<td style="color: red;">${data.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' })}</td>`;
            },
            className: 'dt-center'
        },
        {
            target: 7,
            data: 'discount',
            // render: function (data) {
            //     return`<div class='table-icon-label'>  <a href="#" onclick="openUpdateProductModal(${data})"><i class="fa-solid fa-note-sticky"></i></a><a href="#" onclick="openSpecModal(${data})"><i class="fa-solid fa-eraser"></i></a> <a href="#" onclick="openColorModal(${data})"><i class="fa-solid fa-eraser"></i></a> </div>`
            // },
            render: function (data) {
                return `<td style="color: red;">${data.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' })}</td>`;
            },
            className: 'dt-center'
        },
        {
            target: 8,
            data: 'orderId',
            render: function (data) {
                return`<div class='table-icon-label'>  <a href="#" onclick="openOrderDetailModal(${data})"><i class="fa-solid fa-note-sticky"></i></a> </div>`
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

table.on('select deselect', function () {
    var selectedRows = table.rows({ selected: true }).count();

    table.button(1).enable(selectedRows > 0);
    // table.button(3).enable(selectedRows > 0);
});


function openOrderDetailModal(orderID) {

    if(orderDetail == null) {
        orderDetail = $('#orderdetail').DataTable();
    }
        orderDetail.clear().draw();
        addOrder(orderID);
    

    MicroModal.show('show_detail');
}


$("#update_id_btn").on('click', function () {
    const active = $("#update_active_id").val();
    if((isNullOrEmptyOrWhitespace(active))) {
        notyf.error("Cần nhập đủ thông tin");
    } else{
        $.ajax({
            url: '/Admin/UpdateOrder',
            type: 'GEt',
            data: {
                active: active,
                action: action,
                id: JSON.stringify(setActive)
            },
            success: function (data) {
                if(data.success) {
                    notyf.success(action + " thành công");
                    MicroModal.close('update_status_modal');
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

$('table th').resizable({
    handles: 'e',
    stop: function(e, ui) {
        $(this).width(ui.size.width);
    }
});

function addOrder(orderID) {
    $.ajax({
        url: '/Admin/GetOrderDetail',
        type: 'GET',
        data: {
            orderId: orderID
        },
        success: function (data) {
            console.log(data)
            data.orderdetail.forEach(function (product) {
                
                product.orderDetails.forEach(function (orderDetailTable) {
                    orderDetail.row.add([
                        product.productId,
                        product.name,
                        product.price,
                        orderDetailTable.orderId,
                        orderDetailTable.quantity,
                        orderDetailTable.totalPrice
                    ]).draw();

                    console.log(orderDetailTable);
                });
            });
        },
        error: function (xhr, status, error) {
            console.error("Error:", error);
        }
    });
}