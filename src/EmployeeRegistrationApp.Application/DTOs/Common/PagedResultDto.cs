// src/EmployeeRegistrationApp.Application/DTOs/Common/PagedResultDto.cs
using System;
using System.Collections.Generic;

namespace EmployeeRegistrationApp.Application.DTOs.Common
{
    /// <summary>
    /// DTO genérico para respostas paginadas na camada de aplicação.
    /// </summary>
    public sealed class PagedResultDto<T>
    {
        // ✅ nome “oficial”
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages =>
            PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);

        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        // ✅ compat: muitos lugares usam "Page"
        public int Page
        {
            get => PageNumber;
            set => PageNumber = value;
        }

        public PagedResultDto() { }

        public PagedResultDto(int pageNumber, int pageSize, int totalCount, IReadOnlyList<T> items)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = items ?? Array.Empty<T>();
        }
    }
}
