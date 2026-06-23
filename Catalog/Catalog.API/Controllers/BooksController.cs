using Catalog.Application.Books.Commands;
using Catalog.Application.Common;
using Catalog.Application.DTOs.Books;
using Catalog.Application.DTOs.Reviews;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Catalog.Application.Books.Commands.UpdateReview;
using static Catalog.Application.Books.Commands.AddReview;
using static Catalog.Application.Books.Queries.GetBooks;
using Catalog.Application.Books.Queries;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetBookByIdQuery(id));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Data),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(BookFilterDto bookFilter)
        {
            var result = await _mediator.Send(new GetBooksQuery(bookFilter));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Data),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

        [HttpPost("createBook")]
        public async Task<IActionResult> CreateBook(CreateBookDto book)
        {
            var result = await _mediator.Send(new CreateBookCommand(book));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Message),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
        [HttpPatch("updateBookPrice")]
        public async Task<IActionResult> UpdateBookPrice(UpdatePriceBookDto book)
        {
            var result = await _mediator.Send(new UpdateBookPriceCommand(book));

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
            var result = await _mediator.Send(new DeleteBookCommand(id));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Message),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
        [HttpPost("postReview")]
        public async Task<IActionResult> AddReview(AddReviewDto review)
        {
            var result = await _mediator.Send(new AddReviewCommand(review));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Message),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
        [HttpPut("updateReview")]
        public async Task<IActionResult> UpdateReview(UpdateReviewDto review)
        {
            var result = await _mediator.Send(new UpdateReviewCommand(review));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Message),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

    }
}