 --Creating the tables

--CREATE TABLE Products(
--    Product_ID int IDENTITY(1,1) PRIMARY KEY,
--    Product_Name varchar(255) not null,
--    Product_Price float not null, check(Product_Price > 0),
--	  Amount_In_Stock int not null
--);

--CREATE TABLE ShoppingCartItems(
--	Product_ID int,
--  Quantity smallint not null,
--	FOREIGN KEY (Product_ID) REFERENCES Products(Product_ID)
--);

 --Adding Products

INSERT INTO Product
VALUES 
('Banana',02.30, 700),
('Apple',03.10, 950),
('Peach',05.70, 1010),
('Avocado',08.30, 450),
('Potato',01.30, 10000);

--Get all items in cart
--select p.Product_ID, Product_Name 'Name', Product_Price 'Price', Quantity
--from ShoppingCartItems sc, Products p
--where p.Product_ID = sc.Product_ID

-- edit repository
--UPDATE Products
--SET Amount_In_Stock = Amount_In_Stock - 1
--WHERE Product_ID = 1;

--INSERT INTO ShoppingCartItems
--VALUES (2,3);