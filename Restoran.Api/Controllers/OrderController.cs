using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restoran.Api.DTOs;
using Restoran.Business.Services;
using Restoran.Data.Entities;
using Restoran.Data.Repository;

namespace Restoran.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserRepository _userRepository;

        public OrderController(IOrderService orderService, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _orderService = orderService;
        }
        [HttpPost("createorder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto dto)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(dto.TableId, dto.WaiterId);
                var waiter = await _userRepository.GetByIdAsync(order.WaiterId);
                var response = new OrderResponseDto
                {
                    Id = order.Id,
                    TableId = dto.TableId,
                    WaiterName = waiter?.UserName ?? "Bilinmiyor",
                    OrderItems = new List<OrderItemResponseDto>(),
                    StatusInformation = order.PaymentStatus.ToString(),
                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("addorderitem")]
        public async Task<IActionResult> AddOrderItem([FromBody] AddOrderItemRequestDto dto)
        {
            try

            {
                var order = await _orderService.AddOrderItemAsync(dto.OrderId, dto.ProductId, dto.Quantity, dto.Note);
                var waiter = await _userRepository.GetByIdAsync(order.WaiterId);
                var response = new OrderResponseDto
                {
                    Id = order.Id,
                    TableId = order.TableId,
                    WaiterName = waiter?.UserName ?? "Bilinmiyor",
                    OrderItems = order.OrderItems.Select(x => new OrderItemResponseDto
                    {
                        ProductName = x.Product.Name,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                    }).ToList(),
                    StatusInformation = order.PaymentStatus.ToString(),

                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("pay")]

        public async Task<IActionResult> PayOrder([FromBody] PayOrderRequestDto dto)
        {
            try
            {
                var order = await _orderService.PayOrderAsync(dto.OrderId, dto.PaymentMethod);
                var waiter = await _userRepository.GetByIdAsync(order.WaiterId);
                var response = new OrderResponseDto
                {
                    Id = order.Id,
                    TableId = order.TableId,
                    WaiterName = waiter?.UserName ?? "Bilinmiyor",
                    OrderItems = order.OrderItems.Select(x => new OrderItemResponseDto
                    {
                        ProductName = x.Product.Name,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                    }).ToList(),
                    StatusInformation = order.PaymentStatus.ToString(),
                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("kitchenview")]

        public async Task<IActionResult> KitchenViewer()
        {
            var entry = await _orderService.GetAllUnPaidOrdersAsync();
            var results = new List<OrderResponseDto>();

            foreach (var entries in entry)
            {
                var info = await _userRepository.GetByIdAsync(entries.WaiterId);
                results.Add(new OrderResponseDto
                {
                    Id = entries.Id,
                    StatusInformation = entries.PaymentStatus.ToString(),
                    WaiterName = info?.UserName ?? "Unknown",
                    TableId = entries.TableId,
                    OrderItems = entries.OrderItems.Select(x => new OrderItemResponseDto
                    {
                        ProductName = x.Product.Name,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice

                    }).ToList()


                }
                );


            }
            return Ok(results);
        }

    }
}






