// Örnek katalog verisi. Görseller Frontends/WebProject.WebUI/wwwroot/images altındaki dosyalardır.
// Kullanım: mongosh --quiet seed_catalog.js   (zaten doluysa atlanır)
db = db.getSiblingDB('WebProjectCatalogDb');

if (db.Categories.countDocuments() > 0) {
  print("Catalog zaten seed edilmiş, atlanıyor.");
} else {
  var C = "/images/categoriesImages/";
  var P = "/images/featuresProductImages/";

  var cat = {
    temizlik: ObjectId(), giyim: ObjectId(), elektronik: ObjectId(),
    meyve: ObjectId(), mobilya: ObjectId(), aletler: ObjectId()
  };

  db.Categories.insertMany([
    { _id: cat.temizlik,   CategoryName: "Temizlik",           ImageUrl: C + "cleaning.png" },
    { _id: cat.giyim,      CategoryName: "Giyim",              ImageUrl: C + "clothes.png" },
    { _id: cat.elektronik, CategoryName: "Elektronik",         ImageUrl: C + "electronic.png" },
    { _id: cat.meyve,      CategoryName: "Meyve & Sebze",      ImageUrl: C + "fruit.png" },
    { _id: cat.mobilya,    CategoryName: "Mobilya",            ImageUrl: C + "furniture.png" },
    { _id: cat.aletler,    CategoryName: "Küçük Ev Aletleri",  ImageUrl: C + "smallHouseholdAppliances.png" }
  ]);

  db.Products.insertMany([
    { ProductName: "Çamaşır Deterjanı 4 kg",        ProductPrice: NumberDecimal("189.90"),  ProductImageUrl: P + "detergent.png", ProductDescription: "Yüksek performanslı toz çamaşır deterjanı.", CategoryId: cat.temizlik },
    { ProductName: "Komili Sıvı Sabun 750 ml",      ProductPrice: NumberDecimal("64.50"),   ProductImageUrl: P + "komili-sıvı-sabun.png", ProductDescription: "Nemlendirici etkili sıvı el sabunu.", CategoryId: cat.temizlik },
    { ProductName: "Böğürtlenli Sıvı Sabun 400 ml", ProductPrice: NumberDecimal("42.90"),   ProductImageUrl: P + "hobby_sivi_sabun_bogurtlen_400ml.png", ProductDescription: "Böğürtlen kokulu sıvı sabun.", CategoryId: cat.temizlik },
    { ProductName: "Katı Kalıp Sabun",              ProductPrice: NumberDecimal("29.90"),   ProductImageUrl: P + "kalıp-sabun.jpg", ProductDescription: "Geleneksel kalıp sabun, 4'lü paket.", CategoryId: cat.temizlik },
    { ProductName: "Doğal Zeytinyağlı Sabun",       ProductPrice: NumberDecimal("54.90"),   ProductImageUrl: P + "soap1.png", ProductDescription: "El yapımı doğal zeytinyağlı sabun.", CategoryId: cat.temizlik },
    { ProductName: "Taze Erik 1 kg",                ProductPrice: NumberDecimal("79.90"),   ProductImageUrl: P + "erik1.png", ProductDescription: "Mevsiminde toplanmış taze erik.", CategoryId: cat.meyve },
    { ProductName: "Lenovo Dizüstü Bilgisayar",     ProductPrice: NumberDecimal("24999.00"), ProductImageUrl: P + "lenovo1.png", ProductDescription: "15,6 inç ekranlı, 16 GB RAM, 512 GB SSD.", CategoryId: cat.elektronik },
    { ProductName: "Örgü Kazak",                    ProductPrice: NumberDecimal("449.90"),  ProductImageUrl: P + "sweater1.png", ProductDescription: "Yumuşak dokulu, bisiklet yaka kışlık kazak.", CategoryId: cat.giyim }
  ]);

  db.Brands.insertMany([
    { BrandName: "A101",        ImageUrl: "/images/brandsImages/a101.png" },
    { BrandName: "BİM",         ImageUrl: "/images/brandsImages/bim.png" },
    { BrandName: "Bizim",       ImageUrl: "/images/brandsImages/bizim.png" },
    { BrandName: "Carrefour",   ImageUrl: "/images/brandsImages/carrefour.png" },
    { BrandName: "Macrocenter", ImageUrl: "/images/brandsImages/macrocenter.png" },
    { BrandName: "Metro",       ImageUrl: "/images/brandsImages/metro.png" },
    { BrandName: "Migros",      ImageUrl: "/images/brandsImages/migros.png" },
    { BrandName: "Şok",         ImageUrl: "/images/brandsImages/sok.png" }
  ]);

  db.Features.insertMany([
    { Title: "Ücretsiz Kargo", Icon: "fa fa-shipping-fast" },
    { Title: "7/24 Destek",    Icon: "fa fa-headset" },
    { Title: "Güvenli Ödeme",  Icon: "fa fa-shield-alt" },
    { Title: "Kolay İade",     Icon: "fa fa-undo" }
  ]);

  db.FeatureSliders.insertMany([
    { Title: "Yeni Sezon",   Description: "Yeni sezon ürünlerini keşfedin.",        ImageUrl: "/images/carouselsImages/carousel-1.jpg", Status: true },
    { Title: "Büyük İndirim", Description: "Seçili ürünlerde fırsatları kaçırmayın.", ImageUrl: "/images/carouselsImages/carousel-2.png", Status: true },
    { Title: "Evinize Özel",  Description: "Temizlik ürünlerinde avantajlı fiyatlar.", ImageUrl: "/images/carouselsImages/carousel-3.png", Status: true }
  ]);

  db.SpecialOffers.insertMany([
    { Title: "Yeni Müşterilere", SubTitle: "%10 indirim", ImageUrl: "/images/salesImages/sale10.jpg" },
    { Title: "Hafta Sonu Fırsatı", SubTitle: "%20 indirim", ImageUrl: "/images/salesImages/sale20.jpg" }
  ]);

  db.OfferDiscounts.insertMany([
    { Title: "Temizlikte İndirim", SubTitle: "%20'ye varan indirim", ImageUrl: "/images/salesImages/sale20.jpg", ButtonTitle: "Alışverişe Başla" }
  ]);

  db.Abouts.insertOne({
    Description: "Web Project, mikroservis mimarisiyle geliştirilmiş bir e-ticaret demosudur.",
    Address: "Kocaeli, Türkiye",
    Email: "info@webproject.example",
    Phone: "+90 123 456 78 90"
  });

  // Ürün detay sayfası için her ürüne açıklama ve görsel
  db.Products.find().forEach(function (p) {
    db.ProductDetails.insertOne({
      ProductDescription: p.ProductDescription + " Dayanıklı ve kaliteli malzemeden üretilmiştir.",
      ProductInfo: "Ücretsiz kargo, 14 gün koşulsuz iade.",
      ProductId: p._id.toString()
    });
    db.ProductImages.insertOne({ ProductImage1: p.ProductImageUrl, ProductId: p._id.toString() });
  });

  print("Seed tamamlandı: " + db.Categories.countDocuments() + " kategori, " + db.Products.countDocuments() + " ürün, " + db.Brands.countDocuments() + " marka");
}
