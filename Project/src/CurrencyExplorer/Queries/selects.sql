SELECT * FROM CurrencyCode;
SELECT * FROM CurrencyData;

DELETE FROM CurrencyCode;

DELETE FROM CurrencyData;

DELETE FROM CurrencyCode WHERE Id = 234;
DELETE FROM CurrencyData WHERE CurrencyCodeId = 234