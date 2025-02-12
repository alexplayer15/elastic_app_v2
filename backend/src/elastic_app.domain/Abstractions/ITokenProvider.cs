using elastic_app.domain.Models;

namespace elastic_app.domain.Abstractions
{
    public interface ITokenProvider
    {
        public string Create(UserModel userModel);
    }
}
