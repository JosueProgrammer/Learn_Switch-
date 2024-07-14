using Domain.Endpoint.DTOs;
using Domain.Endpoint.Entities;

namespace Domain.Endpoint.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();

        User GetById(Guid id);

        void Create(User user);

        void Update(Guid Id, UserDTO modificarUsuario);

        void Delete(Guid Id);
    }
}
