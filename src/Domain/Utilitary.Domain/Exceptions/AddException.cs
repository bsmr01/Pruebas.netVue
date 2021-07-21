namespace Ibero.Services.Utilitary.Domain.Exceptions
{
    using System;

    public class AddException : Exception
    {
        public AddException(string name, object key, string message)
            : base($"Add entity \"{name}\" ({key}) failed. {message}")
        {
        }
    }
}