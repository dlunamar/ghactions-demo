using GhActionsDemo.Core;

namespace GhActionsDemo.Core.Tests;

public class RiskBandCalculatorTests
{
    [Theory]
    [InlineData(1, 1, RiskBand.Low)]
    [InlineData(2, 2, RiskBand.Low)]      // 4 -> borde superior de Low
    [InlineData(2, 3, RiskBand.Medium)]   // 6
    [InlineData(3, 3, RiskBand.Medium)]   // 9 -> borde superior de Medium
    [InlineData(3, 4, RiskBand.High)]     // 12
    [InlineData(4, 4, RiskBand.High)]     // 16 -> borde superior de High
    [InlineData(4, 5, RiskBand.Critical)] // 20
    [InlineData(5, 5, RiskBand.Critical)] // 25 -> máximo
    public void Calculate_WithBoundaries_ReturnsExpectedBand(int p, int g, RiskBand expected)
    {
        Assert.Equal(expected, RiskBandCalculator.Calculate(p, g));
    }

    [Theory]
    [InlineData(0, 3)]
    [InlineData(6, 3)]
    [InlineData(3, 0)]
    [InlineData(3, 6)]
    public void Calculate_OutOfRange_Throws(int p, int g)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RiskBandCalculator.Calculate(p, g));
    }
}
