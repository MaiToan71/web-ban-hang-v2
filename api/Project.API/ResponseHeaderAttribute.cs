using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Project.Enums;
using System.Security.Claims;

namespace Project.API
{
    public class ResponseHeaderAttribute : ActionFilterAttribute
    {
        private readonly string _name;
        private readonly string _value;

        public ResponseHeaderAttribute(string name, string value) =>
            (_name, _value) = (name, value);

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add(_name, _value);


           // string isUserType =context.HttpContext.User.Claims.FirstOrDefault(m => m.Type == ClaimTypes.Anonymous).Value;
           //// var enumIsUserType = (TokenUserType)isUserType;
           // if (isUserType != _value)
           // {
           //     context.Result = new UnauthorizedResult();
           // }

            base.OnResultExecuting(context);
        }
    }
}
