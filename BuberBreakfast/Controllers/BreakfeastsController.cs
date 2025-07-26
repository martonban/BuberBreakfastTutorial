using Microsoft.AspNetCore.Mvc;
using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using BuberBreakfast.ServiceErrors;

namespace BuberBreakfast.Controllers;

[ApiController]
// Prefixed by the name of the class without the "Controller"
[Route("[controller]")]
public class BreakfastsController : ControllerBase
{
    private readonly IBreakfastService _breakfastService;

    public BreakfastsController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }
    
    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        var breakfast = new Breakfast(
            Guid.NewGuid(),
            request.Name, 
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            DateTime.UtcNow,
            request.Savory,
            request.Sweet
        );
        // TODO: Save breakfast to db
        _breakfastService.CreateBreakfast(breakfast);

        var response = new BreakfastResponse(
        breakfast.Id,
        breakfast.Name,
        breakfast.Description,
        breakfast.StartDateTime,
        breakfast.EndDateTime,
        breakfast.LastModifiedDateTime,
        breakfast.Savory,
        breakfast.Sweet
        );
        return CreatedAtAction(
            actionName: nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            value: response
        );
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);

        if (getBreakfastResult.IsError &&
            getBreakfastResult.FirstError == Errors.Breakfast.NotFound)
        {
            return NotFound();
        }

        var breakfast = getBreakfastResult.Value;
        
        var response = new BreakfastResponse(
        breakfast.Id,
        breakfast.Name,
        breakfast.Description,
        breakfast.StartDateTime,
        breakfast.EndDateTime,
        breakfast.LastModifiedDateTime,
        breakfast.Savory,
        breakfast.Sweet);

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateBreakfast(Guid id, UpdateBreakfastRequest request)
    {
        var breakfast = new Breakfast(
            id,
            request.Name, 
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            DateTime.UtcNow,
            request.Savory,
            request.Sweet);

        _breakfastService.UpdateBreakfast(breakfast);

        //TODO: Return 201 if a new Breakfast was created
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeletetBreakfast(Guid id)
    {
        _breakfastService.DeleteBreakfast(id);
        return NoContent();
    }
}