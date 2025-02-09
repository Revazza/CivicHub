using CivicHub.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CivicHub.Application.Features.Persons.Commands.UploadPersonPicture;

public record UploadPersonPictureCommand(long PersonId, IFormFile File) : IRequest<Result<string>>;