# HeThongPheDuyetTaiLieu

# Document Approval System

Hệ thống phê duyệt tài liệu phân tán.
# Tuần 1 - Khởi tạo hệ thống

## Công nghệ sử dụng

- C# / .NET 10
- ASP.NET Core Minimal API
- SQL Server
- Entity Framework Core
- RabbitMQ
- Docker / Docker Compose

## Kiến trúc

- DocumentService.Api: xử lý tài liệu và workflow
- DocumentService.Core: models và logic lõi
- DocumentService.Infrastructure: database và hạ tầng
- NotificationService.Worker: xử lý thông báo

## Database

- Documents
- ApprovalHistories
- Notifications

## Khởi chạy hạ tầng

```bash
docker compose up -d
```

## Kiểm tra

```bash
docker compose ps
```

# Tuần 2 - Tạo tài liệu API CRUD

## Chức năng

- Tạo tài liệu
- Xem danh sách tài liệu
- Xem chi tiết tài liệu
- Cập nhật tài liệu Draft
- Xóa tài liệu Draft

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | /documents | Create document |
| GET | /documents | Get all documents |
| GET | /documents/{id} | Get document by ID |
| PUT | /documents/{id} | Update document |
| DELETE | /documents/{id} | Delete document |

## Database

SQL Server - DocumentApprovalDB

- Documents
- ApprovalHistories
- Notifications

## Run application

```bash
dotnet run --project src/DocumentService/DocumentService.Api
```

## Test

Swagger UI: /swagger
