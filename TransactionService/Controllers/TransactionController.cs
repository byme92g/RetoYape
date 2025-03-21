using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedLib.DTOs;
using SharedLib.Messaging;
using TransactionService.Transaction.Application.Commands;
using TransactionService.Transaction.Application.Mappers;
using TransactionService.Transaction.Domain.Interfaces;

namespace TransactionService.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IKafkaProducer<TransactionPostDto> _kafkaProducer;
    private readonly IMediator _mediator;

    public TransactionController(
        IKafkaProducer<TransactionPostDto> kafkaProducer,
        IMediator mediator,
        ITransactionRepository transactionRepository)
    {
        _kafkaProducer = kafkaProducer;
        _mediator = mediator;
        _transactionRepository = transactionRepository;
    }

    [HttpPost("Yapear")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand transaction)
    {
        try
        {
            if (transaction == null) return BadRequest("Datos invalidos.");

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
