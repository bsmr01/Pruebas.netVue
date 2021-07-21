namespace Ibero.Services.Utilitary.API.V1.Controllers
{
    using Ibero.Services.Utilitary.Core.Models;
    using Ibero.Services.Utilitary.Domain.Avaya.Commands;
    using Ibero.Services.Utilitary.Domain.Avaya.Queries;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    public class PersonController : BaseController
    {
        [HttpPost("NewUser")]
        public async Task<ActionResult<Response>> NewPerson([FromBody] NewPersonCommand command)
        {
            try
            {
                var status = Ok(await Mediator.Send(command));
                var response = new Response
                {
                    Title = status.Value.ToString(),
                    Message = "Success",
                    Status = status.StatusCode.ToString()
                };

                return response;
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    Title = ex.Source,
                    Message = ex.Message,
                    Status = StatusCodes.Status400BadRequest.ToString()
                };

                return response;
            }
        }

        [HttpGet("GetAdviserByDocument/{password}/{identificacion}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAdviserByDocument([FromRoute] ListPersonQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

      
    }
}


