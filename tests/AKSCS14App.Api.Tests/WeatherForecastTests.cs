namespace CS14App.Api.Tests;

public class WeatherForecastTests
{
    [Fact]
    public void Summary_IsTrimmed_WhenSetWithSurroundingWhitespace()
    {
        var forecast = new WeatherForecast { Summary = "  Mild  " };

        Assert.Equal("Mild", forecast.Summary);
    }

    [Fact]
    public void Summary_IsNull_WhenSetToNull()
    {
        var forecast = new WeatherForecast { Summary = null };

        Assert.Null(forecast.Summary);
    }
}
