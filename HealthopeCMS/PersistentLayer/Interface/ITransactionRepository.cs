using System.Collections.Generic;
using DomainLayer.Models;
using PersistentLayer.Models;

namespace PersistentLayer.Interface
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// 付款成功時, 修改狀態
        /// </summary>
        bool EditCreditCardTransactionStatusSuccess(CreditCardTransaction creditCardTransaction);

        /// <summary>
        /// 付款失敗時, 修改狀態
        /// </summary>
        bool EditCreditCardTransactionStatusFail(int creditCardTransactionId);

        /// <summary>
        /// 取得付款紀錄
        /// </summary>
        (List<PaymentTransaction> transactions, int totalPage) GetTransaction(RequestGetTransactionDto getTransactionDto);

        /// <summary>
        /// 取得金流資訊(Auth code 跟 外部金流Id)
        /// </summary>
        PaymentTransaction GetCreditCardCashFlowData(int transactionId);
    }
}
