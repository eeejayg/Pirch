CREATE VIEW [dbo].[ProductPrices]
	AS
    SELECT  p.ProductKey as ProductTypeGUID, p.Name as Name,p.[Description] as Description, [dbo].[udf_GetProductPrice](p.ProductId,'A56AB05B-017B-4474-8DD6-11BBA48BD2DA','521DA104-2AC8-DF11-ABB0-00155D046704',1,'7/30/2013',null,null) 'MinimumPrice',
                                [dbo].[udf_GetProductPrice](p.ProductId,'CE954516-DFE0-4959-9493-FA22944FC2AA','521DA104-2AC8-DF11-ABB0-00155D046704',1,'7/30/2013',null,null) 'List Price'
from      ProductCatalog.Product p 
