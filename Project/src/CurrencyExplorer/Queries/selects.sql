SELECT * FROM CurrencyCode;
SELECT * FROM CurrencyData  ORDER BY ActualDate;

DELETE FROM CurrencyData;
DELETE FROM CurrencyCode;


DELETE FROM CurrencyCode WHERE Id = 234;
DELETE FROM CurrencyData WHERE ShortName = 'EUR'

SELECT d.* FROM CurrencyData d INNER JOIN CurrencyCode c ON c.Id = d.CurrencyCodeId WHERE ActualDate = '20.04.2016' AND c.Alias = 'JPY'