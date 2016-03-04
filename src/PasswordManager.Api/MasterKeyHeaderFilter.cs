using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Filters;
using PasswordManager.Core.Interfaces;

namespace PasswordManager.Api
{
    public class MasterKeyHeaderFilter : ActionFilterAttribute
    {
        private readonly IConfigurationSettings _settings;

        public MasterKeyHeaderFilter(IConfigurationSettings settings)
        {
            _settings = settings;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var encryptedMasterKey = context.HttpContext.Request.GetFirstHeaderValueOrDefault<string>("X-Key");
            var masterKey = context.HttpContext.Request.GetFirstHeaderValueOrDefault<string>("X-MasterKey");

            if (String.IsNullOrWhiteSpace(encryptedMasterKey) && String.IsNullOrWhiteSpace(masterKey))
                throw new ApiParameterNullException("MasterKey was not provided via http header");

            if (!String.IsNullOrWhiteSpace(masterKey))
                _settings.MasterKey = masterKey;

            if (!String.IsNullOrWhiteSpace(encryptedMasterKey))
                _settings.EncryptedMasterKey = encryptedMasterKey;

            base.OnActionExecuting(context);
        }
    }
}
