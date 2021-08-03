using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lapka.Identity.Application.Dto;

namespace Lapka.Identity.Application.Services
{
    public interface IValueQueryService
    {
        Task<ValueDto> GetValueById(Guid id);
        Task<IEnumerable<ValueDto>> GetAllValues();
    }
}