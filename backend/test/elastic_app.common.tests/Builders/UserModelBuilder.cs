using elastic_app.application.DTOs;
using elastic_app.domain.Models;
using System.Runtime.CompilerServices;

namespace elastic_app.common.tests.Builders
{
    public class UserModelBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _forename = "Alex";
        private string _surname = "Player";
        private string _email = "example15@example.com";
        private string _username = "AlexPlayer15";
        private string _password = "thisIsNotReal22!";
        private DateTime _createdAt = DateTime.Now;

        public UserModelBuilder WithEmail(string email)
        {
            _email = email;

            return this;
        }

        public UserModelBuilder WithUsername(string username)
        {
            _username = username;

            return this;
        }

        public UserModelBuilder WithPassword(string password)
        {
            _password = password;

            return this;
        }

        public UserModel Build()
        {
            return new UserModel()
            {
                Id = _id,
                Forename = _forename,
                Surname = _surname,
                Email = _email,
                Username = _username,
                PasswordHash = _password,
                CreatedAt = _createdAt
            };
        }
    }
}

