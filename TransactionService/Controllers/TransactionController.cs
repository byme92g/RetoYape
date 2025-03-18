using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Messaging;
using TransactionService.Transaction.Application.Commands;

namespace TransactionService.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController : ControllerBase
{
    private readonly IKafkaProducer<TransactionPostDto> _kafkaProducer;
    private readonly IMediator _mediator;

    public TransactionController(IKafkaProducer<TransactionPostDto> kafkaProducer, IMediator mediator)
    {
        _kafkaProducer = kafkaProducer;
        _mediator = mediator;
    }

    [HttpPost("Yapear")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand transaction)
    {
        if (transaction == null) return BadRequest("Invalid transaction data");

        var transactionExternalIdResponse = await _mediator.Send(transaction);

        return Ok(new { TransactionExternalId = transactionExternalIdResponse });
    }

    [HttpGet("VerYapeos")]
    public async Task<IActionResult> ListTransactions()
    {
        return Ok();
    }
}
