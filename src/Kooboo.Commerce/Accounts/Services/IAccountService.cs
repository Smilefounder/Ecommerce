using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Accounts.Services
{
    public interface IAccountService
    {
        Account GetById(int id);

        Account GetByEmail(string email);

        Account Create(string email);

        bool IsEmailAvailable(string email);

        Account Authenticate(string email, string password);

        Account EncryptPassword(Account account);

        bool ChangePassword(int accountId, string oldPassword, string newPassword, out string message);

        void Save(Account account);

        void Delete(Account account);
    }
}