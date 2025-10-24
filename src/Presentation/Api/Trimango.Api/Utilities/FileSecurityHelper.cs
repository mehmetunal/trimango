using Microsoft.AspNetCore.Http;

namespace TrinkEmlak.Api.Utilities;

/// <summary>
/// Dosya güvenlik kontrolü için yardımcı sınıf
/// Magic bytes (file signature) kontrolü ile gerçek dosya türünü doğrular
/// </summary>
public static class FileSecurityHelper
{
    /// <summary>
    /// Desteklenen dosya türleri ve magic bytes imzaları
    /// </summary>
    private static readonly Dictionary<string, List<byte[]>> FileSignatures = new()
    {
        // Image formats
        {
            ".jpg", new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, // JPEG (JFIF)
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 }, // JPEG (Exif)
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 }, // JPEG (JPEG/JFIF)
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }  // JPEG
            }
        },
        {
            ".jpeg", new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 }
            }
        },
        {
            ".png", new List<byte[]>
            {
                new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } // PNG
            }
        },
        {
            ".gif", new List<byte[]>
            {
                new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, // GIF87a
                new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }  // GIF89a
            }
        },
        {
            ".webp", new List<byte[]>
            {
                new byte[] { 0x52, 0x49, 0x46, 0x46 } // RIFF (WebP başlangıç)
            }
        },
        // Video formats
        {
            ".mp4", new List<byte[]>
            {
                new byte[] { 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D }, // ftypisom
                new byte[] { 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 }  // ftypmp42
            }
        },
        {
            ".avi", new List<byte[]>
            {
                new byte[] { 0x52, 0x49, 0x46, 0x46 } // RIFF (AVI)
            }
        },
        {
            ".mov", new List<byte[]>
            {
                new byte[] { 0x66, 0x74, 0x79, 0x70, 0x71, 0x74 } // ftypqt
            }
        },
        // Document formats
        {
            ".pdf", new List<byte[]>
            {
                new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D } // %PDF-
            }
        }
    };

    /// <summary>
    /// Dosyanın gerçek türünü magic bytes kontrolü ile doğrular
    /// </summary>
    /// <param name="file">Kontrol edilecek dosya</param>
    /// <param name="allowedExtensions">İzin verilen uzantılar</param>
    /// <returns>Dosya geçerliyse true, aksi halde false</returns>
    public static bool IsValidFileType(IFormFile file, string[] allowedExtensions)
    {
        if (file == null || file.Length == 0)
            return false;

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        // Uzantı kontrolü
        if (!allowedExtensions.Contains(fileExtension))
            return false;

        try
        {
            // Magic bytes kontrolü
            using var reader = new BinaryReader(file.OpenReadStream());
            
            // İlk 8 byte'ı oku (çoğu dosya türü için yeterli)
            var headerBytes = reader.ReadBytes(8);

            // Dosya türüne ait imzaları kontrol et
            if (!FileSignatures.TryGetValue(fileExtension, out var validSignatures))
            {
                // İmza yoksa sadece uzantı kontrolü yap (güvenlik riski!)
                return true;
            }

            // En az bir imza eşleşmeli
            foreach (var signature in validSignatures)
            {
                if (headerBytes.Take(signature.Length).SequenceEqual(signature))
                {
                    return true;
                }
            }

            // WebP için özel kontrol (RIFF + WEBP)
            if (fileExtension == ".webp" && headerBytes.Length >= 12)
            {
                var riffSignature = new byte[] { 0x52, 0x49, 0x46, 0x46 }; // RIFF
                var webpSignature = new byte[] { 0x57, 0x45, 0x42, 0x50 }; // WEBP
                
                if (headerBytes.Take(4).SequenceEqual(riffSignature))
                {
                    // Offset 8'den sonraki 4 byte WEBP olmalı
                    reader.BaseStream.Seek(8, SeekOrigin.Begin);
                    var webpBytes = reader.ReadBytes(4);
                    return webpBytes.SequenceEqual(webpSignature);
                }
            }

            return false;
        }
        catch
        {
            // Dosya okunamadıysa güvenlik gereği reddet
            return false;
        }
    }

    /// <summary>
    /// Resim dosyası kontrolü
    /// </summary>
    public static bool IsValidImageFile(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        return IsValidFileType(file, allowedExtensions);
    }

    /// <summary>
    /// Video dosyası kontrolü
    /// </summary>
    public static bool IsValidVideoFile(IFormFile file)
    {
        var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".webm" };
        return IsValidFileType(file, allowedExtensions);
    }

    /// <summary>
    /// PDF dosyası kontrolü
    /// </summary>
    public static bool IsValidPdfFile(IFormFile file)
    {
        var allowedExtensions = new[] { ".pdf" };
        return IsValidFileType(file, allowedExtensions);
    }

    /// <summary>
    /// Dosya boyutu kontrolü
    /// </summary>
    /// <param name="file">Kontrol edilecek dosya</param>
    /// <param name="maxSizeInMB">Maksimum boyut (MB)</param>
    /// <returns>Boyut geçerliyse true</returns>
    public static bool IsValidFileSize(IFormFile file, int maxSizeInMB)
    {
        if (file == null)
            return false;

        var maxSizeInBytes = maxSizeInMB * 1024 * 1024;
        return file.Length > 0 && file.Length <= maxSizeInBytes;
    }

    /// <summary>
    /// Dosya adı güvenlik kontrolü - zararlı karakterleri temizle
    /// </summary>
    public static string SanitizeFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Guid.NewGuid().ToString();

        // Geçersiz karakterleri temizle
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));

        // Path traversal saldırılarını önle
        sanitized = sanitized.Replace("..", "_").Replace("/", "_").Replace("\\", "_");

        // Maksimum uzunluk kontrolü (255 karakter)
        if (sanitized.Length > 255)
        {
            var extension = Path.GetExtension(sanitized);
            var nameWithoutExt = Path.GetFileNameWithoutExtension(sanitized);
            sanitized = nameWithoutExt.Substring(0, 255 - extension.Length) + extension;
        }

        return sanitized;
    }

    /// <summary>
    /// Güvenli dosya adı oluştur (GUID bazlı)
    /// </summary>
    public static string GenerateSecureFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName).ToLowerInvariant();
        return $"{Guid.NewGuid()}{extension}";
    }
}

