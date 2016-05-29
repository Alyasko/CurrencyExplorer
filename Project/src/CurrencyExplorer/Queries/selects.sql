SELECT * FROM CurrencyCodeEntry;
SELECT * FROM CurrencyDataEntry  ORDER BY ActualDate;

SELECT * FROM CurrencyCodeEntry WHERE Alias = 'RUB'

DELETE FROM CurrencyDataEntry;
DELETE FROM CurrencyCodeEntry;

DELETE FROM CorrespondanceEntry;
DELETE FROM UserLanguageEntry;
DELETE FROM UserSettingsEntry;

SELECT * FROM CorrespondanceEntry;
SELECT * FROM UserLanguageEntry;
SELECT * FROM UserSettingsEntry;

DELETE FROM CurrencyCode WHERE Id = 234;
DELETE FROM CurrencyData WHERE ShortName = 'EUR'

SELECT d.* FROM CurrencyData d INNER JOIN CurrencyCode c ON c.Id = d.CurrencyCodeId WHERE ActualDate = '20.04.2016' AND c.Alias = 'JPY'