using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using StorageRoom.Models.Dtos;
using StorageRoom.Models.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PassengerController : ControllerBase
{
    private readonly IPassengerService _passengerService;

    public PassengerController(IPassengerService passengerService)
    {
        _passengerService = passengerService;
    }

    [HttpGet(Name = "GetPassengers")]
    public async Task<ActionResult<List<Passenger>>> GetPassengers()
    {
        var passengers = await _passengerService.GetPassengersAsync();
        if (passengers == null)
        {
            return NotFound();
        }

        var passengerDtos = passengers.Select(passenger => new PassengerDto
        {
            Id = passenger.Id,
            FirstName = passenger.FirstName,
            LastName = passenger.LastName,
            FlightId = passenger.Flight.Id,
           // Baggages = passenger.Baggages,
            Links = new List<LinkDto>
        {       
                // жесточайший костыль ⬇️⬇️⬇️ надо поменять!!
                new LinkDto(Url.Link("GetPassengers",null), "self", "GET"),
                new LinkDto(Url.Link("GetPassengerById", new { id = passenger.Id }), "get_by_id", "GET"),
                new LinkDto(Url.Link("UpdatePassenger", new { id = passenger.Id }), "update_passenger", "PUT"),
                new LinkDto(Url.Link("DeletePassenger", new { id = passenger.Id }), "delete_passenger", "DELETE"),
                new LinkDto(Url.Link("AddPassenger", null), "add_passenger", "POST")
        }
        }).ToList();

        return Ok(passengerDtos);
    }
    [HttpGet("{id}", Name = "GetPassengerById")]
    public async Task<ActionResult<PassengerDto>> GetPassengerById(Guid id)
    {
        var passenger = await _passengerService.GetPassengerByIdAsync(id);
        if (passenger == null)
        {
            return NotFound();
        }

        var passengerDto = new PassengerDto
        {
            Id = passenger.Id,
            FirstName = passenger.FirstName,
            LastName = passenger.LastName,
            FlightId = passenger.Flight.Id,
            //Baggages = passenger.Baggages,
            Links = new List<LinkDto>
            {
                // жесточайший костыль ⬇️⬇️⬇️ надо поменять!!
                new LinkDto(Url.Link("GetPassengers",null), "get_all", "GET"),
                new LinkDto(Url.Link("GetPassengerById", new { id = passenger.Id }), "self", "GET"),
                new LinkDto(Url.Link("UpdatePassenger", new { id = passenger.Id }), "update_passenger", "PUT"),
                new LinkDto(Url.Link("DeletePassenger", new { id = passenger.Id }), "delete_passenger", "DELETE"),
                new LinkDto(Url.Link("AddPassenger", null), "add_passenger", "POST")
            }
        };




        return Ok(passengerDto);
    }


    [HttpPost(Name = "AddPassenger")]
    public async Task<ActionResult<Passenger>> AddPassenger(Passenger passenger)
    {
        var createdPassenger = await _passengerService.AddPassengerAsync(passenger);
        return CreatedAtAction(nameof(GetPassengerById), new { id = createdPassenger.Id }, createdPassenger);
    }

    [HttpPut("{id}", Name = "UpdatePassenger")]
    public async Task<ActionResult<Passenger>> UpdatePassenger(Guid id, Passenger passenger)
    {
        if (id != passenger.Id)
        {
            return BadRequest();
        }

        var updatedPassenger = await _passengerService.UpdatePassengerAsync(passenger);
        return Ok(updatedPassenger);
    }

    [HttpDelete("{id}", Name = "DeletePassenger")]
    public async Task<IActionResult> DeletePassenger(Guid id)
    {
        await _passengerService.DeletePassengerAsync(id);
        return Ok();
    }


}