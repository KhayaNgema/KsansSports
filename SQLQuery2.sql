

SELECT * FROM MatchFormation;

UPDATE MatchFormation
SET Discriminator = 'MatchFormation'
WHERE Discriminator IS NULL OR Discriminator = '';
