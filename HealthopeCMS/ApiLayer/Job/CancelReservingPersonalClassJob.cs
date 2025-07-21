using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Models.Job;
using ApiLayer.Service;
using Hangfire;
using Hangfire.Server;
using NLog;

namespace ApiLayer.Job
{
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 10, 180, 600 })]
    public class CancelReservingPersonalClassJob : IJob
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IMemberClassService memberClassService;

        public CancelReservingPersonalClassJob(IMemberClassService memberClassService)
        {
            this.memberClassService = memberClassService;
        }

        public async Task Execute(PerformContext context)
        {
            try
            {
                await memberClassService.AutoCancelReservingMemberPersonalClass();
            }
            catch (Exception ex)
            {
                logger.Error($"取消預約中課程失敗: {ex.Message}");
                throw;
            }
        }
    }
}