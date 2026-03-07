using CombinationGeneratorAPI.Models;
using FluentValidation;

namespace CombinationGeneratorAPI.Validators
{
    public class GenerateRequestValidator : AbstractValidator<GenerateRequest>
    {
        public GenerateRequestValidator()
        {
            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items list cannot be null")
                .Must(list => list.Count <= 26)
                    .WithMessage("Items list cannot have more than 26 elements (A-Z)")
                .Must(list => list.All(i => i >= 0))
                    .WithMessage("All items must be zero or positive integers");
            // 0 means the letter is absent in the combination

            RuleFor(x => x.Length)
                .GreaterThanOrEqualTo(0).WithMessage("Combination length cannot be negative");
            // Length can be greater than items count; in that case, service returns empty combinations list
        }
    }
}