using Application.Abstractions;
using Application.Commons.DTO;
using Application.Contracts.Persistence;
using Application.Exceptions;
using AutoMapper;
using MediatR;

namespace Application.Features.GetAllUrls;

public sealed class GetAllUrlsQueryHandler : BaseHandler<GetAllUrlsQuery, IReadOnlyList<UrlResponse>>,
    IRequestHandler<GetAllUrlsQuery, IReadOnlyList<UrlResponse>>
{
    private readonly IUrlRepository _urlRepository;

    public GetAllUrlsQueryHandler(IUrlRepository urlRepository, IMapper mapper) : base(mapper)
    {
        _urlRepository = urlRepository;
    }

    public async Task<IReadOnlyList<UrlResponse>> Handle(GetAllUrlsQuery request,
        CancellationToken cancellationToken = default)
    {
        try
        {
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
            throw new Exception($"Error occured when retrieving the user records: {ex.Message}");
        }
    }
}