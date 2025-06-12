$('#zoom_01').elevateZoom({
    zoomType: "lens",
    lensShape: "round",
    lensSize: 200
});
const swiper = new Swiper('.swiper', {
    direction: 'vertical',
    slidesPerView: 4,
    mousewheel: true,
});

// Optional: Add click handler
document.querySelectorAll('.swiper-slide img').forEach(img => {
    img.addEventListener('click', () => {
        alert('Image clicked: ' + img.alt);
        // You can also open it in a lightbox or zoom it here
    });
});