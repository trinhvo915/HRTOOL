using Microsoft.Extensions.Options;
using Orient.Base.Net.Core.Api.Core.Business.Models;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.DirectoryServices;

namespace Orient.Base.Net.Core.Api.Core.Business.Services
{
    public interface IAuthenticationService
    {
        bool CheckUser(string username, string password);

        string GetUserInfo(string usr, string pwd);
    }

    public class LdapAuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _config;
        private readonly IRepository<User> _userRepository;

        public LdapAuthenticationService(IOptions<AppSettings> config, IRepository<User> userRepository)
        {
            _config = config.Value;
            _userRepository = userRepository;
        }

        public bool CheckUser(string usr, string pwd)
        {
            bool authenticated = false;
            try
            {
                DirectoryEntry entry = new DirectoryEntry(_config.UrlServer, usr, pwd);
                object nativeObject = entry.NativeObject;
                authenticated = true;
            }
            catch (Exception ex)
            {
                authenticated = false;
            }
            return authenticated;
        }

        public string GetUserInfo(string usr, string pwd)
        {
            try
            {
                usr = usr.Replace(_config.Domain, "");
                DirectoryEntry entry = new DirectoryEntry(_config.UrlUser, usr, pwd);

                DirectorySearcher dssearch = new DirectorySearcher(entry);
                dssearch.Filter = string.Format(_config.SearchFilter, usr);
                SearchResult sresult = dssearch.FindOne();
                DirectoryEntry dsresult = sresult.GetDirectoryEntry();
                return dsresult.Name.Substring(3);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
