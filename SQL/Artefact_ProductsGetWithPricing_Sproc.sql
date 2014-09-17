CREATE PROCEDURE [dbo].[Artefact_Products_CollectAllWithPrice]
    @StoreCode VARCHAR(100),
    @Date datetime,
	@minPriceLevelId VARCHAR(100) = NULL,
	@maxPriceLevelId VARCHAR(100) = NULL
AS
    SELECT
        [dbo].[udf_GetProductPrice](ProductCatalog.Product.ProductId,@minPriceLevelId ,@StoreCode,1,@Date,null,null) 'ListPrice',
        [dbo].[udf_GetProductPrice](ProductCatalog.Product.ProductId,@maxPriceLevelId,@StoreCode,1,@Date ,null,null) 'MinimumPrice',
        ProductCatalog.Product.*, 
        ProductCatalog.ProductImage.ProductImageId as ImageId, ProductCatalog.ProductImage.ImageUrl, ProductCatalog.ProductImage.Sequence,
        ProductCatalog.BrandSeries.BrandSeriesId, ProductCatalog.BrandSeries.BrandId, ProductCatalog.BrandSeries.Name, ProductCatalog.BrandSeries.Description,
        ProductCatalog.Brand.BrandId as BrandSplit, ProductCatalog.Brand.ManufacturerId, ProductCatalog.Brand.Name, ProductCatalog.Brand.Description
    from
        ProductCatalog.Product
        left join ProductCatalog.ProductImage on ProductCatalog.ProductImage.ProductId = ProductCatalog.Product.ProductId
        left join ProductCatalog.BrandSeries on ProductCatalog.Product.BrandSeriesId = ProductCatalog.BrandSeries.BrandSeriesId
        left join ProductCatalog.Brand on ProductCatalog.BrandSeries.BrandId = ProductCatalog.Brand.BrandId
        left join ProductCatalog.Manufacturer on ProductCatalog.Brand.ManufacturerId = ProductCatalog.Manufacturer.ManufacturerId
