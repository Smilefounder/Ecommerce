using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Accounts.Exceptions;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.Accounts.Services
{
    [Dependency(typeof(IAccountService))]
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _accountRepository;

        public AccountService(IRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Account GetById(int id)
        {
            return _accountRepository.Query(o => o.Id == id).FirstOrDefault();
        }

        public Account GetByEmail(string email)
        {
            return _accountRepository
                .Query()
                .Where(x => x.Email == email)
                .FirstOrDefault();
        }

        public Account Create(string email)
        {
            if (IsEmailAvailable(email))
            {
                throw new CreateAccountException("A account for that e-mail address already exists. Please enter a different e-mail address.");
            }

            Account account = new Account();
            account.Email = email;

            return account;
        }

        public bool IsEmailAvailable(string email)
        {
            Account account = GetByEmail(email);

            return account != null;
        }

        public Account Authenticate(string email, string password)
        {
            Account account = GetByEmail(email);

            if (account.Password != EncryptPassword(password))
            {
                return null;
            }

            return account;
        }

        public Account EncryptPassword(Account account)
        {
            if (account != null)
            {
                account.Password = EncryptPassword(account.Password);
            }
            return account;
        }

        public bool ChangePassword(int accountId, string oldPassword, string newPassword, out string message)
        {
            var account = _accountRepository.Query(o => o.Id == accountId).FirstOrDefault();
            if (account == null)
            {
                message = string.Format("Cannot find account of id: {0}.", accountId);
            }
            else
            {
                if ((string.IsNullOrEmpty(account.Password) && string.IsNullOrEmpty(oldPassword)) || account.Password == EncryptPassword(oldPassword))
                {
                    account.Password = EncryptPassword(newPassword);
                    _accountRepository.Update(account, o => new object[] { o.Id });
                    message = "Password changed.";
                    return true;
                }
                else
                {
                    message = "Wrong old password.";
                }
            }
            return false;
        }

        public void Save(Account account)
        {
            account.Password = EncryptPassword(account.Password);
            if (account.Id > 0)
            {
                bool exists = _accountRepository.Query(o => o.Id == account.Id).Any();
                if (exists)
                    _accountRepository.Update(account, k => new object[] { k.Id });
                else
                    _accountRepository.Insert(account);
            }
            else
                _accountRepository.Insert(account);
        }

        public void Delete(Account account)
        {
            _accountRepository.Delete(account);
        }


        private string EncryptPassword(string password)
        {
            if (password == null)
                return null;

            using (HashAlgorithm alg = HashAlgorithm.Create("SHA1"))
            {
                var bytes = alg.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}