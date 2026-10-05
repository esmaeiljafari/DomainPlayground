using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Infrastructure.Identity.Models
{
    public sealed class RefreshToken
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string TokenHash { get; private set; } = default!;
        public DateTime ExpiresAtUtc { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? RevokedAtUtc { get; private set; }
        public Guid? ReplacedByTokenId { get; private set; }

        public bool IsActive => RevokedAtUtc is null && DateTime.UtcNow < ExpiresAtUtc;

        private RefreshToken() { }   // EF

        public static RefreshToken Create(Guid userId, string tokenHash, DateTime expiresAtUtc) => new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAtUtc = expiresAtUtc,
            CreatedAtUtc = DateTime.UtcNow
        };

        public void Revoke(Guid? replacedByTokenId = null)
        {
            RevokedAtUtc = DateTime.UtcNow;
            ReplacedByTokenId = replacedByTokenId;
        }
    }
}
