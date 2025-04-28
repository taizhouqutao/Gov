using Common;
using DAL.Modles;
using DAL;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
namespace BLL
{
    public class BllWeather
    {
        public IConfiguration configuration;
        public BllWeather()
        {
            configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }
        private DalWeather dal=new DalWeather();

        public async Task<Weather> GetTodayWeatherAsync() {
            Weather Res=null; 
            var Now=DateTime.Now;
            var NeedRefTime=Now.AddMinutes(-5);
            var weather = (await dal.GetWeathersAsync(new WeatherReqDto(){
                StartUpdateTime=NeedRefTime,
                WeatherDate=Now.Date
            })).OrderByDescending(i=>i.UpdateTime).Take(1).FirstOrDefault();
            if(weather==null)
            {
                Res=await ReflashWeather(Now);
            }
            else {
                Res=weather;
            }
            return Res;
        } 

        public async Task<Weather> ReflashWeather(DateTime Now)
        {
            string AK = configuration["BaiduMapApp:AK"];
            var strWeatherRes = await HttpHelp.GetAsync($"https://api.map.baidu.com/weather/v1/?district_id=330400&data_type=all&ak={AK}");
            var WeatherRes = JsonConvert.DeserializeObject<BaiDuWeather>(strWeatherRes);
            if(WeatherRes.status!=0)
            {
                throw new Exception(WeatherRes.message);
            }
            var TodayWather= WeatherRes.result.forecasts.FirstOrDefault();
            var res = await dal.AddWeatherAsync(new Weather() {
                CreateTime= Now,
                CreateUserId=1,
                High = TodayWather.high,
                IfDel=0,
                Low=TodayWather.low,
                TextDay=TodayWather.text_day,
                TextNight=TodayWather.text_night,
                UpdateTime=Now,
                UpdateUserId=1,
                WcDay=TodayWather.wc_day,
                WdDay=TodayWather.wd_day,
                WcNight=TodayWather.wc_night,
                WdNight=TodayWather.wd_night,
                WeatherDate=Now.Date,
                Week= TodayWather.week
            });
            return res;
        }
    }
}