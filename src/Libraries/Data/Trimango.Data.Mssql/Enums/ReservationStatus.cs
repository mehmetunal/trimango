namespace Trimango.Data.Mssql.Enums
{
    /// <summary>
    /// Rezervasyon durumları enum'u
    /// </summary>
    public enum ReservationStatus
    {
        Pending = 1,            // Beklemede
        Confirmed = 2,          // Onaylandı
        CheckedIn = 3,          // Giriş yapıldı
        CheckedOut = 4,         // Çıkış yapıldı
        Completed = 5,          // Tamamlandı
        CancelledByGuest = 6,   // Misafir tarafından iptal
        CancelledBySupplier = 7, // Tedarikçi tarafından iptal
        CancelledByAdmin = 8,   // Admin tarafından iptal
        Refunded = 9,           // İade edildi
        NoShow = 10             // Gelmedi
    }
}
