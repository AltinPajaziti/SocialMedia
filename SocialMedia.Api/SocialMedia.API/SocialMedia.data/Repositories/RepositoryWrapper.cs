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
        private readonly SocialMediaDbContext _repoContext;
        private IUserRepository _userRepository;
        private IFollowRequests _FollowRequestsRepository;



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


        public IFollowRequests FollowRequests
        {
            get
            {
                if (_FollowRequestsRepository == null)
                    _FollowRequestsRepository = new FollowRequestsRepository(_repoContext);
                return _FollowRequestsRepository;
            }
        }



        public async Task SaveAsync(string userName = "", string customMessage = "")
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
