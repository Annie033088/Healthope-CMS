using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ApiLayer.Interface;

namespace ApiLayer.Service
{
    public class FileService : IFileService
    {
        /// <summary>
        /// 儲存檔案
        /// </summary>
        public void SaveFile(string folderPath, string savePath, byte[] fileData)
        {
            try
            {
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                File.WriteAllBytes(savePath, fileData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        public void DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
            }
            catch (Exception)
            {
                throw;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// 取得圖片副檔名
        /// </summary>
        public string GetImageExtension(string mimeType)
        {
            if (mimeType == "image/jpeg") return ".jpg";
            else if (mimeType == "image/png") return ".png";
            else if (mimeType == "image/webp") return ".webp";
            else return "";
        }
    }
}