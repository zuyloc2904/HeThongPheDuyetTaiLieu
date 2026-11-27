CREATE DATABASE DocumentApprovalDB;
GO

USE DocumentApprovalDB;
GO
CREATE TABLE Documents (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX) NULL,
    CreatedBy NVARCHAR(100) NOT NULL,
    Approver NVARCHAR(100) NULL,
    Status NVARCHAR(30) NOT NULL DEFAULT 'Draft',
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
GO
CREATE TABLE ApprovalHistories (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    DocumentId UNIQUEIDENTIFIER NOT NULL,
    Action NVARCHAR(30) NOT NULL,
    Actor NVARCHAR(100) NOT NULL,
    Comment NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT FK_ApprovalHistories_Documents
        FOREIGN KEY (DocumentId)
        REFERENCES Documents(Id)
);
GO
CREATE TABLE Notifications (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    DocumentId UNIQUEIDENTIFIER NOT NULL,
    Recipient NVARCHAR(100) NOT NULL,
    Message NVARCHAR(500) NOT NULL,
    EventType NVARCHAR(100) NOT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT FK_Notifications_Documents
        FOREIGN KEY (DocumentId)
        REFERENCES Documents(Id)
);
GO
CREATE INDEX IX_Documents_Status
ON Documents(Status);
GO

CREATE INDEX IX_ApprovalHistories_DocumentId
ON ApprovalHistories(DocumentId);
GO

CREATE INDEX IX_Notifications_Recipient_IsRead
ON Notifications(Recipient, IsRead);
GO

SELECT * FROM Documents;
SELECT Id, Title, CreatedBy, Approver, Status, CreatedAt
FROM Documents
ORDER BY CreatedAt DESC;
SELECT Id, Title, Content, Approver, Status, UpdatedAt
FROM Documents;
SELECT *
FROM Documents
ORDER BY CreatedAt DESC;