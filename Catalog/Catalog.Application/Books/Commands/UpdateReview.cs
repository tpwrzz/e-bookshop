using Catalog.Application.Common;
using Catalog.Application.DTOs.Reviews;
using Catalog.Domain;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Books.Commands
{
    public class UpdateReview
    {
        public record UpdateReviewCommand(UpdateReviewDto Review) : IRequest<Result>;

        public class UpdateReviewCommandHandler(IBookRepository repository): IRequestHandler<UpdateReviewCommand, Result>
        {
            private readonly IBookRepository _repository = repository;

            public async Task<Result> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
            {
                var book = await _repository.GetByIdAsync(request.Review.BookId);
                if (book is null)
                    return new Result
                    {
                        ResultStatus = ResultStatus.NotFound,
                        Message = $"Book with Id {request.Review.BookId} was not found."
                    };

                try
                {
                    book.UpdateReview(request.Review.ReviewId, request.Review.UserId,
                        request.Review.Message, new Rating(request.Review.Rating));
                }
                catch (UnauthorizedAccessException ex)
                {
                    return new Result { ResultStatus = ResultStatus.Forbidden, Message = ex.Message };
                }

                await _repository.UpdateAsync(book);
                return new Result
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"Review {request.Review.ReviewId} updated."
                };
            }
        }
    }
}
