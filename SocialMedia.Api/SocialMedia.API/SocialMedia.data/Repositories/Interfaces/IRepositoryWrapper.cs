using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.data.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        Task SaveAsync(string userName = "", string customMessage = "");

        IUserRepository Users { get; }

        IFollowRequests FollowRequests { get; }
        IPosts posts { get; }

        ILikes likes { get; }

        IComments comments { get; }

    }
}
