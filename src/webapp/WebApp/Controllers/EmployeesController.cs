using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApp.DTOs;
using WebApp.Application.Employees.Commands.Delete;
using WebApp.Application.Employees.Commands.Update;
using WebApp.Application.Employees.Commands.UploadFile;
using WebApp.Application.Employees.Commands.ClearFile;
using WebApp.Application.Employees.Queries.GetById;
using WebApp.Application.Employees.Queries.GetStatistics;
using WebApp.Application.Employees.Queries.GetFile;
using WebApp.Contracts.Pdf;
using WebApp.Application.Employees.Commands.Create;
using WebApp.Application.Employees.Queries.GetPaged;
using WebApp.Application.Employees.Queries.GetAll;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(ISender sender, IPdfService pdfService) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetPaginated([FromQuery] GetEmployeesPagedQuery query) =>
        HandleResult(await sender.Send(query));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) =>
        HandleResult(await sender.Send(new GetEmployeeByIdQuery(id)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        var result = await sender.Send(dto.Adapt<CreateEmployeeCommand>());
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto) =>
        HandleResult(await sender.Send(new UpdateEmployeeCommand(id, dto.FullName, dto.Position, dto.Department, dto.HireDate, dto.Email, dto.Phone, dto.Salary)));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) =>
        HandleResult(await sender.Send(new DeleteEmployeeCommand(id)));

    [HttpPost("{id}/upload-file")]
    public async Task<IActionResult> UploadFile(int id, IFormFile file)
    {
        if (file is null || file.Length == 0) return BadRequest("Invalid file");
        return HandleResult(await sender.Send(new UploadEmployeeFileCommand(id, file.OpenReadStream(), file.FileName, file.Length)));
    }

    [HttpGet("{id}/file")]
    public async Task<IActionResult> DownloadFile(int id)
    {
        var result = await sender.Send(new GetEmployeeFileQuery(id));
        return result.IsSuccess
            ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName)
            : HandleResult(result);
    }

    [HttpDelete("{id}/file")]
    public async Task<IActionResult> ClearFile(int id) =>
        HandleResult(await sender.Send(new ClearEmployeeFileCommand(id)));

    [HttpGet("export-pdf")]
    public async Task<IActionResult> ExportPdf(
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortBy,
        [FromQuery] string? sortDir)
    {
        var result = await sender.Send(new GetAllEmployeesQuery(searchTerm, sortBy, sortDir));
        if (result.IsFailure) return HandleResult(result);

        var rows = result.Value.Select(r => new EmployeePdfRow(
            r.Id, r.FullName, r.Position, r.Department,
            r.HireDate, r.Email, r.Phone, r.Salary, r.CreatedAt));

        var pdf = await pdfService.GenerateEmployeesPdfAsync(rows);
        return File(pdf, "application/pdf", $"Employees_{DateTime.Now:yyyyMMdd}.pdf");
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics() =>
        HandleResult(await sender.Send(new GetEmployeesStatisticsQuery()));
}
