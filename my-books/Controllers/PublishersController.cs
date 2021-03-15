using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using my_books.Data.ViewModel;
using my_books.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private PublisherService _publisherService;

        public PublishersController(PublisherService publisherService)
        {
            _publisherService = publisherService;

        }

        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher)
        {
            try
            {
                var newPublisher = _publisherService.AddPublisher(publisher);
                return Created(HttpContext.Request.Path , newPublisher);
            }
            catch (PublisherNameException ex)
            {
                return BadRequest($"{ex.Message} , Publisher Name: {ex.PublisherName}");
            
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get-publisher-by-id/{publisherId}")]
        public IActionResult GetPublisherById(int publisherId)
        {

            //throw new Exception("This is an exception that will be handled by middleware");

            var response = _publisherService.GetPublisherById(publisherId);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }

        }


        [HttpGet("get-publisher-books-with-authors/{publisherId}")]
        public IActionResult AddPublisher(int publisherId)
        {
            return Ok(_publisherService.GetPublisherData(publisherId));
        }

        [HttpDelete("delete-publisher-by-id/{publisherId}")]
        public IActionResult DeletePublisherById(int publisherId)
        {
            try
            {
                //int num1 = 1;
                //int num2 = 0;
                //int result = num1 / num2;
                _publisherService.DeletePublisherById(publisherId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
            
        }
    }
}
