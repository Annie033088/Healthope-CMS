using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Coach.Request;
using ApiLayer.Models.Coach.Response;
using ApiLayer.Models.Member;
using ApiLayer.Models.Other;
using AutoMapper;
using DomainLayer.Models;
using DomainLayer.Utility;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using PersistentLayer.Repository;
using static StackExchange.Redis.Role;

namespace ApiLayer.Service
{
    public class CoachService : ICoachService
    {
        private readonly IMapper mapper;
        private readonly ICoachRepository coachRepository;
        private readonly IRedisService redisService;
        private readonly IFileService fileService;

        public CoachService(IMapper mapper, ICoachRepository coachRepository,
            IFileService fileService, IRedisService redisService)
        {
            this.mapper = mapper;
            this.coachRepository = coachRepository;
            this.fileService = fileService;
            this.redisService = redisService;
        }

        /// <summary>
        /// 新增教練
        /// </summary>
        public (ErrorCodeDefine errorCode, Exception exception) AddCoach(RequestAddCoachDto addCoachDto,
            FileDto file)
        {
            try
            {
                Coach coach = mapper.Map<Coach>(addCoachDto);
                Hash hash = new Hash();
                string salt = hash.GenerateSalt();
                coach.Hash = hash.PwdHash(addCoachDto.Pwd, salt);
                coach.PhotoUrl = string.Empty;

                string savePath = "";

                if (file != null)
                {
                    string imageExtension = fileService.GetImageExtension(file.MimeType);
                    string fileName = Guid.NewGuid().ToString() + imageExtension;
                    string folderPath = Path.Combine(HttpContext.Current.Server.MapPath("~/"), "assets", "images", "coach");
                    savePath = Path.Combine(folderPath, fileName);
                    coach.PhotoUrl = Path.Combine("assets", "images", "coach", fileName)
                        .Replace(Path.DirectorySeparatorChar, '/');

                    fileService.SaveFile(folderPath, savePath, file.FileData);
                }

                ResultWithException result = coachRepository.AddCoach(coach);

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
        /// 取得教練清單
        /// </summary>
        public ResponseGetCoachListDto GetCoach(RequestGetCoachDto getCoachDto)
        {
            try
            {
                (List<Coach> coaches, int totalPage) = coachRepository.GetCoach(getCoachDto);
                ResponseGetCoachListDto responseGetMemberDto = new ResponseGetCoachListDto()
                {
                    CoachList = mapper.Map<List<ResponseGetCoachDto>>(coaches),
                    TotalPage = totalPage
                };

                return responseGetMemberDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得修改教練頁面的資料
        /// </summary>
        public ResponseGetCoachEditDataByIdDto GetCoachEditDataById(RequestCoachIdDto coachIdDto)
        {
            try
            {
                Coach coach = coachRepository.GetCoachEditDataById(coachIdDto.CoachId);

                if (coach == null) return null;

                ResponseGetCoachEditDataByIdDto response = mapper.Map<ResponseGetCoachEditDataByIdDto>(coach);

                if (!string.IsNullOrEmpty(response.PhotoUrl))
                    response.PhotoUrl = "/" + response.PhotoUrl;

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改教練
        /// </summary>
        public (ErrorCodeDefine errorCode, Exception exception) EditCoach(RequestEditCoachDto editCoachDto, FileDto file)
        {
            try
            {
                editCoachDto.PhotoUrl = null;
                string savePath = "";
                string rootUrl = HttpContext.Current.Server.MapPath("~/");

                if (file != null)
                {
                    string imageExtension = fileService.GetImageExtension(file.MimeType);
                    string fileName = Guid.NewGuid().ToString() + imageExtension;
                    string folderPath = Path.Combine(rootUrl, "assets", "images", "coach");
                    savePath = Path.Combine(folderPath, fileName);
                    editCoachDto.PhotoUrl = Path.Combine("assets", "images", "coach", fileName)
                        .Replace(Path.DirectorySeparatorChar, '/');

                    fileService.SaveFile(folderPath, savePath, file.FileData);
                }

                (ResultWithException result, string oldPhotoUrl) =
                    coachRepository.EditCoach(editCoachDto);

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
                else if (!string.IsNullOrEmpty(oldPhotoUrl))
                    fileService.DeleteFile(Path.Combine(rootUrl, oldPhotoUrl.Replace('/', Path.DirectorySeparatorChar)));
                // 成功根據修改狀態, 清除教練會話
                else if (editCoachDto.Status == false)
                {
                    string coachRedisKey = "Coach" + editCoachDto.CoachId;
                    redisService.DeleteKey(coachRedisKey);
                }

                return (errorCode, result.Exception);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}