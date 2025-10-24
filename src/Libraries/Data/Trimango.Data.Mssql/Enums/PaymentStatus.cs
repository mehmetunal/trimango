namespace Trimango.Data.Mssql.Enums
{
    /// <summary>
    /// Ödeme durumları enum'u
    /// </summary>
    public enum PaymentStatus
    {
        Pending = 1,        // Beklemede
        Processing = 2,     // İşleniyor
        Completed = 3,      // Tamamlandı
        Failed = 4,         // Başarısız
        Cancelled = 5,      // İptal edildi
        Refunded = 6,       // İade edildi
        PartiallyRefunded = 7 // Kısmi iade
    }
}
