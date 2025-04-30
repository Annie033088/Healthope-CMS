using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainLayer.Models;

namespace ApiLayer.Models.AccountAccess.RequestAccountAccessDto
{
    public class RequestPermissionDto
    {
        public AdminPermission adminPermission {  get; set; }
    }
}