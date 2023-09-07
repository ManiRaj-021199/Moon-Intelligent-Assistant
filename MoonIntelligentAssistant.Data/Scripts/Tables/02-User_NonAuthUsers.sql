IF NOT EXISTS (SELECT * FROM sys.objects
WHERE object_id = OBJECT_ID(N'[User].[NonAuthUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [User].[NonAuthUsers](
	[UserId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[UserEmail] [nvarchar](75) NOT NULL,
	[AuthCode] [nvarchar](50) NOT NULL,
	[RegisterDate] [datetime2](7) NOT NULL
)
END