using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models;
using ApiLayer.Models.Other;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface IPaymentService
    {
        /// <summary>
        /// 刷卡
        /// </summary>
        Task<(ErrorCodeDefine errorCode, DBResponsePaymentDto dbResponse)> PayByCard(
            RequestCardPaymentDto requestCardPaymentDto, int creditCardTransactionId, RequestPayByCardDto payByCardDto);
    }
}
