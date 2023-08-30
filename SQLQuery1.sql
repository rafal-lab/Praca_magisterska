EXEC sp_rename 'Table', 'DellServerPricingTable';

INSERT INTO DellServerPricingTable (Id,ServerName, RAM, Processor, HDD, SSD, Price)
VALUES (1, 'Dell PowerEdge R350', '8GB' , null, null, null, 780.00),
       (2, 'Dell PowerEdge R350', '16GB', null, null, null, 1166.00),
       (3, 'Dell PowerEdge R350', '32GB', null, null, NULL, 2042.00),
       (4, 'Dell PowerEdge R350', null, 'Intel Xeon E2314', null, null, 0.00),
       (5, 'Dell PowerEdge R350', null, 'Intel Xeon E2323G', null, null, 181.00),
       (6, 'Dell PowerEdge R350', null, 'Intel Xeon 2323', null, NULL, 401.00),
       (7, 'Dell PowerEdge R350', null, 'Intel Xeon E2336', null, null, 809.00),
       (8, 'Dell PowerEdge R350', null, null, '1TB', null, 855.00),
       (9, 'Dell PowerEdge R350', null, null, '2TB', NULL, 899.00),
       (10, 'Dell PowerEdge R350', null, null, '4TB', null, 855.00),
       (11, 'Dell PowerEdge R350', null, null, null, '480GB', 22325.00),
       (12, 'Dell PowerEdge R350', null, null, null, '960GB', 2931.00);