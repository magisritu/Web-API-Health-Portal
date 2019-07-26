using BusinessLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace UserHealthPortal.Common
{
    public class BasicAuthentication : AuthorizationFilterAttribute
    {
        UserDetailsBusiness UDBusiness = new UserDetailsBusiness();
        string hashCode = "h0sh@key";
        public string[] UserDetails(string token)
        {
            string decryption = "";
            byte[] data = Convert.FromBase64String(token);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashCode));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    decryption = UTF8Encoding.UTF8.GetString(results);
                }
            }
            string[] encToken = decryption.Split('~');
            return encToken;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //string token = this.Request.Headers.GetValues("Token").Single();
            string token = actionContext.Request.Headers.GetValues("Token").Single();
            string[] userDetails = UserDetails(token);
            string email = userDetails[0];
            string password = userDetails[1];
            int userID = Convert.ToInt32(userDetails[2]);

            if (actionContext.Request.Headers.GetValues("Token").Single() == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                if (UDBusiness.CheckCredential(email,password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(email), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            base.OnAuthorization(actionContext);
        }

    }
}