using Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Model
{
    public class UserModel
    {
        public UserModel(Guid userId, string email, string firstName, string lastName,
            string role)
        {
            this.UserId = userId;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.RoleName = role;
        }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }

    }
}
