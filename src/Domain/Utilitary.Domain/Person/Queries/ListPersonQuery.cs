namespace Ibero.Services.Utilitary.Domain.Avaya.Queries
{
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


    public class ListPersonQuery : IRequest<object>
    { 
        public string password { get; set; }
        public string identificacion { get; set; }
        

        public class Handler : IRequestHandler<ListPersonQuery, object>
        {
            private readonly UtilitaryAppOptions options;
            private readonly string _connection;
            private readonly IExternalService externalService;
            private readonly IInternalService internalService;
           

            public Handler(IConfiguration configuration,IExternalService externalService, IInternalService internalService)
            {
                _connection = configuration.GetConnectionString("UtilitaryDB");
                this.externalService = externalService;
                this.internalService = internalService;
            }

            public async Task<object> Handle(ListPersonQuery request, CancellationToken cancellationToken)
            {
                var response = new object();
                var infoDB = "";

                try
                {
                  
                    using (SqlConnection sql = new SqlConnection(_connection))
                    {
                        using (SqlCommand cmd = new SqlCommand("SP_PRUEBA", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@Reference", SqlDbType.VarChar).Value = 1;
                            cmd.Parameters.Add("@ID", SqlDbType.Int).Value =1;
                            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = "";
                            cmd.Parameters.Add("@IDENTIFICACION", SqlDbType.Int).Value = request.identificacion;
                            cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = "";
                            cmd.Parameters.Add("@Estado", SqlDbType.Int).Value = 1;
                            cmd.Parameters.Add("@PASSWORD", SqlDbType.NVarChar).Value = request.password;
                            cmd.Parameters.Add("@role", SqlDbType.NVarChar).Value = "";

                            await sql.OpenAsync();

                            using (var sqlReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await sqlReader.ReadAsync())
                                {
                                    infoDB += sqlReader[0].ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DeleteFailureException(nameof(ListPersonQuery), ex.Message, ex.Message);
                }
                return infoDB;
            }
        }
    }
}
