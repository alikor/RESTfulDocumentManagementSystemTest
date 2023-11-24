using Documents.Data.Handlers;
using Documents.Data.Handlers.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("v2")]
[ApiController]
[Authorize]
public class CreateDocumentController : ControllerBase 
{
    private readonly ICreateDocumentCommandHandler _createDocumentCommandHandler;

    public CreateDocumentController(ICreateDocumentCommandHandler createDocumentCommandHandler)
    {
        _createDocumentCommandHandler = createDocumentCommandHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentCommand cmd)
    {
        var id = await _createDocumentCommandHandler.Handle(cmd);


        return CreatedAtAction(nameof(CreateDocument), new { id }, cmd);
    }
}