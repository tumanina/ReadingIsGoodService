using ReadingIsGoodService.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ReadingIsGoodService.Common.Models;
using ReadingIsGoodService.Common.Authentication;

namespace ReadingIsGoodService.Customers.Api.Controllers
{
    [ApiController]
    [Authorization]
    public abstract class BaseController : ControllerBase
    {
        internal async Task<BaseApiDataModel<T>> Execute<T>(Func<Task<T>> func)
        {
            try
            {
                var data = await func();
                return new BaseApiDataModel<T> { Data = data };
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetMessage();
                //log errorMessage
                return new BaseApiDataModel<T> { Errors = new List<string> { errorMessage } };
            }
        }

        internal async Task<BaseApiModel> Execute(Func<Task> func)
        {
            try
            {
                await func();
                return new BaseApiModel();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetMessage();
                //log errorMessage
                return new BaseApiModel { Errors = new List<string> { errorMessage } };
            }
        }
    }
}
