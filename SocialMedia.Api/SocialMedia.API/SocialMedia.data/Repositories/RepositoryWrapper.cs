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
        private IComments _CommentsRepository;
        private ILikes _LikesRepository;
        private IPosts _PostRepository;




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


        public IComments comments
        {
            get
            {
                if(_CommentsRepository == null)
                    _CommentsRepository = new CommentsRepository(_repoContext);
                return _CommentsRepository;

            }
        }

        public ILikes likes
        {
            get
            {
                if (_LikesRepository == null)
                    _LikesRepository = new LikesRepository(_repoContext);
                return _LikesRepository;

            }

        }


        public IPosts posts
        {
            get
            {
                if (_PostRepository == null)
                    _PostRepository = new PostRepository(_repoContext);
                return _PostRepository;

            }
        }



        public async Task SaveAsync(string userName = "", string customMessage = "")
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
