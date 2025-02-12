using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elastic_app.application.Services.Email
{
    public interface IEmailService
    {
        public Task SendEmailAsync();
    }
}
