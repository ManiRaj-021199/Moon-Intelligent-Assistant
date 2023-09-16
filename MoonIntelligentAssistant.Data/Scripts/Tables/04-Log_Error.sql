IF NOT EXISTS (SELECT * FROM sys.objects
WHERE object_id = OBJECT_ID(N'[Log].[Error]') AND type in (N'U'))
BEGIN
CREATE TABLE [Log].[Error](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[ApiName] [nvarchar](100) NOT NULL,
	[ApiRequest] [nvarchar](max) NOT NULL,
	[Exception] [nvarchar](max) NOT NULL,
	[LogDate] [datetime2](7) NOT NULL,
)
END