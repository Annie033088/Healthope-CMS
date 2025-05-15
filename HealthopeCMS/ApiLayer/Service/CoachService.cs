using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Coach.Request;
using ApiLayer.Models.Other;
using AutoMapper;
using DomainLayer.Models;
using DomainLayer.Utility;
using PersistentLayer.Interface;
using PersistentLayer.Models;
using static StackExchange.Redis.Role;

namespace ApiLayer.Service
{
    public class CoachService : ICoachService
    {
        private readonly IMapper mapper;
        private readonly ICoachRepository coachRepository;

        public CoachService(IMapper mapper, ICoachRepository coachRepository)
        {
            this.mapper = mapper;
            this.coachRepository = coachRepository;
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
                    string imageExtension = "";

                    if (file.MimeType == "image/jpeg") imageExtension = ".jpg";
                    else if (file.MimeType == "image/png") imageExtension = ".png";
                    else if (file.MimeType == "image/webp") imageExtension = ".webp";

                    string fileName = Guid.NewGuid().ToString() + imageExtension;
                    string folderPath = Path.Combine(HttpContext.Current.Server.MapPath("~/"), "assets", "images", "coach");
                    savePath = Path.Combine(folderPath, fileName);
                    coach.PhotoUrl = Path.Combine("assets", "images", "coach", fileName);

                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    File.WriteAllBytes(savePath, file.FileData);
                }

                OperationResult result = coachRepository.addCoach(coach);

                if (!Enum.IsDefined(typeof(ErrorCodeDefine), result.ErrorCodeNumber) || result.Exception != null)
                {
                    if (File.Exists(savePath)) File.Delete(savePath);

                    return (ErrorCodeDefine.ServerError, result.Exception);
                }

                ErrorCodeDefine errorCode = (ErrorCodeDefine)result.ErrorCodeNumber;

                if(errorCode != ErrorCodeDefine.Success)
                    if (File.Exists(savePath)) File.Delete(savePath);

                return (errorCode, result.Exception);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}