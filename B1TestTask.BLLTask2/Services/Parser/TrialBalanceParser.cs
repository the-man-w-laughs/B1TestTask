using B1TestTask.BLLTask2.Contracts;
using B1TestTask.BLLTask2.Dtos;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace B1TestTask.BLLTask2.Services.Parser
{
    // Класс парсера ОСВ
    public class TrialBalanceParser: ITrialBalanceParser
    {
        private bool[,] _stateArray;
        private bool[] _isFinalStateArray;

        public TrialBalanceParser()
        {
            _stateArray = SetUpStateArray();
            _isFinalStateArray = SetUpIsFinalStateArray();
        }

        // Метод для парсинга ОСВ и возвращения DTO с содержимым файла
        public FileContentDto ParseTrialBalance(string filePath)
        {            
            var fileContent = new FileContentDto();
            
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {                
                HSSFWorkbook workbook = new HSSFWorkbook(file);

                // Получение первого листа из книги
                ISheet worksheet = workbook.GetSheetAt(0);

                // Извлечение информации о файле
                ExtractFileInfo(worksheet, fileContent);

                // Извлечение данных из столбцов
                ExtractColumnData(worksheet, fileContent);
            }
            
            return fileContent;
        }

        // Метод для извлечения информации о файле из листа Excel
        private void ExtractFileInfo(ISheet worksheet, FileContentDto fileContent)
        {
            // Чтение наименования банка из ячейки A1
            ICell bankNameCell = worksheet.GetRow(0).GetCell(0);
            fileContent.BankName = GetNonEmptyStringValue(bankNameCell, "BankName");

            // Чтение наименования файла из ячейки A2
            ICell fileTitleCell = worksheet.GetRow(1).GetCell(0);
            fileContent.FileTitle = GetNonEmptyStringValue(fileTitleCell, "FileTitle");

            // Чтение периода из ячейки A3
            ICell periodCell = worksheet.GetRow(2).GetCell(0);
            fileContent.Period = GetNonEmptyStringValue(periodCell, "Period");

            // Чтение дополнительной информации из ячейки A4
            ICell additionalInfoCell = worksheet.GetRow(3).GetCell(0);
            fileContent.AdditionalInfo = GetNonEmptyStringValue(additionalInfoCell, "AdditionalInfo");

            // Чтение даты генерации из ячейки A6
            ICell generationDateCell = worksheet.GetRow(5).GetCell(0);
            fileContent.GenerationDate = GetDateValueFromNumericCell(generationDateCell, "GenerationDate");

            // Чтение валюты из ячейки G6
            ICell currencyCell = worksheet.GetRow(5).GetCell(6);
            fileContent.Currency = GetNonEmptyStringValue(currencyCell, "Currency");
        }

        // Метод для получения непустого значения строки из ячейки Excel
        private string GetNonEmptyStringValue(ICell cell, string propertyName)
        {
            if (cell != null)
            {
                if (cell.CellType == CellType.String)
                {
                    if (!string.IsNullOrEmpty(cell.StringCellValue))
                    {
                        return cell.StringCellValue;
                    }
                }
                else if (cell.CellType == CellType.Numeric)
                {
                    return cell.NumericCellValue.ToString();
                }

                // В случае, если значение ячейки не является строкой или числом, выбрасывается исключение
                throw new Exception($"{propertyName} is null or empty.");
            }

            // В случае, если ячейка равна null, выбрасывается исключение
            throw new Exception($"{propertyName} is null or empty.");
        }


        // Метод для получения числового значения из числовой ячейки Excel
        private double GetNumericValueFromNumericCell(ICell cell, string propertyName)
        {
            // Проверка наличия ячейки и ее типа (числовой)
            if (cell != null && cell.CellType == CellType.Numeric)
            {
                // Возвращение числового значения из ячейки
                return cell.NumericCellValue;
            }
            else
            {                
                throw new Exception($"{propertyName} is null or does not have a numeric value.");
            }
        }


        // Метод для получения значения даты из числовой ячейки Excel
        private DateTime GetDateValueFromNumericCell(ICell cell, string propertyName)
        {
            // Проверка наличия ячейки, ее типа (числовой) и форматирования как даты
            if (cell == null || cell.CellType != CellType.Numeric || !DateUtil.IsCellDateFormatted(cell))
            {                
                throw new Exception($"{propertyName} is not a valid date.");
            }
            
            return cell.DateCellValue;
        }


        // Метод для извлечения данных из столбцов Excel
        private void ExtractColumnData(ISheet worksheet, FileContentDto fileContent)
        {
            // Установка начальной строки для чтения данных (A9)
            int startRow = 8;
            // Получение номера последней строки в листе
            int lastRow = worksheet.LastRowNum;

            // Список для хранения классов
            var classes = new List<ClassModelDto>();
            // Текущее состояние при парсинге
            var currentState = State.Start;
            // Текущий класс, который обрабатывается
            ClassModelDto currentClass = null;
            // Текущая модель группы счетов
            AccountGroupModelDto currentAccountGroupModel = new AccountGroupModelDto();

            // Итерация по строкам начиная с указанной и до последней строки
            for (int rowIdx = startRow; rowIdx <= lastRow && !_isFinalStateArray[(int)currentState]; rowIdx++)
            {
                // Получение текущей строки
                IRow row = worksheet.GetRow(rowIdx);
                if (row != null)
                {
                    // Получение ячейки в столбце A
                    ICell cellA = row.GetCell(0);

                    // Определение следующего состояния на основе содержимого ячейки A
                    var nextState = GetCurrentState(cellA);

                    // Проверка корректности перехода между состояниями
                    if (!_stateArray[(int)currentState, (int)nextState])
                    {
                        throw new Exception("Error occurred during file parsing.");
                    }

                    // Обновление текущего состояния
                    currentState = nextState;

                    // Обработка текущего состояния
                    switch (currentState)
                    {
                        case State.Class:
                            // Создание нового класса и добавление его в список
                            currentClass = new ClassModelDto
                            {
                                ClassName = GetNonEmptyStringValue(row.GetCell(0), "ClassName")
                            };
                            classes.Add(currentClass);
                            break;

                        case State.Group:
                            // Обновление названия текущей группы счетов и добавление ее в текущий класс
                            currentAccountGroupModel.GroupName = GetNonEmptyStringValue(row.GetCell(0), "GroupName");
                            currentClass!.AccountGroups.Add(currentAccountGroupModel);
                            // Создание новой модели группы счетов
                            currentAccountGroupModel = new AccountGroupModelDto();
                            break;

                        case State.Data:
                            // Создание новой строки данных и добавление ее в текущую группу счетов
                            var data = new RowContentDto
                            {
                                AccountNumber = GetNonEmptyStringValue(row.GetCell(0), "AccountId"),
                                IncomingActive = (decimal)GetNumericValueFromNumericCell(row.GetCell(1), "IncomingActive"),
                                IncomingPassive = (decimal)GetNumericValueFromNumericCell(row.GetCell(2), "IncomingPassive"),
                                TurnoverDebit = (decimal)GetNumericValueFromNumericCell(row.GetCell(3), "TurnoverDebit"),
                                TurnoverCredit = (decimal)GetNumericValueFromNumericCell(row.GetCell(4), "TurnoverCredit"),
                                OutgoingActive = (decimal)GetNumericValueFromNumericCell(row.GetCell(5), "OutgoingActive"),
                                OutgoingPassive = (decimal)GetNumericValueFromNumericCell(row.GetCell(6), "OutgoingPassive")
                            };
                            currentAccountGroupModel!.Rows.Add(data);
                            break;
                    }
                }
            }

            // Проверка корректности завершающего состояния
            if (!_isFinalStateArray[(int)currentState])
            {
                throw new Exception("Error occurred during file parsing.");
            }

            // Обновление списка классов в DTO содержимого файла
            fileContent.ClassList = classes;
        }


        // Метод для определения текущего состояния на основе содержимого ячейки
        private State GetCurrentState(ICell cell)
        {
            // Проверка наличия ячейки
            if (cell != null)
            {
                // Получение значения из ячейки
                string cellValue = GetNonEmptyStringValue(cell, "A1");

                // Определение состояния на основе содержимого ячейки
                if (cellValue.StartsWith("КЛАСС", StringComparison.OrdinalIgnoreCase))
                {
                    return State.Class;
                }
                else if (cellValue.StartsWith("БАЛАНС", StringComparison.OrdinalIgnoreCase))
                {
                    return State.Balance;
                }
                else if (cellValue.Equals("ПО КЛАССУ", StringComparison.OrdinalIgnoreCase))
                {
                    return State.EndOfClass;
                }
                else if (cellValue.Length == 2)
                {
                    return State.Group;
                }
                else
                {
                    return State.Data;
                }
            }

            // В случае, если ячейка равна null, возвращается состояние ошибки
            return State.Error;
        }


        // Метод для инициализации двумерного массива состояний
        private bool[,] SetUpStateArray()
        {
            var stateArray = new bool[Enum.GetValues(typeof(State)).Length, Enum.GetValues(typeof(State)).Length];

            // Инициализация массива значениями false
            for (int i = 0; i < stateArray.GetLength(0); i++)
            {
                for (int j = 0; j < stateArray.GetLength(1); j++)
                {
                    stateArray[i, j] = false;
                }
            }

            // Установка значений для переходов между состояниями
            stateArray[(int)State.Start, (int)State.Class] = true;
            stateArray[(int)State.Class, (int)State.Data] = true;
            stateArray[(int)State.Data, (int)State.Data] = true;
            stateArray[(int)State.Data, (int)State.Group] = true;
            stateArray[(int)State.Group, (int)State.EndOfClass] = true;
            stateArray[(int)State.Group, (int)State.Data] = true;
            stateArray[(int)State.EndOfClass, (int)State.Class] = true;
            stateArray[(int)State.EndOfClass, (int)State.Balance] = true;

            return stateArray;
        }

        // Метод для инициализации массива флагов, указывающих, является ли состояние конечным
        private bool[] SetUpIsFinalStateArray()
        {
            var isFinalStateArray = new bool[Enum.GetValues(typeof(State)).Length];

            // Инициализация массива значениями false
            for (int i = 0; i < isFinalStateArray.Length; i++)
            {
                isFinalStateArray[i] = false;
            }

            // Установка значения true для конечного состояния Balance
            isFinalStateArray[(int)State.Balance] = true;

            return isFinalStateArray;
        }


    }
}
