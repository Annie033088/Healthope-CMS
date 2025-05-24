using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models.GroupClassShowcase.Response;
using ApiLayer.Models.Other;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;

namespace ApiLayer.Service
{
    public class GroupClassShowcaseService : IGroupClassShowcaseService
    {
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        private readonly IHttpService httpService;
        private readonly IGroupClassShowcaseRepository groupClassShowcaseRepository;

        public GroupClassShowcaseService(IMapper mapper, IFileService fileService,
            IGroupClassShowcaseRepository groupClassShowcaseRepository, IHttpService httpService)
        {
            this.mapper = mapper;
            this.fileService = fileService;
            this.groupClassShowcaseRepository = groupClassShowcaseRepository;
            this.httpService = httpService;
        }

        public (ErrorCodeDefine errorCode, Exception exception) AddShowcase(
            RequestAddShowcaseDto addShowcaseDto, FileDto file)
        {
            try
            {
                GroupClassShowcase groupClassShowcase = mapper.Map<GroupClassShowcase>(addShowcaseDto);
                groupClassShowcase.ImageUrl = string.Empty;

                string savePath = "";

                if (file != null)
                {
                    string imageExtension = fileService.GetImageExtension(file.MimeType);
                    string fileName = Guid.NewGuid().ToString() + imageExtension;
                    string folderPath = Path.Combine(httpService.GetRootPath(),
                        "assets", "images", "groupClassShowcase");
                    savePath = Path.Combine(folderPath, fileName);
                    groupClassShowcase.ImageUrl = Path.Combine("assets", "images", "groupClassShowcase", fileName)
                        .Replace(Path.DirectorySeparatorChar, '/');

                    fileService.SaveFile(folderPath, savePath, file.FileData);
                }

                ResultWithException result = groupClassShowcaseRepository.AddShowcase(groupClassShowcase);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), result.ErrorCodeNumber) || result.Exception != null)
                {
                    fileService.DeleteFile(savePath);
                    return (ErrorCodeDefine.ServerError, result.Exception);
                }

                ErrorCodeDefine errorCode = (ErrorCodeDefine)result.ErrorCodeNumber;

                if (errorCode != ErrorCodeDefine.Success) fileService.DeleteFile(savePath);

                return (errorCode, result.Exception);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得展示用課程
        /// </summary>
        public ResponseGetShowcaseListDto GetShowcase(RequestGetShowcaseDto getShowcaseDto)
        {
            try
            {
                (List<GroupClassShowcase> showcases, int totalPage) = groupClassShowcaseRepository.GetShowcase(getShowcaseDto);
                ResponseGetShowcaseListDto responseGetShowcaseDto = new ResponseGetShowcaseListDto()
                {
                    ShowcaseList = mapper.Map<List<ResponseGetShowcaseDto>>(showcases),
                    TotalPage = totalPage
                };

                return responseGetShowcaseDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得展示用團課細項
        /// </summary>
        public ResponseGetShowcaseDetailDto GetShowcaseDetail(RequestShowcaseIdDto showcaseIdDto)
        {
            try
            {
                GroupClassShowcase showcase = groupClassShowcaseRepository.GetShowcaseDetail(showcaseIdDto.GroupClassShowcaseId);
                if (showcase != null && !string.IsNullOrEmpty(showcase.ImageUrl))
                    showcase.ImageUrl = "/" + showcase.ImageUrl;
                return mapper.Map<ResponseGetShowcaseDetailDto>(showcase);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得修改展示用團課頁面的資料
        /// </summary>
        public ResponseGetShowcaseEditDataDto GetShowcaseEditDataById(RequestShowcaseIdDto showcaseIdDto)
        {
            try
            {
                GroupClassShowcase showcase = groupClassShowcaseRepository.GetShowcaseEditDataById(
                    showcaseIdDto.GroupClassShowcaseId);

                if (showcase == null) return null;

                ResponseGetShowcaseEditDataDto response = mapper.Map<ResponseGetShowcaseEditDataDto>(showcase);

                if (response != null && !string.IsNullOrEmpty(response.ImageUrl))
                    response.ImageUrl = "/" + response.ImageUrl;

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改展示用團課
        /// </summary>
        public (ErrorCodeDefine errorCode, Exception exception) EditShowcase(RequestEditShowcaseDto editShowcaseDto, FileDto file)
        {
            try
            {
                editShowcaseDto.ImageUrl = null;
                string savePath = "";
                string rootUrl = httpService.GetRootPath();

                if (file != null)
                {
                    string imageExtension = fileService.GetImageExtension(file.MimeType);
                    string fileName = Guid.NewGuid().ToString() + imageExtension;
                    string folderPath = Path.Combine(HttpContext.Current.Server.MapPath("~/"),
                        "assets", "images", "groupClassShowcase");
                    savePath = Path.Combine(folderPath, fileName);
                    editShowcaseDto.ImageUrl = Path.Combine("assets", "images", "groupClassShowcase", fileName)
                        .Replace(Path.DirectorySeparatorChar, '/');

                    fileService.SaveFile(folderPath, savePath, file.FileData);
                }

                (ResultWithException result, string oldImageUrl) =
                    groupClassShowcaseRepository.EditShowcase(editShowcaseDto);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), result.ErrorCodeNumber)
                    || result.Exception != null)
                {
                    fileService.DeleteFile(savePath);
                    return (ErrorCodeDefine.ServerError, result.Exception);
                }

                ErrorCodeDefine errorCode = (ErrorCodeDefine)result.ErrorCodeNumber;

                // 失敗就刪除之前存的檔案
                if (errorCode != ErrorCodeDefine.Success) fileService.DeleteFile(savePath);
                // 成功刪除舊檔案
                else if (!string.IsNullOrEmpty(oldImageUrl))
                    fileService.DeleteFile(Path.Combine(rootUrl, oldImageUrl.Replace('/', Path.DirectorySeparatorChar)));

                return (errorCode, result.Exception);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 刪除展示用團課
        /// </summary>
        public bool DeleteShowcase(RequestShowcaseIdDto showcaseIdDto)
        {
            try
            {
                (bool successFlag, string oldImageUrl) = groupClassShowcaseRepository.DeleteShowcase(showcaseIdDto.GroupClassShowcaseId);

                if (!string.IsNullOrEmpty(oldImageUrl))
                {
                    string rootUrl = httpService.GetRootPath();
                    fileService.DeleteFile(Path.Combine(rootUrl, oldImageUrl.Replace('/', Path.DirectorySeparatorChar)));
                }

                return successFlag;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}