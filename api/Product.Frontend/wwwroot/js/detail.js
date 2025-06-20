// Initialize Swiper
const swiper = new Swiper('.swiper', {
    pagination: {
        el: ".swiper-pagination",
        type: "fraction",
    },
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
   
});



// Optional: Add click handler
document.querySelectorAll('.swiper-slide img').forEach(img => {
    img.addEventListener('click', () => {
        document.getElementById("zoom_01").src = img.src;
    });
});
document.getElementById("increase").addEventListener("click", () => {
    let quantityInput = document.getElementById("quantity");
    quantityInput.value = parseInt(quantityInput.value) + 1;
});

document.getElementById("decrease").addEventListener("click", () => {
    let quantityInput = document.getElementById("quantity");
    if (parseInt(quantityInput.value) > parseInt(quantityInput.min)) {
        quantityInput.value = parseInt(quantityInput.value) - 1;
    }
});


$(document).ready(function () {
    function updateCartCount() {
        let cart = Cookies.get("cart") ? JSON.parse(Cookies.get("cart")) : [];
        let totalQuantity = cart.reduce((sum, item) => sum + item.Quantity, 0);
        $("#cart-count").text(totalQuantity);
    }

    function addToCart(product, redirect = false) {
        let cart = Cookies.get("cart") ? JSON.parse(Cookies.get("cart")) : [];

        // Kiểm tra nếu product thiếu ProductId
        if (!product.ProductId) {
            console.error("Lỗi: Sản phẩm bị thiếu ProductId", product);
            return;
        }

        let existingProductIndex = cart.findIndex(item => item.ProductId === product.ProductId && item.AttributeId === product.AttributeId);

        if (existingProductIndex !== -1) {
            cart[existingProductIndex].Quantity += product.Quantity;
            toastr.success("Sản phẩm đã được cập nhật trong giỏ hàng!", "Cập nhật thành công");
        } else {
            cart.push(product);
            toastr.success("Sản phẩm đã được thêm vào giỏ hàng!", "Thêm thành công");
        }

        Cookies.set("cart", JSON.stringify(cart), { expires: 7, path: '/' });
        updateCartCount();

        if (redirect) {
            window.location.href = "/gio-hang";
        }
    }

    $(".add-to-cart").click(function () {
        let product = {
            ProductId: $(this).data("id"),
            AttributeId: 0,
            Quantity: parseInt($('#quantity').val(), 10),
        };

        document.querySelectorAll("input[name='option1']").forEach(input => {
            if (input.checked) {
                product.AttributeId = input.getAttribute("data-id");
            }
        });

        addToCart(product);
    });

    $(".addnow").click(function () {
        let product = {
            ProductId: $(this).data("id"),
            AttributeId: 0,
            Quantity: parseInt($('#quantity').val(), 10),
        };

        document.querySelectorAll("input[name='option1']").forEach(input => {
            if (input.checked) {
                product.AttributeId = input.getAttribute("data-id");
            }
        });

        addToCart(product, true);
    });

    updateCartCount();
});