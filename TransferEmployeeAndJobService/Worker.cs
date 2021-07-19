using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TransferEmployeeAndJobService.Models;
using TransferEmployeeAndJobService.ReqModels;
using System.Text;
using System.Text.Json;

namespace TransferEmployeeAndJobService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly EmployeeRequestDBContext _contextReq;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _contextReq = new EmployeeRequestDBContext();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    List<TblUser> list = null;
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:49388/api/person/");
                        var userList = await client.GetAsync(@"get/ym/S33@||");
                        if (userList.IsSuccessStatusCode)
                        {
                            string result = await userList.Content.ReadAsStringAsync();
                            list = JsonConvert.DeserializeObject<List<TblUser>>(result);
                        }
                    }
                    if (list != null)
                    {
                        foreach (var user in list)
                        {
                            #region Transfer
                            var t = await _contextReq.TblEmployeeRequestEmployees.FindAsync(user.Id);
                            if (t == null)
                            {
                                TblEmployeeRequestEmployee TblEmployeeRequestEmployee = new TblEmployeeRequestEmployee()
                                {
                                    FldEmployeeRequestEmployeeId = user.Id,
                                    FldEmployeeRequestEmployeeCurrentLevel = user.CurrentLevel,
                                    FldEmployeeRequestEmployeePassword = user.Password,
                                    FldEmployeeRequestEmployeeUsername = user.Username,
                                    FldEmployeeRequestPagesSequenceId = user.PagesSequenceId
                                };
                                await _contextReq.TblEmployeeRequestEmployees.AddAsync(TblEmployeeRequestEmployee);


                                var TblEmergencyCalls = user.TblEmergencyCalls.Where(a => a.UserId == user.Id);
                                if (TblEmergencyCalls.Any())
                                {
                                    foreach (var item in TblEmergencyCalls)
                                    {
                                        TblEmployeeRequestEmergencyCall tblEmployeeRequestEmergencyCall = new TblEmployeeRequestEmergencyCall()
                                        {
                                            FldEmployeeRequestEmergencyCallDescription = item.Description,
                                            FldEmployeeRequestEmergencyCallFirstName = item.FirstName,
                                            FldEmployeeRequestEmergencyCallId = item.Id,
                                            FldEmployeeRequestEmergencyCallLastName = item.LastName,
                                            FldEmployeeRequestEmergencyCallPhoneNo = item.PhoneNo,
                                            FldEmployeeRequestEmergencyCallRelative = item.Relative,
                                            FldEmployeeRequestEmployeeId = item.UserId
                                        };
                                        await _contextReq.TblEmployeeRequestEmergencyCalls.AddAsync(tblEmployeeRequestEmergencyCall);
                                    }
                                }


                                var TblCustomerDegrees = user.TblCustomerDegrees.Where(a => a.UserId == user.Id);
                                if (TblCustomerDegrees.Any())
                                {
                                    foreach (var item in TblCustomerDegrees)
                                    {
                                        ReqModels.TblCustomerDegree TblCustomerDegree = new ReqModels.TblCustomerDegree()
                                        {
                                            UserId = item.UserId,
                                            DiplomaId = item.DiplomaId,
                                            EducationId = item.EducationId,
                                            FldCustomerDegreeId = item.FldCustomerDegreeId,
                                            FldDes = item.FldDes,
                                            FldEducationName = item.FldEducationName,
                                            FldEndDate = item.FldEndDate,
                                            FldExportDate = item.FldExportDate,
                                            FldExportNo = item.FldExportNo,
                                            FldPoint = item.FldPoint,
                                            FldStartDate = item.FldStartDate,
                                            FldStudyCity = item.FldStudyCity,
                                            FldStudyPlace = item.FldStudyPlace
                                        };
                                        await _contextReq.TblCustomerDegrees.AddAsync(TblCustomerDegree);
                                    }
                                }


                                var TblGeneralRecords = user.TblGeneralRecords.Where(a => a.UserId == user.Id);
                                if (TblGeneralRecords.Any())
                                {
                                    foreach (var item in TblGeneralRecords)
                                    {
                                        ReqModels.TblEmployeeRequestGeneralRecord TblEmployeeRequestGeneralRecord = new TblEmployeeRequestGeneralRecord()
                                        {
                                            FldEmployeeRequestEmployeeId = item.UserId,
                                            FldEmployeeRequestGeneralRecordCriminalDes = item.CriminalDes,
                                            FldEmployeeRequestGeneralRecordCriminalTiltle = item.CriminalTiltle,
                                            FldEmployeeRequestGeneralRecordDescription = item.Description,
                                            FldEmployeeRequestGeneralRecordId = item.Id
                                        };
                                        await _contextReq.TblEmployeeRequestGeneralRecords.AddAsync(TblEmployeeRequestGeneralRecord);
                                    }
                                }


                                TblHowFind TblHowFind = user.TblHowFinds.Where(a => a.UserId == user.Id).FirstOrDefault();
                                if (TblHowFind != null)
                                {
                                    ReqModels.TblEmployeeRequestHowFind TblEmployeeRequestHowFind = new TblEmployeeRequestHowFind()
                                    {
                                        FldEmployeeRequestEmployeeId = TblHowFind.UserId,
                                        FldEmployeeRequestHowFindAdditionalDes = TblHowFind.AdditionalDes,
                                        FldEmployeeRequestHowFindId = TblHowFind.Id,
                                        FldEmployeeRequestHowFindTitle = TblHowFind.HowFindTitle
                                    };
                                    await _contextReq.TblEmployeeRequestHowFinds.AddAsync(TblEmployeeRequestHowFind);
                                }



                                var TblIpLogs = user.TblIpLogs.Where(a => a.UserId == user.Id);
                                if (TblIpLogs.Any())
                                {
                                    foreach (var item in TblIpLogs)
                                    {
                                        ReqModels.TblEmployeeRequestIpLog TblEmployeeRequestIpLog = new TblEmployeeRequestIpLog()
                                        {
                                            FldEmployeeRequestEmployeeId = item.UserId,
                                            FldEmployeeRequestIpLogDateTime = item.DateTime,
                                            FldEmployeeRequestIpLogId = item.Id,
                                            FldEmployeeRequestIpLogIp = item.Ip
                                        };
                                        await _contextReq.TblEmployeeRequestIpLogs.AddAsync(TblEmployeeRequestIpLog);
                                    }
                                }


                                var TblMedicalRecords = user.TblMedicalRecords.Where(a => a.UserId == user.Id);
                                if (TblMedicalRecords.Any())
                                {
                                    foreach (var item in TblMedicalRecords)
                                    {
                                        ReqModels.TblEmployeeRequestMedicalRecord TblEmployeeRequestMedicalRecord = new TblEmployeeRequestMedicalRecord()
                                        {
                                            FldEmployeeRequestEmployeeId = item.UserId,
                                            FldEmployeeRequestMedicalRecordComplicationsDes = item.ComplicationsDes,
                                            FldEmployeeRequestMedicalRecordDescription = item.Description,
                                            FldEmployeeRequestMedicalRecordDisease = item.Disease,
                                            FldEmployeeRequestMedicalRecordEndDate = item.EndDate,
                                            FldEmployeeRequestMedicalRecordHasComplications = item.HasComplications,
                                            FldEmployeeRequestMedicalRecordHasProblem = item.HasProblem,
                                            FldEmployeeRequestMedicalRecordId = item.Id,
                                            FldEmployeeRequestMedicalRecordIsAddict = item.IsAddict,
                                            FldEmployeeRequestMedicalRecordProblemDes = item.ProblemDes,
                                            FldEmployeeRequestMedicalRecordStartDate = item.StartDate
                                        };
                                        await _contextReq.TblEmployeeRequestMedicalRecords.AddAsync(TblEmployeeRequestMedicalRecord);
                                    }
                                }


                                var TblPageTimeLogs = user.TblPageTimeLogs.Where(a => a.UserId == user.Id);
                                if (TblPageTimeLogs.Any())
                                {
                                    foreach (var item in TblPageTimeLogs)
                                    {
                                        ReqModels.TblEmployeeRequestPageTimeLog TblEmployeeRequestPageTimeLog = new TblEmployeeRequestPageTimeLog()
                                        {
                                            FldEmployeeRequestEmployeeId = item.UserId,
                                            FldEmployeeRequestPageTimeLogEndTime = item.EndTime,
                                            FldEmployeeRequestPageTimeLogStartTime = item.StartTime,
                                            FldEmployeeRequestPageTimeLogId = item.Id,
                                            FldEmployeeRequestPageTimeLogPageLevel = item.PageLevel
                                        };
                                        await _contextReq.TblEmployeeRequestPageTimeLogs.AddAsync(TblEmployeeRequestPageTimeLog);
                                    }
                                }



                                TblPrimaryInformation TblPrimaryInformation = user.TblPrimaryInformations.Where(a => a.UserId == user.Id).FirstOrDefault();
                                if (TblPrimaryInformation != null)
                                {
                                    ReqModels.TblEmployeeRequestPrimaryInformation TblEmployeeRequestPrimaryInformation = new TblEmployeeRequestPrimaryInformation()
                                    {
                                        FldEmployeeRequestEmployeeId = TblPrimaryInformation.UserId,
                                        FldEmployeeRequestPrimaryInformationBirthDate = TblPrimaryInformation.BirthDate,
                                        FldEmployeeRequestPrimaryInformationChildrenNo = TblPrimaryInformation.ChildrenNo,
                                        FldEmployeeRequestPrimaryInformationFirstName = TblPrimaryInformation.FirstName,
                                        FldEmployeeRequestPrimaryInformationGender = TblPrimaryInformation.Gender,
                                        FldEmployeeRequestPrimaryInformationId = TblPrimaryInformation.Id,
                                        FldEmployeeRequestPrimaryInformationLastName = TblPrimaryInformation.LastName,
                                        FldEmployeeRequestPrimaryInformationMarital = TblPrimaryInformation.Marital,
                                        FldEmployeeRequestPrimaryInformationNationalCode = TblPrimaryInformation.NationalCode,
                                        FldEmployeeRequestPrimaryInformationPhoneNo = TblPrimaryInformation.PhoneNo,
                                        FldEmployeeRequestPrimaryInformationPostalCode = TblPrimaryInformation.PostalCode,
                                        FldEmployeeRequestPrimaryInformationTrackNo = TblPrimaryInformation.TrackNo,
                                        FldEmployeeRequestPrimaryInformationTutelage = TblPrimaryInformation.Tutelage
                                    };
                                    await _contextReq.TblEmployeeRequestPrimaryInformations.AddAsync(TblEmployeeRequestPrimaryInformation);
                                }



                                var TblUserCompilations = user.TblUserCompilations.Where(a => a.UserId == user.Id);
                                if (TblUserCompilations.Any())
                                {
                                    foreach (var item in TblUserCompilations)
                                    {
                                        ReqModels.TblEmployeeRequestUserCompilation TblEmployeeRequestUserCompilation = new TblEmployeeRequestUserCompilation()
                                        {
                                            FldEmployeeRequestCompilationTypeId = item.CompilationTypeId,
                                            FldEmployeeRequestEmployeeId = item.UserId,
                                            FldEmployeeRequestUserCompilationAuthor = item.Author,
                                            FldEmployeeRequestUserCompilationDate = item.Date,
                                            FldEmployeeRequestUserCompilationDescription = item.Description,
                                            FldEmployeeRequestUserCompilationExplanation = item.Explanation,
                                            FldEmployeeRequestUserCompilationId = item.Id,
                                            FldEmployeeRequestUserCompilationTitle = item.Title
                                        };
                                        await _contextReq.TblEmployeeRequestUserCompilations.AddAsync(TblEmployeeRequestUserCompilation);
                                    }
                                }



                                var TblUserCreativitys = user.TblUserCreativities.Where(a => a.UserId == user.Id);
                                if (TblUserCreativitys.Any())
                                {
                                    foreach (var item in TblUserCreativitys)
                                    {
                                        ReqModels.TblEmployeeRequestUserCreativity TblEmployeeRequestUserCreativity = new TblEmployeeRequestUserCreativity()
                                        {
                                            FldEmployeeRequestEmployeeId = item.UserId,
                                            FldEmployeeRequestCreativityTypeId = item.CreativityTypeId,
                                            FldEmployeeRequestUserCreativityDate = item.Date,
                                            FldEmployeeRequestUserCreativityDescription = item.Description,
                                            FldEmployeeRequestUserCreativityExplanation = item.Explanation,
                                            FldEmployeeRequestUserCreativityId = item.Id,
                                            FldEmployeeRequestUserCreativityTitle = item.Title
                                        };
                                        await _contextReq.TblEmployeeRequestUserCreativities.AddAsync(TblEmployeeRequestUserCreativity);
                                    }
                                }



                                var TblUserJobs = user.TblUserJobs.Where(a => a.UserId == user.Id);
                                if (TblUserJobs.Any())
                                {
                                    foreach (var item in TblUserJobs)
                                    {
                                        ReqModels.TblEmployeeRequestUserJob TblEmployeeRequestUserJob = new TblEmployeeRequestUserJob()
                                        {
                                            FldEmployeeRequestEmployeeId = item.UserId,
                                            FldEmployeeRequestJobsId = item.JobsId,
                                            FldEmployeeRequestUserJobDescription = item.Description,
                                            FldEmployeeRequestUserJobId = item.Id,
                                            FldEmployeeRequestUserJobRequestMoney = item.RequestMoney,
                                            FldEmployeeRequestUserJobTitle = item.JobTitle,
                                            FldEmployeeRequestUserJobWhatKnowAbout = item.WhatKnowAbout
                                        };
                                        await _contextReq.TblEmployeeRequestUserJobs.AddAsync(TblEmployeeRequestUserJob);
                                    }
                                }



                                var TblUserLanguages = user.TblUserLanguages.Where(a => a.UserId == user.Id);
                                if (TblUserLanguages.Any())
                                {
                                    foreach (var item in TblUserLanguages)
                                    {
                                        ReqModels.TblEmployeeRequestUserLanguage TblEmployeeRequestUserLanguage = new TblEmployeeRequestUserLanguage()
                                        {
                                            FldEmployeeRequestEmployeeId = item.UserId,
                                            FldEmployeeRequestUserLanguageDescription = item.Description,
                                            FldEmployeeRequestUserLanguageId = item.Id,
                                            FldEmployeeRequestUserLanguageLanguageTypeId = item.LanguageTypeId,
                                            FldEmployeeRequestUserLanguageListeningLevel = item.ListeningLevel,
                                            FldEmployeeRequestUserLanguageReadingLevel = item.ReadingLevel,
                                            FldEmployeeRequestUserLanguageSpeakingLevel = item.SpeakingLevel,
                                            FldEmployeeRequestUserLanguageWritingLevel = item.WritingLevel,
                                            FldEmployeeRequestUserLanguageIsNative = item.IsNative
                                        };
                                        await _contextReq.TblEmployeeRequestUserLanguages.AddAsync(TblEmployeeRequestUserLanguage);
                                    }
                                }



                                TblUserMilitary TblUserMilitary = user.TblUserMilitaries.Where(a => a.UserId == user.Id).FirstOrDefault();
                                if (TblUserMilitary != null)
                                {
                                    if (TblUserMilitary != null)
                                    {
                                        ReqModels.TblEmployeeRequestUserMilitary TblEmployeeRequestUserMilitary = new TblEmployeeRequestUserMilitary()
                                        {
                                            FldEmployeeRequestEmployeeId = TblUserMilitary.UserId,
                                            FldEmployeeRequestMilitaryId = TblUserMilitary.MilitaryId,
                                            FldEmployeeRequestMilitaryOrganizationId = TblUserMilitary.OrganizationId,
                                            FldEmployeeRequestUserMilitaryCity = TblUserMilitary.City,
                                            FldEmployeeRequestUserMilitaryDescription = TblUserMilitary.Description,
                                            FldEmployeeRequestUserMilitaryEndDate = TblUserMilitary.EndDate,
                                            FldEmployeeRequestUserMilitaryExemptDescription = TblUserMilitary.ExemptDescription,
                                            FldEmployeeRequestUserMilitaryId = TblUserMilitary.Id,
                                            FldEmployeeRequestUserMilitaryStartDate = TblUserMilitary.StartDate,
                                            FldEmployeeRequestUserMilitaryUnit = TblUserMilitary.Unit
                                        };
                                        await _contextReq.TblEmployeeRequestUserMilitaries.AddAsync(TblEmployeeRequestUserMilitary);
                                    }
                                }




                                var TblUserSkills = user.TblUserSkills.Where(a => a.UserId == user.Id);
                                if (TblUserSkills.Any())
                                {
                                    foreach (var item in TblUserSkills)
                                    {
                                        ReqModels.TblEmployeeRequestUserSkill TblEmployeeRequestUserSkill = new TblEmployeeRequestUserSkill()
                                        {
                                            FldEmployeeRequestEmployeeId = item.UserId,
                                            FldEmployeeRequestSkillsId = item.SkillId,
                                            FldEmployeeRequestUserSkillDate = item.Date,
                                            FldEmployeeRequestUserSkillDescription = item.Description,
                                            FldEmployeeRequestUserSkillId = item.Id,
                                            FldEmployeeRequestUserSkillIsSelfTaught = item.IsSelfTaught,
                                            FldEmployeeRequestUserSkillLicenseNo = item.LicenseNo,
                                            FldEmployeeRequestUserSkillLicenseReference = item.LicenseReference,
                                            FldEmployeeRequestUserSkillLocation = item.Location,
                                            FldEmployeeRequestUserSkillSkillLevel = item.SkillLevel,
                                            FldEmployeeRequestUserSkillSkillTitle = item.SkillTitle,
                                            FldEmployeeRequestUserSkillSkillType = item.SkillType
                                        };
                                        await _contextReq.TblEmployeeRequestUserSkills.AddAsync(TblEmployeeRequestUserSkill);
                                    }
                                }

                                List<long> weids = new List<long>();
                                var TblWorkExperiences = user.TblWorkExperiences.Where(a => a.UserId == user.Id);
                                if (TblWorkExperiences.Any())
                                {
                                    foreach (var item in TblWorkExperiences)
                                    {
                                        weids.Add(item.FldWorkExperienceId);
                                        ReqModels.TblWorkExperience TblWorkExperience = new ReqModels.TblWorkExperience()
                                        {
                                            FldAmountOfDailyInsurance = item.FldAmountOfDailyInsurance,
                                            FldCompanyName = item.FldCompanyName,
                                            FldContactInnerNumberOfCompany = item.FldContactInnerNumberOfCompany,
                                            FldContactNumberOfCompany = item.FldContactNumberOfCompany,
                                            FldCustomerName = item.FldCustomerName,
                                            FldDescription = item.FldDescription,
                                            FldEarlySalary = item.FldEarlySalary,
                                            FldEndDate = item.FldEndDate,
                                            FldJobTitle = item.FldJobTitle,
                                            FldLateSalary = item.FldLateSalary,
                                            FldLeaveJobId = item.FldLeaveJobId,
                                            FldReasonsToLeaveJob = item.FldReasonsToLeaveJob,
                                            FldRelatedPeople = item.FldRelatedPeople,
                                            FldStartDate = item.FldStartDate,
                                            FldTaminJobId = item.FldTaminJobId,
                                            FldUnitName = item.FldUnitName,
                                            FldWorkDay = item.FldWorkDay,
                                            FldWorkExperienceId = item.FldWorkExperienceId,
                                            FldWorkTime = item.FldWorkTime,
                                            HasInsurance = item.HasInsurance,
                                            InsuranceNo = item.InsuranceNo,
                                            IsWorking = item.IsWorking,
                                            PreviousJobAchievements = item.PreviousJobAchievements,
                                            UserId = item.UserId,
                                            WhyWantChangeJob = item.WhyWantChangeJob,
                                        };
                                        await _contextReq.TblWorkExperiences.AddAsync(TblWorkExperience);
                                    }
                                }

                                if (weids.Any())
                                {
                                    foreach (var item in weids)
                                    {
                                        var TblWorkExperienceLeaveJobDtls = user.TblWorkExperiences.FirstOrDefault(a => a.FldWorkExperienceId == item).TblWorkExperienceLeaveJobDtls;
                                        if (TblWorkExperienceLeaveJobDtls.Any())
                                        {
                                            foreach (var item2 in TblWorkExperienceLeaveJobDtls)
                                            {
                                                ReqModels.TblWorkExperienceLeaveJobDtl TblWorkExperienceLeaveJobDtl = new ReqModels.TblWorkExperienceLeaveJobDtl()
                                                {
                                                    FldLeaveJobId = item2.FldLeaveJobId,
                                                    FldLeaveJobPercent = item2.FldLeaveJobPercent,
                                                    FldWorkExperienceId = item2.FldWorkExperienceId,
                                                    FldWorkExperienceLeaveJobDtlId = item2.FldWorkExperienceLeaveJobDtlId,
                                                    FldLeaveJob = item2.FldLeaveJob
                                                };
                                                await _contextReq.TblWorkExperienceLeaveJobDtls.AddAsync(TblWorkExperienceLeaveJobDtl);
                                            }
                                        }
                                    }
                                }

                                await _contextReq.SaveChangesAsync();

                                using (var client = new HttpClient())
                                {
                                    client.BaseAddress = new Uri("http://localhost:49388/api/person/");
                                    var userList = await client.GetAsync($"{user.Id}");
                                    if (userList.IsSuccessStatusCode)
                                    {
                                        string result = await userList.Content.ReadAsStringAsync();
                                        _logger.LogInformation(DateTime.Now + Environment.NewLine + "New user submitted successfully");
                                    }
                                    else
                                    {
                                        _logger.LogError("Error on submitting user");
                                    }
                                }

                            }
                            #endregion
                        }
                    }

                    var notPublishedRequestedJobs = _contextReq.TblEmployeeRequestEmployeeRequests
                    .Where(a => a.IsPublished == false && a.FldEmployeeRequestEmployeeRequestIsTransfered == true)
                    .Include(t => t.FldEmployeeRequestJobs)
                    .Include(t => t.FldEmployeeRequestJobOnet)
                    .Include(t => t.FldEmployeeRequestJobTamin).ToList();

                    if (notPublishedRequestedJobs.Any())
                    {
                        foreach (var item in notPublishedRequestedJobs)
                        {
                            #region TransferJob

                            string jobTitleFrom = null;

                            if (item.FldEmployeeRequestJobTitleFromId == 1)
                            {
                                jobTitleFrom = item.FldEmployeeRequestJobs.JobsName;
                            }
                            else if (item.FldEmployeeRequestJobTitleFromId == 2)
                            {
                                jobTitleFrom = item.FldEmployeeRequestJobTamin.FldTaminJobName;
                            }
                            else
                            {
                                jobTitleFrom = item.FldEmployeeRequestJobOnet.FldJobName;
                            }

                            using (var client = new HttpClient())
                            {
                                string res = "";
                                client.BaseAddress = new Uri("http://localhost:49388/api/person");
                                Models.TblJob newJob = new Models.TblJob()
                                {
                                    JobTitle = jobTitleFrom,
                                    IsActive = true,
                                    StartDate = item.FldEmployeeRequestEmployeeRequestStartDate,
                                    EndDate = item.FldEmployeeRequestEmployeeRequestEndDate,
                                    NeedMan = item.FldEmployeeRequestEmployeeRequestNeedMan,
                                    NeedWoman = item.FldEmployeeRequestEmployeeRequestNeedWoman,
                                    Description = item.FldEmployeeRequestEmployeeRequestJobDescription
                                };
                                var stringPayload = System.Text.Json.JsonSerializer.Serialize(newJob);
                                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                                HttpResponseMessage response = await client.PostAsync("", httpContent);
                                if (response.IsSuccessStatusCode)
                                {
                                    string result = await response.Content.ReadAsStringAsync();
                                    res = result;
                                }
                                else
                                {
                                    res = "Internal server Error";
                                }

                                if (res.Trim().ToLower() != "error")
                                {
                                    item.IsPublished = true;
                                    _contextReq.TblEmployeeRequestEmployeeRequests.Update(item);
                                    await _contextReq.SaveChangesAsync();

                                    _logger.LogInformation($"job submitted with id => {res} @ {DateTime.Now}");
                                }
                                else
                                {
                                    _logger.LogError("Error on submit job! @ " + DateTime.Now);

                                }
                            }
                            #endregion
                        }
                    }

                    await Task.Delay(900000, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error has occurd! @ " + DateTime.Now);
                    _logger.LogError(e.Message);

                    await LogErrors(DateTime.Now.ToString() + Environment.NewLine + e.Message + Environment.NewLine + Environment.NewLine);
                }
            }
        }

        private async static Task LogErrors(string error)
        {
            using (StreamWriter writetext = new StreamWriter("log.txt", true))
            {
                await writetext.WriteLineAsync(error);
            }
        }
    }
}
