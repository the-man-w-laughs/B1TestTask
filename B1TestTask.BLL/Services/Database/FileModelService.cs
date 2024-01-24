using B1TestTask.BLLTask2.Contracts;
using B1TestTask.BLLTask2.Dtos;
using B1TestTask.DALTask2.Contracts;
using B1TestTask.DALTask2.Models;

namespace B1TestTask.BLLTask2.Services.Database
{
    public class FileModelService : IFileModelService
    {
        private readonly IFileModelRepository _fileModelRepository;
        private readonly IClassModelRepository _classModelRepository;
        private readonly IAccountGroupModelRepository _accountGroupModelRepository;
        private readonly IAccountModelRepository _accountModelRepository;

        public FileModelService(IFileModelRepository fileRepository,
            IClassModelRepository classModelRepository,
            IAccountGroupModelRepository accountGroupModelRepository, 
            IAccountModelRepository accountModelRepository)
        {
            _fileModelRepository = fileRepository;
            _classModelRepository = classModelRepository;
            _accountGroupModelRepository = accountGroupModelRepository;
            _accountModelRepository = accountModelRepository;
        }

        public async Task AddFile(FileModelDto fileModelDto)
        {
            var newFIleModel = new FileModel();
            newFIleModel.FileName = fileModelDto.FileName;
            newFIleModel.FileContent.AdditionalInfo = fileModelDto.FileContent.AdditionalInfo;
            newFIleModel.FileContent.BankName = fileModelDto.FileContent.BankName;
            newFIleModel.FileContent.FileTitle = fileModelDto.FileContent.FileTitle;
            newFIleModel.FileContent.Currency = fileModelDto.FileContent.Currency;
            newFIleModel.FileContent.Period = fileModelDto.FileContent.Period;
            newFIleModel.FileContent.GenerationDate = fileModelDto.FileContent.GenerationDate;
            
            var existingFile = await _fileModelRepository.AddAsync(newFIleModel);
            await _fileModelRepository.SaveAsync();

            foreach (var classModelDto in fileModelDto.FileContent.ClassList)
            {
                var existingClass = await _classModelRepository.GetAsync(classModel => classModel.ClassName == classModelDto.ClassName);

                if (existingClass == null)
                {
                    var newClassModel = new ClassModel() { ClassName = classModelDto.ClassName };
                    existingClass = await _classModelRepository.AddAsync(newClassModel);
                    await _fileModelRepository.SaveAsync();
                }

                foreach (var accountGroupModelDto in classModelDto.AccountGroups)
                {
                    var existingGroupModel = await _accountGroupModelRepository.GetAsync(
                        accountGroupModel => existingClass.ClassModelId == accountGroupModel.ClassModelId &&
                        accountGroupModel.GroupName == accountGroupModelDto.GroupName);

                    if (existingGroupModel == null)
                    {
                         var newGroupModel = new AccountGroupModel() { 
                            GroupName = accountGroupModelDto.GroupName, 
                            ClassModelId = existingClass.ClassModelId,                            
                        };

                        existingGroupModel = await _accountGroupModelRepository.AddAsync(newGroupModel);
                        await _fileModelRepository.SaveAsync();
                    }
                    
                    foreach (var rowContentDto in accountGroupModelDto.Rows)
                    {
                        var existingAccountModel = await _accountModelRepository.GetAsync(
                            accountModel => accountModel.AccountGroupId == existingGroupModel.AccountGroupId &&
                            accountModel.AccountNumber == rowContentDto.AccountNumber);

                        if (existingAccountModel == null)
                        {
                            var newAccountModel = new AccountModel()
                            {
                                AccountNumber = rowContentDto.AccountNumber,
                                AccountGroupId = existingGroupModel.AccountGroupId
                            };

                            existingAccountModel = await _accountModelRepository.AddAsync(newAccountModel);
                            await _fileModelRepository.SaveAsync();
                        }

                        var newRowModel = new RowModel()
                        {                            
                            AccountId = existingAccountModel.AccountId,
                        };

                        newRowModel.Content.IncomingActive = rowContentDto.IncomingActive; 
                        newRowModel.Content.IncomingPassive = rowContentDto.IncomingPassive;
                        newRowModel.Content.TurnoverDebit = rowContentDto.TurnoverDebit;
                        newRowModel.Content.TurnoverCredit = rowContentDto.TurnoverCredit;
                        newRowModel.Content.OutgoingActive = rowContentDto.OutgoingActive;
                        newRowModel.Content.OutgoingPassive = rowContentDto.OutgoingPassive;

                        existingFile.Rows.Add(newRowModel);
                    }                    
                }
            }

            await _fileModelRepository.SaveAsync();
        }
    }
}
