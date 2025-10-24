namespace Trimango.Data.Mssql.Enums
{
    /// <summary>
    /// Politika türleri enum'u
    /// </summary>
    public enum PolicyType
    {
        Cancel = 1,         // İptal politikası
        House = 2,          // Ev kuralları
        CheckIn = 3,        // Giriş kuralları
        CheckOut = 4,       // Çıkış kuralları
        Pet = 5,            // Evcil hayvan kuralları
        Smoking = 6,        // Sigara kuralları
        Party = 7,          // Parti kuralları
        Other = 99          // Diğer
    }
}
