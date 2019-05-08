using System;
using System.Collections.Generic;
using System.Linq;
using MessageBoardBackend.Core;
using MessageBoardBackend.Models;

namespace MessageBoardBackend.Services
{

    public class UserRepository
    {
        private ApiContext _context;
        public UserRepository()
        {
            _context = new ApiContext();
        }
        public IEnumerable<User> GetAllUsers()
        {
           
            var users = _context.Users.ToList();
            return users;
        }

        public User GetUserByEmail(string email)
        {
            var users = _context.Users
                .FirstOrDefault(x => x.Email == email);
            return users;
        }
        public IEnumerable<User> GetUser(string name)
        {
            var users = _context.Users
                .Where(x=>x.FirstName== name).ToList();
            return users;
        }
        public User GetUser(Guid id)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Id == id);
            return user;
        }

        public void Insert( User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User Login(User user)
        {
            var loginUser = _context.Users
                .FirstOrDefault(x => x.Email == user.Email && x.Password==user.Password);
            return loginUser;
        }
    }
}