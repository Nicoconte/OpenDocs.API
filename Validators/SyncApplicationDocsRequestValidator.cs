using FluentValidation;
using OpenDocs.API.Contracts.Requests;

namespace OpenDocs.API.Validators
{
    public class SyncApplicationDocsRequestValidator : AbstractValidator<SyncApplicationDocsRequest>
    {
        public SyncApplicationDocsRequestValidator() 
        {
            RuleFor(c => c.Environment)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("400")
                .WithMessage("Enviroment cannot be empty")
                .Must(s => new List<string>() { "Production", "Development", "Testing" }.Contains(s))
                .WithErrorCode("400")
                .WithMessage("Invalid environment");

            RuleFor(c => c.ApplicationName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("400")
                .WithMessage("Application name cannot be empty");

            RuleFor(c => c.SwaggerFile.ContentType)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .Must(c => c.Contains("json"))
                .WithErrorCode("400")
                .WithMessage("Swagger file should a json file");
        }
    }
}
