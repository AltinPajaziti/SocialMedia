using Microsoft.AspNetCore.Http;
using SocialMedia.data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.data.Services
{
    public class MySessionService : IMySessionService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public MySessionService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public long GetUserId()
        {
            long userId = 0;
            if (httpContextAccessor != null)
            {
                try
                {
                    userId = Convert.ToInt64(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
                catch (Exception)
                {
                }
            }
            return userId;
        }
        public string GetUsername()
        {
            string userName = "";
            if (httpContextAccessor != null)
            {
                try
                {
                    userName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                }
                catch (Exception)
                {
                }
            }
            return userName;
        }




        public string Getfullname()
        {
            string userName = "";
            if (httpContextAccessor != null)
            {
                try
                {
                    userName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                }
                catch (Exception)
                {
                }
            }
            return userName;
        }






        public string GetUserRole()
        {
            string userRole = "";
            if (httpContextAccessor != null)
            {
                try
                {
                    userRole = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                }
                catch (Exception)
                {
                }
            }
            return userRole;
        }
        public long GetUserOrganizationId()
        {
            long organizationId = 1;
            if (httpContextAccessor != null)
            {
                try
                {
                    organizationId = Convert.ToInt64(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.PrimarySid)?.Value);
                }
                catch (Exception)
                {
                }
            }
            return organizationId;
        }


    }
}
