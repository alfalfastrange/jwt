using System;
using Jwt.Common.Base;

namespace Jwt.Entity.Entities
{
    public class Session : BaseEntity
    {
        public Session() { }

        public Session(long clientId, long profileId, string token, DateTime expirationDate)
        {
            ClientId = clientId;
            ProfileId = profileId;
            Token = token;
            ExpirationDate = expirationDate;
            SetCreateStamp(profileId);
        }

        public long ClientId { get; private set; }

        public long ProfileId { get; private set; }

        public string Token { get; private set; }

        public DateTime ExpirationDate { get; private set; }

        public bool IsForceExpired { get; private set; }

        public void Expire()
        {
            IsForceExpired = true;
        }
    }
}