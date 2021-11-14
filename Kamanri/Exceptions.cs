using System;
using System.Collections.Generic;
using System.Text;

namespace Kamanri
{
    internal class KamanriException : Exception
    {
        public KamanriException() : base() { }
        public KamanriException(string message) : base(message) { }
        public KamanriException(string message, Exception exception) : base($"{message}\nCaused By : ", exception) { }
    }

    internal class DataBaseException : KamanriException 
    {
        public DataBaseException() : base() { }
        public DataBaseException(string message) : base(message) { }
        public DataBaseException(string message, Exception exception) : base(message, exception) { }
    }
    internal class DataBaseModelException : DataBaseException 
    {
        public DataBaseModelException() : base() { }
        public DataBaseModelException(string message) : base(message) { }
        public DataBaseModelException(string message, Exception exception) : base(message, exception) { }
    }
    internal class ExtensionException : KamanriException
    {
        public ExtensionException() : base() { }
        public ExtensionException(string message) : base(message) { }
        public ExtensionException(string message, Exception exception) : base(message, exception) { }
    }
    internal class ByteArrayExtensionException : ExtensionException
    {
        public ByteArrayExtensionException() : base() { }
        public ByteArrayExtensionException(string message) : base(message) { }
        public ByteArrayExtensionException(string message, Exception exception) : base(message, exception) { }
    }
    internal class ObjectExtensionException : ExtensionException 
    {
        public ObjectExtensionException() : base() { }
        public ObjectExtensionException(string message) : base(message) { }
        public ObjectExtensionException(string message, Exception exception) : base(message, exception) { }
    }
    internal class StringExtensionException : ExtensionException
    {
        public StringExtensionException() : base() { }
        public StringExtensionException(string message) : base(message) { }
        public StringExtensionException(string message, Exception exception) : base(message, exception) { }
    }
    internal class WebSocketExtensionException : ExtensionException
    {
        public WebSocketExtensionException() : base() { }
        public WebSocketExtensionException(string message) : base(message) { }
        public WebSocketExtensionException(string message, Exception exception) : base(message, exception) { }
    }
    internal class HttpException : KamanriException 
    {
        public HttpException() : base() { }
        public HttpException(string message) : base(message) { }
        public HttpException(string message, Exception exception) : base(message, exception) { }
    }
    internal class WebSocketException : KamanriException 
    {
        public WebSocketException() : base() { }
        public WebSocketException(string message) : base(message) { }
        public WebSocketException(string message, Exception exception) : base(message, exception) { }
    }
    internal class WebSocketModelException : WebSocketException 
    {
        public WebSocketModelException() : base() { }
        public WebSocketModelException(string message) : base(message) { }
        public WebSocketModelException(string message, Exception exception) : base(message, exception) { }
    }

}
