CREATE PROCEDURE sp_CountryStateDistrict
    @Action VARCHAR(10),  -- Insert / Select / Update / Delete

    @CountryId INT = NULL,
    @StateId INT = NULL,
    @DistrictId INT = NULL,

    @CountryName VARCHAR(100) = NULL,
    @StateName VARCHAR(100) = NULL,
    @DistrictName VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @NewCountryId INT;
        DECLARE @NewStateId INT;

        -- INSERT (with duplicate check)
        IF @Action = 'INSERT'
        BEGIN
            -- Country check
            IF EXISTS (SELECT 1 FROM Country WHERE CountryName = @CountryName)
                SELECT @NewCountryId = Id FROM Country WHERE CountryName = @CountryName;
            ELSE
            BEGIN
                INSERT INTO Country (CountryName, CreatedDateTime)
                VALUES (@CountryName, SYSDATETIME());
                SET @NewCountryId = SCOPE_IDENTITY();
            END

            -- State check
            IF EXISTS (SELECT 1 FROM State WHERE StateName = @StateName AND CountryId = @NewCountryId)
                SELECT @NewStateId = Id FROM State WHERE StateName = @StateName AND CountryId = @NewCountryId;
            ELSE
            BEGIN
                INSERT INTO State (StateName, CountryId, CreatedDateTime)
                VALUES (@StateName, @NewCountryId, SYSDATETIME());
                SET @NewStateId = SCOPE_IDENTITY();
            END

            -- District insert
            INSERT INTO District (DistrictName, StateId, CreatedDateTime)
            VALUES (@DistrictName, @NewStateId, SYSDATETIME());
        END

        -- SELECT (READ)
        ELSE IF @Action = 'SELECT'
        BEGIN
            SELECT 
                c.Id AS CountryId, c.CountryName,
                s.Id AS StateId, s.StateName,
                d.Id AS DistrictId, d.DistrictName
            FROM Country c
            LEFT JOIN State s ON c.Id = s.CountryId
            LEFT JOIN District d ON s.Id = d.StateId;
        END
        
        -- UPDATE
        ELSE IF @Action = 'UPDATE'
        BEGIN
            IF @CountryId IS NOT NULL
                UPDATE Country 
                SET CountryName = @CountryName, ModifiedDateTime = SYSDATETIME()
                WHERE Id = @CountryId;

            IF @StateId IS NOT NULL
                UPDATE State 
                SET StateName = @StateName, ModifiedDateTime = SYSDATETIME() 
                WHERE Id = @StateId;

            IF @DistrictId IS NOT NULL
                UPDATE District 
                SET DistrictName = @DistrictName, ModifiedDateTime = SYSDATETIME() 
                WHERE Id = @DistrictId;
        END
        -- DELETE
        ELSE IF @Action = 'DELETE'
        BEGIN
            IF @DistrictId IS NOT NULL
                DELETE FROM District WHERE Id = @DistrictId;

            IF @StateId IS NOT NULL
            BEGIN
                DELETE FROM District WHERE StateId = @StateId;
                DELETE FROM State WHERE Id = @StateId;
            END

            IF @CountryId IS NOT NULL
            BEGIN
                DELETE FROM District 
                WHERE StateId IN (SELECT Id FROM State WHERE CountryId = @CountryId);

                DELETE FROM State WHERE CountryId = @CountryId;
                DELETE FROM Country WHERE Id = @CountryId;
            END
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
