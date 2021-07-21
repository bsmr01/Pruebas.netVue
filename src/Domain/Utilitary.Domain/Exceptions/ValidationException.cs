namespace Ibero.Services.Utilitary.Domain.Exceptions
{
    using System;

    public class ValidationException : Exception
    {
        public ValidationException(string objectName, string messenger)
            : base($"Entity \"{objectName}\" ({messenger}).")
        {
        }
    }
}
