using System;
using System.Collections.Generic;
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
    public class ErrorInfoControllerBase : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
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