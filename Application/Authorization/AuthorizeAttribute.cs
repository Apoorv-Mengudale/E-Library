﻿
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new { message = "Unauthorized" })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}