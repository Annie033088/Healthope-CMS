using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

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
    }
}
