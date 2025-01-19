function plusquantity(c) {
    var quantity = +$("#quantity" + c).val() + 1;
    var productId = $("#productId" + c).val();
    $.post("/ShoppingCart/ShoppingCartUpdate/" + productId + "/" + quantity);

    pr = setInterval(function () {
        location.reload();
    }, 300)
    setTimeout(() => { clearInterval(pr); }, 400);
};

// script viewcomponent'teki "<script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>" satýrý head viewcomponent in en altýna aldýðýmýzda ajax sorunsuz çalýþýyor.