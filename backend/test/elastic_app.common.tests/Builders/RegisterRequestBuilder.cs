using elastic_app.application.DTOs;

namespace elastic_app.common.tests.Builders
{
    public class RegisterRequestBuilder
    {
        private string _forename = "Alex";
        private string _surname = "Player";
        private string _email = "example15@example.com";
        private string _username = "AlexPlayer15";
        private string _password = "thisisnotreal";
        private string _reEnterPassword = "thisisnotreal";

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
