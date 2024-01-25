﻿using B1TestTask.DALTask2.Models;
using B1TestTask.Shared.Repository;

namespace B1TestTask.DALTask2.Contracts
{
    public interface IFileModelRepository : IBaseRepository<FileModel>
    {
        Task<List<ClassModel>> GetAllClassesFromFile(int fileId);
    }
}