using elastic_app.application.DTOs;

namespace elastic_app.common.tests.Builders
{
    public class RegisterRequestBuilder
    {
        private string _forename = "Alex";
        private string _surname = "Player";
        private string _email = "example15@example.com";
        private string _username = "AlexPlayer15";
        private string _password = "thisIsNotReal22!";
        private string _reEnterPassword = "thisIsNotReal22!";

        public RegisterRequestBuilder WithUsername(string username)
        {
            _username = username;

            return this;
        }

        public RegisterRequestBuilder WithEmail(string email)
        {
            _email = email;

            return this;
        }

        public RegisterRequestBuilder WithPassword(string password)
        {
            _password = password;

            return this;
        }

        public RegisterRequestBuilder WithReEnterPassword(string reEnterPassword)
        {
            _reEnterPassword = reEnterPassword;

            return this;
        }

        public RegisterRequest Build()
        {
            return new RegisterRequest()
            {
                Forename = _forename,
                Surname = _surname, 
                Email = _email,
                Username = _username,
                Password = _password,
                ReEnterPassword = _reEnterPassword
            }; 
        }
    }
}
