using FluentValidation;
using OpenDocs.API.Contracts.Requests;

namespace OpenDocs.API.Validators
{
    public class SyncApplicationDocsRequestValidator : AbstractValidator<SyncApplicationDocsRequest>
    {
        public SyncApplicationDocsRequestValidator() 
        {
            RuleFor(c => c.Environment)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("400")
                .WithMessage("Enviroment cannot be empty")
                .Must(s => new List<string>() { "Production", "Development", "Testing" }.Contains(s))
                .WithErrorCode("400")
                .WithMessage("Invalid environment");

            RuleFor(c => c.ApplicationName)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("400")
                .WithMessage("Application name cannot be empty");

            RuleFor(c => c.AccessKey)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("400")
                .WithMessage("Access key name cannot be empty");

            RuleFor(c => c.SwaggerFile)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithErrorCode("400")
                .WithMessage("Swagger file cannot be empty")
                .Must(c => c.ContentType.Contains("json"))
                .WithErrorCode("400")
                .WithMessage("Swagger file should be a json file");
        }
    }
}
