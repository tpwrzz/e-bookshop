using Catalog.Application.Authors.Commands;
using Catalog.Application.Authors.Queries;
using Catalog.Application.Books.Commands;
using Catalog.Application.Books.Queries;
using Catalog.Application.Common;
using Catalog.Application.DTOs.Auhtors;
using Catalog.Application.DTOs.Books;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Catalog.Application.Books.Queries.GetBooks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetAuthorByIdCommand(id));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Data),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetByFilter()
        {
            var result = await _mediator.Send(new GetAuthorsCommand());

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Data),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        [HttpPost("createAuthor")]
        public async Task<IActionResult> CreateBook(AuthorDto author)
        {
            var result = await _mediator.Send(new CreateAuthorCommand(author));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Message),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
        [HttpPatch("updateAuthor")]
        public async Task<IActionResult> UpdateBookPrice(AuthorDto author)
        {
            var result = await _mediator.Send(new UpdateAuthorCommand(author));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Message),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteAuthorCommand(id));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Message),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
    } 
}
