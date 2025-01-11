<h1>SMTP Ayarlarıyla E-Posta Gönderme Uygulaması</h1>

<p>
Bu uygulama, SMTP ayarlarıyla e-posta göndermenizi veya doğru ayarları otomatik kombinasyonlarla bulmanızı sağlar. 
Manuel giriş ve test seçenekleri sunar, başarılı ayarları JSON formatında gösterir ve zaman aşımı yönetimiyle işlemleri hızlandırır.
</p>

<h2>Özellikler</h2>
<ul>
    <li>SMTP ayarlarıyla kolay e-posta gönderimi</li>
    <li>Otomatik kombinasyonlarla doğru ayarları bulma</li>
    <li>Manuel ayar girişi ve test etme seçeneği</li>
    <li>Başarılı ayarların <code>JSON</code> formatında gösterimi</li>
    <li>Zaman aşımı yönetimi ile işlemleri hızlandırma</li>
</ul>

<h2>Kullanım</h2>
<ol>
    <li>SMTP ayarlarını manuel olarak girin veya otomatik kombinasyonları deneyin.</li>
    <li>Test et butonunu kullanarak ayarlarınızı doğrulayın.</li>
    <li>Başarılı ayarlar otomatik olarak aşağıdaki formatta sunulacaktır.</li>
</ol>

<h2>Başarılı SMTP Ayarları JSON Formatı</h2>
<p>
Uygulama, başarılı SMTP ayarlarını şu şekilde JSON formatında sunar:
</p>
<pre>
<code>
{
    "SmtpServer": "smtp.example.com",
    "SmtpPort": 587,
    "EnableSsl": true,
    "UseDefaultCredentials": false,
    "SmtpUsername": "kullanici@example.com",
    "SenderName": "Gönderen Adı"
}
</code>
</pre>
