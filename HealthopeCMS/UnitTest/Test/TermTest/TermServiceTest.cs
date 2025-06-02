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
using ApiLayer.Models.Member.Response;
using static AutoMapper.Internal.ExpressionFactory;
using ApiLayer.Interface;

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

        [TestMethod]
        public void 根據Id取得要修改條款的資料_成功_回傳條款資料()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 1
            };

            Term term = new Term()
            {
                Name = "okwopekq122",
                DetailContent = "dwdq",
                VersionDescription = "000blob",
                UpdateTime = DateTime.Now,
            };

            ResponseGetTermEditDataByIdDto response = new ResponseGetTermEditDataByIdDto()
            {
                Name = "okwopekq122",
                DetailContent = "dwdq",
                VersionDescription = "000blob",
                UpdateTime = DateTime.Now,
            };

            // Mock 設定
            termRepositoryMock.Setup(s => s.GetTermEditDataById(termIdDto.TermId)).Returns(term);
            mapperMock.Setup(s => s.Map<ResponseGetTermEditDataByIdDto>(term)).Returns(response);

            // Act
            ResponseGetTermEditDataByIdDto result = service.GetTermEditDataById(termIdDto);

            // Assert
            Assert.AreEqual(result, response);
        }

        [TestMethod]
        public void 根據Id取得要修改條款的資料_失敗_回傳空資料()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 1
            };

            Term term = new Term();

            ResponseGetTermEditDataByIdDto response = new ResponseGetTermEditDataByIdDto();

            // Mock 設定
            termRepositoryMock.Setup(s => s.GetTermEditDataById(termIdDto.TermId)).Returns(term);
            mapperMock.Setup(s => s.Map<ResponseGetTermEditDataByIdDto>(term)).Returns(response);

            // Act
            ResponseGetTermEditDataByIdDto result = service.GetTermEditDataById(termIdDto);

            // Assert
            Assert.AreEqual(result, response);
        }

        [TestMethod]
        public void 修改條款_成功_回傳成功()
        {
            // Arrange
            RequestEditTermDto editTermDto = new RequestEditTermDto()
            {
                TermId = 1,
                DetailContent = null,
                VersionDescription = "qwe",
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            termRepositoryMock.Setup(s => s.EditTerm(editTermDto)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.EditTerm(editTermDto);

            // Assert
            Assert.IsTrue(result == (ErrorCodeDefine)errorCodeNumber);
        }

        [TestMethod]
        public void 修改條款_失敗_回傳資料已被他人修改()
        {
            // Arrange
            RequestEditTermDto editTermDto = new RequestEditTermDto()
            {
                TermId = 1,
                DetailContent = null,
                VersionDescription = "qwe",
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.HasBeenModified;

            // Mock 設定
            termRepositoryMock.Setup(s => s.EditTerm(editTermDto)).Returns(errorCodeNumber);

            // Act
            ErrorCodeDefine result = service.EditTerm(editTermDto);

            // Assert
            Assert.IsTrue(result == (ErrorCodeDefine)errorCodeNumber);
        }

        [TestMethod]
        public void 修改條款狀態_成功_回傳成功()
        {
            // Arrange
            RequestEditTermStatusDto editTermStatusDto = new RequestEditTermStatusDto()
            {
                TermId = 1,
                Status = 2,
                UpdateTime = DateTime.Now,
            };

            Term term = new Term()
            {
                TermId = 1,
                Status = 2,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.Success;

            // Mock 設定
            termRepositoryMock.Setup(s => s.EditTermStatus(term)).Returns(errorCodeNumber);
            mapperMock.Setup(s => s.Map<Term>(editTermStatusDto)).Returns(term);

            // Act
            ErrorCodeDefine result = service.EditTermStatus(editTermStatusDto);

            // Assert
            Assert.IsTrue(result == (ErrorCodeDefine)errorCodeNumber);
        }

        [TestMethod]
        public void 修改條款狀態_失敗_回傳失敗()
        {
            // Arrange
            RequestEditTermStatusDto editTermStatusDto = new RequestEditTermStatusDto()
            {
                TermId = 1,
                Status = 2,
                UpdateTime = DateTime.Now,
            };

            Term term = new Term()
            {
                TermId = 1,
                Status = 2,
                UpdateTime = DateTime.Now,
            };

            int errorCodeNumber = (int)ErrorCodeDefine.HasBeenModified;

            // Mock 設定
            termRepositoryMock.Setup(s => s.EditTermStatus(term)).Returns(errorCodeNumber);
            mapperMock.Setup(s => s.Map<Term>(editTermStatusDto)).Returns(term);

            // Act
            ErrorCodeDefine result = service.EditTermStatus(editTermStatusDto);

            // Assert
            Assert.IsTrue(result == (ErrorCodeDefine)errorCodeNumber);
        }
        [TestMethod]
        public void 取得條款細項資料_成功_回傳條款資料()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 1
            };

            Term term = new Term()
            {
                Name = "okwopekq122",
                DetailContent = "dwdq",
                VersionDescription = "000blob",
                Version = 2,
            };

            ResponseGetTermDetailDto response = new ResponseGetTermDetailDto()
            {
                Name = "okwopekq122",
                DetailContent = "dwdq",
                VersionDescription = "000blob",
                Version = 2,
            };

            // Mock 設定
            termRepositoryMock.Setup(s => s.GetTermDetail(termIdDto.TermId)).Returns(term);
            mapperMock.Setup(s => s.Map<ResponseGetTermDetailDto>(term)).Returns(response);

            // Act
            ResponseGetTermDetailDto result = service.GetTermDetail(termIdDto);

            // Assert
            Assert.AreEqual(result, response);
        }

        [TestMethod]
        public void 取得條款細項資料_失敗_回傳空資料()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 1
            };

            Term term = new Term();

            ResponseGetTermDetailDto response = new ResponseGetTermDetailDto();

            // Mock 設定
            termRepositoryMock.Setup(s => s.GetTermDetail(termIdDto.TermId)).Returns(term);
            mapperMock.Setup(s => s.Map<ResponseGetTermDetailDto>(term)).Returns(response);

            // Act
            ResponseGetTermDetailDto result = service.GetTermDetail(termIdDto);

            // Assert
            Assert.AreEqual(result, response);
        }

        [TestMethod]
        public void 刪圖條款_成功_回傳成功()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 10,
            };

            bool successFlag = true;

            // Mock 設定
            termRepositoryMock.Setup(s => s.DeleteTerm(termIdDto.TermId)).Returns(successFlag);

            // Act
            bool result = service.DeleteTerm(termIdDto);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void 刪除條款_失敗_回傳失敗()
        {
            // Arrange
            RequestTermIdDto termIdDto = new RequestTermIdDto()
            {
                TermId = 1000,
            };

            bool successFlag = false;

            // Mock 設定
            termRepositoryMock.Setup(s => s.DeleteTerm(termIdDto.TermId)).Returns(successFlag);

            // Act
            bool result = service.DeleteTerm(termIdDto);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
