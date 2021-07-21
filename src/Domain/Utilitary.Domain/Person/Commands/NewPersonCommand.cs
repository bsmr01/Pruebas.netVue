namespace Ibero.Services.Utilitary.Domain.Avaya.Commands
{
    using Ibero.Services.Utilitary.Domain.Exceptions;
    using Ibero.Services.Utilitary.Domain.Infrastructure.Abstract;
    using Ibero.Services.Utilitary.Domain.Infrastructure.Configuration;
    using Ibero.Services.Utilitary.Domain.Libros.Models;
    using Ibero.Services.Utilitary.Domain.Person.Models;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    public class NewPersonCommand : IRequest<object>
    {
        public string email { get; set; }
        public string identificacion { get; set; }
        public string nombre { get; set; }
        public string password { get; set; }

        public class Handler : IRequestHandler<NewPersonCommand, object>
        {
            private readonly IInternalService internalService;
            private readonly IExternalService externalService;
            private readonly string _connection;

            public Handler(IConfiguration configuration, IExternalService externalService, IInternalService internalService)
            {
                _connection = configuration.GetConnectionString("UtilitaryDB");
                this.externalService = externalService;
                this.internalService = internalService;
            }

            public async Task<object> Handle(NewPersonCommand request, CancellationToken cancellationToken)
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
                            cmd.Parameters.Add("@REFERENCE", SqlDbType.Int).Value = 2;
                            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = 0;
                            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = request.nombre;
                            cmd.Parameters.Add("@IDENTIFICACION", SqlDbType.Int).Value = Convert.ToInt32(request.identificacion);
                            cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = request.email;
                            cmd.Parameters.Add("@Estado", SqlDbType.Int).Value = 1;
                            cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = request.password;
                            cmd.Parameters.Add("@role", SqlDbType.NVarChar).Value = "User";


                            await sql.OpenAsync();

                            var sqlReader = await cmd.ExecuteReaderAsync();
                            await sqlReader.ReadAsync();
                        }
                    }
                    response = infoDB;
                }

                catch (Exception ex)
                {
                    throw new DeleteFailureException(nameof(NewPersonCommand), ex.Message, ex.Message);
                }

                return response;

            } 
        }
    }
}
    

