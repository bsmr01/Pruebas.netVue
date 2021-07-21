namespace Microsoft.Extensions.DependencyInjection
{
    using AutoMapper;
    using global::Ibero.Services.Utilitary.Infrastructure;
    using Ibero.Services.Utilitary.Domain.Infrastructure.Abstract;
    using Ibero.Services.Utilitary.Domain.Infrastructure.Configuration;   
    using Ibero.Services.Utilitary.Persistence;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Ibero.Services.Utilitary.Domain.OrdersEnrollment.Commands;

    public static class UtilitaryServiceCollectionExtensions
    {
        public static IServiceCollection AddUtilitaryService(this IServiceCollection service)
        {
            var sp = service.BuildServiceProvider();
            var configuration = sp.GetService<IConfiguration>();

            service.AddDbContext<IUtilitaryDbContext, UtilitaryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("UtilitaryDB")));

             service.AddMediatR(typeof(ExecuteLoadViewCommand.Handler));/////////////////

          
            // Add framework services.
          
            service.AddTransient<IExternalService, ExternalService>();
            service.AddTransient<IInternalService>();
           
            // Add IdentityAppOptions
            // TODO: Validate Options
            service.Configure<UtilitaryAppOptions>(configuration.GetSection("UtilitaryApp"));
            service.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<UtilitaryAppOptions>>().Value);

            return service;
        }
    }
}
