USE Tabemashou
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
	EXEC @UserError = CreateUser
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

CREATE PROCEDURE [PR_UpdateDishTypes](
	@IdDish INT,
	@TypeList VARCHAR(MAX)
)

AS BEGIN

	DECLARE @TypesId TABLE([IdType] INT)
	INSERT INTO @TypesId ([IdType])
			(	SELECT TR.IdType
				FROM [TypesPerDish] TR
				WHERE TR.IdDish = @IdDish
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

CREATE PROCEDURE [PR_DeleteDishTypes](
	@DishId INT
)AS BEGIN
	DELETE FROM [TypesPerDish] WHERE IdDish = @DishId
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



CREATE PROCEDURE [PR_DishTypes]
AS BEGIN
	SELECT T.* FROM [Type] T INNER JOIN [TypesPerDish] TPD ON T.IdType = TPD.IdType
END
GO

CREATE PROCEDURE [PR_RestaurantTypes]
AS BEGIN
	SELECT T.* FROM [Type] T INNER JOIN [TypesPerRestaurant] TPR ON T.IdType = TPR.IdType
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
	SELECT C.* FROM [Check] C
		INNER JOIN [PaymentByCustomer] PC ON PC.IdCheck = C.IdCheck AND PC.IdCard = @IdCard	
	ORDER BY [Date] DESC
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
		INNER JOIN [Dish] D ON D.IdDish = TPD.IdDish AND D.IdRestaurant = 9
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