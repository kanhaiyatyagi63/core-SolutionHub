using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ST.SolutionHub.Extensions
{
    public static class Extension
    {
        public static IEnumerable<string> GetErrorList(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage.Replace("'", "")));
        }
    }
}
