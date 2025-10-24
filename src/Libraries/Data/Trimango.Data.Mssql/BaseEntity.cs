using Maggsoft.Data.Mssql;

namespace Trimango.Data.Mssql
{
    /// <summary>
    /// Tüm entity'lerin türetileceği temel sınıf
    /// Maggsoft framework'ünün BaseEntity sınıfından türetilmiştir
    /// </summary>
    public abstract class BaseEntity : Maggsoft.Data.Mssql.BaseEntity
    {
        /// <summary>
        /// Entity'nin silinmiş durumdan geri yüklenmesi için kullanılan method
        /// </summary>
        public virtual void Restore()
        {
            IsDeleted = false;
        }
    }
}
