using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetYouServer.Models
{
    public class ServerActionResult : IActionResult
    {
        public enum Status { Ok, Unauthorized }

        public Status StatusCode;
        public ServerResponse Response;
        
        public ServerActionResult(Status status, ServerResponse response = null)
        {
            this.StatusCode = status;
            this.Response = response;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(Response)
            {
                StatusCode = StatusCode.GetHttpCode()
            };
            await objectResult.ExecuteResultAsync(context);
        }
    }

    public static class SARStatusCodeExtensions
    {
        public static int GetHttpCode(this ServerActionResult.Status status)
        {
            switch (status)
            {
                case ServerActionResult.Status.Ok:              return StatusCodes.Status200OK;
                case ServerActionResult.Status.Unauthorized:    return StatusCodes.Status401Unauthorized;
                default:                                        return 0;
            }
        }
    }
}
