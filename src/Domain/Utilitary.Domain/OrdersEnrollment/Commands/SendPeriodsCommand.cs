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

    public class SendPeriodsCommand : IRequest
    {
        public object ListPeriods { get; set; }

        public class Handler : IRequestHandler<SendPeriodsCommand, Unit>
        {
            private readonly string _connection;

            public Handler(IConfiguration configuration)
            {
                _connection = configuration.GetConnectionString("UtilitaryDb");
            }
            public async Task<Unit> Handle(SendPeriodsCommand request, CancellationToken cancellationToken)
            {
                try
                {

                    using (SqlConnection sql = new SqlConnection(_connection))
                    {
                        using (SqlCommand cmd = new SqlCommand("IBEP_SP_Status_Studens", sql))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@Reference", SqlDbType.VarChar).Value = 3;
                            cmd.Parameters.Add("@PeriodList", SqlDbType.VarChar).Value = request.ListPeriods.ToString();

                            await sql.OpenAsync();

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DeleteFailureException(nameof(SendPeriodsCommand), ex.Message, ex.Message);
                }

                return Unit.Value;
            }
        }
    }
}
