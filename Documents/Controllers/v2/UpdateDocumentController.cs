using Documents.Data.Handlers;
using Documents.Data.Handlers.Commands;
using Documents.Models;
using Microsoft.AspNetCore.Mvc;


[Route("v2")]
[ApiController]
public class UpdateDocumentController : ControllerBase
{
    private readonly IUpdateDocumentCommandHandler _updateDocumentCommandHandler;

    public UpdateDocumentController(IUpdateDocumentCommandHandler UpdateDocumentCommandHandler)
    {
        _updateDocumentCommandHandler = UpdateDocumentCommandHandler;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDocument(int id, [FromBody] UpdateDocumentCommand cmd)
    {
        cmd.Id = id;
        await _updateDocumentCommandHandler.Handle(cmd);

        return NoContent();
    }
}