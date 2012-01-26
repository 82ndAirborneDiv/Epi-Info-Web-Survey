﻿using System;
using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using System.Collections.Generic;
namespace Epi.Web.MVC.Controllers
{
    public class SurveyController : Controller
    {


        

        //declare SurveyTransactionObject object
        private ISurveyFacade _isurveyFacade;
        /// <summary>
        /// Injectinting SurveyTransactionObject through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        
        public SurveyController(ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
        
        
                
       
       /// <summary>
       /// create the new resposeid and put it in temp data. create the form object. create the first survey response
       /// </summary>
       /// <param name="surveyId"></param>
       /// <returns></returns>
 
    [HttpGet]
        public ActionResult Index(SurveyInfoModel surveyInfoModel,string surveyId,int PageNumber )
        {

            //if (!string.IsNullOrEmpty(page))
            //{

            
            //get the survey form
            try
            {

             //   var form = _isurveyFacade.GetSurveyFormData(surveyId, this.GetCurrentPage(), this.GetCurrentSurveyAnswer());

                var form = _isurveyFacade.GetSurveyFormData(surveyId, PageNumber, this.GetCurrentSurveyAnswer());
          
                
                //create the responseid
                Guid ResponseIDGuid = Guid.NewGuid();
                string ResponseID = ResponseIDGuid.ToString();
                if (TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] == null)
                {
                    

                    //put the ResponseId in Temp data for later use
                    TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = ResponseID.ToString();
                    _isurveyFacade.CreateSurveyAnswer(surveyId, ResponseID.ToString());
                }
                else
                {
                    ResponseID = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
                    UpdateModel(form);

                    if (form.Validate())
                    {
                        string responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
                        _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form);

                        return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                    }


                    return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);

                }
                
               return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                 
               
            }
            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
            //}
            //return null;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SurveyInfoModel surveyInfoModel,string Submit)
        {

            try
            {
                //get the survey form
                MvcDynamicForms.Form form = _isurveyFacade.GetSurveyFormData(surveyInfoModel.SurveyId, this.GetCurrentPage(), this.GetCurrentSurveyAnswer());
                //Update the model
                UpdateModel(form);

                if (form.Validate())
                {
                    string responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();
                    _isurveyFacade.UpdateSurveyResponse(surveyInfoModel, responseId, form);

                    //return RedirectToAction("Index", "Final", new {id="final" });
                    TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = null;
                    return RedirectToAction("Index", "Final");
                }
                else
                {

                    return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE, form);
                }

            }
            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
        }




        public ActionResult Index5(SurveyInfoModel surveyInfoModel, string Submit)
        {

            try
            {

                return View(Epi.Web.MVC.Constants.Constant.INDEX_PAGE);
            }
            catch (Exception ex)
            {
                return View(Epi.Web.MVC.Constants.Constant.EXCEPTION_PAGE);
            }
        }


        
        private int GetCurrentPage()
        {
            int CurrentPage = 1;
            if (ViewData.ContainsKey(Epi.Web.MVC.Constants.Constant.CURRENT_PAGE) && ViewData[Epi.Web.MVC.Constants.Constant.CURRENT_PAGE] != null)
            {
                int.TryParse(ViewData[Epi.Web.MVC.Constants.Constant.CURRENT_PAGE].ToString(), out CurrentPage);
            }

            return CurrentPage;
        }

        private Epi.Web.Common.DTO.SurveyAnswerDTO GetCurrentSurveyAnswer()
        {
            Epi.Web.Common.DTO.SurveyAnswerDTO result = null;

            if (TempData.ContainsKey(Epi.Web.MVC.Constants.Constant.RESPONSE_ID) && TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] != null
                                                                               && !string.IsNullOrEmpty(TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString()))
            {
               
                string responseId = TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID].ToString();

               
                   //TODO: Now repopulating the TempData (by reassigning to responseId) so it persisits, later we will need to find a better 
                   //way to replace it. 
                    TempData[Epi.Web.MVC.Constants.Constant.RESPONSE_ID] = responseId;
                    return _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
               


            }

            return result;
        }


       


    }
}
