using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Common.Models;

public abstract record PagedQuery
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;

    public int Skip => (Math.Max(PageNumber, 1) - 1) * NormalizedPageSize;
    public int Take => NormalizedPageSize;

    private int NormalizedPageSize => Math.Clamp(PageSize, 1, 100);
}
