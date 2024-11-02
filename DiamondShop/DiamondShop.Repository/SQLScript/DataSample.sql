INSERT INTO [Category] (Name, Status) VALUES
      ('Rings', 'available'),
      ('Necklaces', 'available'),
      ('Bracelets', 'available'),
      ('Earrings', 'available'),
      ('Watches', 'available');
INSERT INTO [Certificate] (ReportNumber, Origin, Shape, Color, Clarity, Cut, CaratWeight) VALUES
      ('12345', 'South Africa', 'Round', 'D', 'VS1', 'Excellent', '1.5'),
      ('12346', 'Australia', 'Princess', 'F', 'VVS1', 'Very Good', '2.0'),
      ('12347', 'India', 'Emerald', 'G', 'SI1', 'Good', '1.2'),
      ('12348', 'Canada', 'Asscher', 'H', 'VS2', 'Fair', '1.8'),
      ('12349', 'Russia', 'Radiant', 'I', 'IF', 'Excellent', '2.5');

-- Bảng Diamond (thêm các bản ghi kim cương mới sao cho phù hợp với chứng chỉ)
INSERT INTO [Diamond] (Origin, Shape, Color, Clarity, Cut, CaratWeight, Price, Quantity, WarrantyPeriod, CertificateId) VALUES
        ('South Africa', 'Round', 'D', 'VS1', 'Excellent', '1.5', 5000, 10, 24, (SELECT Id FROM [Certificate] WHERE ReportNumber = '12345')),
        ('Australia', 'Princess', 'F', 'VVS1', 'Very Good', '2.0', 7000, 8, 18, (SELECT Id FROM [Certificate] WHERE ReportNumber = '12346')),
        ('India', 'Emerald', 'G', 'SI1', 'Good', '1.2', 3000, 5, 12, (SELECT Id FROM [Certificate] WHERE ReportNumber = '12347')),
        ('Canada', 'Asscher', 'H', 'VS2', 'Fair', '1.8', 10000, 12, 36, (SELECT Id FROM [Certificate] WHERE ReportNumber = '12348')),
        ('Russia', 'Radiant', 'I', 'IF', 'Excellent', '2.5', 8000, 15, 24, (SELECT Id FROM [Certificate] WHERE ReportNumber = '12349'));
-- Bảng Product (thêm nhiều sản phẩm giả với thuộc tính từ bảng Diamond)
INSERT INTO [Product] (Name, Material, Gender, Price, Point, Quantity, WarrantyPeriod, DiamondId, CategoryId) VALUES
      ('Diamond Ring - South Africa', 'Gold', 1, 5200, 10, 5, 24, (SELECT Id FROM [Diamond] WHERE Origin = 'South Africa' AND Shape = 'Round'), (SELECT Id FROM [Category] WHERE Name = 'Rings')),
      ('Princess Cut Pendant - Australia', 'Silver', 0, 7200, 15, 3, 18, (SELECT Id FROM [Diamond] WHERE Origin = 'Australia' AND Shape = 'Princess'), (SELECT Id FROM [Category] WHERE Name = 'Necklaces')),
      ('Emerald Diamond Bracelet - India', 'Platinum', 1, 3200, 8, 2, 12, (SELECT Id FROM [Diamond] WHERE Origin = 'India' AND Shape = 'Emerald'), (SELECT Id FROM [Category] WHERE Name = 'Bracelets')),
      ('Asscher Diamond Necklace - Canada', 'Gold', 0, 10500, 20, 4, 36, (SELECT Id FROM [Diamond] WHERE Origin = 'Canada' AND Shape = 'Asscher'), (SELECT Id FROM [Category] WHERE Name = 'Earrings')),
      ('Radiant Diamond Earrings - Russia', 'Rose Gold', 1, 8200, 18, 6, 24, (SELECT Id FROM [Diamond] WHERE Origin = 'Russia' AND Shape = 'Radiant'), (SELECT Id FROM [Category] WHERE Name = 'Watches'));
