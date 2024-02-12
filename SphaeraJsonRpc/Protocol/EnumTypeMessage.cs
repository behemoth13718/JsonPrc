namespace SphaeraJsonRpc.Protocol
{
    public enum EnumTypeMessage
    {
        /// <summary>
        /// Успешный ответ от внешнего сервиса
        /// </summary>
        Succsess,
        /// <summary>
        /// Ошибка ответа от внешнего сервиса
        /// </summary>
        Error,
        /// <summary>
        /// Запрос к сервису
        /// </summary>
        Request
    }
}