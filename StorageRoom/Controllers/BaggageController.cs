using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using StorageRoom.Models.Dtos;
using StorageRoom.Models.Entity;
using StorageRoom.RabbitMQ;
using StorageRoom.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class BaggageController : Controller
{
    private readonly IBaggageService _baggageService;
    private readonly IBus _bus;

    public BaggageController(IBaggageService baggageService, IBus bus)
    {
        _baggageService = baggageService;
        _bus = bus;
    }

    [HttpGet(Name = "GetBagagge")]
    public async Task<ActionResult<List<Baggage>>> GetBaggage()
    {
        var baggage = await _baggageService.GetBaggagesAsync();


        var baggageDto = baggage.Select(baggage => new BaggageDto
        {
            Id = baggage.Id,
            BaggageTag = baggage.BaggageTag,
            Weight = baggage.Weight,
            Passenger = baggage.Passenger,
            Links = new List<LinkDto>
        {

                new LinkDto(Url.Link("GetBaggage",null), "self", "GET"),
                new LinkDto(Url.Link("GetBaggageById", new { id = baggage.Id }), "get_by_id", "GET"),
                new LinkDto(Url.Link("UpdateBaggage", new { id = baggage.Id }), "update_baggage", "PUT"),
                new LinkDto(Url.Link("DeleteBaggage", new { id = baggage.Id }), "delete_baggage", "DELETE"),
                new LinkDto(Url.Link("AddBaggage", null), "add_baggage", "POST")
        }

        }).ToList();
        return Ok(baggageDto);
    }


    [HttpGet("{id}", Name = "GetBaggageById")]
    public async Task<ActionResult<List<Baggage>>> GetBaggageById(Guid id)
    {
        var baggage = await _baggageService.GetBaggageByIdAsync(id);
        if (baggage == null)
        {
            return NotFound();
        }
        var baggageDto = new BaggageDto
        {
            Id = baggage.Id,
            BaggageTag = baggage.BaggageTag,
            Weight = baggage.Weight,
            Passenger = baggage.Passenger,
            Links = new List<LinkDto>
            {

                new LinkDto(Url.Link("GetBaggage",null), "self", "GET"),
                new LinkDto(Url.Link("GetBaggageById", new { id = baggage.Id }), "get_by_id", "GET"),
                new LinkDto(Url.Link("UpdateBaggage", new { id = baggage.Id }), "update_baggage", "PUT"),
                new LinkDto(Url.Link("DeleteBaggage", new { id = baggage.Id }), "delete_baggage", "DELETE"),
                new LinkDto(Url.Link("AddBaggage", null), "add_baggage", "POST")
        }
        };
        return Ok(baggageDto);
    }

    [HttpPost(Name = "AddBaggage")]
    public async Task<ActionResult<Baggage>> AddBaggage(Baggage baggage)
    {
        var createdBaggage = await _baggageService.AddBaggegeAsync(baggage);
        await PublishNewBaggage(createdBaggage);
        return CreatedAtAction(nameof(GetBaggageById), new { id = createdBaggage.Id }, createdBaggage);

    }
    [HttpPost("{id}", Name = "UpdateBaggage")]
    public async Task<ActionResult<Baggage>> UpdateBaggage(Guid id, Baggage baggage)
    {
        if (id != baggage.Id)
        {
            return BadRequest();
        }
        var updatedBaggage = await _baggageService.UpdateBaggageAsync(baggage);
        return Ok(updatedBaggage);
    }
    [HttpDelete("{id}", Name = "DeliteBaggabe")]
    public async Task<IActionResult> DeliteBaggabe(Guid id)
    {
        await _baggageService.DeleteBaggageAsync(id);
        return Ok();
    }

    private async Task PublishNewBaggage(Baggage Baggage)
    {
        var message = Baggage.ToBaggageMessage();
        await _bus.PubSub.PublishAsync(message);
    }



}