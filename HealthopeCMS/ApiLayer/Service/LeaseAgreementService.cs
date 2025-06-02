using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Models.LeaseAgreement.Request;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Repository;

namespace ApiLayer.Service
{
    public class LeaseAgreementService : ILeaseAgreementService
    {
        private readonly IMapper mapper;
        private readonly ILeaseAgreementRepository leaseAgreementRepository;

        public LeaseAgreementService(IMapper mapper, ILeaseAgreementRepository leaseAgreementRepository)
        {
            this.mapper = mapper;
            this.leaseAgreementRepository = leaseAgreementRepository;
        }

        /// <summary>
        /// 新增條款
        /// </summary>
        public bool AddLeaseAgreement(RequestAddLeaseAgreementDto addLeaseAgreementDto)
        {
            try
            {
                LeaseAgreement leaseAgreement = mapper.Map<LeaseAgreement>(addLeaseAgreementDto);
                return leaseAgreementRepository.AddLeaseAgreement(leaseAgreement);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}