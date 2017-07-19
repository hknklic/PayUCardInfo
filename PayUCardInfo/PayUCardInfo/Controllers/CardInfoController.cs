using Newtonsoft.Json;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace PayUCardInfo.Controllers
{
    public class CardInfoController : Controller
    {
        // GET: CardInfo
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult fGetPayuCardBINV1(string _CardNum)
        {
            /// Http Request için kullanacağımız metodumuz.
            WebClient _Client = new WebClient();
            /// Dönen sonucumuzu basacağımız class yapımız.
            BINDataResponseV1 _BinData = new BINDataResponseV1();

            /// İstekte bulunacağımız servisimiz
            string URL = "https://secure.payu.com.tr/api/card-info/v1/";
            /// PayU tarafından size özel sağlanan kodunuz.
            string merchant = "";
            /// PayU tarafından size özel sağlanan gizli kodunuz.
            string secretkey = "";
            /// Unix zaman damgamız
            string timestamp = ConvertToUnixTime(DateTime.Now.AddHours(-3)).ToString();
            /// PayU tarafından sizlere sağlanan gizli kodunuz ile oluşturacağınız imzanız.
            string signature = BitConverter.ToString(hmacSHA256(merchant + timestamp, secretkey)).Replace("-", "").ToLower();

            /// Http Request işleminiz.
            var _Request = _Client.DownloadString(URL + _CardNum + "?merchant=" + merchant + "&timestamp=" + timestamp + "&signature=" + signature);
            /// Sonuç
            _BinData.root = JsonConvert.DeserializeObject<BINDataResponseV1.ROOT>(_Request);

            return Json(new { _BinData = _BinData }, JsonRequestBehavior.AllowGet);

        }

        public byte[] hmacSHA256(String data, String key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(key)))
            {
                return hmac.ComputeHash(Encoding.ASCII.GetBytes(data));
            }
        }

        public long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(datetime - sTime).TotalSeconds;

        }


        public class BINDataResponseV1
        {
            public ROOT root { get; set; }

            public BINDataResponseV1()
            {
                root = new ROOT();
            }

            public class ROOT
            {
                public META meta { get; set; }
                public CARDBININFO cardBinInfo { get; set; }

                public ROOT()
                {
                    meta = new META();
                    cardBinInfo = new CARDBININFO();
                }

                public class META
                {
                    public STATUS status { get; set; }
                    public RESPONSE response { get; set; }

                    public META()
                    {
                        status = new STATUS();
                        response = new RESPONSE();
                    }

                    public class STATUS
                    {
                        public string code { get; set; }
                        public string message { get; set; }
                    }
                    public class RESPONSE
                    {
                        public string httpCode { get; set; }
                        public string httpMessage { get; set; }
                    }
                }

                public class CARDBININFO
                {
                    public string binType { get; set; }
                    public string binIssuer { get; set; }
                    public string cardType { get; set; }
                    public string country { get; set; }
                    public string program { get; set; }
                    public string[] installments { get; set; }
                    public string paymentMethod { get; set; }
                }
            }
        }

    }
}