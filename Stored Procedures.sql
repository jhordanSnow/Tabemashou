USE Tabemashou
GO


CREATE FUNCTION Split (
      @InputString                  VARCHAR(8000),
      @Delimiter                    VARCHAR(50)
)

RETURNS @Items TABLE (
      Item                          VARCHAR(8000)
)

AS
BEGIN
      IF @Delimiter = ' '
      BEGIN
            SET @Delimiter = ','
            SET @InputString = REPLACE(@InputString, ' ', @Delimiter)
      END

      IF (@Delimiter IS NULL OR @Delimiter = '')
            SET @Delimiter = ','

--INSERT INTO @Items VALUES (@Delimiter) -- Diagnostic
--INSERT INTO @Items VALUES (@InputString) -- Diagnostic

      DECLARE @Item                 VARCHAR(8000)
      DECLARE @ItemList       VARCHAR(8000)
      DECLARE @DelimIndex     INT

      SET @ItemList = @InputString
      SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
      WHILE (@DelimIndex != 0)
      BEGIN
            SET @Item = SUBSTRING(@ItemList, 0, @DelimIndex)
            INSERT INTO @Items VALUES (@Item)

            -- Set @ItemList = @ItemList minus one less item
            SET @ItemList = SUBSTRING(@ItemList, @DelimIndex+1, LEN(@ItemList)-@DelimIndex)
            SET @DelimIndex = CHARINDEX(@Delimiter, @ItemList, 0)
      END -- End WHILE

      IF @Item IS NOT NULL -- At least one delimiter was encountered in @InputString
      BEGIN
            SET @Item = @ItemList
            INSERT INTO @Items VALUES (@Item)
      END

      -- No delimiters were encountered in @InputString, so just return @InputString
      ELSE INSERT INTO @Items VALUES (@InputString)

      RETURN

END -- End Function
GO

CREATE PROCEDURE [PR_CreateUser] (
	@IdCard NUMERIC(20),
	@Username VARCHAR(25),
    @Password VARCHAR(255),
    @Gender CHAR(1),
    @BirthDate DATE,
    @Nationality INT,
    @FirstName VARCHAR(50),
    @MiddleName VARCHAR(50) = NULL,
    @LastName VARCHAR(50),
    @SecondLastName VARCHAR(50) = NULL
)
AS BEGIN
	BEGIN TRY
		INSERT INTO [Tabemashou].[dbo].[User]( [IdCard], [Username], [Password], [FirstName], [MiddleName],	
											[LastName], [SecondLastName], [Gender], [BirthDate], [Nationality])
		values(@IdCard, @Username, @Password, @FirstName, @MiddleName, @LastName,
			 @SecondLastName, @Gender, @BirthDate, @Nationality)
	END TRY
	BEGIN CATCH
		RETURN 0
	END CATCH
	RETURN 1
END
GO

CREATE PROCEDURE [PR_CreateAdministrator] (
	@IdCard NUMERIC(20),
	@Username VARCHAR(25),
    @Password VARCHAR(255),
    @Gender CHAR(1),
    @BirthDate DATE,
    @Nationality INT,
    @FirstName VARCHAR(50),
    @MiddleName VARCHAR(50) = NULL,
    @LastName VARCHAR(50),
    @SecondLastName VARCHAR(50) = NULL
)
AS BEGIN
	DECLARE @UserError INT
	EXEC @UserError = [PR_CreateUser]
		@IdCard = @IdCard,
		@Username = @Username,
		@Password = @Password,
		@Gender = @Gender,
		@BirthDate = @BirthDate,
		@Nationality = @Nationality,
		@FirstName = @FirstName,
		@MiddleName = @MiddleName,
		@LastName = @LastName,
		@SecondLastName = @SecondLastName;
	IF @UserError = 1
	BEGIN
		INSERT INTO [Administrator](IdCard) VALUES(@IdCard)
		RETURN @UserError
	END
	ELSE
		RETURN @UserError
END
GO

CREATE PROCEDURE [PR_CreateCustomer] (
	@IdCard NUMERIC(20),
	@Username VARCHAR(25),
    @Password VARCHAR(255),
    @Gender CHAR(1),
    @BirthDate DATE,
    @Nationality INT,
    @FirstName VARCHAR(50),
    @MiddleName VARCHAR(50) = NULL,
    @LastName VARCHAR(50),
    @SecondLastName VARCHAR(50) = NULL,
	@Photo VARBINARY(MAX) = NULL,
	@AccountNumber NUMERIC(20)
)
AS BEGIN
	DECLARE @UserError INT
	EXEC @UserError = [PR_CreateUser]
		@IdCard = @IdCard,
		@Username = @Username,
		@Password = @Password,
		@Gender = @Gender,
		@BirthDate = @BirthDate,
		@Nationality = @Nationality,
		@FirstName = @FirstName,
		@MiddleName = @MiddleName,
		@LastName = @LastName,
		@SecondLastName = @SecondLastName;
	IF @UserError = 1
	BEGIN
		INSERT INTO [Customer](IdCard,Photo,AccountNumber) VALUES(@IdCard,@Photo,@AccountNumber)
		RETURN @UserError
	END
	ELSE
		RETURN @UserError
END
GO

CREATE PROCEDURE [PR_RestaurantInfo] (
	@AminId NUMERIC(20)
)
AS BEGIN
	SELECT 
		R.*,
		ISNULL(CantLocal.CantLocal, 0) CantLocal
	FROM [Restaurant] R
		LEFT JOIN (	SELECT 
						L.IdRestaurant [IdRest], 
						COUNT(L.IdLocal) [CantLocal]
					FROM [Local] L 
					GROUP BY L.IdRestaurant
		) CantLocal ON CantLocal.IdRest = R.IdRestaurant
	WHERE R.IdAdmin = @AminId
END	
GO

CREATE PROCEDURE [PR_DeleteRestaurant](
	@RestaurantId INT
)

AS BEGIN
DECLARE @TypesId TABLE([IdType] INT)
INSERT INTO @TypesId ([IdType])
			(	SELECT TR.IdType
				FROM [TypesPerRestaurant] TR
				WHERE TR.IdRestaurant = @RestaurantId
			)

DELETE FROM Restaurant WHERE IdRestaurant = @RestaurantId

DELETE T FROM [Type] T
	INNER JOIN (SELECT T2.IdType
				FROM [Type] T2
					LEFT JOIN [TypesPerRestaurant] TR ON TR.IdType = T2.IdType
				WHERE T2.IdType IN (SELECT * FROM @TypesId)
				GROUP BY T2.IdType
				HAVING COUNT(TR.IdRestaurant) = 0
	) NoType ON NoType.IdType = T.IdType
END
GO

CREATE PROCEDURE [PR_DeleteRestaurantTypes](
	@RestaurantId INT
)AS BEGIN
	DELETE FROM [TypesPerRestaurant] WHERE IdRestaurant = @RestaurantId
END
GO

CREATE PROCEDURE [PR_UpdateRestaurantTypes](
	@RestaurantId INT,
	@TypeList VARCHAR(MAX)
)

AS BEGIN

	DECLARE @TypesId TABLE([IdType] INT)
	INSERT INTO @TypesId ([IdType])
			(	SELECT TR.IdType
				FROM [TypesPerRestaurant] TR
				WHERE TR.IdRestaurant = @RestaurantId
			)

	EXEC [PR_DeleteRestaurantTypes] @RestaurantId
	IF @TypeList IS NOT NULL
	BEGIN
		INSERT INTO [TypesPerRestaurant]
			SELECT @RestaurantId, VALUE FROM string_split(@TypeList,',')
	END

	DELETE T FROM [Type] T
	INNER JOIN (SELECT T2.IdType
				FROM [Type] T2
					LEFT JOIN [TypesPerRestaurant] TR ON TR.IdType = T2.IdType
				WHERE T2.IdType IN (SELECT IdType FROM @TypesId)
				GROUP BY T2.IdType
				HAVING COUNT(TR.IdRestaurant) = 0
	) NoType ON NoType.IdType = T.IdType
END
GO

CREATE PROCEDURE [PR_GetDistricts]
AS BEGIN

SELECT 
	IdDistrict AS [IdDistrict],
	CONCAT(P.Name, ' - ', C.Name, ' - ', D.Name) AS [Name]
FROM [Province] P
	INNER JOIN [Canton] C ON C.IdProvince = P.IdProvince
	INNER JOIN [District] D ON D.IdCanton = C.IdCanton

END
GO
CREATE PROCEDURE [PR_CreateLocal](
	@IdRestaurant INT,
	@IdDistrict INT,
	@Detail VARCHAR(MAX) = NULL,
	@Latitude VARCHAR(MAX),
	@Longitude VARCHAR(MAX)
) AS BEGIN

	DECLARE @LocalName INT
	SET @LocalName = (SELECT COUNT(*) + 1 FROM [Local] WHERE IdRestaurant = @IdRestaurant)
	INSERT INTO [Local] ([Name], [Latitude], [Longitude], [IdDistrict], [Detail], [IdRestaurant])
				VALUES(@LocalName, @Latitude, @Longitude, @IdDistrict,@Detail,@IdRestaurant)
	
	SELECT @@IDENTITY
END
GO

CREATE PROCEDURE [PR_DeleteLocal](
	@IdLocal INT
) AS BEGIN
	DELETE FROM [DishesPerLocal] WHERE IdLocal = @IdLocal
	DELETE P FROM [PhotosPerLocal] DP 
		INNER JOIN [Photo] P ON P.IdPhoto = DP.IdPhoto
		WHERE DP.IdLocal = @IdLocal
	DELETE FROM [Table] WHERE IdLocal = @IdLocal
	DELETE FROM [Review] WHERE IdLocal = @IdLocal
	DELETE FROM [Local] WHERE IdLocal = @IdLocal
END
GO

CREATE PROCEDURE [PR_DeleteLocalPhoto](
	@IdLocal INT,
	@IdPhoto INT
)AS BEGIN
	DELETE FROM [PhotosPerLocal] WHERE IdLocal = @IdLocal AND IdPhoto = @IdPhoto
	DELETE FROM [Photo] WHERE IdPhoto = @IdPhoto
END
GO

CREATE PROCEDURE [PR_DeleteDishPhoto](
	@IdDish INT,
	@IdPhoto INT
)AS BEGIN
	DELETE FROM [PhotosPerDish] WHERE IdDish = @IdDish AND IdPhoto = @IdPhoto
	DELETE FROM [Photo] WHERE IdPhoto = @IdPhoto
END
GO

CREATE PROCEDURE [PR_DeleteDish](
	@DishId INT
)

AS BEGIN
DECLARE @TypesId TABLE([IdType] INT)
INSERT INTO @TypesId ([IdType])
			(	SELECT TD.IdType
				FROM [TypesPerDish] TD
				WHERE TD.IdDish = @DishId
			)

DELETE FROM Dish WHERE IdDish = @DishId

DELETE T FROM [Type] T
	INNER JOIN (SELECT T2.IdType
				FROM [Type] T2
					LEFT JOIN [TypesPerDish] TD ON TD.IdType = T2.IdType
				WHERE T2.IdType IN (SELECT * FROM @TypesId)
				GROUP BY T2.IdType
				HAVING COUNT(TD.IdDish) = 0
	) NoType ON NoType.IdType = T.IdType
END
GO

CREATE PROCEDURE [PR_DeleteDishTypes](
	@DishId INT
)AS BEGIN
	DELETE FROM [TypesPerDish] WHERE IdDish = @DishId
END
GO

CREATE PROCEDURE [PR_DishTypes]
AS BEGIN
	SELECT T.* FROM [Type] T INNER JOIN [TypesPerDish] TPD ON T.IdType = TPD.IdType
	GROUP BY T.IdType, T.Name
END
GO

CREATE PROCEDURE [PR_RestaurantTypes]
AS BEGIN
	SELECT T.* FROM [Type] T INNER JOIN [TypesPerRestaurant] TPR ON T.IdType = TPR.IdType
	GROUP BY T.IdType, T.Name
END
GO

CREATE PROCEDURE [PR_NewCheck] (
	@IdLocal INT
)AS BEGIN
	INSERT INTO [Check]([Balance], [IdLocal]) VALUES(0, @IdLocal)
	SELECT @@IDENTITY
END
GO

CREATE PROCEDURE [PR_CreateDetailCheck](
	@IdDish INT,
	@IdCheck INT
) AS BEGIN
	DECLARE @PRICE SMALLMONEY
	DECLARE @PRICEPRODUCT SMALLMONEY
	DECLARE @EXISTE INT
	DECLARE @TAX DECIMAL
	DECLARE @SERVICE DECIMAL
	SET @EXISTE = (SELECT [IdDish] FROM [DishesByCheck] WHERE [IdDish] = @IdDish AND [IdCheck] = @IdCheck)
	SET @TAX = (SELECT CONVERT(DECIMAL,[VALUE]) FROM [PARAMETER] WHERE [PARAMETER] = 'SELL_TAX')
	SET @SERVICE = (SELECT CONVERT(DECIMAL,[VALUE]) FROM [PARAMETER] WHERE [PARAMETER] = 'SERVICE_TAX')
	SET @PRICEPRODUCT = (SELECT [Price] FROM [Dish] WHERE [IdDish] = @IdDish)

	IF (@EXISTE = @IdDish)
	BEGIN
		UPDATE [DishesByCheck] SET [Quantity] += 1 WHERE [IdDish] = @IdDish AND [IdCheck] = @IdCheck	
	END 
	ELSE BEGIN
		INSERT INTO [DishesByCheck](IdCheck,IdDish,Quantity,SellTax,ServiceTax,UnitaryPrice)
		VALUES(@IdCheck, @IdDish, 1, @TAX, @SERVICE, @PRICEPRODUCT)
	END

	SET @PRICE = @PRICEPRODUCT + (@PRICEPRODUCT * @TAX / 100) + (@PRICEPRODUCT * @SERVICE / 100)
	UPDATE [Check] SET [Balance] -= @PRICE WHERE [IdCheck] = @IdCheck
END
GO

CREATE PROCEDURE [PR_CreatePaymentCheck](
	@IdCard NUMERIC(20),
	@IdCheck INT
)AS BEGIN
	DECLARE @ACCOUNTNUMBER DECIMAL
	SET @ACCOUNTNUMBER = (SELECT [AccountNumber] FROM [Customer] WHERE [IdCard] = @IdCard)
	INSERT INTO [PaymentByCustomer]([IdCard],[IdCheck],[AccountNumber],[TotalPrice]) VALUES(@IdCard, @IdCheck, @ACCOUNTNUMBER, 0)
END
GO

CREATE PROCEDURE [PR_GetChecks](
	@IdCard NUMERIC(20)
)AS BEGIN
	SELECT C.*, ISNULL(R.CantReviews,0) [CantReviews] FROM [Check] C
		INNER JOIN [PaymentByCustomer] PC ON PC.IdCheck = C.IdCheck AND PC.IdCard = @IdCard	
		LEFT JOIN (SELECT C2.IdCheck IdC2, COUNT(R.IdReview) [CantReviews]
					FROM [Check] C2
					INNER JOIN [Review] R ON R.IdCheck = C2.IdCheck
					GROUP BY C2.IdCheck) R ON R.IdC2 = C.IdCheck
	ORDER BY C.[Date] DESC
END
GO

CREATE PROCEDURE [PR_DeleteUnusedTypes]
AS BEGIN
	DELETE T2 FROM [Type] T2 WHERE T2.[IdType] IN (
		SELECT T.[IdType] FROM [Type] T
		LEFT JOIN  [TypesPerRestaurant] TPR ON T.IdType = TPR.IdType
		LEFT JOIN [TypesPerDish] TPD ON T.IdType = TPD.IdType
		GROUP BY T.[IdType]
		HAVING COUNT(TPR.IdRestaurant) = 0 AND COUNT(TPD.IdDish) = 0
		)
END
GO

CREATE PROCEDURE [PR_GetTypes](
	@IdRestaurant INT
)AS BEGIN
	SELECT T.IdType FROM [Type] T
		INNER JOIN [TypesPerDish] TPD ON TPD.IdType = T.IdType
		INNER JOIN [Dish] D ON D.IdDish = TPD.IdDish AND D.IdRestaurant = @IdRestaurant
	GROUP BY T.IdType
END
GO

CREATE PROCEDURE [PR_GetFriends](
	@IdCard NUMERIC(20)
)AS BEGIN
	SELECT U.* 
	FROM [FriendsByCustomer] FBC
	INNER JOIN [User] U
		ON FBC.IdFriend = U.IdCard
	WHERE FBC.IdCard = @IdCard
END
GO

CREATE PROCEDURE [PR_GetFriends](
	@IdCard NUMERIC(20)
)AS BEGIN
	SELECT U.* 
	FROM [FriendsByCustomer] FBC
	INNER JOIN [User] U
		ON FBC.IdFriend = U.IdCard
	WHERE FBC.IdCard = @IdCard
END
GO

CREATE PROCEDURE [PR_GetFollowers](
	@IdCard NUMERIC(20)
)AS BEGIN
	SELECT U.* 
	FROM [FriendsByCustomer] FBC
	INNER JOIN [User] U
		ON FBC.IdFriend = @IdCard
	WHERE FBC.IdCard = U.IdCard
END
GO

CREATE PROCEDURE [dbo].[PR_GetFriendsCount](
	@IdCard NUMERIC(20),
	@Qty INT OUTPUT
)AS BEGIN
	SELECT @Qty = COUNT(*)
		FROM [FriendsByCustomer] FBC
		INNER JOIN [User] U
			ON FBC.IdFriend = U.IdCard
		WHERE FBC.IdCard = @IdCard
	return
END
GO

CREATE PROCEDURE [dbo].[PR_GetFollowersCount](
	@IdCard NUMERIC(20),
	@Qty INT OUTPUT
)AS BEGIN
	SELECT @Qty = COUNT(*)
		FROM [FriendsByCustomer] FBC
		INNER JOIN [User] U
			ON FBC.IdFriend = @IdCard
		WHERE FBC.IdCard = U.IdCard
	RETURN
END
GO

CREATE PROCEDURE [PR_BalanceAccounts]
AS BEGIN
	UPDATE [Check] SET [Balance] = 0, [State] = 'Paid' WHERE ([Balance] < 1 AND [Balance] > -1)
END
GO

CREATE PROCEDURE [PR_AddPayment](
	@IdCard NUMERIC(20),
	@IdCheck INT,
	@TotalPay SMALLMONEY
)AS BEGIN
	UPDATE [PaymentByCustomer] SET TotalPrice = @TotalPay WHERE IdCard = @IdCard AND IdCheck = @IdCheck
	UPDATE [Check] SET Balance += @TotalPay WHERE IdCheck = @IdCheck
	EXEC [PR_BalanceAccounts]
END
GO

CREATE PROCEDURE [PR_DeleteDishTypes](
	@DishId INT
)AS BEGIN
	DELETE FROM [TypesPerDish] WHERE IdDish = @DishId
END
GO

REATE PROCEDURE [PR_UpdateDishTypes](
	@IdDish INT,
	@TypeList VARCHAR(MAX)
)

AS BEGIN

	DECLARE @TypesId TABLE([IdType] INT)
	INSERT INTO @TypesId ([IdType])
			(	SELECT TR.IdType
				FROM [TypesPerDish] TR
				WHERE TR.IdRestaurant = @IdDish
			)

	EXEC [PR_DeleteDishTypes] @IdDish
	IF @TypeList IS NOT NULL
	BEGIN
		INSERT INTO [TypesPerDish]
			SELECT @IdDish, VALUE FROM string_split(@TypeList,',')
	END

	DELETE T FROM [Type] T
	INNER JOIN (SELECT T2.IdType
				FROM [Type] T2
					LEFT JOIN [TypesPerDish] TR ON TR.IdType = T2.IdType
				WHERE T2.IdType IN (SELECT IdType FROM @TypesId)
				GROUP BY T2.IdType
				HAVING COUNT(TR.IdDish) = 0
	) NoType ON NoType.IdType = T.IdType
END
GO

CREATE PROCEDURE [PR_LocalReport](
	@idRestaurant INT,
	@date1 DATETIME,
	@date2 DATETIME
)AS BEGIN
	SELECT L.IdLocal , Count(C.IdCheck) as Sales
	FROM [Local] L
		RIGHT JOIN [Check] C ON L.IdLocal = C.IdLocal
	WHERE C.[State] = 'Paid' AND L.IdRestaurant = @idRestaurant AND C.[Date] BETWEEN @date1 AND @date2
	GROUP BY L.IdLocal
END
GO

CREATE PROCEDURE [PR_BestSalesDaysReport](
	@idRestaurant INT,
	@date1 DATETIME,
	@date2 DATETIME,
	@top INT
)AS BEGIN
	SELECT TOP(@top)
		C.IdLocal [LocalId], CONVERT(DATE, C.[Date]) [Day], SUM(TotalCheck.Total) [Total]
	FROM [Check] C
	INNER JOIN (
			SELECT C.IdCheck [IdC], SUM(PBC.TotalPrice) [Total]
			FROM [PaymentByCustomer] PBC
				INNER JOIN [Check] C ON C.IdCheck = PBC.IdCheck
				INNER JOIN [Local] L ON L.IdLocal = C.IdLocal
			WHERE C.[State] = 'Paid' AND L.IdRestaurant = @idRestaurant AND C.[Date] BETWEEN @date1 AND @date2
			GROUP BY C.IdCheck) TotalCheck ON TotalCheck.IdC = C.IdCheck
	GROUP BY CONVERT(DATE, C.[Date]), C.IdLocal
	ORDER BY SUM(TotalCheck.Total) DESC
END
GO

CREATE PROCEDURE [PR_DishSalesReport](
	@IdLocal INT,
	@DateStart DATETIME,
	@DateEnd DATETIME
)AS BEGIN
	SELECT 
		D.IdDish ID , 
		ISNULL(SUM(DBC.Quantity),0) AS [Quantity] , 
		SUM((DBC.UnitaryPrice + (DBC.UnitaryPrice * DBC.SellTax / 100) + (DBC.UnitaryPrice * DBC.ServiceTax / 100)) * DBC.Quantity) AS [Total]
	FROM [Dish] D
		INNER JOIN [DishesByCheck] DBC ON D.IdDish = DBC.IdDish
		INNER JOIN [Check] C ON DBC.IdCheck = C.IdCheck AND C.[State] = 'Paid'
	WHERE C.IdLocal = @IdLocal AND C.[Date] BETWEEN @DateStart AND @DateEnd
	GROUP BY D.IdDish
END
GO

CREATE PROCEDURE [PR_LocalRestaurantInfo] (
	@AminId NUMERIC(20)
)
AS BEGIN
	SELECT 
		L.IdLocal,
		R.[Name] + ' - Local #' + CONVERT(VARCHAR,L.[Name]) AS [Name]
	FROM [Restaurant] R
		INNER JOIN [Local] L ON L.IdRestaurant = R.IdRestaurant
	WHERE R.IdAdmin = @AminId
	ORDER BY R.[Name] ASC
END	
GO

CREATE PROCEDURE [PR_SalesAge](
	@LocalId INT
)AS BEGIN
	SELECT 
		Ages.Gender, 
		Ages.AgeRange,
		COUNT(PPC.IdCheck) AS [Total Sales]
	FROM (SELECT U.Gender, U.IdCard [CardId],
						CASE  
							WHEN DATEDIFF(yy,U.BirthDate, GETDATE()) between 0 and 9 then '0 - 9'
							WHEN DATEDIFF(yy,U.BirthDate, GETDATE()) between 10 and 19 then '10 - 19'
							WHEN DATEDIFF(yy,U.BirthDate, GETDATE()) between 20 and 29 then '20 - 29'
							WHEN DATEDIFF(yy,U.BirthDate, GETDATE()) between 30 and 39 then '30 - 39'
							WHEN DATEDIFF(yy,U.BirthDate, GETDATE()) between 40 and 49 then '40 - 49'
							WHEN DATEDIFF(yy,U.BirthDate, GETDATE()) between 50 and 59 then '50 - 59'
							WHEN DATEDIFF(yy,U.BirthDate, GETDATE()) between 60 and 69 then '60 - 69'
							WHEN DATEDIFF(yy,U.BirthDate, GETDATE()) between 70 and 79 then '70 - 79'
							WHEN DATEDIFF(yy,U.BirthDate, GETDATE()) between 80 and 89 then '80 - 89'
							WHEN DATEDIFF(yy,U.BirthDate, GETDATE()) between 90 and 99 then '90 - 99'
							ELSE '100+' END AS AgeRange
				FROM [User] U) Ages 
		INNER JOIN [PaymentByCustomer] PPC ON Ages.CardId = PPC.IdCard
		INNER JOIN [Check] CH ON PPC.IdCheck = CH.IdCheck AND CH.[State] = 'Paid'
	WHERE CH.IdLocal = @LocalId
	GROUP by Ages.Gender, Ages.AgeRange
END