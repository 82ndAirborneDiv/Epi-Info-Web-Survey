﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html textbox input element.
    /// </summary>
    [Serializable]
    public class TextBox : TextField
    {


        

        public override string RenderHtml()
        {
            var html = new StringBuilder();
            var inputName = _form.FieldPrefix + _key;
            string ErrorStyle = string.Empty;
            // prompt label
            var prompt = new TagBuilder("label");
            
            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", "EpiLabel");
            prompt.Attributes.Add("Id", "Label" + inputName);
            StringBuilder StyleValues = new StringBuilder();
            StyleValues.Append(GetContolStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(),_IsHidden));
            prompt.Attributes.Add("style", StyleValues.ToString());    
            html.Append(prompt.ToString());

            // error label
            if (!IsValid)
            {
                //Add new Error to the error Obj
                ErrorStyle = ";border-color: red";
            }

            // input element
            var txt = new TagBuilder("input");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "text");
           // txt.Attributes.Add("value", Value);
            ////////////Check code start//////////////////
                EnterRule FunctionObjectAfter = (EnterRule)_form.FormCheckCodeObj.Context.GetCommand("level=field&event=after&identifier=" + _key);
                if (FunctionObjectAfter != null)
                {
                    StringBuilder JavaScript = new StringBuilder();
                    FunctionObjectAfter.ToJavaScript(JavaScript); 
                    txt.Attributes.Add("onblur", "function " + _key + JavaScript.ToString() + "; " + _key + "_After();"); //After
                }
                EnterRule FunctionObjectBefore = (EnterRule)_form.FormCheckCodeObj.Context.GetCommand("level=field&event=before&identifier=" + _key);
                if (FunctionObjectBefore != null)
                {
                    StringBuilder JavaScript = new StringBuilder();
                    FunctionObjectBefore.ToJavaScript(JavaScript); 
                    txt.Attributes.Add("onfocus", "function " + _key + JavaScript.ToString() + "; " + _key + "_Before();"); //Before
                }

            ////////////Check code end//////////////////
            txt.Attributes.Add("value", Value);
            if(_IsRequired ==true){
            txt.Attributes.Add("class", "validate[required] text-input");
            txt.Attributes.Add("data-prompt-position", "topRight:15");
            }
            if (_MaxLength.ToString() != "0" && !string.IsNullOrEmpty(_MaxLength.ToString()))
            {
                txt.Attributes.Add("MaxLength", _MaxLength.ToString());
            }
            string IsHiddenStyle = "";
            if (_IsHidden)
            {
                IsHiddenStyle = "display:none";
            }
            txt.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle);            
          
            txt.MergeAttributes(_inputHtmlAttributes);
            html.Append(txt.ToString(TagRenderMode.SelfClosing));

            // If readonly then add the following jquery script to make the field disabled 
            if (ReadOnly)
            {
                var scriptReadOnlyText = new TagBuilder("script");
                scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            }


            var wrapper = new TagBuilder(_fieldWrapper);
            wrapper.Attributes["class"] = _fieldWrapperClass;
            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
        }
        
    }
}
