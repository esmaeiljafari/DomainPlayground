using DomainPlayground.Core.Application.Features.Identity.Models;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Identity.Interfaces;

public interface ITokenService
{
    Task<TokenPair> IssueAsync(UserId userId, CancellationToken ct = default);

    Task<Result<TokenPair>> RefreshAsync(string refreshToken, CancellationToken ct = default);

    Task<Result> RevokeAsync(string refreshToken, CancellationToken ct = default);

    Task RevokeAllAsync(UserId userId, CancellationToken ct = default);
}
