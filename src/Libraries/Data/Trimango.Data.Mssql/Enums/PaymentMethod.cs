namespace Trimango.Data.Mssql.Enums
{
    /// <summary>
    /// Ödeme yöntemleri enum'u
    /// </summary>
    public enum PaymentMethod
    {
        CreditCard = 1,     // Kredi kartı
        DebitCard = 2,      // Banka kartı
        BankTransfer = 3,   // Havale/EFT
        Cash = 4,           // Nakit
        Check = 5,          // Çek
        PayPal = 6,         // PayPal
        Stripe = 7,         // Stripe
        Iyzico = 8,         // Iyzico
        PayTR = 9,          // PayTR
        Other = 99          // Diğer
    }
}
