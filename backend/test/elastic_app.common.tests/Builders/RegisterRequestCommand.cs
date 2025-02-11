using elastic_app.application.Commands;

namespace elastic_app.common.tests.Builders
{
    public class RegisterRequestCommandBuilder
    {
        private string _forename = "Alex";
        private string _surname = "Player";
        private string _email = "example15@example.com";
        private string _username = "AlexPlayer15";
        private string _password = "thisIsNotReal22!";
        private string _reEnterPassword = "thisIsNotReal22!";

        public RegisterRequestCommandBuilder WithUsername(string username)
        {
            _username = username;

            return this;
        }

        public RegisterRequestCommandBuilder WithEmail(string email)
        {
            _email = email;

            return this;
        }

        public RegisterRequestCommandBuilder WithPassword(string password)
        {
            _password = password;

            return this;
        }

        public RegisterRequestCommandBuilder WithReEnterPassword(string reEnterPassword)
        {
            _reEnterPassword = reEnterPassword;

            return this;
        }

        public RegisterRequestCommandBuilder WithValidRegistrationDetails(bool isValid)
        {
            if (isValid)
            {
                _forename = "Alex";
                _surname = "Player";
                _email = "example15@example.com";
                _username = "AlexPlayerTest15";
                _password = "thisIsNotReal22!";
                _reEnterPassword = "thisIsNotReal22!";

                return this;
            }

            else
            {
                _forename = "Alex";
                _surname = "Player";
                _email = "example15@example.com";
                _username = "AlexPlayer15";
                _password = "thisIsNotReal22!";
                _reEnterPassword = "!";

                return this;
            }
        }

        public RegisterRequestCommandBuilder WithExistingUsername(bool usernameExists)
        {
            if (usernameExists)
            {
                _forename = "Alex";
                _surname = "Player";
                _email = "example150@example.com";
                _username = "AlexPlayer15";
                _password = "thisIsNotReal22!";
                _reEnterPassword = "thisIsNotReal22!";

                return this;
            }

            else
            {
                _forename = "Alex";
                _surname = "Player";
                _email = "example150@example.com";
                _username = "AlexPlayerTest15";
                _password = "thisIsNotReal22!";
                _reEnterPassword = "!";

                return this;
            }
        }

        public RegisterRequestCommandBuilder WithExistingEmail(bool emailExists)
        {
            if (emailExists)
            {
                _forename = "Alex";
                _surname = "Player";
                _email = "example@example.com";
                _username = "AlexPlayerTest15";
                _password = "thisIsNotReal22!";
                _reEnterPassword = "thisIsNotReal22!";

                return this;
            }

            else
            {
                _forename = "Alex";
                _surname = "Player";
                _email = "example15@example.com";
                _username = "AlexPlayerTest15";
                _password = "thisIsNotReal22!";
                _reEnterPassword = "!";

                return this;
            }
        }

        public RegisterRequestCommand Build()
        {
            return new RegisterRequestCommand()
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

