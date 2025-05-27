using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace WebApi.GlobalException
{
    public class GenericExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            // Log the stack trace here for monitoring purpose
            // and display only the message we thrown from lower layer

            var response = context.Request.CreateErrorResponse(
                HttpStatusCode.InternalServerError,
                context.Exception.Message
            );
            context.Response = response;
        }
    }
}