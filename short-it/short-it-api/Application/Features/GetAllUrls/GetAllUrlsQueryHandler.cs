using Application.Abstractions;
using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.GetAllUrls;

public sealed class GetAllUrlsQueryHandler : BaseHandler<GetAllUrlsQuery, IReadOnlyList<UrlResponse>>,
    IRequestHandler<GetAllUrlsQuery, IReadOnlyList<UrlResponse>>
{
    private readonly IUrlRepository _urlRepository;

    public GetAllUrlsQueryHandler(IUrlRepository urlRepository, IMapper mapper, ILogger<GetAllUrlsQueryHandler> logger)
        : base(mapper, logger)
    {
        _urlRepository = urlRepository;
    }

    public async Task<IReadOnlyList<UrlResponse>> Handle(GetAllUrlsQuery request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Logger.LogInformation("Processing handler {handler} with request {@request}", RequestName, request);
            var urls = await _urlRepository.GetAllAsync(cancellationToken);

            if (0 == urls.Count)
            {
                throw new NotFoundException("No urls found");
            }

            var response = Mapper.Map<IReadOnlyList<UrlResponse>>(urls);

            return response;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error when getting the urls due to: {message}", ex.Message);
            throw new Exception($"Error occured when retrieving the user records: {ex.Message}");
        }
    }
}