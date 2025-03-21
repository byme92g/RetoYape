using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionService.Transaction.Application.Commands;
using TransactionService.Transaction.Application.Mappers;
using TransactionService.Transaction.Domain.Interfaces;

namespace TransactionService.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IValidator<CreateTransactionCommand> _validator;
    private readonly IMediator _mediator;


    public TransactionController(
        IMediator mediator,
        ITransactionRepository transactionRepository,
        IValidator<CreateTransactionCommand> validator)
    {
        _mediator = mediator;
        _transactionRepository = transactionRepository;
        _validator = validator;
    }

    [HttpPost("Yapear")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand transaction)
    {
        try
        {
            if (transaction == null) return BadRequest("Datos invalidos.");

            var validationResult = await _validator.ValidateAsync(transaction);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(errors);
            }

            var transactionExternalIdResponse = await _mediator.Send(transaction);

            return Ok(new { TransactionExternalId = transactionExternalIdResponse });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("BuscarYapeo")]
    public async Task<IActionResult> GetTransactionById([FromQuery] Guid externalId)
    {
        try
        {
            var transaction = await _transactionRepository.GetByExternalIdAsync(externalId);

            if (transaction is null) return NotFound("No se encontró esta transacción.");

            return Ok(transaction.ToReadDto());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("VerYapeos")]
    public async Task<IActionResult> GetTransactions()
    {
        try
        {
            var transactions = await _transactionRepository.GetAll();

            return Ok(transactions.ToReadDto());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
