using FluentValidation;
using PokemonAPI.Domain.DTOs;

namespace PokemonAPI.Domain.Validators
{
    public class PokemonRequestValidator : AbstractValidator<PokemonRequest>
    {
        public PokemonRequestValidator()
        {
            RuleFor(x => x.Limit)
                .InclusiveBetween(1, 100).WithMessage("Limit precisa estar entre 1 e 100.");
            RuleFor(x => x.Offset)
                .GreaterThanOrEqualTo(0).WithMessage("Offset precisa ser um inteiro positivo.");
        }
    }
}
