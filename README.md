# <img src="https://raw.githubusercontent.com/hknklic/PayUCardInfo/master/README/card_info_payu.png">
> Durum Senaryomuz : C# ile yazdığımız bir projede, PayU ödeme sistemini kullanıyoruz. İhtiyacımız ise PayU'nun bizlere sağlamış olduğu v1 kredi kartı sorgulama api'sini kullanmak ve kartın sadece ilk 6 hanesi(BIN) ile; kart tipi, hangi bankaya ait olduğu, taksit vb bir çok bilgiyi çekmek.  
## Card Info - API v1
Card Info - API v1 ile bize olası dönebilecek bilgiler:

- "binType":"VISA"
- "binIssuer":"IS BANK", 
- "cardType":"CREDIT",
- "country":"Turkey",
- "program":"Maximum",
- "installments":[1,"2","3","4","5","6","7","8","9","10","11","12"],
- "paymentMethod":"CCVISAMC"
<p>binType: VISA, MasterCard vb olarak kartın tipini öğrenebilir, ilgili sayfanızda hazırlayacağınız temsili bir kart ile de logosunu gösterebilirsiniz. Ayrıca kartın ilk 6 hanenin girilmesini beklemeden de kartın ilk iki hanesi ve küçük bir liste ile aynı kontrolü sağlayabilirsiniz.</p>
<p>binIssuer / program / installments Bu alanlar ise çalışmamızda ihtiyacımız olan en önemli alanlar. Bunun en büyük sebebi PayU'nun taksit oranlarınızı içeren bir servis paylaşmıyor oluşu ve aynı tabloyu kendi sisteminizde tekrar hazırmanızı istemesi:</p>
<p align="center"><img src="https://raw.githubusercontent.com/hknklic/PayUCardInfo/master/README/payu_taksit.PNG"><p>            
<p>Ayrıca bir yanlış anlaşılmaya izin vermemek için değinmeliyim, eğer v2 kullanırsanız karta ait taksit komisyonlarını çekebiliyorsunuz ancak burada ki asıl sorun, bizden karta ait tüm bilgileri istemesi.</p>
<p>En nihayetinde ilgili ürüne ait sayfada, tüm kartların taksit oranlarını paylaşmak istiyorsak.. Kendi tablomuzu oluşturmak zorundayız.</p>

## Neden v2 değil de v1 kullanıyoruz ?

<p>Her ne kadar v2 ile girilen kartın taksit komisyonlarına ulaşabiliyor olsak da bunun için karta ait tüm bilgileri girmemiz istenmekte. Bizim tek amacımız çalışan bir sistem ise tercih edilebilir gözükmekte ancak doğru çalışan bir sistem ise, durup düşünmekte fayda var! Zira yapılan araştırmalarda sepeti terketme oralarını hiç %60 altında görmedim. Hal böyleyken kullanıcıya tüm kart bilgilerini girdikten sonra taksit bilgilerini sunmak, bir çok sabırsız müşteriyi kaybetmek olacaktır.<p>
<p>Bizlerin amacı ise v1 ile gerekli bilgileri elde etmek daha sonra bunu kendi taksit tablomuz ile karşılaştırarak ilgili oranları daha ilk altı hanede sunmuş olacağız.</p>

## Başlayalım!

Elimizde Http ile haberleşeceğimiz bir servis bulunmakta. İstekte bulunacağımız bu yapıyı inceleyelim:
<p>Dökümanlarında yer alan örnek yapı</p>

> https://secure.payu.com.tr/api/card-info/v1/ (İstekte bulunacağımız servisimiz.)
- 4444444444 (Kartımızın en az ilk altı hanesi.)
- ?timestamp=1421426073 (Unix zaman damgamız)
- &merchant=MERCHANTCODE (PayU tarafından size özel sağlanan kodunuz.)
- &signature=8ab027fdaf1eee0e1ecf2dd82cabc9a6668420011d05a1de329782b5b2566c57 (Yine PayU tarafından sizlere sağlanan gizli kodunuz ile oluşturabileceğiniz imzanız.)

#### Adım adım isteğimizi oluşturalım.

1. Kartın ilk 6 hanesini yakalayıp controller'a değerimizi gönderiyoruz :

```html

    <div style="width:600px; margin:60px auto; text-align:center;" >
        <h1>PayU Card Info v1</h1>
        <h5>- CARD NO -</h5>
        
        <input id="CardBIN" type="number"/>
        
        <div style="margin: 40px auto; border: 1px solid; border-radius: 36px; height: 245px; width: 400px; font-size: 1.3em;">
            <div id="binType" style="float: right; padding: 20px 28px;"></div>
            <div id="country" style="float: left; padding: 20px 28px;"></div>
            <div id="cardno" style="margin-top: 100px; font-size: 1.6em;"></div>
            <div id="binIssuer" style="float: left; padding: 48px 20px;"></div>
            <div id="program" style="float: right; padding: 48px 20px;"></div>
        </div>
      </div>

```
```javascript
$('#CardBIN').on('keyup change', function () {

                $("#cardno").html($(this).val().replace(/\W/gi, '').replace(/(.{4})/g, '$1 '))
                
                if ($(this).val().length === 6) {
                    var data = { _CardNum: $(this).val() }
                    $.ajax({
                        url: "fGetPayuCardBINV1",
                        type: "POST",
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        datatype: "json",
                        success: function (data)
                        {
                            $("#binType").html(data._BinData.root.cardBinInfo.binType);
                            $("#country").html(data._BinData.root.cardBinInfo.country);
                            $("#binIssuer").html(data._BinData.root.cardBinInfo.binIssuer);
                            $("#program").html(data._BinData.root.cardBinInfo.program);
                        }
                    });
                }
            });
```
Bu alanda açıklayacak pek bir şey yok diye düşünüyorum... Ajax ile değerimizi gönderiyor, dönen sonucumuzu ise ilgili alanlara basıyoruz.

2. timestamp

Bunun için isteğin gerçekleştiği zamanı gerekli formata çevirmemiz gerekmekte :

```c#
public long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(datetime - sTime).TotalSeconds;

        }
```

3. merchant

PayU tarafından sizlere özel sağlanan kodunuz.

4. signature

Bu alanda istek imzamızı oluşturuyoruz. Belirli verilerimiz ile oluşturduğumuz imzamızı, PayU'nun bize özel verdiği gizli kodu kullanarak SHA256 ile kriptoluyoruz. Gizli kodu merchant ile karıştırmayın.

```c#

   public byte[] hmacSHA256(String data, String key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(Encoding.ASCII.GetBytes(key)))
            {
                return hmac.ComputeHash(Encoding.ASCII.GetBytes(data));
            }
        }
      
```

5. Sonuç (Http Request)


```c#

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
      
```
