﻿CREATE TABLE [dbo].[Groups]
(
    [Name] VARCHAR(50)  NOT NULL, 
    [CustomType] VARCHAR(50) NULL, 
    [Description] VARCHAR(50) NULL, 
    [Image] VARBINARY(50) NULL, 
    PRIMARY KEY ([Name]), 
    CONSTRAINT [CK_Groups_Name] UNIQUE([Name])
)

GO

CREATE INDEX [IX_Groups_Name] ON [dbo].[Groups] ([Name])
