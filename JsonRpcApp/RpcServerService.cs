using System;

namespace JsonRpcApp
{
    public class RpcServerService //: IRpcService
    {
        public int Sum(WeatherForecast data) => data.TemperatureC;
        public DateTime GetDataTime() => DateTime.Now;
        public int Sum_1(int onePar, int twoPar) => onePar + twoPar;
        public int Subtract(int subtrahend, int minuend) => minuend - subtrahend;
        public string Foobar(string subtrahend, string minuend) => subtrahend + "----" + minuend;
        
    }

    // public interface IRpcService
    // {
    //     [JsonRpcMethod(Name = "sum")]
    //     public int Sum(WeatherForecast data);
    // }
}