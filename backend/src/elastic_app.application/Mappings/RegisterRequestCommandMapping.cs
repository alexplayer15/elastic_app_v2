using Mapster;
using elastic_app.application.Commands;
using elastic_app.application.DTOs;

namespace elastic_app.application.Mappings
{
    public class RegisterRequestMappings : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequestCommand, RegisterRequest>();
        }
    }
}
