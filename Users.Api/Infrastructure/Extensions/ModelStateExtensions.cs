using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Users.Api.Infrastructure.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddErrors(this ModelStateDictionary modelStateDictionary, IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }
        }
    }
}