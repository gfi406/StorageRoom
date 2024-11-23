using Microsoft.AspNetCore.Mvc;
using StorageRoom.Models.Dtos;
using StorageRoom.Models.Entity;
using StorageRoom.Service;
using StorageRoom.Service.serv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class FlightController : Controller
{
    private readonly IFlightService _flightService;

    public FlightController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet(Name = "GetFlights")]
    public async Task<ActionResult<List<FlightDto>>> GetFlights()
    {
        var flights = await _flightService.GetFlightsAsync();

        var flightDtoList = flights.Select(flight => new FlightDto
        {
            Id = flight.Id,
            FlightNumber = flight.FlightNumber,
            Destination = flight.Destination,
            DepartureTime = flight.DepartureTime,
            ArrivalTime = flight.ArrivalTime,
            Passengers = flight.Passengers,
            Links = new List<LinkDto>
            {
                new LinkDto(Url.Link("GetFlights", null), "self", "GET"),
                new LinkDto(Url.Link("GetFlightById", new { id = flight.Id }), "get_by_id", "GET"),
                new LinkDto(Url.Link("UpdateFlight", new { id = flight.Id }), "update_flight", "PUT"),
                new LinkDto(Url.Link("DeleteFlight", new { id = flight.Id }), "delete_flight", "DELETE"),
                new LinkDto(Url.Link("AddFlight", null), "add_flight", "POST")
            }
        }).ToList();

        return Ok(flightDtoList);
    }

    [HttpGet("{id}", Name = "GetFlightById")]
    public async Task<ActionResult<FlightDto>> GetFlightById(Guid id)
    {
        var flight = await _flightService.GetFlightByIdAsync(id);
        if (flight == null)
        {
            return NotFound();
        }

        var flightDto = new FlightDto
        {
            Id = flight.Id,
            FlightNumber = flight.FlightNumber,
            Destination = flight.Destination,
            DepartureTime = flight.DepartureTime,
            ArrivalTime = flight.ArrivalTime,
            Passengers = flight.Passengers,
            Links = new List<LinkDto>
            {
                new LinkDto(Url.Link("GetFlights", null), "self", "GET"),
                new LinkDto(Url.Link("GetFlightById", new { id = flight.Id }), "get_by_id", "GET"),
                new LinkDto(Url.Link("UpdateFlight", new { id = flight.Id }), "update_flight", "PUT"),
                new LinkDto(Url.Link("DeleteFlight", new { id = flight.Id }), "delete_flight", "DELETE"),
                new LinkDto(Url.Link("AddFlight", null), "add_flight", "POST")
            }
        };

        return Ok(flightDto);
    }

    [HttpPost(Name = "AddFlight")]
    public async Task<ActionResult<Flight>> AddFlight(Flight flight)
    {
        var createdFlight = await _flightService.AddFlightAsync(flight);
        return CreatedAtAction(nameof(GetFlightById), new { id = createdFlight.Id }, createdFlight);
    }

    [HttpPut("{id}", Name = "UpdateFlight")]
    public async Task<ActionResult<Flight>> UpdateFlight(Guid id, Flight flight)
    {
        if (id != flight.Id)
        {
            return BadRequest();
        }

        var updatedFlight = await _flightService.UpdateFlightAsync(flight);
        return Ok(updatedFlight);
    }

    [HttpDelete("{id}", Name = "DeleteFlight")]
    public async Task<IActionResult> DeleteFlight(Guid id)
    {
        await _flightService.DeleteFlightAsync(id);
        return Ok();
    }
}