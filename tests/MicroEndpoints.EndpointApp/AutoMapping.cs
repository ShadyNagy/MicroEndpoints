using AutoMapper;
using MicroEndpoints.EndpointApp.DomainModel;
using MicroEndpoints.EndpointApp.Endpoints.Authors;

namespace MicroEndpoints.EndpointApp;

public class AutoMapping : Profile
{
  public AutoMapping()
  {
    CreateMap<CreateAuthorCommand, Author>();
    CreateMap<UpdateAuthorCommand, Author>();
    CreateMap<UpdateAuthorCommandById, Author>();

    CreateMap<Author, CreateAuthorResult>();
    CreateMap<Author, UpdatedAuthorResult>();
    CreateMap<Author, UpdatedAuthorByIdResult>();
    CreateMap<Author, AuthorListResult>();
    CreateMap<Author, AuthorResult>();
  }
}
