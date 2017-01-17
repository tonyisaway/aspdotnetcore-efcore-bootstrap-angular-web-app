using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    using System.Diagnostics;

    public class DebugMailService : IMailService
    {
        public void SendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending Mail: To {to}, From {from}, Subject {subject}");
        }
    }
}
