using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DomainLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using PersistentLayer.Repository;

namespace UnitTest.Test.AdminTest
{
    [TestClass]
    public class AdminRepositoryTest
    {
        private Mock<IDbExecutor> dbExecutorMock;
        private AdminRepository repository;

        [TestInitialize]
        public void Setup()
        {
            dbExecutorMock = new Mock<IDbExecutor>();
            repository = new AdminRepository(dbExecutorMock.Object);
        }

        [TestMethod]
        public void GetLoggingInAdmin_有資料_回傳Admin()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("f_adminId", typeof(int));
            dt.Columns.Add("f_hash", typeof(string));
            dt.Columns.Add("f_status", typeof(bool));
            dt.Columns.Add("f_identity", typeof(byte));
            dt.Rows.Add(1, "hash", true, (byte)2);
            string account = "admin123";

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteDataTable(It.IsAny<SqlCommand>())).Returns(dt);

            // Act
            Admin result = repository.GetLoggingInAdmin(account);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.AdminId);
            Assert.AreEqual("hash", result.Hash);
            Assert.AreEqual(true, result.Status);
            Assert.AreEqual((byte)2, result.Identity);
        }

        [TestMethod]
        public void GetLoggingInAdmin_無資料_回傳Null()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("f_adminId", typeof(int));
            dt.Columns.Add("f_hash", typeof(string));
            dt.Columns.Add("f_status", typeof(bool));
            dt.Columns.Add("f_identity", typeof(byte));
            string account = "admin123";

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteDataTable(It.IsAny<SqlCommand>())).Returns(dt);

            // Act
            Admin result = repository.GetLoggingInAdmin(account);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void EditSelfPwd_成功_回傳True()
        {
            // Arrange
            EditPwdDto editPwdDto = new EditPwdDto
            {
                AdminId = 1,
                OldPwd = "old",
                NewPwd = "new"
            };
            int exeCnt = 1;

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteNonQuery(It.IsAny<SqlCommand>())).Returns(exeCnt);

            // Act
            bool result = repository.EditSelfPwd(editPwdDto);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EditSelfPwd_失敗回傳False()
        {
            // Arrange
            EditPwdDto editPwdDto = new EditPwdDto
            {
                AdminId = 1,
                OldPwd = "old",
                NewPwd = "new"
            };
            int exeCnt = 0;

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteNonQuery(It.IsAny<SqlCommand>())).Returns(exeCnt);

            // Act
            bool result = repository.EditSelfPwd(editPwdDto);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AddAdmin_成功回傳True()
        {
            // Arrange
            Admin admin = new Admin
            {
                Account = "abceee123",
                Hash = "hhhhhhhhwwe",
                Identity = 1
            };
            int exeCnt = 1;

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteNonQuery(It.IsAny<SqlCommand>())).Returns(exeCnt);

            // Act
            bool result = repository.AddAdmin(admin);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddAdmin_失敗回傳False()
        {
            // Arrange
            Admin admin = new Admin
            {
                Account = "abceee123",
                Hash = "hhhhhhhhwwe",
                Identity = 1
            };
            int exeCnt = 0;

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteNonQuery(It.IsAny<SqlCommand>())).Returns(exeCnt);

            // Act
            bool result = repository.AddAdmin(admin);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetAdmin_有資料_回傳清單與頁數()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("f_adminId", typeof(int));
            dt.Columns.Add("f_account", typeof(string));
            dt.Columns.Add("f_status", typeof(bool));
            dt.Columns.Add("f_identity", typeof(byte));
            dt.Columns.Add("f_updateTime", typeof(DateTime));
            dt.Rows.Add(1, "admin", true, (byte)2, DateTime.Now);

            Dictionary<string, object> output = new Dictionary<string, object> { { "@totalPage", 5 } };

            RequestGetAdminDto dto = new RequestGetAdminDto
            {
                Status = true,
                SortOrder = "desc",
                SortOption = "account",
                RecordPerPage = 10,
                SearchAccount = "admin",
                Page = 1
            };

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteDataTableWithOutput(It.IsAny<SqlCommand>(), "@totalPage"))
                .Returns((dt, output));

            // Act
            (List<Admin> admins, int totalPage) = repository.GetAdmin(dto);

            // Assert
            Assert.AreEqual(1, admins.Count);
            Assert.AreEqual(5, totalPage);
            Assert.AreEqual("admin", admins[0].Account);
        }

        [TestMethod]
        public void GetAdmin_無資料_回傳空清單與預設頁數()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("f_adminId", typeof(int));
            dt.Columns.Add("f_account", typeof(string));
            dt.Columns.Add("f_status", typeof(bool));
            dt.Columns.Add("f_identity", typeof(byte));
            dt.Columns.Add("f_updateTime", typeof(DateTime));

            Dictionary<string, object> output = new Dictionary<string, object> { { "@totalPage", 1 } };

            RequestGetAdminDto dto = new RequestGetAdminDto
            {
                Status = true,
                SortOrder = "desc",
                SortOption = "account",
                RecordPerPage = 10,
                SearchAccount = "admin",
                Page = 1
            };

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteDataTableWithOutput(It.IsAny<SqlCommand>(), "@totalPage"))
                .Returns((dt, output));

            // Act
            (List<Admin> admins, int totalPage) = repository.GetAdmin(dto);

            // Assert
            Assert.AreEqual(0, admins.Count);
            Assert.AreEqual(1, totalPage);
        }

        [TestMethod]
        public void GetAdminById_有資料_回傳Admin()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("f_adminId", typeof(int));
            dt.Columns.Add("f_account", typeof(string));
            dt.Columns.Add("f_status", typeof(bool));
            dt.Columns.Add("f_identity", typeof(byte));
            dt.Columns.Add("f_updateTime", typeof(DateTime));
            dt.Rows.Add(1, "admin", true, (byte)2, DateTime.Now);
            int adminId = 1;

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteDataTable(It.IsAny<SqlCommand>())).Returns(dt);

            // Act
            Admin result = repository.GetAdminById(adminId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.AdminId);
            Assert.AreEqual("admin", result.Account);
        }

        [TestMethod]
        public void GetAdminById_無資料_回傳Null()
        {
            // Arrange
            DataTable dt = new DataTable();
            dt.Columns.Add("f_adminId", typeof(int));
            dt.Columns.Add("f_account", typeof(string));
            dt.Columns.Add("f_status", typeof(bool));
            dt.Columns.Add("f_identity", typeof(byte));
            dt.Columns.Add("f_updateTime", typeof(DateTime));
            int adminId = 1;   

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteDataTable(It.IsAny<SqlCommand>())).Returns(dt);

            // Act
            Admin result = repository.GetAdminById(adminId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void EditAdmin_成功回傳True()
        {
            // Arrange
            RequestEditAdminDto requestEditAdminDto =
                new RequestEditAdminDto { AdminId = 1, Status = true, Identity = 1, UpdateTime = DateTime.Now };
            int exeCnt = 1;

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteNonQuery(It.IsAny<SqlCommand>())).Returns(exeCnt);

            // Act
            bool result = repository.EditAdmin(requestEditAdminDto);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EditAdmin_失敗回傳False()
        {
            // Arrange
            RequestEditAdminDto requestEditAdminDto =
                new RequestEditAdminDto { AdminId = 1, Status = true, Identity = 1, UpdateTime = DateTime.Now };
            int exeCnt = 0;

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteNonQuery(It.IsAny<SqlCommand>())).Returns(exeCnt);

            // Act
            bool result = repository.EditAdmin(requestEditAdminDto);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteAdmin_成功回傳True()
        {
            // Arrange
            int exeCnt = 1;

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteNonQuery(It.IsAny<SqlCommand>())).Returns(exeCnt);

            // Act
            bool result = repository.DeleteAdmin(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteAdmin_失敗回傳False()
        {
            // Arrange
            int exeCnt = 0;

            // Mock
            dbExecutorMock.Setup(x => x.ExecuteNonQuery(It.IsAny<SqlCommand>())).Returns(exeCnt);

            // Act
            bool result = repository.DeleteAdmin(1);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
