using Microsoft.AspNetCore.Mvc;
 
using Restoran.Api.DTOs;
using Restoran.Data.Repository;
using Restoran.Data.Entities;
using System.Runtime.Serialization;

namespace Restoran.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController:ControllerBase
    {
        private readonly IGenericRepository<Table> _iRepository;
       
        public TableController(IGenericRepository<Table> iRepository)
        {
            _iRepository = iRepository;
        }
        [HttpPost("createaction")]
        public async Task<IActionResult> CreateAction([FromBody]  CreateTableRequestDto dto)
        {
            Table table = new Table {TableNumber = dto.TableNumber, TableStatus = Data.Enums.TableStatusType.Empty,IsActive =true };
            var controlTable = await _iRepository.AddAsync(table);
            await _iRepository.SaveChangesAsync();
            var response = new TableResponseDto {
                Id =controlTable.Id,
                IsActive = controlTable.IsActive,
                TableNumber = controlTable.TableNumber,
                 TableStatus =  controlTable.TableStatus.ToString()
            };
            return Ok(response);
        }
        [HttpGet("gettable")]
        public async Task<IActionResult> GetAllTable()
        { 
        
        var tables = await _iRepository.GetAllAsync();
            var response = tables.Select(x=>new TableResponseDto { Id =x.Id,TableNumber = x.TableNumber, IsActive = x.IsActive, TableStatus = x.TableStatus.ToString() }).ToList();

            return Ok(response);
        }
        [HttpPut("updatestatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateTableStatusRequestDto dto)
        {
            var find = await _iRepository.GetByIdAsync(dto.TableId);
            if (find == null)
            {
                return BadRequest("Masa Bulunamadi");
            }
            find.TableStatus = dto.TableStatus;
            var response = new TableResponseDto 
            {
                Id =find.Id,
                TableNumber=find.TableNumber,
                IsActive=find.IsActive,
                TableStatus=dto.TableStatus.ToString()
            };
            await _iRepository.SaveChangesAsync();
            return Ok(response);
            
        }

    
    }
}
