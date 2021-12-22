using ReadingIsGoodService.Common.Extensions;
using ReadingIsGoodService.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Api.Controllers
{
    [ApiController]
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
