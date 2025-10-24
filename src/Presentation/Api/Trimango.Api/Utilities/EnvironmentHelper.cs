namespace Trimango.Api.Utilities;

/// <summary>
/// Environment variable işlemleri için yardımcı sınıf
/// </summary>
public static class EnvironmentHelper
{
    /// <summary>
    /// Environment variable placeholder'larını resolve eder
    /// ${ENV_VAR} formatındaki değerleri environment variable'lardan okur
    /// </summary>
    /// <param name="value">Resolve edilecek değer</param>
    /// <returns>Resolve edilmiş değer veya orijinal değer</returns>
    /// <example>
    /// var connectionString = EnvironmentHelper.ResolveEnvironmentVariable("${DATABASE_CONNECTION_STRING}");
    /// // Eğer DATABASE_CONNECTION_STRING environment variable varsa onun değerini döndürür
    /// // Yoksa "${DATABASE_CONNECTION_STRING}" stringini döndürür
    /// </example>
    public static string ResolveEnvironmentVariable(string value)
    {
        if (string.IsNullOrEmpty(value)) 
            return value;

        // ${ENV_VAR} formatındaki placeholder'ları resolve et
        if (value.StartsWith("${") && value.EndsWith("}"))
        {
            var envVarName = value.Substring(2, value.Length - 3);
            var envValue = Environment.GetEnvironmentVariable(envVarName);
            return envValue ?? value; // Environment variable yoksa orijinal değeri döndür
        }

        return value;
    }

    /// <summary>
    /// Birden fazla environment variable'ı resolve eder
    /// </summary>
    /// <param name="values">Resolve edilecek değerler</param>
    /// <returns>Resolve edilmiş değerler</returns>
    public static Dictionary<string, string> ResolveMultiple(Dictionary<string, string> values)
    {
        var resolved = new Dictionary<string, string>();
        
        foreach (var kvp in values)
        {
            resolved[kvp.Key] = ResolveEnvironmentVariable(kvp.Value);
        }
        
        return resolved;
    }

    /// <summary>
    /// Environment variable'ın tanımlı olup olmadığını kontrol eder
    /// </summary>
    public static bool IsEnvironmentVariableSet(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Gerekli environment variable'ların tanımlı olup olmadığını kontrol eder
    /// </summary>
    /// <param name="requiredVariables">Zorunlu environment variable adları</param>
    /// <returns>Tüm variable'lar tanımlıysa true</returns>
    public static bool ValidateRequiredEnvironmentVariables(params string[] requiredVariables)
    {
        return requiredVariables.All(IsEnvironmentVariableSet);
    }

    /// <summary>
    /// Eksik environment variable'ları döndürür
    /// </summary>
    public static List<string> GetMissingEnvironmentVariables(params string[] requiredVariables)
    {
        return requiredVariables
            .Where(v => !IsEnvironmentVariableSet(v))
            .ToList();
    }
}

