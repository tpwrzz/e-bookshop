using Bookshop.SharedKernel.Application.Common;
using Catalog.Application.DTOs.Reviews;
using Catalog.Domain;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Books.Commands
{
    public class AddReview
    {
        public record AddReviewCommand(AddReviewDto Review) : IRequest<Result>;
        public class AddReviewCommandHandler(IBookRepository repository) : IRequestHandler<AddReviewCommand, Result>
        {
            private readonly IBookRepository _repository = repository;

            public async Task<Result> Handle(AddReviewCommand request, CancellationToken cancellationToken)
            {
                var bookToUpdate = await _repository.GetByIdAsync(request.Review.BookId);
                if (bookToUpdate is null)
                    return new Result()
                    {
                        ResultStatus = ResultStatus.NotFound,
                        Message = $"Book with Id {request.Review.BookId} was not found."
                    };
                bookToUpdate.AddReview(request.Review.UserId, request.Review.Message, new Rating(request.Review.Rating));
                await _repository.UpdateAsync(bookToUpdate);
                return new Result()
                {
                    ResultStatus = ResultStatus.Created,
                    Message = $"A review is added to book with Id: {bookToUpdate.Id}"
                };
            }
        }
    }
}
