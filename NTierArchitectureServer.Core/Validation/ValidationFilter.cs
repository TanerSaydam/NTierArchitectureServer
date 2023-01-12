using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NTierArchitectureServer.Core.Validation
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(p => p.Value.Errors.Any())
                    .ToDictionary(e => e.Key, e => e.Value.Errors.Select(s => s.ErrorMessage))
                    .ToArray();
                context.Result = new BadRequestObjectResult(errors);
                return;
            }

            await next();
        }
    }
}
