function removebasket(count) {
    const productId = document.getElementById(`productId${count}`).value;

    fetch(`/ShoppingCart/RemoveProduct`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ productId: productId }),
    })
        .then(response => response.text())
        .then(html => {
            document.getElementById('basket-total').innerHTML = html;
        });
}
