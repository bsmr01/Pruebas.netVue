
namespace Ibero.Services.Utilitary.Domain.Libros.Queries
{
    using Ibero.Services.Utilitary.Core.Models;
    using Ibero.Services.Utilitary.Domain.Exceptions;
    using Ibero.Services.Utilitary.Domain.Infrastructure.Abstract;
    using Ibero.Services.Utilitary.Domain.Infrastructure.Configuration;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Data.SqlClient;
    using System.Data;
    using Newtonsoft.Json;

    public class BestMovieQuery : IRequest<object>
    {
        public string Lenguaje { get; set; }

        public class Handler : IRequestHandler<BestMovieQuery, object>
        {
            private readonly string _connection;
            private readonly IExternalService externalService;
            private readonly IInternalService internalService;
            private readonly UtilitaryAppOptions options;


            public Handler(IConfiguration configuration, UtilitaryAppOptions options, IExternalService externalService, IInternalService internalService)
            {
                _connection = configuration.GetConnectionString("UtilitaryDB");
                this.externalService = externalService;
                this.internalService = internalService;
                this.options = options;
            }

            public async Task<object> Handle(BestMovieQuery request, CancellationToken cancellationToken)
            {
                var response = new object();
                try
                {
                    var JsonLibrosListOnly = await externalService.GETExternalServiceToken(options.Urlthemoviedb + "popular?" + options.ClaveAPIthemoviedb + "&language=en-"+ request.Lenguaje, "Bearer" + " " + options.Tokenthemoviedb);
                    response = JsonConvert.DeserializeObject(JsonLibrosListOnly.ToString());
                }
                catch (Exception ex)
                {
                    throw new DeleteFailureException(nameof(BestMovieQuery), ex.Message, ex.Message);
                }
                return response;
            }
        }
    }
}
