using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace PicturesAPI.Helpers
{
    public class PushNotification
    {
        /// <summary>
        /// Send GCM Message to device
        /// </summary>
        /// <param name="registrationId">registrationId</param>
        /// <param name="message">message</param>
        /// <returns>C2DMResponse object</returns>
        public static string SendGCMMessage(string registrationID, string message, string messagetype)
        {
            string status = string.Empty;
            string messageId = string.Empty;
            WebRequest request;
            var senderId = ConfigurationManager.AppSettings["AndriodDeviceSenderID"].ToString();
            string googleKey = ConfigurationManager.AppSettings["GoogleCloudAuthorizationkey"].ToString();
            string url = ConfigurationManager.AppSettings["GoogleCloudMessageUrl"].ToString();
            request = WebRequest.Create(url);
            request.Method = "post";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            request.Headers.Add(string.Format("Authorization: key={0}", googleKey));
            request.Headers.Add(string.Format("Sender: id={0}", senderId));
            String collaspeKey = Guid.NewGuid().ToString("n");
            String postData = string.Format("registration_id={0}&data.message={1}&collapse_key={2}&data.messagetype={3}", registrationID, message, collaspeKey, messagetype);
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                WebResponse tResponse = request.GetResponse();
                var webResp = request.GetResponse() as HttpWebResponse;

                if (webResp != null)
                {
                    //result.ResponseStatus = "Ok";
                    //result.ResponseCode = "Ok";

                    //Get the response body
                    var responseBody = "Error=";
                    try { responseBody = (new StreamReader(webResp.GetResponseStream())).ReadToEnd(); }
                    catch { }

                    //Handle the type of error
                    if (responseBody.StartsWith("Error="))
                    {
                        var wrErr = responseBody.Substring(responseBody.IndexOf("Error=") + 6);
                        switch (wrErr.ToLower().Trim())
                        {
                            case "quotaexceeded":
                                message = "QuotaExceeded";
                                status = "Failure";
                                break;
                            case "devicequotaexceeded":
                                message = "DeviceQuotaExceeded";
                                status = "Failure";
                                break;
                            case "invalidregistration":
                                message = "InvalidRegistration";
                                status = "Failure";
                                break;
                            case "notregistered":
                                message = "NotRegistered";
                                status = "Failure";
                                break;
                            case "messagetoobig":
                                message = "MessageTooBig";
                                status = "Failure";
                                break;
                            case "missingcollapsekey":
                                message = "MissingCollapseKey";
                                status = "Failure";
                                break;
                            default:
                                message = "Error";
                                status = "Failure";
                                break;
                        }
                    }
                    else
                    {
                        //Get the message ID
                        messageId = responseBody.Substring(3).Trim();
                        message = "Success";
                        status = "Success";
                    }
                }
            }
            catch (WebException webEx)
            {
                var webResp = webEx.Response as HttpWebResponse;

                if (webResp != null)
                {
                    if (webResp.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        //401 bad auth token
                        message = "InvalidAuthToken";
                        status = "Failure";

                    }
                    else if (webResp.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        //503 exponential backoff, get retry-after header
                        message = "ServiceUnavailable";
                        status = "Error";
                    }
                }
            }
            return messageId;
        }
    }

    public enum PushNotificationMessage
    {
        KioskEnabled,
        KioskDisabled
    }

    public enum PushNotificationMessageType
    {
        KioskEnabled,
        KioskDisabled
    }
}