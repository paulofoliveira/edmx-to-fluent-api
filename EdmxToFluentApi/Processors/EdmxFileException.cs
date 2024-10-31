using System;

namespace EdmxToFluentApi.Processors
{
    public class EdmxFileException : Exception
    {
        public EdmxFileException() { }
        public EdmxFileException(string message) : base(message) { }
        public EdmxFileException(string message, Exception ex) : base(message, ex) { }
    }
}
