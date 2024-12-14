UPDATE [dbo].[Usertbl]
SET UserName = 'default_user'
WHERE UserName IS NULL;

UPDATE [dbo].[Usertbl]
SET Password = 'default_password'
WHERE Password IS NULL;

UPDATE [dbo].[Usertbl]
SET Email = 'default_email@mail.com'
WHERE Email IS NULL;

UPDATE [dbo].[Usertbl]
SET ConfirmPassword = 'default_password'
WHERE ConfirmPassword IS NULL;

UPDATE [dbo].[Usertbl]
SET IsAdmin = 0
WHERE IsAdmin IS NULL;