using System;
using System.ComponentModel;
using System.Reflection;

namespace SphaeraJsonRpc.Extensions
{
    public static class ExtentionsCommon
    {
        //
        // Сводка:
        //     Получить сообщение из атрибута Description
        //
        // Параметры:
        //   value:
        public static string GetDescription(this Enum value)
        {
            string text = value.ToString();
            return ((DescriptionAttribute)value.GetType().GetField(text).GetCustomAttribute(typeof(DescriptionAttribute), inherit: false))?.Description ?? text;
        }
        //
        // Сводка:
        //     Получить код из Enum
        //
        // Параметры:
        //   value:
        public static int GetCode<T>(this T value) where T : Enum, IConvertible
        {
            try
            {
                return value.ToInt32(null);
            }
            catch (OverflowException innerException)
            {
                throw new InvalidOperationException($"Sphaera.Http.EnumExtensions. Enum is not 'uint32' type. value: {value}", innerException);
            }
        }
    }
}