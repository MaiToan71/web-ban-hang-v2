using Project.ViewModels.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Emails
{
    public interface IEmailService
    {
        Task<bool> SendMail(MailContent mailContent);
    }
}
