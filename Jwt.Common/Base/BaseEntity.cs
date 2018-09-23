using System;
using System.Collections.Generic;
using System.Linq;

namespace Jwt.Common.Base
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            ValidationErrors = new Dictionary<string, string>();
        }

        public long Id { get; private set; }

        public virtual long CreatedBy { get; private set; }

        public virtual DateTime CreatedDate { get; private set; }

        public virtual long? UpdatedBy { get; private set; }

        public virtual DateTime? UpdatedDate { get; private set; }

        public virtual IDictionary<string, string> ValidationErrors { get; private set; }

        public virtual bool IsValid => !ValidationErrors.Any();

        public virtual void SetErrors(Dictionary<string, string> errors)
        {
            ValidationErrors = errors;
        }

        public virtual void SetCreateStamp(long profileId)
        {
            CreatedBy = profileId;
            CreatedDate = DateTime.UtcNow;
        }

        public virtual void SetUpdateStamp(long profileId)
        {
            UpdatedBy = profileId;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}