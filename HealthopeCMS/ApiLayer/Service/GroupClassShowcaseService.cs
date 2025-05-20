using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.GroupClassShowcase.Request;
using ApiLayer.Models.Other;
using AutoMapper;
using DomainLayer.Models;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using PersistentLayer.Repository;

namespace ApiLayer.Service
{
    public class GroupClassShowcaseService : IGroupClassShowcaseService
    {
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        private readonly IGroupClassShowcaseRepository groupClassShowcaseRepository;

        public GroupClassShowcaseService(IMapper mapper, IFileService fileService, 
            IGroupClassShowcaseRepository groupClassShowcaseRepository)
        {
            this.mapper = mapper;
            this.fileService = fileService;
            this.groupClassShowcaseRepository = groupClassShowcaseRepository;
        }

        public (ErrorCodeDefine errorCode, Exception exception) AddShowcase(RequestAddShowcaseDto addShowcaseDto, FileDto file)
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
                    string folderPath = Path.Combine(HttpContext.Current.Server.MapPath("~/"), 
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
    }
}