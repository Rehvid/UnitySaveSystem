namespace RehvidGames.Exceptions
{
    using System;

    public class SerializerException: Exception
    {
        public SerializerException(string message, Exception inner) : base(message, inner) { }
    }
}