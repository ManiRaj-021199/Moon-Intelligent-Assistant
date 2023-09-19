IF NOT EXISTS (SELECT * FROM sys.objects
WHERE object_id = OBJECT_ID(N'[Log].[Info]') AND type in (N'U'))
BEGIN
CREATE TABLE [Log].[Info](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[ApiName] [nvarchar](100) NOT NULL,
	[ApiSeverity] [nvarchar](max) NOT NULL,
	[ApiRequest] [nvarchar](max) NOT NULL,
	[ApiResponse] [nvarchar](max) NOT NULL,
	[LogDate] [datetime2](7) NOT NULL,
)
END