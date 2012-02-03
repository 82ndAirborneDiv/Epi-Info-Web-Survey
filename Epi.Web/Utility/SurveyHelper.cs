﻿using System;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.Common.Message;
using Epi.Web.MVC.Constants;
using Epi.Web.MVC.Utility;
using Epi.Web.MVC.Models;
using Epi.Web.MVC.Facade;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Xml.XPath;
namespace Epi.Web.MVC.Utility
{
    public class SurveyHelper
    {
        /// <summary>
        /// Creates the first survey response in the response table
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="responseId"></param>
        /// <param name="surveyAnswerRequest"></param>
        /// <param name="surveyAnswerDTO"></param>
        /// <param name="surveyResponseXML"></param>
        /// <param name="iSurveyAnswerRepository"></param>
        public static void CreateSurveyResponse(string surveyId, string responseId,SurveyAnswerRequest surveyAnswerRequest,
                                          Common.DTO.SurveyAnswerDTO surveyAnswerDTO,
                                          SurveyResponseXML surveyResponseXML, ISurveyAnswerRepository iSurveyAnswerRepository)
        {
            bool AddRoot = false;
            surveyAnswerRequest.Criteria.SurveyAnswerIdList.Add(responseId.ToString());
            surveyAnswerDTO.ResponseId = responseId.ToString();
            surveyAnswerDTO.DateCompleted = DateTime.Now;
            surveyAnswerDTO.SurveyId = surveyId;
            surveyAnswerDTO.Status = (int)Constant.Status.InProgress;
            surveyAnswerDTO.XML = surveyResponseXML.CreateResponseXml(surveyId, AddRoot,0).InnerXml;
            surveyAnswerRequest.SurveyAnswerList.Add(surveyAnswerDTO);
            surveyAnswerRequest.Action = Epi.Web.MVC.Constants.Constant.CREATE;  //"Create";
            iSurveyAnswerRepository.SaveSurveyAnswer(surveyAnswerRequest);
        }

        public static void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel,MvcDynamicForms.Form form, SurveyAnswerRequest surveyAnswerRequest,
                                                             SurveyResponseXML surveyResponseXML,
                                                            ISurveyAnswerRepository iSurveyAnswerRepository,
                                                             SurveyAnswerResponse surveyAnswerResponse,string responseId, Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswerDTO,bool IsSubmited)
        {
            // 1 Get the record for the current survey response
            // 2 update the current survey response
            // 3 save the current survey response

           
            // 2 a. update the current survey answer request
            surveyAnswerRequest.SurveyAnswerList = surveyAnswerResponse.SurveyResponseList;
            surveyResponseXML.Add(form);
            XDocument SavedXml = XDocument.Parse(surveyAnswerDTO.XML);
            bool AddRoot = false;
            if (SavedXml.Root.FirstAttribute.Value.ToString() == "0")
            {
                  AddRoot = true;
            }
            surveyAnswerRequest.SurveyAnswerList[0].XML = surveyResponseXML.CreateResponseXml(surveyInfoModel.SurveyId,AddRoot,form.CurrentPage).InnerXml;
            // 2 b. save the current survey response
            surveyAnswerRequest.Action = Epi.Web.MVC.Constants.Constant.UPDATE;  //"Update";
            //Append to Response Xml
           
            XDocument CurrentPageResponseXml = XDocument.Parse(surveyAnswerRequest.SurveyAnswerList[0].XML);
            if (SavedXml.Root.FirstAttribute.Value.ToString() != "0")
            { 
                surveyAnswerRequest.SurveyAnswerList[0].XML = MergeXml(SavedXml, CurrentPageResponseXml,form.CurrentPage).ToString(); 
            }
            if (IsSubmited)
            {
           
            surveyAnswerRequest.SurveyAnswerList[0].Status= 2;
            
            }
 
            iSurveyAnswerRepository.SaveSurveyAnswer(surveyAnswerRequest);
           
        }



      

        /// <summary>
        /// Returns a SurveyInfoDTO object
        /// </summary>
        /// <param name="surveyInfoRequest"></param>
        /// <param name="iSurveyInfoRepository"></param>
        /// <param name="SurveyId"></param>
        /// <returns></returns>
        public static Epi.Web.Common.DTO.SurveyInfoDTO GetSurveyInfoDTO(SurveyInfoRequest surveyInfoRequest,
                                                  ISurveyInfoRepository iSurveyInfoRepository,                 
                                                  string SurveyId)
        {
            surveyInfoRequest.Criteria.SurveyIdList.Add(SurveyId);
            return iSurveyInfoRepository.GetSurveyInfo(surveyInfoRequest).SurveyInfoList[0];
        }

        public static XDocument MergeXml(XDocument SavedXml, XDocument CurrentPageResponseXml, int Pagenumber)
        {

            XDocument xdoc = XDocument.Parse(SavedXml.ToString());
            XElement oldXElement = xdoc.XPathSelectElement("SurveyResponse/Page[@PageNumber = '" + Pagenumber.ToString() + "']");
 

            if (oldXElement == null)
            {
                SavedXml.Root.Add(CurrentPageResponseXml.Elements());
                return SavedXml;
            }

            else 
            {
                oldXElement.Remove();
                xdoc.Root.Add(CurrentPageResponseXml.Elements());
                return xdoc;
            }
            
        
        }

    }
}