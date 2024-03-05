using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using SphaeraJsonRpc.Attributes;
using SphaeraJsonRpc.Models;
using SphaeraJsonRpc.Protocol.Enums;

namespace SphaeraJsonRpc.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class DocumentaionRpcServerController : ControllerBase
    {
        [HttpGet("Error-info")]
        public IActionResult ErrorInfo()
        {

            var listInfoDetail = typeof(EnumJsonRpcErrorCode)
                .GetFields()
                .Where(x => x.Name != "value__")
                .Select(enumField => 
                    new ErrorInfoDetail()
                    {
                        Code = (int)(EnumJsonRpcErrorCode)Enum.Parse(typeof(EnumJsonRpcErrorCode), enumField.Name), 
                        Message = enumField.GetCustomAttribute<DescriptionAttribute>()?.Description, 
                        Description = enumField.GetCustomAttribute<ErrorEnumDescriptionAttribute>()?.DescriptionError,
                    }).ToList();

            return Ok(listInfoDetail);
        }
    }
    
    
}