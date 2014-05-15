USE [kooboo.commerce]
GO
SET IDENTITY_INSERT [dbo].[Country] ON 

INSERT [dbo].[Country] ([Id], [Name], [ThreeLetterISOCode], [TwoLetterISOCode], [NumericISOCode]) VALUES (1, N'United States', N'USA', N'US', N'')
INSERT [dbo].[Country] ([Id], [Name], [ThreeLetterISOCode], [TwoLetterISOCode], [NumericISOCode]) VALUES (2, N'China', N'CHN', N'CN', N'')
SET IDENTITY_INSERT [dbo].[Country] OFF
SET IDENTITY_INSERT [dbo].[Brand] ON 

INSERT [dbo].[Brand] ([Id], [Name], [Description], [Logo]) VALUES (1, N'H&M', N'', N'')
INSERT [dbo].[Brand] ([Id], [Name], [Description], [Logo]) VALUES (2, N'Lenovo', N'', N'')
INSERT [dbo].[Brand] ([Id], [Name], [Description], [Logo]) VALUES (3, N'Apple', N'', N'')
INSERT [dbo].[Brand] ([Id], [Name], [Description], [Logo]) VALUES (4, N'Sumsung', N'', N'')
INSERT [dbo].[Brand] ([Id], [Name], [Description], [Logo]) VALUES (5, N'Jack Jones', N'', N'')
INSERT [dbo].[Brand] ([Id], [Name], [Description], [Logo]) VALUES (6, N'Canon', N'', N'')
INSERT [dbo].[Brand] ([Id], [Name], [Description], [Logo]) VALUES (7, N'JVC', N'', N'')
INSERT [dbo].[Brand] ([Id], [Name], [Description], [Logo]) VALUES (8, N'Xiaomi', N'', N'')
SET IDENTITY_INSERT [dbo].[Brand] OFF
SET IDENTITY_INSERT [dbo].[ProductType] ON 

INSERT [dbo].[ProductType] ([Id], [Name], [SkuAlias], [IsEnabled], [IsDeleted], [DeletedAtUtc]) VALUES (1, N'Laptop', N'laptop', 1, 1, CAST(0x0000A2E400680FDE AS DateTime))
INSERT [dbo].[ProductType] ([Id], [Name], [SkuAlias], [IsEnabled], [IsDeleted], [DeletedAtUtc]) VALUES (2, N'iPhone 4S', N'iPhone 4S', 1, 0, NULL)
INSERT [dbo].[ProductType] ([Id], [Name], [SkuAlias], [IsEnabled], [IsDeleted], [DeletedAtUtc]) VALUES (3, N'Xiaomi', N'Xiaomi', 1, 0, NULL)
SET IDENTITY_INSERT [dbo].[ProductType] OFF
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([Id], [Name], [BrandId], [ProductTypeId], [CreatedAtUtc], [IsDeleted], [DeletedAtUtc], [IsPublished], [PublishedAtUtc]) VALUES (1, N'Mi2', 8, 3, CAST(0x0000A2E400F07DFA AS DateTime), 0, NULL, 1, NULL)
INSERT [dbo].[Product] ([Id], [Name], [BrandId], [ProductTypeId], [CreatedAtUtc], [IsDeleted], [DeletedAtUtc], [IsPublished], [PublishedAtUtc]) VALUES (2, N'Mi3', 8, 3, CAST(0x0000A2E400F18D40 AS DateTime), 0, NULL, 0, NULL)
SET IDENTITY_INSERT [dbo].[Product] OFF
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (1, N'Books', N'', N'', 1, NULL)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (2, N'Phones', N'', N'', 1, NULL)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (3, N'Computers', N'', N'', 1, NULL)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (4, N'Clothes', N'', N'', 1, NULL)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (5, N'Camera', N'', N'', 1, NULL)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (6, N'Car', N'', N'', 1, NULL)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (7, N'Fiction', N'', N'', 1, 1)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (8, N'Masterpiece', N'', N'', 1, 1)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (9, N'Poem', N'', N'', 1, 1)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (10, N'Prose', N'', N'', 1, 1)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (11, N'Music', N'', N'', 1, NULL)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (12, N'Pop', N'', N'', 1, 11)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (13, N'Blues', N'', N'', 1, 11)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (14, N'Classical', N'', N'', 1, 11)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (15, N'Men', N'', N'', 1, 4)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (16, N'Wemen', N'', N'', 1, 4)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (17, N'Laptop', N'', N'', 1, 3)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (18, N'Desktop', N'', N'', 1, 3)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (19, N'IOS', N'', N'', 1, 2)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (20, N'Android', N'', N'', 1, 2)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (21, N'WP', N'', N'', 1, 2)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (22, N'Dresses', N'', N'', 1, 16)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (23, N'Jacket', N'', N'', 1, 15)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (24, N'Socks', N'', N'', 1, 15)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (25, N'Scarf', N'', N'', 1, 16)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (26, N'iPhone', N'', N'', 1, 19)
INSERT [dbo].[Category] ([Id], [Name], [Description], [Photo], [Published], [Parent_Id]) VALUES (27, N'Galaxy', N'', N'', 1, 20)
SET IDENTITY_INSERT [dbo].[Category] OFF
INSERT [dbo].[ProductCategory] ([ProductId], [CategoryId]) VALUES (1, 2)
INSERT [dbo].[ProductCategory] ([ProductId], [CategoryId]) VALUES (1, 20)
INSERT [dbo].[ProductCategory] ([ProductId], [CategoryId]) VALUES (2, 2)
INSERT [dbo].[ProductCategory] ([ProductId], [CategoryId]) VALUES (2, 20)
SET IDENTITY_INSERT [dbo].[CustomField] ON 

INSERT [dbo].[CustomField] ([Id], [Name], [FieldType], [DataType], [Label], [Tooltip], [ControlType], [DefaultValue], [Length], [Sequence], [Modifiable], [Indexable], [AllowNull], [ShowInGrid], [Summarize], [IsEnabled], [CustomSettings], [SelectionItems]) VALUES (1, N'Color', 0, 0, N'Color', N'', N'DropDownList', N'', 0, 1, 1, 0, 1, 1, 0, 1, N'', N'[{"Key":"red","Value":"red"},{"Key":"white","Value":"white"},{"Key":"black","Value":"black"}]')
INSERT [dbo].[CustomField] ([Id], [Name], [FieldType], [DataType], [Label], [Tooltip], [ControlType], [DefaultValue], [Length], [Sequence], [Modifiable], [Indexable], [AllowNull], [ShowInGrid], [Summarize], [IsEnabled], [CustomSettings], [SelectionItems]) VALUES (2, N'Screen', 0, 0, N'Screen', N'', N'DropDownList', N'', 0, 2, 1, 0, 1, 0, 0, 1, N'', N'[{"Key":"4","Value":"4"},{"Key":"4.7","Value":"4.7"},{"Key":"5","Value":"5"}]')
INSERT [dbo].[CustomField] ([Id], [Name], [FieldType], [DataType], [Label], [Tooltip], [ControlType], [DefaultValue], [Length], [Sequence], [Modifiable], [Indexable], [AllowNull], [ShowInGrid], [Summarize], [IsEnabled], [CustomSettings], [SelectionItems]) VALUES (3, N'SDCard', 0, 0, N'SD Card', N'', N'DropDownList', N'', 0, 1, 1, 0, 1, 1, 0, 1, N'', N'[{"Key":"16G","Value":"16G"},{"Key":"32G","Value":"32G"}]')
INSERT [dbo].[CustomField] ([Id], [Name], [FieldType], [DataType], [Label], [Tooltip], [ControlType], [DefaultValue], [Length], [Sequence], [Modifiable], [Indexable], [AllowNull], [ShowInGrid], [Summarize], [IsEnabled], [CustomSettings], [SelectionItems]) VALUES (4, N'Battery', 0, 0, N'Battery', N'', N'DropDownList', N'', 0, 2, 1, 0, 1, 0, 0, 1, N'', N'[{"Key":"Normal","Value":"Normal"},{"Key":"Enhanced","Value":"Enhanced"}]')
INSERT [dbo].[CustomField] ([Id], [Name], [FieldType], [DataType], [Label], [Tooltip], [ControlType], [DefaultValue], [Length], [Sequence], [Modifiable], [Indexable], [AllowNull], [ShowInGrid], [Summarize], [IsEnabled], [CustomSettings], [SelectionItems]) VALUES (5, N'Color', 0, 0, N'Color', N'', N'DropDownList', N'', 0, 1, 1, 0, 1, 1, 0, 1, N'', N'[{"Key":"red","Value":"red"},{"Key":"blue","Value":"blue"},{"Key":"yellow","Value":"yellow"},{"Key":"white","Value":"white"},{"Key":"black","Value":"black"}]')
INSERT [dbo].[CustomField] ([Id], [Name], [FieldType], [DataType], [Label], [Tooltip], [ControlType], [DefaultValue], [Length], [Sequence], [Modifiable], [Indexable], [AllowNull], [ShowInGrid], [Summarize], [IsEnabled], [CustomSettings], [SelectionItems]) VALUES (6, N'CPU', 0, 0, N'CPU', N'', N'DropDownList', N'', 0, 1, 1, 0, 1, 1, 0, 1, N'', N'[{"Key":"4 core","Value":"4 core"},{"Key":"8 core","Value":"8 core"}]')
INSERT [dbo].[CustomField] ([Id], [Name], [FieldType], [DataType], [Label], [Tooltip], [ControlType], [DefaultValue], [Length], [Sequence], [Modifiable], [Indexable], [AllowNull], [ShowInGrid], [Summarize], [IsEnabled], [CustomSettings], [SelectionItems]) VALUES (7, N'ProcessorSpeed', 0, 0, N'Processor Speed', N'', N'DropDownList', N'', 0, 2, 1, 0, 1, 0, 0, 1, N'', N'[{"Key":"1.3GHz","Value":"1.3GHz"},{"Key":"1.8GHz","Value":"1.8GHz"},{"Key":"2.3GHz","Value":"2.3GHz"}]')
INSERT [dbo].[CustomField] ([Id], [Name], [FieldType], [DataType], [Label], [Tooltip], [ControlType], [DefaultValue], [Length], [Sequence], [Modifiable], [Indexable], [AllowNull], [ShowInGrid], [Summarize], [IsEnabled], [CustomSettings], [SelectionItems]) VALUES (8, N'Screen', 0, 0, N'Screen', N'', N'DropDownList', N'', 0, 3, 1, 0, 1, 0, 0, 1, N'', N'[{"Key":"4 inch.","Value":"4 inch."},{"Key":"4.1 inch.","Value":"4.1 inch."},{"Key":"4.3 inch.","Value":"4.3 inch."},{"Key":"5 inch.","Value":"5 inch."}]')
INSERT [dbo].[CustomField] ([Id], [Name], [FieldType], [DataType], [Label], [Tooltip], [ControlType], [DefaultValue], [Length], [Sequence], [Modifiable], [Indexable], [AllowNull], [ShowInGrid], [Summarize], [IsEnabled], [CustomSettings], [SelectionItems]) VALUES (9, N'SDCard', 0, 0, N'SD Card', N'', N'DropDownList', N'', 0, 4, 1, 0, 1, 0, 0, 1, N'', N'[{"Key":"16G","Value":"16G"},{"Key":"32G","Value":"32G"},{"Key":"64G","Value":"64G"}]')
INSERT [dbo].[CustomField] ([Id], [Name], [FieldType], [DataType], [Label], [Tooltip], [ControlType], [DefaultValue], [Length], [Sequence], [Modifiable], [Indexable], [AllowNull], [ShowInGrid], [Summarize], [IsEnabled], [CustomSettings], [SelectionItems]) VALUES (10, N'Camera', 0, 0, N'Camera', N'', N'DropDownList', N'', 0, 5, 1, 0, 1, 0, 0, 1, N'', N'[{"Key":"5 megapixel","Value":"5 megapixel"},{"Key":"8 megapixel","Value":"8 megapixel"},{"Key":"13 megapixel","Value":"13 megapixel"}]')
SET IDENTITY_INSERT [dbo].[CustomField] OFF
INSERT [dbo].[ProductCustomFieldValue] ([ProductId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (1, 5, N'white', NULL)
INSERT [dbo].[ProductCustomFieldValue] ([ProductId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (2, 5, N'red', NULL)
SET IDENTITY_INSERT [dbo].[ProductPrice] ON 

INSERT [dbo].[ProductPrice] ([Id], [ProductId], [Name], [Sku], [PurchasePrice], [RetailPrice], [Stock], [DeliveryDays], [CreatedAtUtc], [IsPublished], [PublishedAtUtc]) VALUES (1, 1, N'', N'mi2-001', CAST(1299.00 AS Decimal(18, 2)), CAST(1299.00 AS Decimal(18, 2)), 50, 2, CAST(0x0000A2E400F1262D AS DateTime), 1, CAST(0x0000A2E400F1262D AS DateTime))
INSERT [dbo].[ProductPrice] ([Id], [ProductId], [Name], [Sku], [PurchasePrice], [RetailPrice], [Stock], [DeliveryDays], [CreatedAtUtc], [IsPublished], [PublishedAtUtc]) VALUES (2, 1, N'', N'mi2-002', CAST(1799.00 AS Decimal(18, 2)), CAST(1799.00 AS Decimal(18, 2)), 50, 2, CAST(0x0000A2E400F15AF5 AS DateTime), 1, CAST(0x0000A2E400F15AF5 AS DateTime))
INSERT [dbo].[ProductPrice] ([Id], [ProductId], [Name], [Sku], [PurchasePrice], [RetailPrice], [Stock], [DeliveryDays], [CreatedAtUtc], [IsPublished], [PublishedAtUtc]) VALUES (3, 2, N'', N'mi3-001', CAST(1999.00 AS Decimal(18, 2)), CAST(1999.00 AS Decimal(18, 2)), 50, 2, CAST(0x0000A2E400F19DFD AS DateTime), 1, CAST(0x0000A2E400F19DFD AS DateTime))
INSERT [dbo].[ProductPrice] ([Id], [ProductId], [Name], [Sku], [PurchasePrice], [RetailPrice], [Stock], [DeliveryDays], [CreatedAtUtc], [IsPublished], [PublishedAtUtc]) VALUES (4, 2, N'', N'mi3-002', CAST(2499.00 AS Decimal(18, 2)), CAST(2499.00 AS Decimal(18, 2)), 50, 2, CAST(0x0000A2E400F1C1E7 AS DateTime), 1, CAST(0x0000A2E400F1C1E7 AS DateTime))
SET IDENTITY_INSERT [dbo].[ProductPrice] OFF
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (1, 6, N'4 core', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (1, 7, N'1.3GHz', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (1, 8, N'4.3 inch.', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (1, 9, N'16G', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (1, 10, N'8 megapixel', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (2, 6, N'8 core', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (2, 7, N'1.8GHz', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (2, 8, N'4.3 inch.', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (2, 9, N'32G', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (2, 10, N'13 megapixel', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (3, 6, N'4 core', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (3, 7, N'1.8GHz', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (3, 8, N'5 inch.', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (3, 9, N'16G', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (3, 10, N'13 megapixel', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (4, 6, N'8 core', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (4, 7, N'2.3GHz', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (4, 8, N'5 inch.', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (4, 9, N'64G', NULL)
INSERT [dbo].[ProductPriceVariantValue] ([ProductPriceId], [CustomFieldId], [FieldValue], [FieldText]) VALUES (4, 10, N'13 megapixel', NULL)

SET IDENTITY_INSERT [dbo].[PaymentMethod] ON 
GO
INSERT [dbo].[PaymentMethod] ([Id], [DisplayName], [PaymentProcessorName], [AdditionalFeeChargeMode], [AdditionalFeeAmount], [AdditionalFeePercent], [IsEnabled], [CreatedAtUtc]) 
VALUES (1, N'Fake Payment', N'Fake', 0, CAST(3.00 AS Decimal(18, 2)), 0, 1, GetDate())
GO
SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF
GO