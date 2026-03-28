using FluentValidation;

namespace Application.Services.Category.Request.CreateCategory;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(500)
            .Matches("^[a-z0-9-]+$")
            .WithMessage("Slug must be lowercase, alphanumeric, and hyphen-separated.");
    }
}