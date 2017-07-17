# <img src="https://raw.githubusercontent.com/hknklic/PayUCardInfo/master/README/card_info_payu.png">
> Durum Senaryomuz : C# ile yazdığımız bir projede, PayU ödeme sistemini kullanıyoruz. İhtiyacımız ise PayU'nun bizlere sağlamış olduğu v1 kredi kartı sorgulama api'sini kullanmak ve kartın sadece ilk 6 hanesi(BIN) ile; kart tipi, hangi bankaya ait olduğu, taksit vb bir çok bilgiyi çekmek.  
## Card Info - API v1
Card Info - API v1 ile bize olası dönebilecek bilgiler :

- "binType":"VISA" (ya da MasterCard vb olarak kartın tipini öğrenebilir, ilgili sayfanızda hazırlayacağınız temsili bir kart ile de logosunu gösterebilirsiniz.)
- "binIssuer":"Credit Europe Bank",
- "cardType":"CREDIT",
- "country":"Romania",
- "program":"AVANTAJ",
- "installments":[1,6,12],
- "paymentMethod":"CARD_AVANTAJ"
