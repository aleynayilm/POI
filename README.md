# GeoFeature Manager

Bu proje, React ve OpenLayers kullanarak harita üzerinde geometrik nesneler oluşturmanızı, listelemenizi, güncellemenizi ve silmenizi sağlar. Backend ile REST API üzerinden iletişim kurar.

---

## Özellikler

- **Geometri Çizimi:** Nokta (Point), Çizgi (LineString), Çokgen (Polygon) çizim desteği  
- **Geometri İsmi:** Her yeni çizime isim verilebilir  
- **Listeleme:** Harita üzerindeki tüm geometriler tabloda listelenir  
- **Güncelleme & Silme:** Liste üzerinden isim güncelleme ve geometri silme işlemleri yapılabilir  
- **Client-side Pagination:** Tabloda sayfalama ile performans artırılır  
- **Dropdown Reset:** Yeni geometri eklendiğinde geometri tipi dropdown’u otomatik sıfırlanır  
- **Popup:** Harita üzerindeki geometri tıklandığında bilgi ve işlem butonları gösterilir  

---

## Teknolojiler

### Frontend

- React  
- OpenLayers  
- Axios (HTTP istekleri için)  
- WKT (Well-Known Text) formatı kullanımı  

### Backend

- ASP.NET Core Web API  
- Entity Framework Core  
- **UnitOfWork** ve **GenericRepository** patternleri ile katmanlı mimari  
- PostgreSQL veritabanı üzerinde **PostGIS** eklentisi  
- Coğrafi veri işlemleri için **TopologySuit** kütüphanesi  

---

## Veritabanı

- **PostgreSQL** kullanılmıştır.  
- Mekansal veri türleri ve sorguları için **PostGIS** uzantısı aktif.  
- Coğrafi verilerin yönetimi ve topoloji kontrolleri için **TopologySuit** kullanılır.  


