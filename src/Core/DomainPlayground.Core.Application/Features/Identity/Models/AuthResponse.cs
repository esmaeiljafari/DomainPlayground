using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Identity.Models
{
    public sealed record AuthResponse(
        Guid UserId,
        string AccessToken,
        DateTime AccessTokenExpiresAtUtc,
        string RefreshToken,
        DateTime RefreshTokenExpiresAtUtc);
}
