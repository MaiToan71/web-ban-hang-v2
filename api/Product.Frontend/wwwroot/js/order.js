let list = []
const provinces = [
    { code: "01", name: "Hà Nội" },
    { code: "79", name: "Hồ Chí Minh" },
    { code: "48", name: "Đà Nẵng" },
    { code: "89", name: "An Giang" },
    { code: "77", name: "Bà Rịa - Vũng Tàu" },
    { code: "74", name: "Bình Dương" },
    { code: "70", name: "Bình Phước" },
    { code: "60", name: "Bình Thuận" },
    { code: "52", name: "Bình Định" },
    { code: "95", name: "Bạc Liêu" },
    { code: "24", name: "Bắc Giang" },
    { code: "06", name: "Bắc Kạn" },
    { code: "27", name: "Bắc Ninh" },
    { code: "83", name: "Bến Tre" },
    { code: "04", name: "Cao Bằng" },
    { code: "96", name: "Cà Mau" },
    { code: "92", name: "Cần Thơ" },
    { code: "64", name: "Gia Lai" },
    { code: "02", name: "Hà Giang" },
    { code: "35", name: "Hà Nam" },
    { code: "42", name: "Hà Tĩnh" },
    { code: "30", name: "Hải Dương" },
    { code: "31", name: "Hải Phòng" },
    { code: "93", name: "Hậu Giang" },
    { code: "56", name: "Khánh Hòa" },
    { code: "91", name: "Kiên Giang" },
    { code: "62", name: "Kon Tum" },
    { code: "12", name: "Lai Châu" },
    { code: "20", name: "Lạng Sơn" },
    { code: "10", name: "Lào Cai" },
    { code: "68", name: "Lâm Đồng" },
    { code: "80", name: "Long An" },
    { code: "36", name: "Nam Định" },
    { code: "40", name: "Nghệ An" },
    { code: "37", name: "Ninh Bình" },
    { code: "58", name: "Ninh Thuận" },
    { code: "25", name: "Phú Thọ" },
    { code: "54", name: "Phú Yên" },
    { code: "44", name: "Quảng Bình" },
    { code: "49", name: "Quảng Nam" },
    { code: "51", name: "Quảng Ngãi" },
    { code: "22", name: "Quảng Ninh" },
    { code: "45", name: "Quảng Trị" },
    { code: "94", name: "Sóc Trăng" },
    { code: "17", name: "Hòa Bình" },
    { code: "75", name: "Đồng Nai" },
    { code: "87", name: "Đồng Tháp" }
];



var Cart = function () {
    var self = this;
    self.convertToKoObject = function (data) {
        var newObj = ko.mapping.fromJS(data);
        newObj.Selected = ko.observable(false);
        return newObj;


    }

    self.getDefault = function () {
        let provinceSelect = $("select[name='customer_shipping_province']");
        // Xóa option cũ
        provinceSelect.empty();
        provinceSelect.append('<option value="">Chọn tỉnh / thành</option>');

        // Thêm tỉnh từ mảng provinces
        provinces.forEach(province => {
            provinceSelect.append(`<option value="${province.code}">${province.name}</option>`);
        });
        let options = '<option value="">Chọn tỉnh / thành</option>';
        provinces.forEach(province => {
            options += `<option value="${province.code}">${province.name}</option>`;
        });
    
        console.log(provinceSelect.length)
        // Khi thay đổi tỉnh, tải danh sách huyện
        $('#provinces').change(function () {
            let provinceCode = $(this).val(); // Lấy giá trị được chọn

            if (provinceCode) { // This automatically checks for "" and null
             
                loadDistricts(provinceCode); // Calls function only if provinceCode exists
            }
        });


        function loadDistricts(provinceCode) {
            $.ajax({
                url: `https://vn-public-apis.fpo.vn/districts/getByProvince?provinceCode=${provinceCode}&limit=1000`,
                type: "GET",
                success: function (data) {
                    let districts = data.data.data;
                    let districtSelect = $("select[name='customer_shipping_district']");

                    districtSelect.empty();
                    districtSelect.append('<option value="">Chọn quận / huyện</option>');

                    districts.forEach(district => {
                        districtSelect.append(`<option value="${district.code}">${district.name}</option>`);
                    });

                    console.log("Danh sách huyện đã hiển thị!");
                },
                error: function (xhr) {
                    console.error(`Lỗi ${xhr.status}: Không thể tải danh sách huyện!`);
                }
            });
        }
    }

    self.data = ko.observableArray([])
    self.total = ko.observable(0)
    self.discount = ko.observable(0)
    self.getCart = function () {
        let carts = $.cookie("cart") ? JSON.parse($.cookie("cart")) : [];


        var input = {
            "PageIndex": 1,
            "PageSize": 100,
            "PostTypeEnum": 2,
            "Keyword": "",
            ProductAttributes: carts
        }
        $.ajax({
            url: $("#domain").val() + '/api/client/Products/search',
            type: 'post',
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(input),
        }).done(function (result) {
            self.data([])
            self.total(0)
            let totalItem = 0;
            list = []
            $.each(carts, function (ex, i) {

                $.each(result.Items, function (ex, j) {
                    if (j.Id == i.ProductId) {
                        if (j.Images.length > 0) {
                            j.Image = $("#domain").val() + j.Images[0].ImagePath
                        }
                        j.LastSellingPrice = Number(j.SellingPrice) - Number(j.SellingPrice) * (Number(j.Discount) / 100)
                        let total = Number(i.Quantity) * (j.LastSellingPrice);
                        i.Total = total
                        i.Detail = j
                        totalItem += total
                    }
                })
                self.total(totalItem)
                list.push(i)
                self.data.push(self.convertToKoObject(i))
            })
           
            self.modifyMoney()
            self.submit();




        })
    }

    self.submit = function () {
    
        $("#myForm").validate({
            rules: {
                full_name: "required",
                email: {
                    required: true,
                    email: true
                },
                address: "required",
                phone: {
                    required: true,
                    minlength: 10,
                    maxlength: 11,
                    digits: true // Chỉ cho phép số
                },
                customer_shipping_province: {
                    required: true


                },
                customer_shipping_district: {
                    required: true
                }

            },
            messages: {
                full_name: "Vui lòng nhập họ và tên",
                email: {
                    required: "Vui lòng nhập email",
                    email: "Vui lòng nhập địa chỉ email hợp lệ"
                },
                address: "Vui lòng nhập địa chỉ",
                phone: {
                    required: "Vui lòng nhập số điện thoại",
                    minlength: "Số điện thoại phải có ít nhất 10 chữ số",
                    maxlength: "Số điện thoại không quá 11 chữ số",
                    digits: "Chỉ được nhập số"
                },
                customer_shipping_province: "Vui lòng chọn tỉnh/thành",
                customer_shipping_district: "Vui lòng chọn quận/huyện"

            },
            submitHandler: function (form) {
                let formData = {};
                // Duyệt qua tất cả input theo name
                $(form).serializeArray().forEach(field => {
                    formData[field.name] = field.value;
                });
                // Lấy tên của tỉnh/thành phố và quận/huyện
                formData.customer_shipping_province_name = $("select[name='customer_shipping_province'] option:selected").text();
                formData.customer_shipping_district_name = $("select[name='customer_shipping_district'] option:selected").text();
                let today = new Date();
                let formattedDate = today.toISOString().split('T')[0]; // Extract YYYY-MM-DD
                let input = {
                    "Code": "string",
                    "DateOrder": formattedDate,
                    "DateDelivery": formattedDate,
                    "UnitDelivery": "string",
                    "PaymentMethod": 0,
                    "Note": formData.address,
                    "ProvinceCode": formData.customer_shipping_province_name,
                    "Province": formData.customer_shipping_province_name,
                    "DistrictCode": formData.customer_shipping_province_name,
                    "District": formData.customer_shipping_district_name,
                    "FullName": formData.full_name,
                    "Phone": formData.phone,
                    "OrderType": 1,
                    "Workflow": 1,
                    "PostTypeId": 0,
                    "DeloveryMan": "string",
                    "DeloveryManPhonenumber": "string",
                    "OrderDetails": [

                    ]
                }

                $.each(self.data(), function (ex, i) {
                    let detail = {
                        "OrderId": 0,
                        "ProductId": i.Detail.Id(),
                        "CapitalPrice": i.Detail.SellingPrice(),
                        "SellingPrice": i.Detail.LastSellingPrice(),
                        "Quantity": i.Quantity(),
                        "Discount": i.Detail.Discount(),
                        "Fee": 0
                    }
                    //console.log(detail)
                    input.OrderDetails.push(detail)
                })
                console.log(input)
                $.ajax({
                    url: $("#domain").val() + '/api/client/Order/add',
                    type: 'post',
                    contentType: 'application/json',
                    dataType: 'json',
                    data: JSON.stringify(input),
                }).done(function (result) {
                    if (result.Status == true) {
                        Cookies.remove("cart", { path: '/' });
                        console.log("Đã xóa cookie giỏ hàng!")
                        toastr.success("Đã gửi thông tin tới cửa hàng!", "Cập nhật thành công");
                       window.location.href = "/"
                    }
                })
                //  form.submit();
            }
        });
    }
    self.modifyMoney = function () {
        // Lấy tất cả phần tử có class "money"
        const moneyElements = document.querySelectorAll('.money');

        moneyElements.forEach(el => {
            const rawValue = parseFloat(el.textContent.replace(/,/g, '')); // Bỏ dấu phẩy nếu có
            if (!isNaN(rawValue)) {
                el.textContent = formatMoneyVND(rawValue);
            }
        });
    }
    self.add = function (item) {
        item.Quantity(item.Quantity() + 1)
        item.Total(item.Detail.LastSellingPrice() * item.Quantity())

    }

    self.sub = function (item) {
        if (item.Quantity() > 0) {
            item.Quantity(item.Quantity() - 1)
            item.Total(item.Detail.LastSellingPrice() * item.Quantity())

        }
    }

    self.addToCart = function () {
        let newCookies = []
        $.each(self.data(), function (ex, i) {
            let product = {
                ProductId: i.ProductId(),
                AttributeId: i.AttributeId(),
                Quantity: i.Quantity()
            };
            newCookies.push(product)
        })

        Cookies.remove("cart", { path: '/' });
        console.log("Đã xóa cookie giỏ hàng!")



        Cookies.set("cart", JSON.stringify(newCookies), { expires: 7, path: '/' });
        console.log("Đã thêm giỏ hàng mới!");
        toastr.success("Sản phẩm đã được cập nhật trong giỏ hàng!", "Cập nhật thành công");
    }



}
jQuery(document).ready(function ($) {


    var viewModel = new Cart();
    viewModel.getCart();
    viewModel.getDefault()
    ko.applyBindings(viewModel);
});
