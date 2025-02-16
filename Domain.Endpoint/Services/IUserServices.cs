﻿using Domain.Endpoint.DTOs;
using Domain.Endpoint.Entities;

namespace Domain.Endpoint.Services
{
    public interface IUserServices
    {
        Task<List<User>> Get(); 

        User FilterUser(Guid id);

        User createUser(UserDTO newUserDTO);

        void Update(Guid Id, UserDTO nuevosCampos);

        void Delete(Guid Íd);
    }
}
