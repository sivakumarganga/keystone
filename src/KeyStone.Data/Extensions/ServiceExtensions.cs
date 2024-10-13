using KeyStone.Data.RepoContracts;
using KeyStone.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyStone.Data.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomDataServices(this IServiceCollection services)
        {

            services.AddTransient<ISampleEntityRepository, SampleEntityRepository>();
                
            return services;
        }
    }
}
