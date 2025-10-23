# Trimango - Proje Analizi ve Gereksinim Dokümanı (README)

## 0. Amaç & Kapsam
Trimango projesi; villa, bungalov ve apart gibi konaklama seçeneklerinin listelendiği, arandığı, rezerve edildiği ve ödemesinin yapıldığı bir tatil pazar yeri sistemidir.

**Teknoloji:** ASP.NET Core MVC, Entity Framework Core, MSSQL, Identity Core  
**Roller:** Ziyaretçi, Ücye (Müşteri), Tedarikçi (Supplier), Admin, Operasyon, Muhasebe  
**Amaç:** Tedarikçilerin (ev sahiplerinin) ilan ekleyebildiği, müşterilerin filtreleme yaparak konaklama bulduğu ve rezerve ettiği, yönetilebilir bir turizm sistemi kurmak.

---

## 1. Ana Modüller
1. Envanter (Property / Unit / Supplier)
2. Arama & Filtreleme
3. Rezervasyon & Uygunluk
4. Fiyatlama & Kurallar
5. Ödeme & Faturalama
6. İçerik & SEO (Pages, Blog, Text Blocks)
7. Yorum & Değerlendirme
8. Kullanıcı Hesapları (Identity)
9. Mesajlaşma & Bildirimler
10. Kampanya / Kupon / Banner
11. Raporlama & Analitik
12. Yetki & Denetim
13. Entegrasyonlar (Payment, iCal, Harita)

---

## 2. Kullanıcı Rolleri & Yetkiler

| Rol | Yetkiler |
|------|-----------|
| **Ziyaretçi** | Arama, listeleme, detay görüntüleme |
| **Müşteri** | Rezervasyon, yorum, favori, mesajlaşma |
| **Supplier** | Villa ekleme, fiyat/takvim düzenleme, rezervasyon yönetimi |
| **Admin** | Tüm sistem, içerik, finans, kampanya, log |
| **Operasyon** | Rezervasyon onay, iptal, iade yönetimi |
| **Muhasebe** | Fatura, ödeme, komisyon raporu |

---

## 3. Web (Kullanıcı Arayüzü)

### 3.1 Ana Sayfa
- Arama modülü (konum, tarih, kişi)
- SEO text blokları (CMS yönetimi)
- Kampanya ve öne çıkan villalar

### 3.2 Listeleme & Filtreleme
- **Filtreler:** PropertyType, fiyat aralığı, kişi sayısı, oda sayısı, özellikler (Feature tablosu)
- **Sıralama:** Fiyat, popülerlik, yeni, puan
- Harita görünümü (opsiyonel)

### 3.3 Ürün Detay
- Galeri (kategorili: Oturma odası, Mutfak, Yatak odası, Banyo, Bahçe, Dış)
- Fiyat ve uygunluk takvimi
- Özellikler (kategori bazlı)
- Uzaklık tablosu & harita
- Yorumlar, soru-cevap, benzer ilanlar

### 3.4 Rezervasyon Akışı
- 1. Adım: Tarih & kişi sayısı
- 2. Adım: Bilgiler & ek hizmetler
- 3. Adım: Fiyat özeti & ödeme
- 4. Adım: Onay ve bildirim

### 3.5 Hesabım
- Profil, rezervasyonlar, iade, favoriler, mesajlar

---

## 4. Supplier Paneli

### 4.1 Envanter
- Property ve Unit ekleme/düzenleme
- Görsel galerisi (kategori bazlı)
- Konum, ev kuralları, iptal politikaları

### 4.2 Fiyat & Takvim
- SeasonalPricing (sezonluk fiyat)
- Block (kapatma), min/max gece, kontrol kuralları

### 4.3 Rezervasyon
- Gelen talepler, onay/red
- İptal talebi ve iade oranları

### 4.4 Finans
- Komisyon, cüzdan, bakiye raporları

---

## 5. Admin Paneli
- Dashboard: rezervasyon, gelir, iptal, doluluk
- CMS: Pages, SeoContents, Blog, Menüler
- Envanter Onayı, Supplier Doğrulama
- Rezervasyon, Ödeme, Fatura
- Kupon, Kampanya, Banner
- Kullanıcı/Rol & Log
- Raporlama (Gelir, Dönüşüm, Performans)

---

## 6. İş Kuralları

### 6.1 Rezervasyon Durumları
Pending → Confirmed → CheckedIn → CheckedOut → Completed  
CancelledByGuest / CancelledBySupplier / Refunded

### 6.2 Fiyatlama
- SeasonalPricing zorunlu
- Ek üretler: temizlik, depozito, evcil, hizmet
- İndirim: erken rezervasyon, uzun konaklama, kupon

### 6.3 İptal Politikaları
- Esnek, Orta, Katı veya Özel (% ve tarih eşikleri)
- İade hesaplama sistemsel

### 6.4 Ödeme
- 3D Secure zorunlu
- Ön ödeme / tam ödeme desteği
- İade ve faturalama modülü

---

## 7. İçerik & SEO
- **Pages:** Hakkımızda, SSS, KVKK vb.
- **SeoContents:** anasayfa/footer yazıları
- **Blog:** kategori, etiket, yorum

---

## 8. Medya / Galeri
- **PropertyImages**:  
  - Id, PropertyId, ImageUrl, Category, IsMain, Order, CreatedDate
- **Kategori örnekleri:** LivingRoom, Kitchen, DiningArea, Bedroom1, Bedroom2, Bathroom, Exterior, BackGarden, ExtraPhotos

---

## 9. Database Tasarımı (Tam)

### **Identity**
- AspNetUsers, AspNetRoles, AspNetUserRoles, Claims, Tokens

### **Supplier**
- `Id`, `Name`, `TaxNumber`, `IBAN`, `ContractStatus`, `Score`, `CreatedDate`

### **Property**
- `Id`, `SupplierId (FK)`, `Title`, `Description`, `PropertyTypeId (FK)`, `LocationId (FK)`, `Capacity`, `RoomCount`, `BathroomCount`, `SquareMeter`, `IsActive`, `CreatedDate`

### **Unit**
- `Id`, `PropertyId (FK)`, `Name`, `Capacity`, `BedConfig`, `PrivatePool`, `IsActive`

### **PropertyType**
- `Id`, `Name`, `Slug`, `Description`, `IconUrl`, `DisplayOrder`, `IsActive`

### **Feature** & **PropertyFeatureMapping**
- Özellikler ve N-N ilişki

### **Location**
- `Id`, `City`, `District`, `Region`, `Address`, `Latitude`, `Longitude`

### **DistanceInfo**
- `Id`, `PropertyId (FK)`, `PlaceName`, `DistanceKm`

### **PropertyImages**
- `Id`, `PropertyId`, `ImageUrl`, `Category`, `IsMain`, `Order`, `CreatedDate`

### **SeasonalPricing**
- `Id`, `Scope (Property/Unit)`, `ScopeId`, `StartDate`, `EndDate`, `PricePerNight`

### **StayRule**
- `Id`, `Scope`, `ScopeId`, `MinNights`, `MaxNights`, `CheckInDays`, `GapNights`

### **ExtraFee**
- `Id`, `Scope`, `ScopeId`, `Name`, `Type (Flat/Percent)`, `Amount`, `Mandatory`

### **Reservation**
- `Id`, `UnitId`, `PropertyId`, `UserId`, `CheckInDate`, `CheckOutDate`, `GuestCount`, `Status`, `PolicyId`, `CreatedDate`

### **ReservationPriceBreakdown**
- `Id`, `ReservationId`, `LineType`, `Name`, `Qty`, `UnitPrice`, `Total`

### **Payment**
- `Id`, `ReservationId`, `Amount`, `Method`, `Status`, `TransactionRef`, `PaidAt`

### **Policy**
- `Id`, `PolicyType (Cancel/House)`, `Name`, `Terms`, `IsVisible`

### **Page / SeoContent / BlogPost / BlogCategory / BlogComment**
CMS içerikleri için

### **Review / Question / Favorite / Message / Dispute**
Kullanıcı etkileşimi ve destek sistemi

### **Coupon / Banner / AuditLog**
Kampanya, görsel, denetim için

---

## 10. Mesajlaşma & Bildirim
- Müşteri ↔️ Supplier mesajlaşma
- Email/SMS/Push bildirimleri: rezervasyon, onay, iptal, bakiye vb.

---

## 11. Kampanya & Banner
- Kupon tipleri: Yüzde/Flat, tarih, limit, segment
- Banner: konum, link, gösterim tavanı

---

## 12. Raporlama
- Satış, gelir, doluluk, iptal oranları
- Tedarikçi performansı (onay süresi, iptal oranı)

---

## 13. Güvenlik & Uyumluluk
- ASP.NET Identity, MFA, loglama
- KVKK & PCI uyumlu ödeme
- Fraud koruması, denetim kayıtları

---

## 14. Entegrasyonlar
- Ödeme: iyzico / PayTR / Stripe
- Harita: Google Maps
- iCal / Channel Manager (ops.)
- E-fatura / SMS servisleri

---

## 15. Performans & UX
- LCP < 2.5s, mobil uyumlu
- Lazy-load görseller, cache, CDN
- Erişilebilirlik (WCAG AA)

---

## 16. Yol Haritası
1. **MVP:** Arama, detay, rezervasyon, ödeme, admin CMS
2. **V1:** Kampanya, mesajlaşma, iade, raporlar
3. **V2:** Entegrasyonlar, A/B test, çokdilli destek

---

## 17. Sonuç
Bu doküman, Trimango projesinin tüm iş akışlarını, veritabanı yapısını, modüllerini, rollerini ve iş mantıklarını kapsar.  
Tüm içerikler admin panelinden yönetilebilir olacak ve sistem tam entegre, esnek, ölçeklenebilir bir mimaride kurgulanmalıdır.

