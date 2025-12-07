# 🧑‍💼 KariyerPortalı – Job Board & Recruitment System

KariyerPortalı, **iş arayanlar (Aday)** ve **işverenlerin** buluştuğu, modern bir iş ilanı ve başvuru takip platformudur.  
Uygulama tamamen **full-stack** bir yapıda geliştirilmiş olup kimlik doğrulama, rol tabanlı yetkilendirme, ilan yönetimi, başvuru sistemi, profil düzenleme gibi **gerçek dünya fonksiyonları** içerir.

Bu proje, hem **.NET MVC teknolojilerini** hem de **modern web geliştirme pratiklerini** temel alan profesyonel bir iş uygulamasıdır.

---

## 🚀 Özellikler

### 🔐 **1. Kimlik Doğrulama (Authentication)**
- Kullanıcı kayıt & giriş sistemi  
- Şifre hashleme  
- Email doğrulama (opsiyonel)  
- “Şifremi Unuttum” & resetleme  
- Identity kullanılarak güvenli kimlik yönetimi  

---

### 📝 **2. Profil Yönetimi**
- Kullanıcı adı, email ve profil bilgilerini düzenleme  
- Profil fotoğrafı yükleme (dosya upload / Cloud hizmeti)  
- Rol bazlı profil görünümü (Aday / İşveren)

---

### 🏢 **3. Rol Tabanlı Yetkilendirme (Authorization)**
Sistem 2 ana rolden oluşur:

| Rol | Açıklama |
|-----|----------|
| **Aday (Job Seeker)** | İlanları görüntüler, filtreler ve başvuru yapabilir |
| **İşveren (Employer)** | İlan oluşturur, düzenler, siler, başvuruları yönetir |
| **Admin (isteğe bağlı)** | Kullanıcı ve ilan yönetimi |

Her sayfa rol kontrolü ile korunur.

---

### 📄 **4. İş İlanı Yönetimi**
- İşveren yeni ilan oluşturabilir  
- İlan düzenleme & silme  
- Adaylar ilanları listeler & detaylarını görüntüler  
- İlanlarda:
  - Pozisyon  
  - Açıklama  
  - Lokasyon  
  - Maaş aralığı  
  - Yayın tarihi  

---

### 🔎 **5. Arama ve Filtreleme**
Adaylar ilanları şu kriterlere göre filtreleyebilir:

- Pozisyon  
- Lokasyon  
- Yayın tarihi  
- Maaş aralığı  

Filtreleme **Ajax / jQuery** ile anlık yapılır.

---

### 📬 **6. Başvuru Sistemi**
- Aday bir ilana başvuru yapabilir  
- Başvuruya CV yükleyebilir (PDF/DOC)  
- İşveren gelen başvuruları görebilir & değerlendirebilir  
- Başvurular veritabanında saklanır

---

### 📊 **7. Dashboard (İşveren)**
İşveren giriş yaptığında özel bir dashboard görür:

- Yayında olan ilan sayısı  
- Gelen başvuru sayıları  
- Basit grafikler (Chart.js kullanılabilir)

---

### ✉️ **8. E-posta Bildirimleri (Opsiyonel)**
- Aday başvuru yaptığında  
- İşverene yeni başvuru geldiğinde  

MailKit / SendGrid ile gönderim yapılabilir.

---

## 🛠️ Kullanılan Teknolojiler

### Backend
- **ASP.NET MVC 5 / .NET Core**
- **Entity Framework**
- **Identity**
- LINQ
- SQL Server

### Frontend
- HTML5 / CSS3  
- **Bootstrap 5** (Responsive tasarım)
- jQuery / AJAX
- Razor View Engine

### Diğer
- File Upload (CV / Profil resmi)
- Logging (Serilog / NLog – opsiyonel)
- Error Handling (Custom error pages)

---

## 🗂️ Proje Mimarisi
KariyerPortali/
│── Controllers/
│── Models/
│── Views/
│── ViewModels/
│── wwwroot/
│── Services/ (opsiyonel)
│── Data/
│── wwwroot/uploads/
│── KariyerPortali.sln
