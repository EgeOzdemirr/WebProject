db = db.getSiblingDB('WebProjectCatalogDb');

if (db.Categories.countDocuments() > 0) {
  print("Catalog zaten seed edilmiş, atlanıyor.");
} else {
  var catElektronik = ObjectId();
  var catGiyim = ObjectId();
  var catEvYasam = ObjectId();

  db.Categories.insertMany([
    { _id: catElektronik, CategoryName: "Elektronik", ImageUrl: "img/category-1.jpg" },
    { _id: catGiyim, CategoryName: "Giyim", ImageUrl: "img/category-2.jpg" },
    { _id: catEvYasam, CategoryName: "Ev & Yaşam", ImageUrl: "img/category-3.jpg" }
  ]);

  db.Brands.insertMany([
    { BrandName: "TechNova", ImageUrl: "img/brand-1.jpg" },
    { BrandName: "UrbanWear", ImageUrl: "img/brand-2.jpg" },
    { BrandName: "HomeStyle", ImageUrl: "img/brand-3.jpg" }
  ]);

  db.Products.insertMany([
    { ProductName: "Kablosuz Kulaklık", ProductPrice: NumberDecimal("899.90"), ProductImageUrl: "img/product-1.jpg", ProductDescription: "Gürültü önleyici kablosuz kulaklık.", CategoryId: catElektronik },
    { ProductName: "Akıllı Saat", ProductPrice: NumberDecimal("1499.00"), ProductImageUrl: "img/product-2.jpg", ProductDescription: "Nabız ve uyku takibi yapan akıllı saat.", CategoryId: catElektronik },
    { ProductName: "Erkek Mont", ProductPrice: NumberDecimal("649.90"), ProductImageUrl: "img/product-3.jpg", ProductDescription: "Su geçirmez kışlık mont.", CategoryId: catGiyim },
    { ProductName: "Kadın Sneaker", ProductPrice: NumberDecimal("399.90"), ProductImageUrl: "img/product-4.jpg", ProductDescription: "Rahat günlük spor ayakkabı.", CategoryId: catGiyim },
    { ProductName: "Kahve Makinesi", ProductPrice: NumberDecimal("1249.00"), ProductImageUrl: "img/product-5.jpg", ProductDescription: "Otomatik filtre kahve makinesi.", CategoryId: catEvYasam },
    { ProductName: "Masa Lambası", ProductPrice: NumberDecimal("219.90"), ProductImageUrl: "img/product-6.jpg", ProductDescription: "Dokunmatik LED masa lambası.", CategoryId: catEvYasam }
  ]);

  db.Features.insertMany([
    { Title: "Ücretsiz Kargo", Icon: "fa fa-shipping-fast" },
    { Title: "7/24 Destek", Icon: "fa fa-headset" },
    { Title: "Güvenli Ödeme", Icon: "fa fa-shield-alt" }
  ]);

  db.FeatureSliders.insertMany([
    { Title: "Yeni Sezon", Description: "Yeni sezon ürünlerini keşfedin.", ImageUrl: "img/carousel-1.jpg", Status: true },
    { Title: "Büyük İndirim", Description: "Seçili ürünlerde %30'a varan indirim.", ImageUrl: "img/carousel-2.jpg", Status: true }
  ]);

  db.OfferDiscounts.insertMany([
    { Title: "Elektronikte İndirim", SubTitle: "%20'ye varan indirim", ImageUrl: "img/offer-1.jpg", ButtonTitle: "Alışverişe Başla" }
  ]);

  print("Seed tamamlandı: " + db.Categories.countDocuments() + " kategori, " + db.Products.countDocuments() + " ürün, " + db.Brands.countDocuments() + " marka");
}
