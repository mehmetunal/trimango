# Ege Tatil Evleri – Admin Panel Modül Haritası, Menü Yapısı ve Yetkiler (README)

Bu doküman, **Admin Paneli**nin bilgi mimarisini, menü/sayfa hiyerarşisini ve yetki (RBAC) modelini **işlevsel** seviyede açıklar. Kod veya teknoloji tercihinden bağımsızdır; ürün/iş analizi niteliğindedir. 

---

## 1) Tasarım İlkeleri (Kısa)
- **Net hiyerarşi:** En fazla 3 derinlikte menü.
- **İş akışı odaklı:** Rezervasyon, içerik onayı, iade gibi süreçler tek noktadan tamamlanır.
- **RBAC:** Rol tabanlı yetki, sayfa/aksiyon seviyesinde kontrol.
- **İzlenebilirlik:** Her kritik işlem **AuditLog**’a düşer.
- **Arama/Filtre:** Tüm liste sayfalarında gelişmiş filtre + hızlı arama.
- **Kitle işlemleri:** Toplu onay, toplu kapama, toplu fiyat.

---

## 2) Üst Düzey Menü Haritası

```
Dashboard
Rezervasyonlar
  ├─ Tüm Rezervasyonlar
  ├─ Onay Bekleyenler
  ├─ İptal/İade Talepleri
  ├─ Takvim Görünümü
  └─ Rezervasyon Oluştur (manuel)
Envanter
  ├─ Properties (Tesis/İlanlar)
  │   ├─ Tüm Properties
  │   ├─ Onay Bekleyen İlanlar
  │   └─ Yeni Property
  ├─ Units (Üniteler)
  ├─ Property Types (Türler)
  ├─ Features (Özellikler)
  ├─ Locations (Konumlar)
  └─ Distance Info (Uzaklık Tablosu)
Fiyatlama & Kurallar
  ├─ Seasonal Pricing (Sezon Fiyatları)
  ├─ Stay Rules (Min/Max gece, giriş/çıkış günleri)
  ├─ Extra Fees (Temizlik, depozito vb.)
  └─ Availability Blocks (Kapatmalar)
Tedarikçiler (Suppliers)
  ├─ Tüm Tedarikçiler
  ├─ Doğrulama/KYC
  └─ Sözleşme & Komisyon Ayarları
Müşteriler (Users)
  ├─ Tüm Müşteriler
  └─ Hesap Durumları / Notlar
Ödemeler & Muhasebe
  ├─ Ödemeler (Payments)
  ├─ İadeler (Refunds)
  ├─ Tahsilatlar & Payouts (Tedarikçi ödemeleri)
  ├─ Komisyon & Faturalar (E-Arşiv/EFatura)
  └─ Mutabakat & Uyuşmazlıklar (Disputes)
İçerik & SEO (CMS)
  ├─ Pages (Statik Sayfalar)
  ├─ SeoContents (Ana sayfa/section yazıları)
  ├─ Blog Posts
  ├─ Blog Categories
  ├─ Blog Comments (Moderasyon)
  ├─ Menüler (Header/Footer)
  └─ Banner Yönetimi
Yorumlar & Moderasyon
  ├─ Reviews (Yorumlar)
  └─ Q&A (Soru-Cevap)
Mesajlaşma & Destek
  ├─ Mesajlar (Kullanıcı↔️Tedarikçi/Operasyon)
  └─ Destek Talepleri (Ticketing)
Kampanyalar
  ├─ Kuponlar (Coupons)
  └─ Kampanya Kuralı (Segment, kısıt, süre)
Raporlar & Analitik
  ├─ Satış & Gelir
  ├─ Doluluk & Performans
  ├─ İptal & İade Analizi
  ├─ Pazarlama (UTM/Kaynak)
  └─ Tedarikçi Performans
Sistem & Ayarlar
  ├─ Roller & Yetkiler (RBAC)
  ├─ Bildirim Şablonları (E-posta/SMS)
  ├─ Ödeme Sağlayıcıları
  ├─ E-Arşiv/EFatura Entegrasyonu
  ├─ Harita & iCal Entegrasyonları
  ├─ Dosya/Medya Ayarları
  └─ Loglar (Audit, Güvenlik, İşlem)
Araçlar
  ├─ İçeri Aktar / Dışarı Aktar
  ├─ Önbellek & Arama İndeksi
  └─ Sistem Sağlığı (Health)
```

> **Not:** Menü isimleri ve sırası A/B test veya operasyonel geri bildirimlerle güncellenebilir.

---

## 3) Sayfa Hiyerarşisi & Temel Ekranlar (Seçme)

### 3.1 Dashboard
- **KPI kartları:** Bugün/hafta/ay toplam rezervasyon, gelir, iptal, dönüşüm.
- **Grafikler:** Satış trendi, kanal kırılımı.
- **İş listesi:** Onay bekleyen ilanlar/rezervasyonlar, iade talepleri, kritik uyarılar.
- **Kısayollar:** Yeni kupon, yeni sayfa, tedarikçi doğrulama.

### 3.2 Rezervasyonlar
- **Liste:** Durum, tarih aralığı, tedarikçi, property/unit, müşteri, tutar, kanal filtresi.
- **Aksiyonlar:** Görüntüle, Düzenle, Onayla/İptal Et, İade Hesapla, Fatura Gönder, Mesaj Gönder.
- **Detay:** Fiyat kırılımı, ödemeler, politika, mesajlaşma geçmişi, günlükler.
- **Takvim:** Aylık/haftalık görünüm; blok ve min-gece kurallarını göster.

### 3.3 Envanter
- **Property:** Onay kuyruklu içerik; zorunlu alan validasyonu; SEO alanları.
- **Unit:** Kapasite/yatak planı; özel fiyat/kurallar (opsiyonel override).
- **Images:** Kategoriye göre galeri; kapak; sürükle-bırak sıralama; toplu yükleme.
- **Property Types/Features/Locations:** Sözlük yönetimi; çakışma ve silme korumaları.

### 3.4 Fiyatlama & Kurallar
- **Seasonal Pricing:** Tarih aralığına fiyat; döviz; kopyala-çoğalt.
- **Stay Rules:** Min/Max gece; giriş/çıkış günü; gap-night; lead-time.
- **Extra Fees:** Zorunlu/opsiyonel; sabit/yüzde; vergilendirme notu.
- **Availability Blocks:** Kapatma nedenleri; toplu kapama; içe/dışa aktar.

### 3.5 Tedarikçiler
- **KYC:** Vergi levhası, IBAN doğrulama; sözleşme durumu.
- **Komisyon:** Oranlar, kanal bazlı kural; ödeme takvimi.
- **Performans:** Onay hızı, iptal oranı, puan.

### 3.6 Ödemeler & Muhasebe
- **Ödemeler:** İşlem referansı, 3D sonucu, iade/partial refund.
- **Payouts:** Tedarikçi bazında bekleyen/ödenen; kesinti/komisyon listesi.
- **Vergi/Fatura:** E-Arşiv üreteci, durum takibi; hata logları.

### 3.7 CMS & SEO
- **Pages:** Versiyonlama, planlı yayın; SEO meta; link doğrulama.
- **SeoContents:** Section bazlı bloklar (anasayfa üst/alt, footer, sidebar).
- **Blog:** Yazı, kategori, etiket; yorum moderasyonu; öne çıkarma.
- **Menüler:** Hiyerarşik yapı; drag&drop; hedef link kontrolü.
- **Banner:** Yerleşim alanı, tarih, gösterim/tıklama metriği.

### 3.8 Yorumlar & Moderasyon
- **Reviews:** Onayla/red; uygunsuz içerik işaretleme; puan ortalamaları.
- **Q&A:** Cevapla; tedarikçiye yönlendir; SSS’e dönüştür.

### 3.9 Mesajlaşma & Destek
- **Mesajlar:** Konu bazlı thread; şablon yanıt; dosya ekleri; SLA uyarıları.
- **Ticketing:** Kategori/öncelik; durum akışı; iç notlar; devretme.

### 3.10 Kampanyalar
- **Kuponlar:** Kod üretimi; kısıt (kullanıcı/ürün/segment/tarih); kullanım limiti; rapor.
- **Kampanya Kuralları:** Sezon dışı indirim; uzun konaklama; kaynak bazlı promosyon.

### 3.11 Raporlar & Analitik
- Satış/gelir; doluluk; iptal neden analizi; UTM performansı; tedarikçi skoru.
- İndirme: CSV/XLSX/PDF.

### 3.12 Sistem & Ayarlar
- **RBAC:** Rol oluştur, izin ata; tekil kullanıcıya ek izin; kural çakışma uyarısı.
- **Bildirim Şablonları:** E-posta/SMS; değişken yer tutucular; test gönderimi.
- **Entegrasyonlar:** Ödeme, harita, iCal, e-Fatura; API anahtarları; webhook log.
- **Medya Ayarları:** Maks. boyut, dönüşüm (webp), CDN.
- **Loglar:** Audit, güvenlik (oturum, IP, rate-limit), sistem olayları.

---

## 4) Yetkiler (RBAC) Modeli

### 4.1 Rol Tanımları (Öneri)
- **SuperAdmin:** Tüm izinler.
- **Admin:** Sistem genel yönetim (kısıtlı güvenlik ayarları hariç).
- **Operasyon (Ops):** Rezervasyon, iade, envanter onayı.
- **İçerik Editörü (CMS):** Pages, SeoContents, Blog, Menü, Banner.
- **Finans:** Ödeme/iade, payout, fatura.
- **Tedarikçi Yöneticisi:** KYC, komisyon, sözleşme, tedarikçi performansı.
- **Destek (CS):** Mesajlar, ticket, Q&A; sınırlı rezervasyon görüntüleme.
- **Analist:** Raporlar; salt okunur kritik sayfalar.
- **ReadOnly:** Tüm liste/inceleme; aksiyon yok.

### 4.2 İzin Tipleri (Sayfa/Aksiyon Bazlı)
- **View** (görüntüle)
- **Create** (oluştur)
- **Edit** (düzenle)
- **Delete** (sil)
- **Approve/Publish** (onayla/yayınla)
- **Moderate** (yorum/Q&A/şikayet moderasyonu)
- **Finance** (ödeme/iade/payout işlemleri)
- **Export** (CSV/PDF çıktısı)
- **Configure** (sistem/entegrasyon ayarları)

### 4.3 Örnek Yetki Matrisi (Özet)

| Modül | Admin | Ops | CMS | Finans | Tedarikçi Yön. | CS | Analist | ReadOnly |
|---|:--:|:--:|:--:|:--:|:--:|:--:|:--:|:--:|
| Rezervasyonlar | V/E/A | V/E/A | – | V | – | V/E | V | V |
| Envanter (Property/Unit) | V/E/A | V/E/A | – | – | V/E/A | V | V | V |
| Fiyat/Kural | V/E/A | V/E/A | – | – | V/E | – | V | V |
| Tedarikçi | V/E/A | V/E | – | – | V/E/A | – | V | V |
| Ödemeler/İadeler | V | V | – | V/E/A | – | – | V | V |
| CMS/SEO | V/E/A | – | V/E/A | – | – | – | V | V |
| Yorum/Q&A | V/E/A | V/E/A | V/E/A | – | – | V/E/A | V | V |
| Mesajlaşma | V/E/A | V/E/A | – | – | – | V/E/A | V | V |
| Kampanyalar | V/E/A | – | V/E | – | – | – | V | V |
| Raporlar | V | V | V | V | V | V | V/E/A | V |
| Sistem/Ayarlar | V/E/A | – | – | – | – | – | V | – |

> **Not:** “A” = Approve/Publish/Onay; “V”=View; “E”=Edit. Gerektiğinde sayfa/endpoint özelinde granular yetki açılabilir.

---

## 5) Sayfa Kalıpları (Liste/Detay/Düzenleme)
- **Liste:** Filtre, kolon seçimi, hızlı eylemler (menü), sayfalama, dışa aktar.
- **Detay:** Sağ panelde durum/aksiyonlar; ilişkili kayıtlar (ödemeler, mesajlar, loglar).
- **Düzenleme:** Zorunlu alan uyarıları, canlı validasyon, değişiklik özeti.
- **Kitle İşlemleri:** Toplu onay, toplu kapama, toplu fiyat/kupon.

---

## 6) Navigasyon & URL Örüntüleri
```
/admin/dashboard
/admin/reservations
/admin/reservations/{id}
/admin/inventory/properties
/admin/inventory/properties/{id}
/admin/inventory/units
/admin/pricing/seasons
/admin/suppliers
/admin/payments
/admin/cms/pages
/admin/cms/seo-contents
/admin/campaigns/coupons
/admin/reports/overview
/admin/settings/roles
/admin/logs/audit
```
- **Breadcrumbs:** Dashboard > Envanter > Properties > #12345
- **Derin link:** İnceleme e-postalarına derin link koyulabilir (yetki kontrolü şartıyla).

---

## 7) İş Akışları (Önemli Süreçler)

### 7.1 İlan Onay Süreci
1) Supplier ekler → 2) İçerik kontrol (foto, açıklama, konum) → 3) SEO alanları → 4) Admin onayı → 5) Yayın.

### 7.2 Rezervasyon & İade
1) Talep/Ödeme → 2) (Opsiyonel) Supplier onay → 3) Onaylandı → 4) İptal talebi → 5) Politika/ceza oranı → 6) İade işlemi → 7) Fatura güncelleme.

### 7.3 Payout (Tedarikçi Ödemesi)
Tamamlanan konaklama → Komisyon kesimi → Payout kuyruk → Finans onayı → Ödeme dekontu.

### 7.4 İçerik Yayınlama
Taslak → İnceleme → Yayın → Versiyonlama & değişiklik günlükleri.

---

## 8) Audit & Güvenlik
- Her **Create/Update/Delete/Approve** aksiyonu AuditLog’da: kullanıcı, IP, zaman, önce/sonra (JSON diff).
- Oturum güvenliği: MFA (ops.), oturum sonlandırma, rate-limit.
- KVKK: Kişisel veri alanlarının maskelemesi (liste görünümünde).

---

## 9) Raporlama ve İndirmeler
- Tüm raporlar tarih aralığı + kanal + tedarikçi filtreleriyle indirilebilir.
- Büyük veri indirme işlemleri **asenkron kuyruk + e-posta link** ile sunulur.

---

## 10) Performans ve Kullanılabilirlik Notları
- Liste sayfaları için **sunucu taraflı sayfalama** ve **kolon indeksleri**.
- Görseller için **thumb** kullanımı, webp, lazy-load.
- Ekran boş kalmamalı: boş durum (empty state) + kısayol eylemleri.

---

## 11) Modül–Veri Modeli Eşlemesi (Kısa Referans)
- Rezervasyonlar → `Reservation`, `ReservationPriceBreakdown`, `Payment`, `Policy`, `Dispute`
- Envanter → `Property`, `Unit`, `PropertyType`, `Feature`, `PropertyFeatureMapping`, `Location`, `DistanceInfo`, `PropertyImages`
- Fiyat/Kural → `SeasonalPricing`, `StayRule`, `ExtraFee`, `AvailabilityBlock`
- Tedarikçi → `Supplier`
- Müşteriler → `AspNetUsers` (Identity)
- CMS & SEO → `Page`, `SeoContent`, `BlogPost`, `BlogCategory`, `BlogPostCategoryMapping`, `BlogComment`, `Banner`
- Kampanya → `Coupon`
- Sistem & Log → `AuditLog`

---

## 12) Yol Haritası Önerisi
- **MVP:** Rezervasyon, Envanter, Fiyat/Kural, CMS/SEO temel, Ödemeler temel, RBAC.
- **V1:** Payout, Kampanya, Mesaj/Ticket, Raporlar, Banner/AB testi.
- **V2:** Çokdilli CMS, Channel Manager, gelişmiş Fraud, ileri analitik.

---

Bu modül haritası; yönetim ekiplerinin günlük operasyonlarını hızlı, güvenli ve denetlenebilir şekilde yürütmesi için referans niteliğindedir. İhtiyaca göre alt sayfalar genişletilebilir veya bölünebilir.