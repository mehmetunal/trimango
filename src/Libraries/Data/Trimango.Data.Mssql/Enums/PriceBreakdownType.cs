namespace Trimango.Data.Mssql.Enums
{
    /// <summary>
    /// Fiyat kırılım türleri enum'u
    /// </summary>
    public enum PriceBreakdownType
    {
        Nightly = 1,        // Gecelik fiyat
        Fee = 2,            // Ücret
        Discount = 3,       // İndirim
        Tax = 4,            // Vergi
        Service = 5,        // Hizmet ücreti
        Cleaning = 6,       // Temizlik ücreti
        Deposit = 7,        // Depozito
        Other = 99          // Diğer
    }
}
