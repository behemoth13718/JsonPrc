using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SphaeraJsonRpc.Exceptions;
using SphaeraJsonRpc.Extensions;
using SphaeraJsonRpc.Protocol.Enums;
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
        private readonly string[] methods = new string[] {"sum"};

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IJsonRpc jsonRpc)
        {
            _logger = logger;
            _jsonRpc = jsonRpc;
        }

        [HttpGet]
        public async Task<IActionResult> SendMessageRpc()
        {
            JsonRpcResult res;
            try
            {
                var obj = new WeatherForecast() { Date = DateTime.Now, Summary = "dfgdfgdf", TemperatureC = 456465 };
                res = await _jsonRpc
                    .SendMessageAsync("get-temperature1", obj, 45645);
                
                
                return Ok(res.TryReadJsonRpcResultMessage<long>());
            }
            catch (JsonRpcHandlerMessageExeption e)
            {
                return BadRequest(e.JsonRpcError.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}