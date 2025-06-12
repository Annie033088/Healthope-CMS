using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLayer.Models;
using ApiLayer.Models.Order.Request;
using ApiLayer.Models.Order.Response;
using PersistentLayer.Models;

namespace ApiLayer.Interface
{
    public interface IOrderService
    {
        /// <summary>
        /// 新增訂單
        /// </summary>
        (ResponseAddOrderDto response, ErrorCodeDefine errorCode) AddOrder(RequestAddOrderDto addOrderDto);

        /// <summary>
        /// 現金付款
        /// </summary>
        ErrorCodeDefine PayByCash(RequestPayByCashDto payByCashDto);
    }
}
