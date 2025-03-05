using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Routing;
using EcologySite.Models.Ecology;

namespace EcologySite.Services.Apis;

public class ApiExplorerService
{
    public List<ApiDescriptionViewModel> GetApiDescriptions(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t => typeof(ControllerBase).IsAssignableFrom(t))
            .SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            .Where(m => m.IsPublic && m.DeclaringType != typeof(object))
            .Select(m => new ApiDescriptionViewModel
            {
                Controller = m.DeclaringType.Name,
                Action = m.Name,
                HttpMethod = GetHttpMethod(m),
                Route = GetRoute(m)
            })
            .ToList();
    }

    private string GetHttpMethod(MethodInfo method)
    {
        var attributes = method.GetCustomAttributes(typeof(HttpMethodAttribute), true);
        if (attributes.Length > 0)
        {
            return ((HttpMethodAttribute)attributes.First()).HttpMethods.First();
        }

        return "GET"; // Assume GET if not specified
    }

    private string GetRoute(MethodInfo method)
    {
        var routeAttr = method.GetCustomAttribute<RouteAttribute>();
        return routeAttr != null ? routeAttr.Template : method.Name;
    }
}
