function refreshbasketminus(productId, price) {
    const quantityInput = document.getElementById('quantity-' + productId);
    let newQuantity = parseInt(quantityInput.value) - 1;

    if (newQuantity < 1) newQuantity = 1; // Miktar 1'den küçük olmamalı

    quantityInput.value = newQuantity;

    const totalElement = document.getElementById('total-' + productId);
    const newTotal = newQuantity * price;
    totalElement.textContent = `₺ ${newTotal.toFixed(2)}`;

    fetch(`/ShoppingCart/UpdateQuantity`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId: productId, quantity: newQuantity }),
    })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                $('#basket-total-amount').html(result.basketHtml); // Sepet toplamını güncelle
            } else {
                alert('Bir hata oluştu.');
            }
        });
}
