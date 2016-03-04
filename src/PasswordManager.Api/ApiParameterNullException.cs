using System;

namespace PasswordManager.Api
{
    public class ApiParameterNullException : Exception
    {
        public ApiParameterNullException(string masterkeyWasNotProvidedViaHttpHeader) : base(masterkeyWasNotProvidedViaHttpHeader) { }
    }
}