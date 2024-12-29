using Microsoft.Extensions.DependencyInjection;
using SocialMedia.data.Repositories.Interfaces;
using SocialMedia.data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

using System.Globalization;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace SocialMedia.data.Services
{
    public static class ServiceExtensions
    {



        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }


    }

}