
namespace Ibero.Services.Utilitary.API.V1.Controllers
{
    using Ibero.Services.Utilitary.Domain.Libros.Models;
    using Ibero.Services.Utilitary.Domain.Libros.Queries;
    using Ibero.Services.Utilitary.Domain.Pelicula.Queries;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

   
    public class PeliculasController : BaseController
    {
        [HttpGet("LastMovie")]
        public async Task<ActionResult<IEnumerable<MovieModel>>> GetAdviserByDocument()
       {
            return Ok(await Mediator.Send(new ListMovieQuery()));
        }
        [HttpGet("BestMovie/{Lenguaje}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAdviserByDocument([FromRoute] BestMovieQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("AllMovie/{Lenguaje}/{id}")]
      
        public async Task<ActionResult<IEnumerable<object>>> GetAdviserByDocument([FromRoute] AllMovieQuery query)
        {
            return Ok(await Mediator.Send(query));
        }


        [HttpGet("AllLengua")]

        public async Task<ActionResult<IEnumerable<MovieModel>>> AllLengua()
        {
            return Ok(await Mediator.Send(new AllLenguaeQuery()));
        }

    }
}
