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
- &merchant=MERCHANTCODE (PayU tarafından sizlere özel sağlanan kodunuz.)
- &signature=8ab027fdaf1eee0e1ecf2dd82cabc9a6668420011d05a1de329782b5b2566c57 (Yine PayU tarafından sizlere sağlanan gizli kodunuz ile oluşturabileceğiniz imzanız.)
