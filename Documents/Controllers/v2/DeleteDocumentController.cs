using Documents.Data.Handlers;
using Documents.Data.Handlers.Commands;
using Documents.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("v2")]
[ApiController]
[Authorize]
public class DeleteDocumentController : ControllerBase
{
    private readonly IDeleteDocumentCommandHandler _deleteDocumentCommandHandler;

    public DeleteDocumentController(IDeleteDocumentCommandHandler deleteDocumentCommandHandler)
    {
        _deleteDocumentCommandHandler = deleteDocumentCommandHandler;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocument(int id)
    {
        await _deleteDocumentCommandHandler.Handle(new DeleteDocumentCommand(id));

        return NoContent();
    }
}