using Application.Abstractions;
using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using MediatR;

namespace Application.Features.GetUrlById;

public sealed class GetUrlByIdQueryHandler :
    BaseHandlerWithValidator<GetUrlByIdQuery, UrlResponse, GetUrlByIdQueryValidator>,
    IRequestHandler<GetUrlByIdQuery, UrlResponse>
{
    private readonly IUrlRepository _urlRepository;

    public GetUrlByIdQueryHandler(IMapper mapper, IUrlRepository repository) : base(mapper)
    {
        _urlRepository = repository;
    }

    public async Task<UrlResponse> Handle(GetUrlByIdQuery request, CancellationToken cancellationToken = default)
    {
        try
        {
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
            if (ex is BadRequestException or NotFoundException)
            {
                throw;
            }

            throw new Exception($"Error occured when retrieving the user record: {ex.Message}");
        }
    }
}