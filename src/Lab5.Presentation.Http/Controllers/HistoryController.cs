using Lab5.Application.Contracts.History;
using Lab5.Application.Contracts.History.Dtos;
using Lab5.Application.Contracts.History.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab5.Presentation.Http.Controllers;

[ApiController]
[Route("api/history")]
public sealed class HistoryController : ControllerBase
{
    private readonly IOperationHistoryService _historyService;

    public HistoryController(IOperationHistoryService historyService)
    {
        _historyService = historyService;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<OperationRecordDto>> GetHistory(
        [FromHeader(Name = "X-User-Token")] Guid userToken)
    {
        var request = new GetHistory.Request(userToken);
        GetHistory.Response response = _historyService.GetHistory(request);

        return response switch
        {
            GetHistory.Response.Success success => Ok(success.Records),
            GetHistory.Response.Unauthorized unauthorized => Unauthorized(),
            _ => throw new UnreachableException(),
        };
    }
}
