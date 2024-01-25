using AutoMapper;
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

        // Асинхронный метод для добавления файла в репозиторий
        public async Task AddFile(FileModelDto fileModelDto)
        {
            // Маппинг DTO файла в соответствующую модель
            var newFileModel = _mapper.Map<FileModelDto, FileModel>(fileModelDto);

            // Добавление новой модели файла в репозиторий
            var existingFile = await _fileModelRepository.AddAsync(newFileModel);
            await _fileModelRepository.SaveAsync();

            // Обход DTO моделей классов из содержимого файла
            foreach (var classModelDto in fileModelDto.FileContent.ClassList)
            {
                // Поиск существующей модели класса
                var existingClass = await _classModelRepository.GetAsync(classModel => classModel.ClassName == classModelDto.ClassName);

                // Если модель класса не существует, то создаем новую
                if (existingClass == null)
                {
                    var newClassModel = new ClassModel() { ClassName = classModelDto.ClassName };
                    existingClass = await _classModelRepository.AddAsync(newClassModel);
                    await _fileModelRepository.SaveAsync();
                }

                // Обход DTO моделей групп счетов
                foreach (var accountGroupModelDto in classModelDto.AccountGroups)
                {
                    // Поиск существующей модели группы счетов
                    var existingGroupModel = await _accountGroupModelRepository.GetAsync(
                        accountGroupModel => existingClass.ClassModelId == accountGroupModel.ClassModelId &&
                        accountGroupModel.GroupName == accountGroupModelDto.GroupName);

                    // Если модель группы счетов не существует, то создаем новую
                    if (existingGroupModel == null)
                    {
                        var newGroupModel = new AccountGroupModel()
                        {
                            GroupName = accountGroupModelDto.GroupName,
                            ClassModelId = existingClass.ClassModelId,
                        };

                        existingGroupModel = await _accountGroupModelRepository.AddAsync(newGroupModel);
                        await _fileModelRepository.SaveAsync();
                    }

                    // Обход DTO моделей строк в группе счетов
                    foreach (var rowContentDto in accountGroupModelDto.Rows)
                    {
                        // Поиск существующей модели счета
                        var existingAccountModel = await _accountModelRepository.GetAsync(
                            accountModel => accountModel.AccountGroupId == existingGroupModel.AccountGroupId &&
                            accountModel.AccountNumber == rowContentDto.AccountNumber);

                        // Если модель счета не существует, то создаем новую
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

                        // Создаем новую модель строки и добавляем в коллекцию существующей модели файла
                        var newRowModel = new RowModel()
                        {
                            AccountId = existingAccountModel.AccountId,
                            Content = _mapper.Map<RowContent>(rowContentDto)
                        };

                        existingFile.Rows.Add(newRowModel);
                    }
                }
            }

            // Сохраняем изменения в репозитории
            await _fileModelRepository.SaveAsync();
        }

        // Асинхронный метод для получения списка всех файлов в виде DTO
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
                            // Создаем DTO для содержимого строки счета
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
