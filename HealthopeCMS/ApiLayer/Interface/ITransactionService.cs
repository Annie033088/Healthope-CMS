using ApiLayer.Models.Transaction.Request;
using ApiLayer.Models.Transaction.Response;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface ITransactionService
    {
        /// <summary>
        /// 取得付款紀錄
        /// </summary>
        ResponseGetTransactionListDto GetTransaction(RequestGetTransactionDto getTransactionDto);

        /// <summary>
        /// 取得金流資訊(Auth code 跟 外部金流Id)
        /// </summary>
        ResponsetGetCreditCardCashFlowDto GetCreditCardCashFlowData(RequestTransactionIdDto transactionIdDto);
    }
}
