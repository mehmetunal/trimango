# Trimango - Veritabanı Tasarımı (Database Design)

Bu doküman, Trimango projesinin veritabanı yapısını ve tabloların ilişkilerini detaylı olarak açıklar.  
Tüm yapılar **MSSQL** ve **Entity Framework Core (Code-First)** yaklaşımına uygundur.

---

## 1. Kimlik Yönetimi (Identity)

ASP.NET Core Identity yapısı kullanılır.

| Tablo | Açıklama |
|--------|-----------|
| **AspNetUsers** | Kullanıcı bilgileri (müşteri, admin, tedarikçi vb.) |
| **AspNetRoles** | Roller: Admin, Supplier, Customer |
| **AspNetUserRoles** | Kullanıcı–Rol ilişkisi |
| **AspNetUserClaims** | Ek kimlik bilgileri |
| **AspNetUserLogins** | Sosyal medya girişleri (Google, Facebook) |
| **AspNetUserTokens** | Oturum token bilgileri |

Ek Alanlar (`ApplicationUser`):  
- `FullName`  
- `ProfilePictureUrl`  
- `IsSupplier` (Ev sahibi olup olmadığını belirtir)

---

## 2. Tedarikçi (Supplier)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | Benzersiz kimlik |
| `Name` | nvarchar(150) | Tedarikçi adı |
| `TaxNumber` | nvarchar(20) | Vergi numarası |
| `IBAN` | nvarchar(34) | Ödeme hesabı |
| `ContractStatus` | nvarchar(50) | Sözleşme durumu (Bekliyor, Onaylı) |
| `Score` | decimal | Performans puanı |
| `CreatedDate` | datetime | Kayıt tarihi |

**İlişki:** 1 Supplier → N Property

---

## 3. Konaklama (Property)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `SupplierId` | int (FK → Supplier) | Sahibi |
| `Title` | nvarchar(250) | Başlık |
| `Description` | nvarchar(max) | Açıklama |
| `PropertyTypeId` | int (FK → PropertyType) | Türü (Villa, Bungalov, Apart) |
| `LocationId` | int (FK → Location) | Konum bilgisi |
| `Capacity` | int | Maksimum kişi sayısı |
| `RoomCount` | int | Oda sayısı |
| `BathroomCount` | int | Banyo sayısı |
| `SquareMeter` | int | Alan |
| `IsActive` | bit | Aktif mi? |
| `CreatedDate` | datetime | Oluşturulma tarihi |

**İlişkiler:**
- 1 Property → N Unit
- 1 Property → N PropertyImage
- 1 Property → N Reservation

---

## 4. Ünite (Unit)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `PropertyId` | int (FK) | Hangi konaklamaya ait |
| `Name` | nvarchar(100) | Ünite adı (örneğin "Villa 1") |
| `Capacity` | int | Maksimum kişi |
| `BedConfig` | nvarchar(100) | Yatak konfigürasyonu |
| `PrivatePool` | bit | Özel havuz var mı |
| `IsActive` | bit | Aktif mi |

---

## 5. Konaklama Türleri (PropertyType)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `Name` | nvarchar(100) | Villa, Bungalov, Apart vb. |
| `Slug` | nvarchar(100) | SEO dostu isim |
| `Description` | nvarchar(500) | Açıklama |
| `IconUrl` | nvarchar(255) | Görsel/ikon yolu |
| `DisplayOrder` | int | Filtrelerde sıralama |
| `IsActive` | bit | Yayında mı |

---

## 6. Özellikler (Feature)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `Name` | nvarchar(100) | Havuz, Jakuzi, Sauna vb. |
| `Category` | nvarchar(50) | Özellik grubu (Genel, Mutfak, Bahçe) |

### PropertyFeatureMapping
| Alan | Tip | Açıklama |
|------|-----|----------|
| `PropertyId` | int (FK) | |
| `FeatureId` | int (FK) | |

---

## 7. Konum (Location)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `City` | nvarchar(100) | Şehir |
| `District` | nvarchar(100) | İlçe |
| `Region` | nvarchar(100) | Bölge |
| `Address` | nvarchar(255) | Adres |
| `Latitude` | decimal | Enlem |
| `Longitude` | decimal | Boylam |

---

## 8. Uzaklık Bilgileri (DistanceInfo)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `PropertyId` | int (FK) | |
| `PlaceName` | nvarchar(100) | Plaj, Havaalanı vb. |
| `DistanceKm` | decimal | Uzaklık km cinsinden |

---

## 9. Görseller (PropertyImages)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `PropertyId` | int (FK) | |
| `ImageUrl` | nvarchar(255) | Görsel yolu |
| `Category` | nvarchar(50) | Kategori (LivingRoom, Kitchen, Exterior vb.) |
| `IsMain` | bit | Kapak fotoğrafı mı |
| `Order` | int | Sıralama |
| `CreatedDate` | datetime | Eklenme tarihi |

---

## 10. Fiyat & Sezon (SeasonalPricing)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `Scope` | nvarchar(50) | Property veya Unit |
| `ScopeId` | int | İlişkilendirilen ID |
| `StartDate` | datetime | Başlangıç |
| `EndDate` | datetime | Bitiş |
| `PricePerNight` | decimal | Gecelik fiyat |
| `Currency` | nvarchar(10) | Para birimi |

---

## 11. Rezervasyon Kuralları (StayRule)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `Scope` | nvarchar(50) | Property/Unit |
| `ScopeId` | int | |
| `MinNights` | int | Minimum gece |
| `MaxNights` | int | Maksimum gece |
| `CheckInDays` | nvarchar(50) | Uygun günler (ör: Cumartesi) |
| `GapNights` | int | Boşluk geceleri |

---

## 12. Ek Ücretler (ExtraFee)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `Scope` | nvarchar(50) | Property/Unit |
| `ScopeId` | int | |
| `Name` | nvarchar(100) | Temizlik Ücreti vb. |
| `Type` | nvarchar(20) | Flat/Percent |
| `Amount` | decimal | Ücret miktarı |
| `Mandatory` | bit | Zorunlu mu? |

---

## 13. Rezervasyon (Reservation)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `UnitId` | int (FK) | |
| `PropertyId` | int (FK) | |
| `UserId` | string (FK → AspNetUsers) | Müşteri |
| `CheckInDate` | datetime | Giriş tarihi |
| `CheckOutDate` | datetime | Çıkış tarihi |
| `GuestCount` | int | Kişi sayısı |
| `Status` | nvarchar(30) | Durum |
| `PolicyId` | int (FK) | İptal politikası |
| `CreatedDate` | datetime | Kayıt zamanı |

---

## 14. Fiyat Kırılımı (ReservationPriceBreakdown)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `ReservationId` | int (FK) | |
| `LineType` | nvarchar(50) | Nightly / Fee / Discount |
| `Name` | nvarchar(100) | Açıklama |
| `Qty` | int | Adet |
| `UnitPrice` | decimal | Birim fiyat |
| `Total` | decimal | Toplam |

---

## 15. Ödeme (Payment)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `ReservationId` | int (FK) | |
| `Amount` | decimal | Tutar |
| `Method` | nvarchar(20) | Kredi Kartı, Havale |
| `Status` | nvarchar(20) | Paid, Failed, Pending |
| `TransactionRef` | nvarchar(50) | Ödeme sağlayıcı referansı |
| `PaidAt` | datetime | Ödeme zamanı |

---

## 16. Politikalar (Policy)

| Alan | Tip | Açıklama |
|------|-----|----------|
| `Id` | int (PK) | |
| `PolicyType` | nvarchar(20) | Cancel / House |
| `Name` | nvarchar(100) | Politika adı |
| `Terms` | nvarchar(max) | Şartlar |
| `IsVisible` | bit | Kullanıcıya gösterilsin mi |

---

## 17. İçerik Yönetimi (CMS)

**Pages, SeoContents, BlogPost, BlogCategory, BlogComment** tabloları bulunur.

- Statik sayfalar (Hakkımızda, SSS, KVKK)
- SEO metinleri (Anasayfa blokları)
- Blog yazıları, kategoriler, yorumlar

---

## 18. Kullanıcı Etkileşimi

| Tablo | Açıklama |
|--------|-----------|
| **Review** | Kullanıcı yorumları |
| **Question** | Soru-cevap sistemi |
| **Favorite** | Favori kayıtları |
| **Message** | Kullanıcı ↔️ Tedarikçi mesajları |
| **Dispute** | Rezervasyon anlaşmazlıkları |

---

## 19. Kampanya & Operasyonel

| Tablo | Açıklama |
|--------|-----------|
| **Coupon** | Kupon yönetimi |
| **Banner** | Görsel banner alanları |
| **AuditLog** | Sistem değişiklik geçmişi |

---

## 20. İlişki Özeti (ER Mantığı)

- 1 **Supplier** → N **Property**  
- 1 **Property** → N **Unit**, N **Image**, N **Reservation**  
- N **Property** ↔ N **Feature**  
- 1 **PropertyType** → N **Property**  
- 1 **Reservation** → N **Payment**, N **Breakdown**  
- 1 **User** → N **Reservation**, N **Review**, N **Favorite**

---

## 21. Performans ve Index Önerileri
- `IX_Property_LocationId`
- `IX_Reservation_CheckIn_CheckOut`
- `IX_Payment_ReservationId`
- `IX_PropertyType_Slug`
- `IX_PropertyImages_Category`

---

## 22. Gelecek Genişletme Alanları
- Çoklu dil desteği (`PropertyTranslation`)
- Dinamik fiyatlama algoritması
- Entegrasyon log tablosu (`IntegrationLogs`)
- Takvim senkronizasyonu (`ExternalCalendarLinks`)

---

Bu yapı, hem **admin paneli** hem **kullanıcı/supplier tarafı** için tüm iş akışlarını karşılayacak şekilde optimize edilmiştir.