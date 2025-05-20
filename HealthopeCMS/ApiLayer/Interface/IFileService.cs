namespace ApiLayer.Interface
{
    public interface IFileService
    {
        /// <summary>
        /// 儲存檔案
        /// </summary>
        void SaveFile(string folderPath, string savePath, byte[] fileData);

        /// <summary>
        /// 刪除檔案
        /// </summary>
        void DeleteFile(string path);

        /// <summary>
        /// 取得圖片副檔名
        /// </summary>
        string GetImageExtension(string mimeType);
    }
}
