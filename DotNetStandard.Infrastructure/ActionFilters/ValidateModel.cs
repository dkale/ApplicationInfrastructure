using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DotNetStandard.Infrastructure.ActionFilters
{
    public class ValidateModel : FilterAttribute, IActionFilter
    {
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Where(modelStateItem => modelStateItem.Value.Errors.Any())
                    .ToDictionary(modelStateItem => modelStateItem.Key,
                        modelStateItem => modelStateItem.Value.Errors.Select(error => error.ErrorMessage));
                var response = string.Empty;
                foreach (var item in errors)
                {
                    foreach (var error in item.Value)
                    {
                        response += (error + "<br/>");
                    }
                }
                throw new Exception(response);
            }
            return await continuation();
        }
    }
}
