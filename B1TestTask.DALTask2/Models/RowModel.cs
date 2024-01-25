﻿namespace B1TestTask.DALTask2.Models
{
    public class RowModel
    {
        public int RowId { get; set; }
        public RowContent Content { get; set; } = new RowContent();
        public int FileId { get; set; }
        public int AccountId { get; set; }
        public FileModel? File { get; set; } = null;
        public AccountModel? Account { get; set; } = null;
    }

}