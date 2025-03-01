namespace RehvidGames.Exceptions
{
    using System;
    
    public class BackupException: Exception
    {
        public BackupException(string message, Exception inner) : base(message, inner) { }
    }
}