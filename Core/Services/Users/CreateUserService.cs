using System;
using System.Collections.Generic;
using BusinessEntities;
using Common;
using Core.Factories;
using Data.Repositories;

namespace Core.Services.Users
{
    [AutoRegister]
    public class CreateUserService : ICreateUserService
    {
        private readonly IUpdateUserService _updateUserService;
        private readonly IIdObjectFactory<User> _userFactory;
        private readonly IUserRepository _userRepository;

        public CreateUserService(IIdObjectFactory<User> userFactory, IUserRepository userRepository, IUpdateUserService updateUserService)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _updateUserService = updateUserService;
        }

        public User Create(Guid id, string name, string email, int age, UserTypes type, decimal? annualSalary, IEnumerable<string> tags)
        {
            var existingUser = _userRepository.Get(id);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"A user with id {id} already exists.");
            }

            var user = _userFactory.Create(id);
            _updateUserService.Update(user, name, email, age, type, annualSalary, tags);
            _userRepository.Save(user);
            return user;
        }
    }
}