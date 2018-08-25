using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace ProgressTracker.App_Start
{
    public class GanttAPIExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {

            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, new
            {
                action = "error",
                message = context.Exception.Message
            });
        }
    }
}