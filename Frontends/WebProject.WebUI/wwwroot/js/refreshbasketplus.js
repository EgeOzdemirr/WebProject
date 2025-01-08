function plusQuantity(productId) {
    let quantityInput = $('#quantity-' + productId); // �r�n miktar�n� bul
    let currentQuantity = parseInt(quantityInput.val());

    if (currentQuantity < 20) {
        currentQuantity++;
        quantityInput.val(currentQuantity);

        // AJAX �a�r�s�
        $.ajax({
            url: '/ShoppingCart/UpdateQuantity/' + productId + '/' + currentQuantity,
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    $('#basket-total-amount').html(response.basketHtml); // sepet toplam�n� g�ncelle
                }
            }
        });
    } else {
        alert('Maksimum 20 �r�n eklenebilir.');
    }
}
