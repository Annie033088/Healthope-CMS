using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using ApiLayer.Models;

namespace UnitTest.utils
{
    class ResponseIsEqual
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

    class ResponseIsEqual<T>
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

        public bool ErrorCodeAndObjectIsEqual(IHttpActionResult actionResult, ErrorCodeDefine errorCodeDefine, T expectedObject)
        {
            if (actionResult is OkNegotiatedContentResult<ResultResponse> okResultWithoutData)
            {
                if (okResultWithoutData.Content is ResultResponse<T> resultWithData)
                {
                    bool successFlag = true;

                    // 比較陣列
                    if (resultWithData.ApiDataObject is IEnumerable<object> resultList && expectedObject is IEnumerable<object> expectedList)
                    {
                        successFlag = resultList.SequenceEqual(expectedList);
                    }
                    // 比較物件
                    else
                    {
                        successFlag = resultWithData.ApiDataObject.Equals(expectedObject);
                    }

                    // 比較 errorcode
                    if (resultWithData.ErrorCode != errorCodeDefine) successFlag = false;

                    return successFlag;
                }
            }

            return false;
        }
    }
}
