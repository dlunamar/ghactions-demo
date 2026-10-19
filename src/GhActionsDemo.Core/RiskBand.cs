namespace GhActionsDemo.Core;

/// <summary>
/// Clasificación de un riesgo en bandas, al estilo del cuantificador
/// del microservicio Conversia.GeneradorDoc (probabilidad x gravedad).
/// </summary>
public enum RiskBand
{
    Low,         // 1 - Aceptable
    Medium,      // 2 - Moderado
    High,        // 3 - Elevado
    Critical,    // 4 - Extremo
}

public static class RiskBandCalculator
{
    /// <summary>
    /// Calcula la banda de riesgo a partir de probabilidad (1-5) y gravedad (1-5).
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Si probabilidad o gravedad no están en el rango 1-5.
    /// </exception>
    public static RiskBand Calculate(int probability, int severity)
    {
        if (probability is < 1 or > 5)
            throw new ArgumentOutOfRangeException(nameof(probability), probability, "La probabilidad debe estar entre 1 y 5.");
        if (severity is < 1 or > 5)
            throw new ArgumentOutOfRangeException(nameof(severity), severity, "La gravedad debe estar entre 1 y 5.");

        var score = probability * severity;

        return score switch
        {
            <= 4 => RiskBand.Low,
            <= 9 => RiskBand.Medium,
            <= 16 => RiskBand.High,
            _ => RiskBand.Critical,
        };
    }
}
