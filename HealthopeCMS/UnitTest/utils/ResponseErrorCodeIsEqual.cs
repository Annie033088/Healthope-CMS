using System.Web.Http;
using System.Web.Http.Results;
using ApiLayer.Models;

namespace ToDoListTest.utils
{
    class ResponseErrorCodeIsEqual
    {
        public bool ErrorCodeIsEqual(IHttpActionResult actionResult, ErrorCodeDefine errorCodeDefine)
        {
            if (actionResult is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse resultWithoutData)
                {
                    return resultWithoutData.ErrorCode == errorCodeDefine;
                }
            }

            return false;
        }
    }
    class ResponseErrorCodeIsEqual<T>
    {
        public bool ErrorCodeIsEqual(IHttpActionResult actionResult, ErrorCodeDefine errorCodeDefine)
        {
            if (actionResult is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse<T> resultWithData)
                {
                    return resultWithData.ErrorCode == errorCodeDefine;
                }
            }

            return false;
        }
    }
}
