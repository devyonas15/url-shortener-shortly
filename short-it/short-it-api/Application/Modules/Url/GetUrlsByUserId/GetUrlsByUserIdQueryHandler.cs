using Application.Abstractions;
using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Modules.Url.GetUrlsByUserId;

public sealed class GetUrlsByUserIdQueryHandler :
    BaseHandlerWithValidator<GetUrlsByUserIdQuery, IReadOnlyList<UrlResponse>, GetUrlsByUserIdQueryValidator>,
    IRequestHandler<GetUrlsByUserIdQuery, IReadOnlyList<UrlResponse>>
{
    private readonly IUrlRepository _urlRepository;

    public GetUrlsByUserIdQueryHandler(IMapper mapper, IUrlRepository repository,
        ILogger<GetUrlsByUserIdQueryHandler> logger) :
        base(mapper, logger)
    {
        _urlRepository = repository;
    }

    public async Task<IReadOnlyList<UrlResponse>> Handle(GetUrlsByUserIdQuery request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Processing handler {handler} with request {@request}", RequestName, request);
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Validation payload:", validationResult);
            }

            var urls = await _urlRepository.GetUrlsByUserIdAsync(request.UserId, cancellationToken);

            if (urls is null)
            {
                throw new NotFoundException($"Urls not found for user id: {request.UserId}");
            }

            var response = Mapper.Map<IReadOnlyList<UrlResponse>>(urls);

            return response;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when getting the urls due to: {message}", ex.Message);
            if (ex is BadRequestException or NotFoundException)
            {
                throw;
            }

            throw new Exception($"Error occured when retrieving the records: {ex.Message}");
        }
    }
}