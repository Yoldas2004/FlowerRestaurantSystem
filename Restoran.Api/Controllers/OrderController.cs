    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Restoran.Api.DTOs;
    using Restoran.Business.Services;
    using Restoran.Data.Repository;

    namespace Restoran.Api.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class OrderController:ControllerBase
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
                    var order = await _orderService.CreateOrderAsync(dto.TableId,dto.WaiterId);
                    var waiter =await _userRepository.GetByIdAsync (order.WaiterId);
                    var response = new OrderResponseDto
                    {
                        Id = order.Id,
                        TableId = dto.TableId,
                        WaiterName =  waiter?.UserName?? "Bilinmiyor",
                        OrderItems =  new List<OrderItemResponseDto>(),
                        StatusInformation =order.PaymentStatus.ToString(),
                    };
                    return Ok(response);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }





            }
        }
    }
