var Cart = function () {
    var self = this;
    self.convertToKoObject = function (data) {
        var newObj = ko.mapping.fromJS(data);
        newObj.Selected = ko.observable(false);
        return newObj;
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
            $.each(carts, function (ex, i) {
                i.Detail =""
                $.each(result.Items, function (ex, j) {
                    if (Number(j.Id) == Number(i.ProductId)) {
                        j.Image =""
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
                self.data.push(self.convertToKoObject(i))
            })
         
            self.modifyMoney()

        })
    }
    self.modifyMoney = function () {
        // Lấy tất cả phần tử có class "money"
        const moneyElements = document.querySelectorAll('.product-price');

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
    self.removeItem = function (item) {
        self.data.remove(item)
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

    self.addPayment = function () {
        self.addToCart();
        window.location.href = "/thanh-toan"
    }


}
jQuery(document).ready(function ($) {


    var viewModel = new Cart();
    viewModel.getCart();
    ko.applyBindings(viewModel);
});

