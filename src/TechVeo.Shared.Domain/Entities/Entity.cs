using System;
using System.Collections.Generic;
using System.Linq;
using TechVeo.Shared.Domain.Events;

namespace TechVeo.Shared.Domain.Entities;

public class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public bool IsDeleted { get; set; }

    protected readonly List<IDomainEvent> _events = [];

    public List<IDomainEvent> PopEvents()
    {
        var copy = _events.ToList();

        _events.Clear();

        return copy;
    }
}
