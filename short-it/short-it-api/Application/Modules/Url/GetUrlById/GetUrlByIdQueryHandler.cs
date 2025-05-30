using Application.Abstractions;
using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Modules.Url.GetUrlById;

public sealed class GetUrlByIdQueryHandler :
    BaseHandlerWithValidator<GetUrlByIdQuery, UrlResponse, GetUrlByIdQueryValidator>,
    IRequestHandler<GetUrlByIdQuery, UrlResponse>
{
    private readonly IUrlRepository _urlRepository;

    public GetUrlByIdQueryHandler(IMapper mapper, IUrlRepository repository, ILogger<GetUrlByIdQueryHandler> logger) :
        base(mapper, logger)
    {
        _urlRepository = repository;
    }

    public async Task<UrlResponse> Handle(GetUrlByIdQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Processing handler {handler} with request {@request}", RequestName, request);
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException("Validation payload:", validationResult);
            }

            var url = await _urlRepository.GetByIdAsync(request.UrlId, cancellationToken);

            if (url is null)
            {
                throw new NotFoundException($"Url not found for id: {request.UrlId}");
            }

            var response = Mapper.Map<UrlResponse>(url);

            return response;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when getting the url due to: {message}", ex.Message);
            if (ex is BadRequestException or NotFoundException)
            {
                throw;
            }

            throw new Exception($"Error occured when retrieving the user record: {ex.Message}");
        }
    }
}