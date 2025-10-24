namespace Trimango.Data.Mssql.Enums
{
    /// <summary>
    /// Ek ücret türleri enum'u
    /// </summary>
    public enum ExtraFeeType
    {
        Flat = 1,           // Sabit tutar
        Percent = 2,        // Yüzde
        PerPerson = 3,      // Kişi başı
        PerNight = 4,       // Gece başı
        PerWeek = 5,        // Hafta başı
        PerMonth = 6        // Ay başı
    }
}
