using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Kooboo.Commerce.Payments.PayPal
{
    /// <summary>
    /// Sample Payment
    /// 
    /// The payment gateway for _daptivePayments
    /// https://cms.paypal.com/cms_content/US/en_US/files/developer/PP_AdaptivePayments.pdf
    /// </summary>
    public class AdaptivePayment
    {
        Authentication Authentication
        {
            get;
            set;
        }

        public AdaptivePayment(Authentication authentication)
        {
            this.Authentication = authentication;
            //read api setting from config
            Authentication.X_PAYPAL_SECURITY_USERID = ConfigurationManager.AppSettings["APIUsername"];
            Authentication.X_PAYPAL_SECURITY_PASSWORD = ConfigurationManager.AppSettings["APIPassword"];
            Authentication.X_PAYPAL_SECURITY_SIGNATURE = ConfigurationManager.AppSettings["Signature"];
        }

        string strSandBox_EndpointURL = "https://svcs.sandbox.paypal.com/AdaptivePayments/Pay";

        string strGoLive_EndpointURL = "https://svcs.paypal.com/AdaptivePayments/Pay";

        string strSandBox_RedirectURL = "https://www.sandbox.paypal.com/webscr?cmd=_ap-payment&paykey=";

        string strGoLive_RedirectURL = "https://www.paypal.com/webscr?cmd=_ap-payment&paykey=";

        /// <summary>
        /// 
        /// </summary>
        Boolean GoLive
        {
            get;
            set;
        }

        /// <summary>
        /// get endpoint url
        /// </summary>
        String EndpointURL
        {
            get
            {
                if (!GoLive)
                {
                    return strSandBox_EndpointURL;
                }
                else
                {
                    return strGoLive_EndpointURL;
                }
            }
        }

        /// <summary>
        /// get approve payment url
        /// </summary>
        String RedirectURL
        {
            get
            {
                if (!GoLive)
                {
                    return strSandBox_RedirectURL;
                }
                else
                {
                    return strGoLive_RedirectURL;
                }
            }
        }

        public void Pay(Payload payload, ClientIndentification client, Boolean goLive, string merchantAccount,
            decimal amount, String currencyCode, Int32 orderID)
        {
            if (String.IsNullOrEmpty(merchantAccount))
            {
                throw new Exception("Please specify receiver email for paypal.");
            }

            GoLive = goLive;

            //The pay key is valid for 3 hours; the payment must be approved while the pay key is valid.
            //check order payment status, is there already has a payment record waiting buyer approve??
            var orderToken = Services.Current.DataConext.
                    Payment_Paypal_OrderTokens.Where(o => o.OrderID == orderID).OrderByDescending(o => o.Id).FirstOrDefault();
            if (orderToken != null && (DateTime.Now - orderToken.CreateTime).TotalHours <= 3) //waiting for approve
            {
                HttpContext.Current.Response.Redirect(RedirectURL + orderToken.PayKey);
                return;
            }

            Guid orderTrackToken = Guid.NewGuid();

            var objectWebRequest = (HttpWebRequest)WebRequest.Create(EndpointURL);

            objectWebRequest.Method = "POST";
            objectWebRequest.ContentType = "application/x-www-form-urlencoded";

            objectWebRequest.Headers.Set("X-PAYPAL-SECURITY-USERID", this.Authentication.X_PAYPAL_SECURITY_USERID);
            objectWebRequest.Headers.Set("X-PAYPAL-SECURITY-PASSWORD", this.Authentication.X_PAYPAL_SECURITY_PASSWORD);
            objectWebRequest.Headers.Set("X-PAYPAL-SECURITY-SIGNATURE", this.Authentication.X_PAYPAL_SECURITY_SIGNATURE);

            objectWebRequest.Headers.Set("X-PAYPAL-REQUEST-DATA-FORMAT", payload.X_PAYPAL_REQUEST_DATA_FORMAT.ToString());
            objectWebRequest.Headers.Set("X-PAYPAL-RESPONSE-DATA-FORMAT", payload.X_PAYPAL_RESPONSE_DATA_FORMAT.ToString());
            objectWebRequest.Headers.Set("X-PAYPAL-APPLICATION-ID", client.X_PAYPAL_APPLICATION_ID);

            StringBuilder reqBuilder = new StringBuilder();
            reqBuilder.AppendFormat("actionType={0}", HttpUtility.UrlEncode("PAY"));
            reqBuilder.AppendFormat("&feesPlayer={0}", HttpUtility.UrlEncode("EACHRECEIVER"));

            String cancelUrl = retrieveUrl("PayPal_Cancel", new RouteValueDictionary(new { token = orderTrackToken.ToString("N") }));
            reqBuilder.AppendFormat("&cancelUrl={0}", HttpUtility.UrlEncode(cancelUrl));
            reqBuilder.AppendFormat("&clientDetails.ipAddress={0}", HttpUtility.UrlEncode(HttpContext.Current.Request.UserHostAddress));
            reqBuilder.AppendFormat("&clientDetails.partnerName={0}", HttpUtility.UrlEncode("Koobo Commerce"));

            String ipnUrl = retrieveUrl("PayPal_IPN", new RouteValueDictionary(new { token = orderTrackToken.ToString("N") }));
            reqBuilder.AppendFormat("&ipnNotificationUrl=" + HttpUtility.UrlEncode(ipnUrl));

            reqBuilder.AppendFormat("&trackingId={0}", orderTrackToken.ToString("N"));
            reqBuilder.AppendFormat("&currencyCode={0}", HttpUtility.UrlEncode(currencyCode));
            reqBuilder.AppendFormat("&receiverList.receiver(0).email={0}", HttpUtility.UrlEncode(merchantAccount));
            reqBuilder.AppendFormat("&receiverList.receiver(0).amount={0}", HttpUtility.UrlEncode(amount.ToString()));
            reqBuilder.Append("&requestEnvelope.errorLanguage=en_US");//requestEnvelope.errorLanguage must be en_US
            reqBuilder.Append("&requestEnvelope.detailLevel=ReturnAll"); //ReturnAll – This value provides the maximum level of detail (default)

            String returnUrl = retrieveUrl("PayPal_Return", new RouteValueDictionary(new { token = orderTrackToken.ToString("N") }));
            reqBuilder.AppendFormat("&returnUrl={0}", HttpUtility.UrlEncode(returnUrl));

            //Create a byte array of the data we want to send 
            var byteData = UTF8Encoding.UTF8.GetBytes(reqBuilder.ToString());

            //Set the content length in the request headers  
            objectWebRequest.ContentLength = byteData.Length;

            //Write data 
            Stream postStream = null;

            postStream = objectWebRequest.GetRequestStream();

            postStream.Write(byteData, 0, byteData.Length);

            //Sent request / get response from server
            HttpWebResponse response = (HttpWebResponse)objectWebRequest.GetResponse();

            //Get response stream into a reader
            StreamReader reader = new StreamReader(response.GetResponseStream());

            //Read received response into a string array            
            var strResponse = reader.ReadToEnd();

            //Split
            var strSplited = strResponse.Split('&');

            //Output
            string strOutput = "";

            foreach (var s in strSplited)
            {
                if (string.IsNullOrWhiteSpace(s) == false && s.Length > 0)
                {
                    strOutput = strOutput + s + "<br />";
                }
            }

            //Output to HTML
            // ResultDump.Text = "<br /><b>Printing received response:</b><br />" & strOutput

            //----------------------------------------------------------------------
            //'Do check if sucess and assign redirect string value
            //'----------------------------------------------------------------------
            String strPayKey, strPaymentExecStatus;
            String strTmp = strSplited[1];
            strTmp = strTmp.Substring(21, 7);

            if (strTmp == "Success")
            {
                //Get PayKey
                strPayKey = strSplited[4];
                strPayKey = strPayKey.Substring(7, 20);

                //Get PaymentExecStatus
                strPaymentExecStatus = strSplited[5].Substring(18);

                //Save Payment statue
                var newOrderToken = new Payment_Paypal_OrderToken()
                {
                    OrderID = orderID,
                    OrderTraceToken = orderTrackToken,
                    CreateTime = DateTime.Now,
                    PayKey = strPayKey,
                    ExecStatus = (byte)(PaymentStatus)Enum.Parse(typeof(PaymentStatus), strPaymentExecStatus, true)
                };
                Services.Current.DataConext.Payment_Paypal_OrderTokens.InsertOnSubmit(newOrderToken);
                Services.Current.DataConext.SubmitChanges();

                //update order status
                //var host = new ServiceHost();
                //IOrder order = host
                //      .QueryOrders()
                //      .Where(i => i.Id == orderID)
                //      .FirstOrDefault();
                //if (order != null)
                //{
                //order.OrderStatus = OrderStatus.PaymentPending;
                //host.Put(order);
                // }

                HttpContext.Current.Response.Redirect(RedirectURL + strPayKey);

                //Print redirect URL to HTML page
                //RedirectUrl.Text = "<br /><b>Please redirect user to following URL:</b><br />" & strRedirectURL
            }
            else //error handler
            {
                var errors = ParseError(strOutput.Replace("<br />", "&"));
                StringBuilder msgBuilder = new StringBuilder();
                foreach (var item in errors)
                {
                    msgBuilder.Append(item.Message);
                    msgBuilder.Append(HttpUtility.UrlEncode("\r"));
                }
                throw new Exception(msgBuilder.ToString());
            }
        }

        private String retrieveUrl(String routeName, RouteValueDictionary values = null)
        {
            return UrlHelper.GenerateUrl(routeName, null, null,
            "http", HttpContext.Current.Request.Url.Host, "", values, RouteTable.Routes,
            ((MvcHandler)HttpContext.Current.Handler).RequestContext, false);
        }

        private IEnumerable<Error> ParseError(String rspContent)
        {
            List<Error> errorList = new List<Error>();
            //find error index
            var errorIndexRegex = new Regex(@"error\((?<index>\d)\)\.errorId");
            var matches = errorIndexRegex.Matches(rspContent);
            if (matches.Count > 0)
            {
                foreach (Match item in matches)
                {
                    var error = Error.Parse(ushort.Parse(item.Groups["index"].Value), rspContent);
                    errorList.Add(error);
                }
            }
            return errorList;
        }
    }

    /// <summary>
    /// HTTP Header for Authentication
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// Your API username
        /// </summary>
        public string X_PAYPAL_SECURITY_USERID
        {
            get;
            set;
        }

        /// <summary>
        /// Your API password
        /// </summary>
        public string X_PAYPAL_SECURITY_PASSWORD
        {
            get;
            set;
        }

        /// <summary>
        /// Your API singature,which is required only if you use 3-token authorization;a certificate does not use a signature
        /// </summary>
        public string X_PAYPAL_SECURITY_SIGNATURE
        {
            get;
            set;
        }

        /// <summary>
        /// Thrid-party permission specification, which specifies the email address or phone number(for mobile) of the party on whose behanlf you are calling the API operation. The subject must grant you third-party access in their PayPal profile.
        /// </summary>
        public string X_PAYPAL_SECURITY_SUBJECT
        {
            get;
            set;
        }

    }

    public class Payload
    {
        /// <summary>
        /// The payload format for the request
        /// </summary>
        public PayloadDataFormat X_PAYPAL_REQUEST_DATA_FORMAT
        {
            get;
            set;
        }

        /// <summary>
        /// The payload format for the response
        /// </summary>
        public PayloadDataFormat X_PAYPAL_RESPONSE_DATA_FORMAT
        {
            get;
            set;
        }
    }

    public enum PayloadDataFormat
    {
        /// <summary>
        /// Name-value paris
        /// </summary>
        NV,
        /// <summary>
        /// Exensible markup language
        /// </summary>
        XML,
        /// <summary>
        /// JavaScript object notation
        /// </summary>
        JSON
    }

    /// <summary>
    /// HTTP Headers for Application and Device identification
    /// </summary>
    public class ClientIndentification
    {
        public ClientIndentification()
        {
            this.X_PAYPAL_APPLICATION_ID = "APP-80W284485P519543T";
        }

        /// <summary>
        /// (Required) Your application's identification, which is issued by PayPal.
        /// "APP-80W284485P519543T" is the default App ID for Sandbox
        /// </summary>
        public string X_PAYPAL_APPLICATION_ID
        {
            get;
            set;
        }

        /// <summary>
        /// (Optional) Client's device ID, such as a mobile device's IMEI number or a web browser cokie.
        /// </summary>
        public string X_PAYPAL_DEVICE_ID
        {
            get;
            set;
        }

        /// <summary>
        /// (Requried) Client's IP address.
        /// </summary>
        public string X_PAYPAL_DEVICE_IPADDRESS
        {
            get;
            set;
        }
    }

    public enum FeesPayer
    {
        /// <summary>
        /// Sender pays all fees (for personal, implicit simple/parallel payments; do not use for chained or unilateral payments)
        /// </summary>
        SENDER,
        /// <summary>
        /// Primary receiver pays all fees (chained payments only)
        /// </summary>
        PRIMARYRECEIVER,
        /// <summary>
        /// Each receiver pays their own fee (default, personal and unilateral payments)
        /// </summary>
        EACHRECEIVER,
        /// <summary>
        /// Secondary receivers pay all fees (use only for chained payments with one secondary receiver)
        /// </summary>
        SECONDARYONLY
    }

    public class Error
    {
        private const String PrefixTemplate = @"error\({0}\)\.(?<Key>[a-zA-Z]+(\((?<pIndex>(\d))\))?)\=(?<value>[^&]+)";

        public UInt16 Index { get; set; }

        public String ID { get; set; }

        public String Domain { get; set; }

        public String Severity { get; set; }

        public String Category { get; set; }

        public String Message { get; set; }

        public String SubDomain { get; set; }

        public List<ErrorParameter> Parameters { get; set; }

        public static Error Parse(UInt16 index, String rspContent)
        {
            Error error = new Error() { Index = index, Parameters = new List<ErrorParameter>() };

            String regExTemplate = String.Format(PrefixTemplate, index);
            var regEx = new Regex(regExTemplate);
            var matches = regEx.Matches(rspContent);
            if (matches.Count > 0)
            {
                foreach (Match item in matches)
                {
                    if (item.Success)
                    {
                        String key = item.Groups["Key"].Value;
                        String value = item.Groups["value"].Value;

                        switch (key)
                        {
                            case "errorId":
                                error.ID = value;
                                break;
                            case "domain":
                                error.Domain = value;
                                break;
                            case "subdomain":
                                error.SubDomain = value;
                                break;
                            case "severity":
                                error.Severity = value;
                                break;
                            case "category":
                                error.Category = value;
                                break;
                            case "message":
                                error.Message = value;
                                break;
                            default:
                                if (key.StartsWith("parameter") && !String.IsNullOrEmpty(item.Groups["pIndex"].Value))
                                {
                                    error.Parameters.Add(new ErrorParameter() { Index = ushort.Parse(item.Groups["pIndex"].Value), Value = value });
                                }
                                break;
                        }
                    }
                }
            }
            return error;
        }
    }

    public class ErrorParameter
    {
        public UInt16 Index { get; set; }

        public String Value { get; set; }
    }

    public class AdaptivePaymentRequest
    {
        public decimal Amount { get; set; }

        public String CurrencyCode { get; set; }

        public String MerchantAccount { get; set; }
    }

    public class AdaptivePaymentResponse
    {
        public AdaptivePaymentResponse(String response)
        {
            if (!String.IsNullOrEmpty(response))
            {
                ResponseEnvelop = new PayPal.ResponseEnvelop();

                String[] keyValues = response.Split('&');
                foreach (var item in keyValues)
                {
                    if (item.StartsWith("responseEnvelope.timestamp"))
                    {
                        continue;
                    }

                    if (item.StartsWith("responseEnvelope.ack"))
                    {

                        continue;
                    }
                }
            }
        }

        public ResponseEnvelop ResponseEnvelop { get; set; }

        public String PayKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String PaymentExecStatus { get; set; }
    }

    public class ResponseEnvelop
    {
        public DateTime TimeStamp { get; private set; }

        /// <summary>
        /// Acknowledgement code. 
        ///  Success – Operation completed successfully
        ///  Failure – Operation failed
        ///  Warning – Warning
        ///  SuccessWithWarning – Operation completed successfully; however, there is a warning message
        ///  FailureWithWarning – Operation failed with a warning message
        /// </summary>
        public String Ack { get; private set; }

        public String CorrelationId { get; set; }

        public String Build { get; set; }
    }

    public enum PaymentStatus
    {
        /// <summary>
        /// The payment request was received; funds will be transferred once the payment is approved
        /// </summary>
        CREATED = 1,
        /// <summary>
        /// The payment was successful
        /// </summary>
        COMPLETED = 2,
        /// <summary>
        /// Some transfers succeeded and some failed for a parallel payment or, for a delayed chained payment, secondary receivers have not been paid
        /// </summary>
        INCOMPLETE = 4,
        /// <summary>
        /// The payment failed and all attempted transfers failed or all completed transfers were successfully reversed
        /// </summary>
        ERROR = 8,
        /// <summary>
        /// One or more transfers failed when attempting to reverse a payment
        /// </summary>
        REVERSALERROR = 16,
        /// <summary>
        /// The payment is in progress
        /// </summary>
        PROCESSING = 32,
        /// <summary>
        /// The payment is awaiting processing
        /// </summary>
        PENDING = 64
    }
}