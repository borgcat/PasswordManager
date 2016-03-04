using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Primitives;

namespace PasswordManager.Api
{
    public static class HttpResponseMessageExtensions
    {
        public static T GetFirstHeaderValueOrDefault<T>(this HttpRequest request, string headerKey)
        {
            var toReturn = default(T);

            StringValues headerValues;

            if (request.Headers.TryGetValue(headerKey, out headerValues))
            {
                var valueString = headerValues.FirstOrDefault();
                if (valueString != null)
                {
                    return (T)Convert.ChangeType(valueString, typeof(T));
                }
            }

            return toReturn;
        }
    }
}
