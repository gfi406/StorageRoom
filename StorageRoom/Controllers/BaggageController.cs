using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using StorageRoom.Models.Dtos;
using StorageRoom.Models.Entity;
using StorageRoom.Service;
using System;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BaggageController : Controller
{
    private readonly IBaggageService _baggageService;


    public BaggageController(IBaggageService baggageService)
    {
        _baggageService = baggageService;
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
        if(baggage == null)
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

}





