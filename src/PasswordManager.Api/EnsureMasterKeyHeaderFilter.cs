using System;
using Microsoft.AspNet.Mvc.Filters;

namespace PasswordManager.Api
{
    public class EnsureMasterKeyHeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var masterKey = context.HttpContext.Request.GetFirstHeaderValueOrDefault<string>("X-MasterKey");
            if (String.IsNullOrWhiteSpace(masterKey))
                throw new ApiParameterNullException("MasterKey was not provided via http header");

            base.OnActionExecuting(context);
        }
    }
}