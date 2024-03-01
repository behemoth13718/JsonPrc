using System;

namespace SphaeraJsonRpc.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ErrorEnumDescriptionAttribute : Attribute
    {
        public string DescriptionError { get; set; }
        
        public ErrorEnumDescriptionAttribute(string description)
        {
            DescriptionError = description;
        }
    }
}