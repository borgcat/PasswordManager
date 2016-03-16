using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ApiExplorer;
using Swashbuckle.SwaggerGen;

namespace PasswordManager.Api.Swagger
{
    public class AddMasterKeyHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            try
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<IParameter>();

                var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
                var serviceFilters = filterPipeline.Select(filterInfo => filterInfo.Filter).OfType<ServiceFilterAttribute>();

                var serviceFilterAttributes = serviceFilters as IList<ServiceFilterAttribute> ?? serviceFilters.ToList();

                var hasMasterKey = serviceFilterAttributes.Any(a => a.ServiceType.FullName == "PasswordManager.Api.MasterKeyHeaderFilter");

                var hasEnsureMasterKey = serviceFilterAttributes.Any(a => a.ServiceType.FullName == "PasswordManager.Api.EnsureMasterKeyHeaderFilter");

                if (hasMasterKey || hasEnsureMasterKey)
                {
                    operation.Parameters.Add(new NonBodyParameter
                    {
                        Name = "X-MasterKey",
                        In = "header",
                        Description = "Master Key for api key management",
                        Required = false,
                        Type = "string",
                    });

                    operation.Parameters.Add(new NonBodyParameter
                    {
                        Name = "X-Key",
                        In = "header",
                        Description = "Encrypted Master Key for api key management",
                        Required = false,
                        Type = "string",
                    });
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                throw;
            }
          
        }
    }
}
