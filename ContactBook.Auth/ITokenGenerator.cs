using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Auth
{
    public interface ITokenGenerator
    {
        object GenerateToken(string username, string id, string email, IConfiguration config, string[] roles);
    }
}

