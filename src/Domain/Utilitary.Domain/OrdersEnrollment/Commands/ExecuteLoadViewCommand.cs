namespace Ibero.Services.Utilitary.Domain.OrdersEnrollment.Commands
{
    using Ibero.Services.Utilitary.Domain.Exceptions;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    public class ExecuteLoadViewCommand : IRequest
    {
        public class Handler : IRequestHandler<ExecuteLoadViewCommand, Unit>
        {
            private readonly string _connection;

            public Handler(IConfiguration configuration)
            {
                _connection = configuration.GetConnectionString("UtilitaryDb");
            }
            public async Task<Unit> Handle(ExecuteLoadViewCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    using (SqlConnection sql = new SqlConnection(_connection))
                    {
                        using (SqlCommand cmd = new SqlCommand("IBEP_JobLoadStatusStudents", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@Reference", SqlDbType.Int).Value = 1;

                            await sql.OpenAsync();

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DeleteFailureException(nameof(ExecuteLoadViewCommand), ex.Message, ex.Message);
                }

                return Unit.Value;
            }
        }
    }
}
