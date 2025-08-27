namespace CqrsApiExample.Controllers
{
    using CqrsApiExample.Features.Products.Commands;
    using CqrsApiExample.Models;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
        {
            var res = new ResultViewModel<Product>();
            try
            {
               var command = new CreateProductCommand(productDTO);
               res = await _mediator.Send(command);

                return CreatedAtAction(nameof(GetProductById), new { id = res.Data.ID }, res);
            }
            catch (Exception ex) { 
                res.Success = false;
                res.Message = ex.Message;
                return StatusCode(500, res);
            }
            
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var res = new ResultViewModel<Product>();
            try {

                var query = new Features.Products.Queries.GetProductById(id);
                res = _mediator.Send(query).Result;
                return Ok(res);

            }
            catch(Exception ex) {
                res.Success = false;
                res.Message = ex.Message;
                return BadRequest(res);
            }
           
        }
    }
}