using FluentValidation;
using TransactionService.Transaction.Application.Commands;

namespace TransactionService.Transaction.Application.Validators;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.SourceAccountId).NotEmpty();
        RuleFor(x => x.TargetAccountId).NotEmpty();
        RuleFor(x => x.TransferTypeId).NotEmpty();
        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage("El valor de la transacción debe de ser MAYOR a CERO.");

        RuleFor(x => x.SourceAccountId)
            .NotEqual(x => x.TargetAccountId)
            .WithMessage("La cuenta de DESTINO y ORIGEN no pueden ser la misma.");
    }
}