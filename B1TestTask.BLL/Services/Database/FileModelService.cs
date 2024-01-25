using AutoMapper;
using B1TestTask.BLLTask2.Contracts;
using B1TestTask.BLLTask2.Dtos;
using B1TestTask.DALTask2.Contracts;
using B1TestTask.DALTask2.Models;
using NPOI.SS.Formula.Functions;
using System.Security.Principal;

namespace B1TestTask.BLLTask2.Services.Database
{
    public class FileModelService : IFileModelService
    {
        private readonly IFileModelRepository _fileModelRepository;
        private readonly IClassModelRepository _classModelRepository;
        private readonly IAccountGroupModelRepository _accountGroupModelRepository;
        private readonly IAccountModelRepository _accountModelRepository;
        private readonly IMapper _mapper;

        public FileModelService(IFileModelRepository fileRepository,
            IClassModelRepository classModelRepository,
            IAccountGroupModelRepository accountGroupModelRepository, 
            IAccountModelRepository accountModelRepository,
            IMapper mapper)
        {
            _fileModelRepository = fileRepository;
            _classModelRepository = classModelRepository;
            _accountGroupModelRepository = accountGroupModelRepository;
            _accountModelRepository = accountModelRepository;
            _mapper = mapper;
        }

        public async Task AddFile(FileModelDto fileModelDto)
        {
            var newFileModel = _mapper.Map<FileModelDto, FileModel>(fileModelDto);

            var existingFile = await _fileModelRepository.AddAsync(newFileModel);
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
                            Content = _mapper.Map<RowContent>(rowContentDto)
                        };                        

                        existingFile.Rows.Add(newRowModel);
                    }                    
                }
            }

            await _fileModelRepository.SaveAsync();
        }

        public async Task<List<FileModelDto>> GetAllFiles()
        {
            var fileModelList = await _fileModelRepository.GetAllAsync();

            var fileModelDtoList = new List<FileModelDto>();

            foreach (var fileModel in fileModelList)
            {
                var classes = await _fileModelRepository.GetAllClassesFromFile(fileModel.FileId);

                var fileModelDto = new FileModelDto()
                {
                    FileName = fileModel.FileName,
                    FileContent = _mapper.Map<FileContentDto>(fileModel.FileContent)
                };

                foreach (var @class in classes)
                {
                    var classModelDto = new ClassModelDto { ClassName = @class.ClassName };

                    foreach (var group in @class.AccountGroups)
                    {
                        var accountGroupModelDto = new AccountGroupModelDto { GroupName = group.GroupName };

                        foreach (var account in group.Accounts)
                        {
                            var rowContentDto = _mapper.Map<RowContentDto>(account.Rows.First().Content);
                            rowContentDto.AccountNumber = account.AccountNumber;
                            accountGroupModelDto.Rows.Add(rowContentDto);
                        }

                        classModelDto.AccountGroups.Add(accountGroupModelDto);
                    }

                    fileModelDto.FileContent.ClassList.Add(classModelDto);
                }

                fileModelDtoList.Add(fileModelDto);
            }

            return fileModelDtoList;
        }

    }
}
