using System;

namespace Utils.Exceptions
{
    public class FileNotFound : Exception
    {
        public FileNotFound(string message)
        {
            MessageError = message;
        }

        public string MessageError { get; protected set; }
    }
}