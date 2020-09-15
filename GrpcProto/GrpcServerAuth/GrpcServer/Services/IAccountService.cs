using System.Collections.Generic;
using GrpcServer.Authentication;
using GrpcServer.Models;

namespace GrpcServer.Services
{
    public interface IAccountService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}