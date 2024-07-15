
SELECT FirstName, LastName FROM AspNetUsers WHERE Id IN (SELECT UserId FROM ActivityLogs)


SELECT a.Activity, u.FirstName, u.LastName
FROM ActivityLogs a
JOIN AspNetUsers u ON a.UserId = u.Id
