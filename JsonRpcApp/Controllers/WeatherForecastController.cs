using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SphaeraJsonRpc.Protocol;
using SphaeraJsonRpc.Protocol.Interfaces;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;

namespace JsonRpcApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IJsonRpc _jsonRpc;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IJsonRpc jsonRpc)
        {
            _logger = logger;
            _jsonRpc = jsonRpc;
        }

        [HttpGet]
        public async Task<IActionResult> SendMessageRpc()
        {
            var obj = new WeatherForecast() { Date = DateTime.Now, Summary = "dfgdfgdf", TemperatureC = 456465};
            var res = await _jsonRpc
                .SetUrlService("http://localhost:8080")
                .SendMessageAsync("test", new List<WeatherForecast>() { obj }, new RequestId("756456"));

            if (res.TypeMessage == EnumTypeMessage.Succsess)
            {
                try
                {
                    WeatherForecast result = res.GetPayload<WeatherForecast>().FirstOrDefault();
                    if(result != null)
                        return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }

            if (res.TypeMessage == EnumTypeMessage.Error)
                return BadRequest(((JsonRpcError)res).ToString());

            return Ok("Нет данных");
        }
    }
}