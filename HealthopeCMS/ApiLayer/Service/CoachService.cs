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
        private readonly IFileService fileService;

        public CoachService(IMapper mapper, ICoachRepository coachRepository, IFileService fileService)
        {
            this.mapper = mapper;
            this.coachRepository = coachRepository;
            this.fileService = fileService;
        }

        /// <summary>
        /// 新增教練
        /// </summary>
        public (ErrorCodeDefine errorCode, Exception exception) AddCoach(RequestAddCoachDto addCoachDto, FileDto file)
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
                    coach.PhotoUrl = Path.Combine("assets", "images", "coach", fileName);

                    fileService.SaveFile(folderPath, savePath, file.FileData);
                }

                OperationResult result = coachRepository.AddCoach(coach);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), result.ErrorCodeNumber) || result.Exception != null)
                {
                    fileService.DeleteFile(savePath);
                    return (ErrorCodeDefine.ServerError, result.Exception);
                }

                ErrorCodeDefine errorCode = (ErrorCodeDefine)result.ErrorCodeNumber;

                if(errorCode != ErrorCodeDefine.Success) fileService.DeleteFile(savePath);

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
                ResponseGetCoachEditDataByIdDto response = mapper.Map<ResponseGetCoachEditDataByIdDto>(coach);
                response.PhotoUrl = response.PhotoUrl.Replace("\\", "/"); ;
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}