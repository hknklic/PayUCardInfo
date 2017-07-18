# <img src="https://raw.githubusercontent.com/hknklic/PayUCardInfo/master/README/card_info_payu.png">
> Durum Senaryomuz : C# ile yazdığımız bir projede, PayU ödeme sistemini kullanıyoruz. İhtiyacımız ise PayU'nun bizlere sağlamış olduğu v1 kredi kartı sorgulama api'sini kullanmak ve kartın sadece ilk 6 hanesi(BIN) ile; kart tipi, hangi bankaya ait olduğu, taksit vb bir çok bilgiyi çekmek.  
## Card Info - API v1
Card Info - API v1 ile bize olası dönebilecek bilgiler :

- "binType":"VISA"
- "binIssuer":"IS BANK", 
- "cardType":"CREDIT",
- "country":"Turkey",
- "program":"Maximum",
- "installments":[1,"2","3","4","5","6","7","8","9","10","11","12"],
- "paymentMethod":"CCVISAMC"
<p>binType: VISA, MasterCard vb olarak kartın tipini öğrenebilir, ilgili sayfanızda hazırlayacağınız temsili bir kart ile de logosunu gösterebilirsiniz. Ayrıca kartın ilk 6 hanenin girilmesini beklemeden de kartın ilk iki hanesi ve küçük bir liste ile aynı kontrolü sağlayabilirsiniz.</p>
<p>binIssuer / program / installments Bu alanlar ise çalışmamızda ihtiyacımız olan en önemli alanlar. Bunun en büyük sebebi PayU'nun taksit oranlarınızı içeren bir servis paylaşmıyor oluşu ve aynı tabloyu kendi sisteminizde tekrar hazırmanızı istemesi :</p>
<p align="center"><img src="https://raw.githubusercontent.com/hknklic/PayUCardInfo/master/README/card_info_payu.png"><p>            
