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

            WebClient _Client = new WebClient();

            string URL = "https://secure.payu.com.tr/api/card-info/v1/";
            string merchant = "OPU_TEST";
            string secretkey = "SECRET_KEY";
            string timestamp = ConvertToUnixTime(DateTime.Now.AddHours(-3)).ToString();
            string signature = BitConverter.ToString(hmacSHA256(merchant + timestamp, secretkey)).Replace("-", "").ToLower();

            var _Request = _Client.DownloadString(URL + _CardNum + "?merchant=" + merchant + "&timestamp=" + timestamp + "&signature=" + signature);

            return Json(new { _Request = _Request }, JsonRequestBehavior.AllowGet);

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

    }
}