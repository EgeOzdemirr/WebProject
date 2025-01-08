function plusQuantity(productId) {
    let quantityInput = $('#quantity-' + productId); // ürün miktarýný bul
    let currentQuantity = parseInt(quantityInput.val());

    if (currentQuantity < 20) {
        currentQuantity++;
        quantityInput.val(currentQuantity);

        // AJAX çaðrýsý
        $.ajax({
            url: '/ShoppingCart/UpdateQuantity/' + productId + '/' + currentQuantity,
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    $('#basket-total-amount').html(response.basketHtml); // sepet toplamýný güncelle
                }
            }
        });
    } else {
        alert('Maksimum 20 ürün eklenebilir.');
    }
}
