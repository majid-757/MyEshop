using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Web.Mvc;
using DataLayer;

namespace MyEshop
{
    public class RecaptchaHelper
    {
        private const string SecretKey = "6Lfpaf0pAAAAAOYzHevPaEQMxsAmCJTYr4wg8qD6"; // change this

        public RecaptchaResponseViewModel ValidateRecaptcha(string gRecaptchaResponse)
        {
            string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            var postData = "secret=" + SecretKey + "&response=" + gRecaptchaResponse;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            string result;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResponseViewModel>(result);
            return captchaResponse;
        }
    }
}
