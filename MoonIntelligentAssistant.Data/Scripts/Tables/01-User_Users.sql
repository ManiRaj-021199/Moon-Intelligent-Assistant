IF NOT EXISTS (SELECT * FROM sys.objects
WHERE object_id = OBJECT_ID(N'[User].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [User].[Users](
	[UserId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[UserEmail] [nvarchar](75) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[PasswordSalt] [varbinary](max) NOT NULL,
	[FailedLoginCount] [tinyint] NOT NULL,
	[RegisterDate] [datetime2](7) NOT NULL
)
END