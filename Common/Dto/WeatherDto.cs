namespace Common
{
    [Serializable]
    public class WeatherReqDto
    {
        public DateTime? StartUpdateTime { get; set; }

        public DateTime? WeatherDate { get; set; }
    }

    [Serializable]
    public class BaiDuWeather
    {
        public int status { get; set; }

        public string? message { get; set; }

        public BaiDuWeatherResult? result { get; set; }
    }

    [Serializable]
    public class BaiDuWeatherResult
    {
        public required List<BaiDuWeatherForecasts> forecasts { get; set; }
    }

    [Serializable]
    public class BaiDuWeatherForecasts
    {
        public required string text_day { get; set; }

        public required string text_night { get; set; }

        public required decimal high { get; set; }

        public required decimal low { get; set; }

        public required string wc_day { get; set; }

        public required string wd_day { get; set; }

        public required string wc_night { get; set; }

        public required string wd_night { get; set; }

        public required string date { get; set; }

        public required string week { get; set; }
    }
}