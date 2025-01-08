function updateQuantity(change, productId) {
    const quantityInput = document.getElementById('quantity-' + productId);
    let newQuantity = parseInt(quantityInput.value) + change;

    if (newQuantity < 1) newQuantity = 1;

    quantityInput.value = newQuantity;

    fetch(`/ShoppingCart/UpdateQuantity/${productId}/${newQuantity}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId: productId, quantity: newQuantity }),
    })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                document.getElementById('basket-total-amount').innerHTML = result.basketHtml; // Sepet toplamýný güncelle
            } else {
                alert('Bir hata oluþtu.');
            }
        });
}
