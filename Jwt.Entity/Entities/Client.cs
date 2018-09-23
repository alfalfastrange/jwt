using Jwt.Common.Base;

namespace Jwt.Entity.Entities
{
    public class Client : BaseEntity
    {
        public string ClientId { get; private set; }

        public string Name { get; private set; }

        public string Secret { get; private set; }
    }
}