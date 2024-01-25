CREATE DEFINER=`root`@`localhost` PROCEDURE `CalculateSumAndMedian`()
BEGIN    
    DECLARE SumOutput DECIMAL(18, 0);
    DECLARE MedianOutput DECIMAL(18, 8);

    SET SumOutput = (SELECT SUM(RandomEvenInteger) FROM b1testtask1db.generateddatamodels);
    
SET MedianOutput = (
    SELECT AVG(RandomDecimal) AS Median
    FROM (
        SELECT RandomDecimal,
            ROW_NUMBER() OVER (ORDER BY RandomDecimal) AS RowAsc,
            COUNT(*) OVER () AS TotalRows
        FROM b1testtask1db.generateddatamodels
    ) data
    WHERE
        (TotalRows % 2 = 1 AND RowAsc = (TotalRows + 1) / 2) 
        OR
        (TotalRows % 2 = 0 AND RowAsc IN (TotalRows / 2, (TotalRows / 2) + 1)) 
);
    
   INSERT INTO tempOutputTable (Id, SumOfIntegers, MedianOfDecimals)
    VALUES (1, SumOutput, MedianOutput) AS alias
    ON DUPLICATE KEY UPDATE
    SumOfIntegers = alias.SumOfIntegers,
    MedianOfDecimals = alias.MedianOfDecimals;
END