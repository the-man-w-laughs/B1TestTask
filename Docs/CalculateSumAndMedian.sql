CREATE DEFINER=`root`@`localhost` PROCEDURE `CalculateSumAndMedian`()
BEGIN    
    DECLARE SumOutput DECIMAL(18, 0);
    DECLARE MedianOutput DECIMAL(18, 8);

    SET SESSION group_concat_max_len = 1000000;
    
    SET SumOutput = (SELECT SUM(RandomEvenInteger) FROM b1testtask1db.generateddatamodels);
    
    SET MedianOutput = (
        SELECT
            CASE
                WHEN COUNT(RandomDecimal) % 2 = 1 THEN
                    CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(GROUP_CONCAT(RandomDecimal ORDER BY RandomDecimal), ',', (COUNT(RandomDecimal) + 1) / 2), ',', -1) AS DECIMAL(18, 8))
                ELSE
                    CAST((SUBSTRING_INDEX(SUBSTRING_INDEX(GROUP_CONCAT(RandomDecimal ORDER BY RandomDecimal), ',', COUNT(RandomDecimal) / 2), ',', -1) +
                          SUBSTRING_INDEX(SUBSTRING_INDEX(GROUP_CONCAT(RandomDecimal ORDER BY RandomDecimal), ',', -COUNT(RandomDecimal) / 2), ',', -1)) / 2 AS DECIMAL(18, 8))
            END
        FROM b1testtask1db.generateddatamodels
    );
    
    SET SESSION group_concat_max_len = 1024;
    
   INSERT INTO tempOutputTable (Id, SumOfIntegers, MedianOfDecimals)
    VALUES (1, SumOutput, MedianOutput) AS alias
    ON DUPLICATE KEY UPDATE
    SumOfIntegers = alias.SumOfIntegers,
    MedianOfDecimals = alias.MedianOfDecimals;
END