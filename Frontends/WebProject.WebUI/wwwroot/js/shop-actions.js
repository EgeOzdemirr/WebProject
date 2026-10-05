/* Ürün kartı butonları: favori, karşılaştır, hızlı bakış (sepete ekle düz bağlantı olarak çalışır). */
(function () {
    'use strict';

    var toastTimer;
    function toast(message, kind) {
        var el = document.getElementById('wp-toast');
        if (!el) {
            el = document.createElement('div');
            el.id = 'wp-toast';
            el.setAttribute('role', 'status');
            document.body.appendChild(el);
        }
        el.className = 'wp-toast wp-toast-show' + (kind ? ' wp-toast-' + kind : '');
        el.textContent = message;
        clearTimeout(toastTimer);
        toastTimer = setTimeout(function () { el.classList.remove('wp-toast-show'); }, 2600);
    }

    function goToLogin() {
        location.href = '/Login/Index?ReturnUrl=' + encodeURIComponent(location.pathname + location.search);
    }

    function post(url) {
        return fetch(url, { method: 'POST', credentials: 'same-origin', headers: { 'X-Requested-With': 'XMLHttpRequest' } })
            .then(function (res) {
                // giriş gerekiyorsa sunucu giriş sayfasına yönlendirir
                if (res.redirected && res.url.indexOf('/Login/') !== -1) { goToLogin(); return null; }
                if (!res.ok) { throw new Error('http ' + res.status); }
                return res.json();
            });
    }

    function setActive(btn, active, iconOn, iconOff) {
        btn.classList.toggle('is-active', active);
        var icon = btn.querySelector('i');
        if (icon && iconOn) {
            icon.className = (active ? iconOn : iconOff);
        }
    }

    document.addEventListener('click', function (e) {
        var btn = e.target.closest('[data-wp-action]');
        if (!btn) { return; }
        e.preventDefault();
        var id = btn.getAttribute('data-id');

        switch (btn.getAttribute('data-wp-action')) {
            case 'wishlist':
                post('/Wishlist/Toggle/' + encodeURIComponent(id)).then(function (r) {
                    if (!r) { return; }
                    setActive(btn, r.added, 'fas fa-heart', 'far fa-heart');
                    toast(r.message, r.added ? 'ok' : '');
                }).catch(function () { toast('İşlem yapılamadı, tekrar deneyin', 'err'); });
                break;

            case 'compare':
                post('/Compare/Toggle/' + encodeURIComponent(id)).then(function (r) {
                    if (!r) { return; }
                    setActive(btn, r.added);
                    var counter = document.getElementById('wp-compare-count');
                    if (counter) { counter.textContent = r.count; }
                    toast(r.message, r.full ? 'err' : (r.added ? 'ok' : ''));
                }).catch(function () { toast('İşlem yapılamadı, tekrar deneyin', 'err'); });
                break;

            case 'quickview':
                fetch('/ProductList/QuickView/' + encodeURIComponent(id), { credentials: 'same-origin' })
                    .then(function (res) { if (!res.ok) { throw new Error(); } return res.text(); })
                    .then(function (html) {
                        document.getElementById('wpQuickViewBody').innerHTML = html;
                        window.jQuery('#wpQuickView').modal('show');
                    })
                    .catch(function () { toast('Ürün bilgisi alınamadı', 'err'); });
                break;
        }
    });
})();
