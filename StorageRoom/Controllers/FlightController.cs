using Microsoft.AspNetCore.Mvc;
using StorageRoom.Api.Controllers;
using StorageRoom.Api.Responses;
using StorageRoom.Api.Request;
using StorageRoom.Models.Dtos;
using StorageRoom.Models.Entity;
using StorageRoom.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class FlightController : ControllerBase, IFlightApi
{
    private readonly IFlightService _flightService;

    public FlightController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    //[HttpGet(Name = "GetFlights")]
    //public async Task<ActionResult<List<FlightDto>>> GetFlights()
    //{
    //    var flights = await _flightService.GetFlightsAsync();

    //    var flightDtoList = flights.Select(flight => new FlightDto
    //    {
    //        Id = flight.Id,
    //        FlightNumber = flight.FlightNumber,
    //        Destination = flight.Destination,
    //        DepartureTime = flight.DepartureTime,
    //        ArrivalTime = flight.ArrivalTime,
    //        Passengers = flight.Passengers,
    //        Links = new List<LinkDto>
    //        {
    //            new LinkDto(Url.Link("GetFlights", null), "self", "GET"),
    //            new LinkDto(Url.Link("GetFlightById", new { id = flight.Id }), "get_by_id", "GET"),
    //            new LinkDto(Url.Link("UpdateFlight", new { id = flight.Id }), "update_flight", "PUT"),
    //            new LinkDto(Url.Link("DeleteFlight", new { id = flight.Id }), "delete_flight", "DELETE"),
    //            new LinkDto(Url.Link("AddFlight", null), "add_flight", "POST")
    //        }
    //    }).ToList();

    //    return Ok(flightDtoList);
    //}
    [HttpGet(Name = "GetFlights")]
    public async Task<ActionResult<IEnumerable<FlightResponse>>> GetFlights()
    {
        var flights = await _flightService.GetFlightsAsync();

        var flightResponseList = flights.Select(flight => new FlightResponse
        {
            Id = flight.Id,
            FlightNumber = flight.FlightNumber,
            Destination = flight.Destination,
            DepartureTime = flight.DepartureTime,
            ArrivalTime = flight.ArrivalTime,
                       
        }).ToList();

        return Ok(flightResponseList);
    }

    [HttpGet("{id}", Name = "GetFlightById")]
    public async Task<ActionResult<FlightResponse>> GetFlightById(Guid id)
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

    //[HttpPost(Name = "AddFlight")]
    //public async Task<ActionResult<FlightResponse>> AddFlight(FlightRequest flight)
    //{
    //    var createdFlight = await _flightService.AddFlightAsync(flight);
    //    return CreatedAtAction(nameof(GetFlightById), new { id = createdFlight.Id }, createdFlight);
    //}|
    [HttpPost(Name = "AddFlight")]
    public async Task<ActionResult<FlightResponse>> AddFlight(FlightRequest request)
    {
        var flight = new Flight
        {
            FlightNumber = request.FlightNumber,
            Destination = request.Destination,
            DepartureTime = request.DepartureTime,
            ArrivalTime = request.ArrivalTime
        };

        var createdFlight = await _flightService.AddFlightAsync(flight);
        var flightResponse = new FlightResponse
        {
            Id = createdFlight.Id,
            FlightNumber = createdFlight.FlightNumber,
            Destination = createdFlight.Destination,
            DepartureTime = createdFlight.DepartureTime,
            ArrivalTime = createdFlight.ArrivalTime,
            
        };

        

        return CreatedAtAction(nameof(GetFlightById), new { id = createdFlight.Id }, flightResponse);
    }

    [HttpPut("{id:guid}", Name = "UpdateFlight")]
    public async Task<ActionResult<FlightResponse>> UpdateFlight(Guid id, FlightRequest request)
    {
        var flight = await _flightService.GetFlightByIdAsync(id);
        if (flight == null) return NotFound();

        flight.FlightNumber = request.FlightNumber;
        flight.Destination = request.Destination;
        flight.DepartureTime = request.DepartureTime;
        flight.ArrivalTime = request.ArrivalTime;

        await _flightService.UpdateFlightAsync(flight);

        var flightResponse = new FlightResponse
        {
            Id = flight.Id,
            FlightNumber = flight.FlightNumber,
            Destination = flight.Destination,
            DepartureTime = flight.DepartureTime,
            ArrivalTime = flight.ArrivalTime,
           
        };

        return Ok(flightResponse);
    }

    [HttpDelete("{id}", Name = "DeleteFlight")]
    public async Task<IActionResult> DeleteFlight(Guid id)
    {
        await _flightService.DeleteFlightAsync(id);
        return Ok();
    }
}