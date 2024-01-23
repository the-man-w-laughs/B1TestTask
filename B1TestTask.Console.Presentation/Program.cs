using BLL.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;
var parser = new TrialBalanceParser();

var path = @"C:\Users\nazar\Downloads\ОСВ для тренинга.xls";
var fileInfo = parser.ParseTrialBalance(path);
Console.WriteLine(fileInfo);