using FluentValidation;
using OpenDocs.API.Contracts.Requests;

namespace OpenDocs.API.Validators
{
    public class UpdateRetentionDaysRequestValidator : AbstractValidator<UpdateRetentionDaysRequest>
    {
        public UpdateRetentionDaysRequestValidator() 
        {
            RuleFor(c => c.Days)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .WithErrorCode("400")
                .WithMessage("Days cannot be empty");
        }
    }
}
