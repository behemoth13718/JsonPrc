using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SphaeraJsonRpc.Protocol.Enums;
using SphaeraJsonRpc.Protocol.Interfaces;
using SphaeraJsonRpc.Protocol.ModelMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.ErrorMessage;
using SphaeraJsonRpc.Protocol.ModelMessage.RequestMessage;

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

        [HttpPost]
        public async Task<IActionResult> ReciveMessage()
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var reciveMessage = await reader.ReadToEndAsync();

            var requst = JsonConvert.DeserializeObject<JsonRpcRequestServer>(reciveMessage);
            
            var dataMessage = requst.GetPayload<WeatherForecast>();
            var method = requst.Method;
            var version = requst.Version;
            var id = requst.RequestId;

            if (methods.Contains(method))
                return Ok(JsonConvert.SerializeObject(new JsonRpcResult(){RequestId = id, Version = version, Result = new string[] {"sdfsd","sfsdf"}}));
            
            
            return BadRequest(JsonConvert.SerializeObject(new JsonRpcError(){RequestId = id, Version = version, Error = new ErrorDetail(){ Code = EnumJsonRpcErrorCode.MethodNotFound, Message = "Method not found"}}));
        }
    }
}