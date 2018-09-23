using Jwt.Common.Base;
using Jwt.Entity.Enums;

namespace Jwt.Entity.Entities
{
    public class Profile : BaseEntity
    {
        public ProfileType ProfileTypeId { get; private set; }

        public ProfileStatusType ProfileStatusTypeId { get; private set; }

        public string Username { get; private set; }

        public string Email { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Salt { get; private set; }

        public string PasswordHash { get; private set; }

        public int FailedLoginCount { get; private set; }

        public bool IsEnabled => ProfileStatusTypeId == ProfileStatusType.Enabled;

        public string FullName => FirstName + " " + LastName;

        public void IncrementFailedLoginCount()
        {
            FailedLoginCount++;
            SetUpdateStamp(Id);
        }

        public void ClearFailedLoginCount()
        {
            FailedLoginCount = 0;
            SetUpdateStamp(Id);
        }
    }
}