# Restaurant Order Management System

![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=flat-square)
![EF Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4?style=flat-square)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?style=flat-square)
![JWT](https://img.shields.io/badge/Auth-JWT-000000?style=flat-square)
![Docker](https://img.shields.io/badge/MySQL-Docker-2496ED?style=flat-square)

An internal-facing REST API for restaurant operations — table and order management, payments, and role-based access (Admin, Waiter, Kitchen). Built as a portfolio project to demonstrate layered architecture, EF Core, and real business-rule design in ASP.NET Core.

**Languages:** [English](#english) · [Türkçe](#türkçe)

---

## English

### Overview

This is not a customer-facing ordering app — it's a staff-only system a restaurant's own team would use: waiters open orders and add items, the kitchen sees what to prepare, and admins manage tables, categories, and products. Reservations are out of scope by design (tables are booked by phone; staff mark status manually).

### Architecture

Three-project, layered architecture with a strict top-down dependency direction:

```
Restoran.Api  →  Restoran.Business  →  Restoran.Data
(Controllers,      (Services,             (Entities, DbContext,
 DTOs)              business rules)        Repositories)
```

- **Restoran.Data** — entities, `DbContext`, migrations, and a Generic Repository Pattern (`IGenericRepository<T>` / `GenericRepository<T>`) with entity-specific repositories (`IUserRepository`, `IOrderRepository`) for queries the generic layer can't express.
- **Restoran.Business** — `UserService` (registration/login, BCrypt password hashing) and `OrderService` (order lifecycle, business rules).
- **Restoran.Api** — Controllers and DTOs. Entities are never exposed directly over the wire; every endpoint has a dedicated request/response DTO.

CQRS/MediatR was evaluated and deliberately dropped — for this project's scope, it would have added ceremony without a matching payoff. The generic repository pattern was a better fit.

### Domain Model

| Entity | Purpose |
|---|---|
| `User` | Staff account — `Role` (Admin / Waiter / Kitchen), BCrypt password hash, soft-delete via `IsActive`. |
| `Table` | Physical table — `TableStatus` (Empty / Occupied / Reserved, set manually by staff), soft-delete via `IsActive`. |
| `Category` | Menu category (e.g. Soups, Mains, Desserts). |
| `Product` | Menu item — price, category (FK), `IsAvailable` toggle instead of hard delete. |
| `Order` | A table's open tab — `PaymentStatus` (Unpaid / Paid), `PaymentMethod`, timestamps. |
| `OrderItem` | A line item — quantity, note, and a **price snapshot** independent of the product's current price. |

### Key Design Decisions

- **Price snapshot** — `OrderItem.UnitPrice` is copied at order time, not referenced live from `Product.Price`. A later price change never rewrites historical orders.
- **No stored order total** — computed on demand from `OrderItems` (single source of truth), rather than cached and risking drift.
- **One status field, not two** — `Order.PaymentStatus` (Unpaid/Paid) doubles as the open/closed flag; no separate `OrderStatus`.
- **Table-conflict rule** — a table can have at most one open (`Unpaid`) order at a time; enforced in `OrderService`, not the database.
- **Soft delete + `Restrict` cascade** — `Table`/`User`/`Product` are never physically deleted (`IsActive`/`IsAvailable` flags), and the FK relationships that reference historical order data (`Order→Table`, `Order→Waiter`, `OrderItem→Product`) are `Restrict` — two independent layers protecting historical data from ever being silently lost.
- **Eager loading fix** — the generic repository's `FindAsync` doesn't load navigation properties; a dedicated `GetByIdWithDetailsAsync` (`Include`/`ThenInclude`) was added to `IOrderRepository` once this surfaced as a real bug (an order's items disappearing from a payment response).

### Tech Stack

- ASP.NET Core Web API (.NET 10)
- Entity Framework Core 9.0 + Pomelo.EntityFrameworkCore.MySql (pinned to 9.0 — Pomelo doesn't yet support EF Core 10)
- MySQL 8.0, containerized via Docker
- BCrypt.Net-Next for password hashing
- JWT Bearer authentication

### API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Auth/register` | Register a new staff account |
| POST | `/api/Auth/login` | Authenticate |
| POST | `/api/Order/createorder` | Open a new order for a table |
| POST | `/api/Order/addorderitem` | Add a line item to an order |
| POST | `/api/Order/pay` | Mark an order paid |
| GET | `/api/Order/kitchenview` | List all open orders with items (kitchen display) |
| POST | `/api/Category/create` | Create a category |
| GET | `/api/Category/getall` | List categories |
| PUT | `/api/Category/update` | Rename a category |
| POST | `/api/Table/createaction` | Create a table |
| GET | `/api/Table/gettable` | List tables |
| PUT | `/api/Table/updatestatus` | Change a table's status (Empty/Occupied/Reserved) |
| PUT | `/api/Table/updateactive` | Activate/deactivate a table |
| POST | `/api/Product/createproduct` | Create a product |
| GET | `/api/Product/getall` | List products |
| PUT | `/api/Product/updateproduct` | Update a product's price/availability |

### Getting Started

**Prerequisites:** .NET 10 SDK, Docker

```bash
# 1. Start MySQL
docker run --name restoran-mysql -e MYSQL_ROOT_PASSWORD=<your-password> -p 3307:3306 -d mysql:8.0

# 2. Configure the connection string in Restoran.Api/appsettings.json

# 3. Apply migrations
dotnet ef database update --project Restoran.Data --startup-project Restoran.Api

# 4. Run
cd Restoran.Api
dotnet run
```

### Project Status

Data, Repository, and Business layers are complete. All controllers (Auth, Order, Category, Table, Product) are built; most endpoints are verified end-to-end against a live database. JWT authentication is in progress — configuration and token generation are wired up, with token issuance on login and `[Authorize]`-based role protection as the remaining step. No frontend yet.

---

## Türkçe

### Genel Bakış

Bu, müşteri karşısında çalışan bir sipariş uygulaması değil — bir restoranın kendi personelinin kullandığı, iç operasyona yönelik bir sistem. Garsonlar sipariş açar ve kalem ekler, mutfak neyin hazırlanacağını görür, admin masa/kategori/ürün yönetimini yapar. Rezervasyon bilinçli olarak kapsam dışı (masalar telefonla ayrılıyor, personel durumu elle işaretliyor).

### Mimari

Üç projeli, katmanlı mimari — bağımlılık her zaman yukarıdan aşağı akar:

```
Restoran.Api  →  Restoran.Business  →  Restoran.Data
(Controller'lar,   (Servisler,            (Entity'ler, DbContext,
 DTO'lar)           iş kuralları)          Repository'ler)
```

- **Restoran.Data** — entity'ler, `DbContext`, migration'lar ve Generic Repository Pattern (`IGenericRepository<T>` / `GenericRepository<T>`), generic katmanın karşılayamadığı sorgular için entity'ye özel repository'lerle (`IUserRepository`, `IOrderRepository`) birlikte.
- **Restoran.Business** — `UserService` (kayıt/giriş, BCrypt ile şifre hash'leme) ve `OrderService` (sipariş yaşam döngüsü, iş kuralları).
- **Restoran.Api** — Controller'lar ve DTO'lar. Entity'ler hiçbir zaman doğrudan dışarı açılmaz; her endpoint'in kendine ait bir request/response DTO'su vardır.

CQRS/MediatR değerlendirildi, bilinçli olarak vazgeçildi — bu projenin ölçeği için karşılığını vermeyecek bir yük getirirdi. Generic repository pattern daha uygun bir tercih oldu.

### Domain Model

| Entity | Amacı |
|---|---|
| `User` | Personel hesabı — `Role` (Admin / Waiter / Kitchen), BCrypt şifre hash'i, `IsActive` ile soft delete. |
| `Table` | Fiziksel masa — `TableStatus` (Empty / Occupied / Reserved, personel tarafından elle ayarlanır), `IsActive` ile soft delete. |
| `Category` | Menü kategorisi (ör. Çorbalar, Ana Yemekler, Tatlılar). |
| `Product` | Menü öğesi — fiyat, kategori (FK), gerçek silme yerine `IsAvailable` bayrağı. |
| `Order` | Bir masanın açık hesabı — `PaymentStatus` (Unpaid / Paid), `PaymentMethod`, zaman damgaları. |
| `OrderItem` | Bir sipariş kalemi — adet, not, ve ürünün güncel fiyatından bağımsız bir **fiyat anlık görüntüsü (snapshot)**. |

### Kritik Tasarım Kararları

- **Fiyat anlık görüntüsü** — `OrderItem.UnitPrice`, sipariş anında kopyalanır, `Product.Price`'a canlı referans değildir. Sonradan yapılan bir fiyat değişikliği geçmiş siparişleri asla etkilemez.
- **Toplam tutar saklanmıyor** — ihtiyaç anında `OrderItems` üzerinden hesaplanır (tek doğru kaynak prensibi), önbelleklenip senkron kalma riskine girmez.
- **İki değil, tek durum alanı** — `Order.PaymentStatus` (Unpaid/Paid) hem ödeme durumunu hem açık/kapalı bilgisini taşır; ayrı bir `OrderStatus` yok.
- **Masa çakışma kuralı** — bir masada aynı anda en fazla bir açık (`Unpaid`) sipariş olabilir; bu kural veritabanında değil `OrderService`'te uygulanır.
- **Soft delete + `Restrict` cascade** — `Table`/`User`/`Product` hiçbir zaman fiziksel olarak silinmez (`IsActive`/`IsAvailable` bayrakları), ve geçmiş sipariş verisine referans veren FK ilişkileri (`Order→Table`, `Order→Waiter`, `OrderItem→Product`) `Restrict` — geçmiş verinin sessizce kaybolmasını önleyen iki bağımsız katman.
- **Eager loading düzeltmesi** — generic repository'nin `FindAsync`'i navigation property'leri yüklemiyor; bu gerçek bir hata olarak ortaya çıkınca (bir siparişin kalemlerinin ödeme cevabında kaybolması), `IOrderRepository`'ye `Include`/`ThenInclude` kullanan özel bir `GetByIdWithDetailsAsync` eklendi.

### Teknoloji Yığını

- ASP.NET Core Web API (.NET 10)
- Entity Framework Core 9.0 + Pomelo.EntityFrameworkCore.MySql (9.0'a sabitlendi — Pomelo henüz EF Core 10'u desteklemiyor)
- MySQL 8.0, Docker container içinde
- Şifre hash'leme için BCrypt.Net-Next
- JWT Bearer kimlik doğrulama

### API Endpoint'leri

| Metod | Endpoint | Açıklama |
|---|---|---|
| POST | `/api/Auth/register` | Yeni personel hesabı oluştur |
| POST | `/api/Auth/login` | Giriş yap |
| POST | `/api/Order/createorder` | Bir masa için yeni sipariş aç |
| POST | `/api/Order/addorderitem` | Siparişe kalem ekle |
| POST | `/api/Order/pay` | Siparişi ödenmiş olarak işaretle |
| GET | `/api/Order/kitchenview` | Tüm açık siparişleri kalemleriyle listele (mutfak görünümü) |
| POST | `/api/Category/create` | Kategori oluştur |
| GET | `/api/Category/getall` | Kategorileri listele |
| PUT | `/api/Category/update` | Kategori adını güncelle |
| POST | `/api/Table/createaction` | Masa oluştur |
| GET | `/api/Table/gettable` | Masaları listele |
| PUT | `/api/Table/updatestatus` | Masa durumunu değiştir (Empty/Occupied/Reserved) |
| PUT | `/api/Table/updateactive` | Masayı aktif/pasif yap |
| POST | `/api/Product/createproduct` | Ürün oluştur |
| GET | `/api/Product/getall` | Ürünleri listele |
| PUT | `/api/Product/updateproduct` | Ürün fiyatını/aktifliğini güncelle |

### Başlarken

**Gereksinimler:** .NET 10 SDK, Docker

```bash
# 1. MySQL'i başlat
docker run --name restoran-mysql -e MYSQL_ROOT_PASSWORD=<sifren> -p 3307:3306 -d mysql:8.0

# 2. Restoran.Api/appsettings.json içindeki connection string'i ayarla

# 3. Migration'ları uygula
dotnet ef database update --project Restoran.Data --startup-project Restoran.Api

# 4. Çalıştır
cd Restoran.Api
dotnet run
```

### Proje Durumu

Data, Repository ve Business katmanları tamamlandı. Tüm Controller'lar (Auth, Order, Category, Table, Product) kuruldu; endpoint'lerin çoğu gerçek bir veritabanına karşı uçtan uca doğrulandı. JWT kimlik doğrulaması sürüyor — konfigürasyon ve token üretimi bağlandı, login'de token verme ve `[Authorize]` bazlı rol koruması kalan adım. Henüz bir frontend yok.
