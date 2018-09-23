using Jwt.Entity.Entities;
using Jwt.Repository.Interfaces;

namespace Jwt.Repository.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository { }
}
