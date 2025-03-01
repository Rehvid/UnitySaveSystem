namespace RehvidGames.Factory
{
    using System;
    using DataHandler;
    using Enums;
    using Providers;

    public static class DataHandlerFactory
    {
        public static IDataHandler Create(DataHandlerType handlerType, IHandlerProvider handlerProvider)
        {
            return handlerType switch
            {
                DataHandlerType.File => new FileDataHandler(handlerProvider),
                _ => throw new ArgumentOutOfRangeException(nameof(handlerType), handlerType, null)
            };
        }
    }
}