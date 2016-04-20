--ALTER TABLE CurrencyCode ALTER COLUMN Alias nvarchar(MAX) NOT NULL
--ALTER TABLE CurrencyCode ALTER COLUMN Value nvarchar(MAX) NOT NULL
ALTER TABLE CurrencyCode ADD CONSTRAINT UC_Alias_Value UNIQUE(Alias, Value);

--ALTER TABLE CurrencyCode ALTER COLUMN Alias nvarchar(32) NOT NULL
--ALTER TABLE CurrencyCode ALTER COLUMN Value nvarchar(32) NOT NULL