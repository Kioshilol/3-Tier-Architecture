﻿
namespace TrainingTask.ViewModels
{
    public class PageViewModel
    {
        public int PageNumber { get; set; }
        public int TotalRecords { get; set; }
        public int RowsPerPage { get; set; }
        public int TotalPages { get; set; }
    }
}
