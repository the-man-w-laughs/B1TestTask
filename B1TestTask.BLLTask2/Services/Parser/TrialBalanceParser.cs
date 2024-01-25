using B1TestTask.BLLTask2.Contracts;
using B1TestTask.BLLTask2.Dtos;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace B1TestTask.BLLTask2.Services.Parser
{
    public class TrialBalanceParser: ITrialBalanceParser
    {
        private bool[,] _stateArray;
        private bool[] _isFinalStateArray;

        public TrialBalanceParser()
        {
            _stateArray = SetUpStateArray();
            _isFinalStateArray = SetUpIsFinalStateArray();
        }

        public FileContentDto ParseTrialBalance(string filePath)
        {
            var fileContent = new FileContentDto();

            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(file);
                ISheet worksheet = workbook.GetSheetAt(0);

                ExtractFileInfo(worksheet, fileContent);
                ExtractColumnData(worksheet, fileContent);
            }

            return fileContent;
        }

        private void ExtractFileInfo(ISheet worksheet, FileContentDto fileContent)
        {
            // Read BankName from A1
            ICell bankNameCell = worksheet.GetRow(0).GetCell(0);
            fileContent.BankName = GetNonEmptyStringValue(bankNameCell, "BankName");

            // Read FileTitle from A2
            ICell fileTitleCell = worksheet.GetRow(1).GetCell(0);
            fileContent.FileTitle = GetNonEmptyStringValue(fileTitleCell, "FileTitle");

            // Read Period from A3
            ICell periodCell = worksheet.GetRow(2).GetCell(0);
            fileContent.Period = GetNonEmptyStringValue(periodCell, "Period");

            // Read AdditionalInfo from A4
            ICell additionalInfoCell = worksheet.GetRow(3).GetCell(0);
            fileContent.AdditionalInfo = GetNonEmptyStringValue(additionalInfoCell, "AdditionalInfo");

            // Read GenerationDate from A6
            ICell generationDateCell = worksheet.GetRow(5).GetCell(0);
            fileContent.GenerationDate = GetDateValueFromNumericCell(generationDateCell, "GenerationDate");

            // Read Currency from G6
            ICell currencyCell = worksheet.GetRow(5).GetCell(6);
            fileContent.Currency = GetNonEmptyStringValue(currencyCell, "Currency");
        }

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

                throw new Exception($"{propertyName} is null or empty.");
            }

            throw new Exception($"{propertyName} is null or empty.");
        }


        private double GetNumericValueFromNumericCell(ICell cell, string propertyName)
        {
            if (cell != null && cell.CellType == CellType.Numeric)
            {
                return cell.NumericCellValue;
            }
            else
            {
                throw new Exception($"{propertyName} is null or does not have a numeric value.");
            }
        }

        private DateTime GetDateValueFromNumericCell(ICell cell, string propertyName)
        {
            if (cell == null || cell.CellType != CellType.Numeric || !DateUtil.IsCellDateFormatted(cell))
            {
                throw new Exception($"{propertyName} is not a valid date.");
            }

            return cell.DateCellValue;
        }


        private void ExtractColumnData(ISheet worksheet, FileContentDto fileContent)
        {
            int startRow = 8; // A9
            int lastRow = worksheet.LastRowNum;

            var classes = new List<ClassModelDto>();
            var currentState = State.Start;
            ClassModelDto currentClass = null;
            AccountGroupModelDto currentAccountGroupModel = new AccountGroupModelDto();

            for (int rowIdx = startRow; rowIdx <= lastRow && !_isFinalStateArray[(int)currentState]; rowIdx++)
            {
                IRow row = worksheet.GetRow(rowIdx);
                if (row != null)
                {
                    ICell cellA = row.GetCell(0);

                    var nextState = GetCurrentState(cellA);

                    if (!_stateArray[(int)currentState, (int)nextState])
                    {
                        throw new Exception("Error occurred during file parsing.");
                    }

                    currentState = nextState;

                    switch (currentState)
                    {
                        case State.Class:
                            currentClass = new ClassModelDto
                            {
                                ClassName = GetNonEmptyStringValue(row.GetCell(0), "ClassName")
                            };
                            classes.Add(currentClass);
                            break;

                        case State.Group:
                            currentAccountGroupModel.GroupName = GetNonEmptyStringValue(row.GetCell(0), "GroupName");
                            currentClass!.AccountGroups.Add(currentAccountGroupModel);
                            currentAccountGroupModel = new AccountGroupModelDto();
                            break;

                        case State.Data:
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

            if (!_isFinalStateArray[(int)currentState])
            {
                throw new Exception("Error occurred during file parsing.");
            }

            fileContent.ClassList = classes;
        }

        private State GetCurrentState(ICell cell)
        {
            if (cell != null)
            {
                string cellValue = GetNonEmptyStringValue(cell, "A1");

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

            return State.Error;
        }


        private bool[,] SetUpStateArray()
        {
            var stateArray = new bool[Enum.GetValues(typeof(State)).Length, Enum.GetValues(typeof(State)).Length];

            for (int i = 0; i < stateArray.GetLength(0); i++)
            {
                for (int j = 0; j < stateArray.GetLength(1); j++)
                {
                    stateArray[i, j] = false;
                }
            }

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

        private bool[] SetUpIsFinalStateArray()
        {
            bool[] IsFinalStateArray = new bool[Enum.GetValues(typeof(State)).Length];

            for (int i = 0; i < IsFinalStateArray.Length; i++)
            {
                if (!IsFinalStateArray[i])
                {
                    IsFinalStateArray[i] = false;
                }
            }

            IsFinalStateArray[(int)State.Balance] = true;

            return IsFinalStateArray;
        }

    }
}
