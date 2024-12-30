using SocialMedia.data.Repositories.Interfaces;
using SocialMedia.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.data.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IUserRepository _userRepository;
        private readonly SocialMediaDbContext _repoContext;


        public RepositoryWrapper(SocialMediaDbContext repoContext)
        {
            _repoContext = repoContext;
        }

        public IUserRepository Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_repoContext);
                return _userRepository;
            }
        }
    }
}
