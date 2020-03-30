using InvestmentManager.Core.DataAccess;
using InvestmentManager.DataAccess.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentManager.DataAccess.EF
{
    public static class IServiceCollectionExtensions
    {


        public static void RegisterEfDataAccessClasses(this IServiceCollection services,
            String connectionString)
        {
            services.AddDbContext<InvestmentContext>(options =>
                options//.UseLazyLoadingProxies()
                .UseSqlServer(connectionString)                
                );

            services.AddScoped<ISecurityRepository, SecurityRepository>();
            services.AddScoped<ITradeDateRepository, TradeDateRepository>();
            services.AddScoped<IInvestmentAccountRepository, InvestmentAccountRepository>();
            services.AddScoped<IAccountPositionsRepository, AccountPositionsRepository>();
            services.AddScoped<IAccountMarketValueRepository, AccountMarketValueRepository>();
            services.AddScoped<IAccountCashFlowsRepository, AccountCashFlowsRepository>();
            services.AddScoped<ISecurityPriceRepository, SecurityPriceRepository>();
        }

    }
}
