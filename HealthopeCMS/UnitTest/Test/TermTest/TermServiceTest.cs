using System.Collections.Generic;
using ApiLayer.Models;
using System.Web.Http;
using ApiLayer.Models.Term.Request;
using ApiLayer.Models.Term.Response;
using ApiLayer.Service;
using AutoMapper;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using UnitTest.utils;
using System;

namespace UnitTest.Test.TermTest
{
    [TestClass]
    public class TermServiceTest
    {
        private TermService service;
        private Mock<IMapper> mapperMock;
        private Mock<ITermRepository> termRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            termRepositoryMock = new Mock<ITermRepository>();
            mapperMock = new Mock<IMapper>();
            service = new TermService(mapperMock.Object, termRepositoryMock.Object);
        }

        [TestMethod]
        public void 取得舊條款清單_成功_回傳清單()
        {
            // Arrange
            RequestGetOldTermDto getOldTermDto = new RequestGetOldTermDto()
            {
                ApplicableTarget = 1,
                Type = 2,
            };

            List<ResponseGetOldTermDto> responseTerms = new List<ResponseGetOldTermDto>()
            {
                new ResponseGetOldTermDto
                {
                    TermId = 1,
                    DetailContent="qwe",
                    Name="-",
                    Version=1,
                }
            };

            List<Term> terms = new List<Term>()
            {
                new Term
                {
                    TermId = 1,
                    DetailContent="qwe",
                    Name="-",
                    Version=1,
                }
            };

            Term getTerm = new Term()
            {
                ApplicableTarget = 1,
                Type = 2,
            };

            // Mock 設定
            termRepositoryMock.Setup(s => s.GetOldTerm(getTerm)).Returns(terms);
            mapperMock.Setup(s => s.Map<Term>(getOldTermDto)).Returns(getTerm);
            mapperMock.Setup(s => s.Map<List<ResponseGetOldTermDto>>(terms)).Returns(responseTerms);

            // Act
            List<ResponseGetOldTermDto> response = service.GetOldTerm(getOldTermDto);

            // Assert
            CollectionAssert.AreEqual(response, responseTerms);
        }

        [TestMethod]
        public void 取得舊條款清單_失敗_回傳空資料()
        {
            // Arrange
            RequestGetOldTermDto getOldTermDto = new RequestGetOldTermDto()
            {
                ApplicableTarget = 1,
                Type = 2,
            };

            List<ResponseGetOldTermDto> responseTerms = null;

            List<Term> terms = null;
            Term getTerm = null;

            // Mock 設定
            termRepositoryMock.Setup(s => s.GetOldTerm(getTerm)).Returns(terms);
            mapperMock.Setup(s => s.Map<Term>(getOldTermDto)).Returns(getTerm);
            mapperMock.Setup(s => s.Map<List<ResponseGetOldTermDto>>(terms)).Returns(responseTerms);

            // Act
            List<ResponseGetOldTermDto> response = service.GetOldTerm(getOldTermDto);

            // Assert
            Assert.IsTrue(response == null);
        }

        [TestMethod]
        public void 新增_成功_回傳成功()
        {
            // Arrange
            RequestAddTermDto addTermDto = new RequestAddTermDto()
            {
                ApplicableTarget = 1,
                DetailContent = "w",
                Type = 2,
                VersionDescription = "2426"
            };
            Term term = new Term()
            {
                ApplicableTarget = 1,
                DetailContent = "w",
                Type = 2,
                VersionDescription = "2426"
            };

            bool successFlag = true;

            // Mock 設定
            termRepositoryMock.Setup(s
                => s.AddTerm(term)).Returns(successFlag);
            mapperMock.Setup(s => s.Map<Term>(addTermDto)).Returns(term);

            // Act
            bool response = service.AddTerm(addTermDto);

            // Assert
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void 新增_成功_回傳失敗()
        {
            // Arrange
            RequestAddTermDto addTermDto = new RequestAddTermDto()
            {
                ApplicableTarget = 1,
                DetailContent = "w",
                Type = 2,
                VersionDescription = "2426"
            };
            Term term = new Term()
            {
                ApplicableTarget = 1,
                DetailContent = "w",
                Type = 2,
                VersionDescription = "2426"
            };

            bool successFlag = false;

            // Mock 設定
            termRepositoryMock.Setup(s
                => s.AddTerm(term)).Returns(successFlag);
            mapperMock.Setup(s => s.Map<Term>(addTermDto)).Returns(term);

            // Act
            bool response = service.AddTerm(addTermDto);

            // Assert
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void 取得條款清單_成功_回傳清單()
        {
            // Arrange
            RequestGetTermDto getTermDto = new RequestGetTermDto()
            {
                ApplicableTarget = 1,
                Type = 2,
                Status = 3,
                Page = 1,
                RecordPerPage = 8
            };
            List<ResponseGetTermDto> responseGetTerms = new List<ResponseGetTermDto>()
            {
                new ResponseGetTermDto()
                {
                    ApplicableTarget = 1,
                    EffectiveTime = DateTime.Now,
                    Name="qq",
                    Status = 3,
                    TermId = 1,
                    Type=2,
                    Version = 1,
                }
            };

            List<Term> terms = new List<Term>()
            {
                new Term()
                {
                    ApplicableTarget = 1,
                    EffectiveTime = DateTime.Now,
                    Name = "qq",
                    Status = 3,
                    TermId = 1,
                    Type = 2,
                    Version = 1,
                }
            };
            int totalPage = 1;

            // Mock 設定
            termRepositoryMock.Setup(s
                => s.GetTerm(getTermDto)).Returns((terms, totalPage));
            mapperMock.Setup(s
                => s.Map<List<ResponseGetTermDto>>(terms)).Returns(responseGetTerms);

            // Act
            ResponseGetTermListDto result = service.GetTerm(getTermDto);

            // Assert
            CollectionAssert.AreEqual(responseGetTerms, result.TermList);
        }

        [TestMethod]
        public void 取得條款清單_失敗_回傳空資料()
        {
            // Arrange
            RequestGetTermDto getTermDto = new RequestGetTermDto()
            {
                ApplicableTarget = 1,
                Type = 2,
                Status = 3,
                Page = 1,
                RecordPerPage = 8
            };
            List<ResponseGetTermDto> responseGetTerms = null;

            List<Term> terms = null;
            int totalPage = 1;

            // Mock 設定
            termRepositoryMock.Setup(s
                => s.GetTerm(getTermDto)).Returns((terms, totalPage));
            mapperMock.Setup(s
                => s.Map<List<ResponseGetTermDto>>(terms)).Returns(responseGetTerms);

            // Act
            ResponseGetTermListDto result = service.GetTerm(getTermDto);

            // Assert
            CollectionAssert.AreEqual(responseGetTerms, result.TermList);
        }
    }
}
