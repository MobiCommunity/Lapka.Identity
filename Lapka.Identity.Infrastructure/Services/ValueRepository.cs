using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lapka.Identity.Application.Dto;
using Lapka.Identity.Application.Exceptions;
using Lapka.Identity.Application.Services;
using Lapka.Identity.Core.Entities;

namespace Lapka.Identity.Infrastructure.Services
{
    public class ValueRepository : IValueRepository
    {
        private readonly IList<Value> _values;

        public ValueRepository()
        {
            _values = new List<Value>();
        }

#pragma warning disable 1998
        public async Task AddValue(Value value)
        {
            _values.Add(value);
        }

        public async Task<ValueDto> GetById(Guid id)
        {
            var entity = _values.FirstOrDefault(x => x.Id.Value == id);
            if (entity is null) throw new ValueNotFoundException();
            return new ValueDto { Id = entity.Id.Value, Description = entity.Description, Name = entity.Name };
        }
    }
}