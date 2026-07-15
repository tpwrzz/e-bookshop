using Bookshop.SharedKernel.Application.Common;
using Catalog.Application.Authors.Commands;
using Catalog.Application.Authors.Queries;
using Catalog.Application.DTOs.Auhtors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            var result = await _mediator.Send(new GetAuthorByIdQuery(id));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Data),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAuthorsQuery());

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Data),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(AddAuthorDto author)
        {
            var result = await _mediator.Send(new CreateAuthorCommand(author));

            return result.ResultStatus switch
            {
                ResultStatus.Created => CreatedAtAction(nameof(Create), result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateAuthor(AuthorDto author)
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
