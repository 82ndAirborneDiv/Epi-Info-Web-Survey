﻿using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using System;
using System.Web.Security;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Reflection;
using System.Diagnostics;
namespace Epi.Web.MVC.Controllers
{
    public class FinalController : Controller
    {
        private ISurveyFacade _isurveyFacade;

        /// <summary>
        /// Injecting SurveyTransactionObject through constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        public FinalController(ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }

        [HttpGet]
        public ActionResult Index(string surveyId, string final, string responseId)
        {
            
            try
            {
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                ViewBag.Version = version;

                string SurveyMode = "";
                SurveyInfoModel surveyInfoModel = GetSurveyInfo(surveyId);
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\r\n|\r|\n)+");

                string exitText = regex.Replace(surveyInfoModel.ExitText.Replace("  ", " &nbsp;"), "<br />");
                surveyInfoModel.ExitText = MvcHtmlString.Create(exitText).ToString();

                string strPassCode = Epi.Web.MVC.Utility.SurveyHelper.GetPassCode();

                surveyInfoModel.PassCode = strPassCode;
                TempData["PassCode"] = strPassCode;

                if (surveyInfoModel.IsDraftMode)
                {
                    surveyInfoModel.IsDraftModeStyleClass = "draft";
                }
                else
                {
                    surveyInfoModel.IsDraftModeStyleClass = "final";
                }
                bool IsMobileDevice = false;
                IsMobileDevice = this.Request.Browser.IsMobileDevice;
                Omniture OmnitureObj = Epi.Web.MVC.Utility.OmnitureHelper.GetSettings(SurveyMode, IsMobileDevice);
                ViewBag.ResponseId = responseId; 
                ViewBag.Omniture = OmnitureObj;
                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, surveyInfoModel);
            }
            catch (Exception ex)
            {
                Epi.Web.Utility.ExceptionMessage.SendLogMessage( ex, this.HttpContext);
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
        }

        [HttpPost]
        public ActionResult Index(string surveyId, SurveyAnswerModel surveyAnswerModel)
        {
            try
            {
                bool isMobileDevice = this.Request.Browser.IsMobileDevice;

                if (isMobileDevice == false)
                {
                    isMobileDevice = Epi.Web.MVC.Utility.SurveyHelper.IsMobileDevice(this.Request.UserAgent.ToString());
                }
                if (!string.IsNullOrEmpty(this.Request.Form["is_print_action"]) && this.Request.Form["is_print_action"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                    string ResponseId = this.Request.Form["ResponseId"];
                    ActionResult actionResult = RedirectToAction("Index", "Print", new { responseId = ResponseId , FromFinal = true});
                    return actionResult;
                    }
                FormsAuthentication.SignOut();
                FormsAuthentication.SetAuthCookie("BeginSurvey", false);
                Guid responseId = Guid.NewGuid();
                Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer = _isurveyFacade.CreateSurveyAnswer(surveyId, responseId.ToString());

                // Pass Code Logic  start 
                Epi.Web.Common.Message.UserAuthenticationResponse AuthenticationResponse = _isurveyFacade.GetAuthenticationResponse(responseId.ToString());

                string strPassCode = Epi.Web.MVC.Utility.SurveyHelper.GetPassCode();
                if (string.IsNullOrEmpty(AuthenticationResponse.PassCode))
                    {
                    _isurveyFacade.UpdatePassCode(responseId.ToString(), TempData["PassCode"].ToString());
                    }


                SurveyInfoModel surveyInfoModel = GetSurveyInfo(SurveyAnswer.SurveyId);
                XDocument xdoc = XDocument.Parse(surveyInfoModel.XML);
                MvcDynamicForms.Form form = _isurveyFacade.GetSurveyFormData(SurveyAnswer.SurveyId, 1, SurveyAnswer, isMobileDevice);

                var _FieldsTypeIDs = from _FieldTypeID in
                                     xdoc.Descendants("Field")
                                     select _FieldTypeID;

                foreach (var _FieldTypeID in _FieldsTypeIDs)
                {
                    bool isRequired;
                    string attributeValue = _FieldTypeID.Attribute("IsRequired").Value;

                    if (bool.TryParse(attributeValue, out isRequired))
                    {
                        if (isRequired)
                        {
                            if (!form.RequiredFieldsList.Contains(_FieldTypeID.Attribute("Name").Value))
                            {
                                if (form.RequiredFieldsList != "")
                                {
                                    form.RequiredFieldsList = form.RequiredFieldsList + "," + _FieldTypeID.Attribute("Name").Value.ToLower();
                                }
                                else
                                {
                                    form.RequiredFieldsList = _FieldTypeID.Attribute("Name").Value.ToLower();
                                }
                            }
                        }
                    }
                }
                
                _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, SurveyAnswer.ResponseId, form, SurveyAnswer, false, false, 1);

                return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseId = responseId, PageNumber = 1 });
            }
            catch (Exception ex)
            {
                Epi.Web.Utility.ExceptionMessage.SendLogMessage(  ex, this.HttpContext);
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }

        }
        public SurveyInfoModel GetSurveyInfo(string SurveyId)
        {
            SurveyInfoModel surveyInfoModel = _isurveyFacade.GetSurveyInfoModel(SurveyId);
            return surveyInfoModel;
        }
    }
}
