using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.data.Repositories.Interfaces
{
    public interface IMySessionService
    {
        long GetUserOrganizationId();
        long GetUserId();
        string GetUsername();
        string GetUserRole();
        string Getfullname();
    }
}
