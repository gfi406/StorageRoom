using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using StorageRoom.Models.Dtos;
using StorageRoom.Models.Entity;
using StorageRoom.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;


namespace StorageRoom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaggageController : ControllerBase
    {
        private readonly IBaggageService _baggageService;
        private readonly IBus _bus;

        public BaggageController(IBaggageService baggageService, IBus bus)
        {
            _baggageService = baggageService;
            _bus = bus;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Получить список всех багажей", Description = "Возвращает список всех зарегистрированных багажей.")]
        [SwaggerResponse(200, "Список багажей успешно возвращен", typeof(IEnumerable<BaggageDto>))]
        [SwaggerResponse(500, "Внутренняя ошибка сервера")]
        public async Task<ActionResult<IEnumerable<BaggageDto>>> GetBaggage()
        {
            var baggage = await _baggageService.GetBaggagesAsync();

            var baggageDto = baggage.Select(b => new BaggageDto
            {
                Id = b.Id,
                BaggageTag = b.BaggageTag,
                Weight = b.Weight,
                PassengerId = b.PassengerId,
                Links = new List<LinkDto>
                {
                    new LinkDto(Url.Link("GetBaggage", null), "self", "GET"),
                    new LinkDto(Url.Link("GetBaggageById", new { id = b.Id }), "get_by_id", "GET"),
                    new LinkDto(Url.Link("UpdateBaggage", new { id = b.Id }), "update_baggage", "PUT"),
                    new LinkDto(Url.Link("DeleteBaggage", new { id = b.Id }), "delete_baggage", "DELETE"),
                    new LinkDto(Url.Link("AddBaggage", null), "add_baggage", "POST")
                }
            }).ToList();
            
            return Ok(baggageDto);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Получить багаж по идентификатору", Description = "Возвращает информацию о багаже по идентификатору.")]
        [SwaggerResponse(200, "Багаж успешно возвращен", typeof(BaggageDto))]
        [SwaggerResponse(404, "Багаж не найден")]
        public async Task<ActionResult<BaggageDto>> GetBaggageById(Guid id)
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
                PassengerId = baggage.PassengerId,
                Links = new List<LinkDto>
                {
                    new LinkDto(Url.Link("GetBaggage", null), "self", "GET"),
                    new LinkDto(Url.Link("GetBaggageById", new { id = baggage.Id }), "get_by_id", "GET"),
                    new LinkDto(Url.Link("UpdateBaggage", new { id = baggage.Id }), "update_baggage", "PUT"),
                    new LinkDto(Url.Link("DeleteBaggage", new { id = baggage.Id }), "delete_baggage", "DELETE"),
                    new LinkDto(Url.Link("AddBaggage", null), "add_baggage", "POST")
                }
            };
            await PublishNewBaggage(baggage);
            return Ok(baggageDto);
        }


        [HttpPost]
        [SwaggerOperation(Summary = "Добавить новый багаж", Description = "Добавляет новый багаж в систему.")]
        [SwaggerResponse(201, "Багаж успешно добавлен", typeof(BaggageDto))]
        [SwaggerResponse(400, "Ошибка при добавлении багажа")]
        public async Task<ActionResult<BaggageDto>> AddBaggage(Baggage baggage)
        {
            try
            {
                var createdBaggage = await _baggageService.AddBaggegeAsync(baggage);
                

                var baggageDto = new BaggageDto
                {
                    Id = createdBaggage.Id,
                    BaggageTag = createdBaggage.BaggageTag,
                    Weight = createdBaggage.Weight,
                    PassengerId = createdBaggage.PassengerId,
                    Links = new List<LinkDto>
                    {
                        new LinkDto(Url.Link("GetBaggage", null), "self", "GET"),
                        new LinkDto(Url.Link("GetBaggageById", new { id = createdBaggage.Id }), "get_by_id", "GET"),
                        new LinkDto(Url.Link("UpdateBaggage", new { id = createdBaggage.Id }), "update_baggage", "PUT"),
                        new LinkDto(Url.Link("DeleteBaggage", new { id = createdBaggage.Id }), "delete_baggage", "DELETE"),
                        new LinkDto(Url.Link("AddBaggage", null), "add_baggage", "POST")
                    }
                };
                await PublishNewBaggage(createdBaggage);
                return CreatedAtAction(nameof(GetBaggageById), new { id = createdBaggage.Id }, baggageDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error while adding baggage: {ex.Message}");
            }
        }

        [HttpPut("{id:guid}")]
        [SwaggerOperation(Summary = "Обновить информацию о багаже", Description = "Обновляет данные о багаже по его идентификатору.")]
        [SwaggerResponse(200, "Багаж успешно обновлен", typeof(BaggageDto))]
        [SwaggerResponse(400, "Некорректные данные")]
        [SwaggerResponse(404, "Багаж не найден")]
        public async Task<ActionResult<BaggageDto>> UpdateBaggage(Guid id, Baggage baggage)
        {
            if (id != baggage.Id)
            {
                return BadRequest();
            }

            var updatedBaggage = await _baggageService.UpdateBaggageAsync(baggage);

            var baggageDto = new BaggageDto
            {
                Id = updatedBaggage.Id,
                BaggageTag = updatedBaggage.BaggageTag,
                Weight = updatedBaggage.Weight,
                PassengerId = updatedBaggage.PassengerId,
                Links = new List<LinkDto>
                {
                    new LinkDto(Url.Link("GetBaggage", null), "self", "GET"),
                    new LinkDto(Url.Link("GetBaggageById", new { id = updatedBaggage.Id }), "get_by_id", "GET"),
                    new LinkDto(Url.Link("UpdateBaggage", new { id = updatedBaggage.Id }), "update_baggage", "PUT"),
                    new LinkDto(Url.Link("DeleteBaggage", new { id = updatedBaggage.Id }), "delete_baggage", "DELETE"),
                    new LinkDto(Url.Link("AddBaggage", null), "add_baggage", "POST")
                }
            };

            return Ok(baggageDto);
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(Summary = "Удалить багаж", Description = "Удаляет багаж по его идентификатору.")]
        [SwaggerResponse(200, "Багаж успешно удален")]
        [SwaggerResponse(404, "Багаж не найден")]
        public async Task<IActionResult> DeleteBaggage(Guid id)
        {
            var baggage = await _baggageService.GetBaggageByIdAsync(id);
            if (baggage == null)
            {
                return NotFound();
            }

            await _baggageService.DeleteBaggageAsync(id);
            return Ok(new { message = $"Baggage {id} deleted" });
        }
        [HttpPost("Tickets")]
        public async Task<IActionResult> PublishAllBaggages()
        {
            try
            {
                var baggage = await _baggageService.GetBaggagesAsync();

                // Отправка сообщений для каждого багажа
                foreach (var b in baggage)
                {
                    await PublishNewBaggage(b);
                }

                return Ok(new { message = "Сообщения для всех багажей успешно отправлены." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при отправке сообщений: {ex.Message}");
            }
        }
       

        private async Task PublishNewBaggage(Baggage baggage)
        {
            var message = baggage.ToBaggageMessage();
            await _bus.PubSub.PublishAsync(message);
        }
    }
    /*
     test JSON
    {
        "weight": 23.5,
        "baggageTag": "TAG53",
        "passengerId": "0001417e-a34f-495b-9d56-a9649a8139e8"
    }

     */
}
