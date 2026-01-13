using System;

namespace TechVeo.Shared.Domain.Dto;

public class PagingRequest<TSort> : PagingRequest where TSort : struct, IConvertible
{
    public TSort Sort { get; set; }
}
