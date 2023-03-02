using FluentValidation;
using OpenDocs.API.Contracts.Requests;

namespace OpenDocs.API.Validators
{
    public class UpdateStoragePathRequestValidator : AbstractValidator<UpdateStoragePathRequest>
    {
        public UpdateStoragePathRequestValidator() 
        {
            RuleFor(c => c.Path)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("400")
                .WithMessage("Storage path cannot be empty");
        }
    }
}
