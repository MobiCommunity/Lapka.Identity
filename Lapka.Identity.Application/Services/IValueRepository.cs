using System;
using System.Threading.Tasks;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Application.Services
{
    public interface IValueRepository
    {
        Task AddValue(Value value);
        Task<Value> GetById(Guid id);
    }
}