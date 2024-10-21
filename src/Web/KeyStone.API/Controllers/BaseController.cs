using FluentResults;
using FluentValidation.Results;
using KeyStone.Shared.API;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyStone.API.Controllers
{
   
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }

        protected string GetRequestId()
        {
            return (string)HttpContext.Items["RequestId"];
        }
        protected IActionResult ResultResponse<T>(Result<T> result)
        {
            if (result.IsFailed)
            {
                return BadRequestResponse(GetApiError(result.Errors));
            }
            return OkResponse(result.Value);
        }

        protected IActionResult ResultResponse(Result result)
        {
            if (result.IsFailed)
            {
                return BadRequestResponse(GetApiError(result.Errors));
            }
            return OkResponse();
        }

        protected IActionResult ResultResponse(List<ValidationFailure> failures)
        {
            return BadRequestResponse(GetApiError(failures));
        }

        //generate OkResponse
        protected IActionResult OkResponse<T>(T data)
        {
            return Ok(new ApiResponse<T>(true, ApiError.None, data));
        }
        //generate OkResponse
        protected IActionResult OkResponse()
        {
            return Ok(new ApiResponse(true, ApiError.None));
        }
        //generate BadRequestResponse
        protected IActionResult BadRequestResponse(ApiError error)
        {
            return BadRequest(new ApiResponse(false, error));
        }
        private ApiError GetApiError(List<IError> errors)
        {
            string message = string.Empty;
            foreach (var error in errors)
            {
                message += error.Message + "\n";
            }
            return new ApiError(message);
        }

        private ApiError GetApiError(List<ValidationFailure> errors)
        {
            string message = string.Empty;
            foreach (var error in errors)
            {
                message += error.ErrorMessage + "\n";
            }
            return new ApiError(message);
        }
    }
}
