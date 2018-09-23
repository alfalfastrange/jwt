using Jwt.Entity.Entities;
using Jwt.Repository.Interfaces;

namespace Jwt.Repository.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository { }
}