﻿IF (NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'User')) 
BEGIN
    EXEC ('CREATE SCHEMA [User] AUTHORIZATION [dbo]')
END


IF (NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Log')) 
BEGIN
    EXEC ('CREATE SCHEMA [Log] AUTHORIZATION [dbo]')
END