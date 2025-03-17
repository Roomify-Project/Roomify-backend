using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Roomify.GP.API.Errors;
using Roomify.GP.Core.DTOs.PortfolioPost;
using Roomify.GP.Core.Entities;
using Roomify.GP.Core.Service.Contract;
using Roomify.GP.Service.Services;

namespace Roomify.GP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioPostController : ControllerBase
    {
        private readonly IPortfolioPostService _service;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;

        public PortfolioPostController(IPortfolioPostService service, ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _service = service;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }


        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _service.GetAllAsync();
            var response = _mapper.Map<List<PortfolioPostResponseDto>>(posts);
            return Ok(response);
        }


        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var posts = await _service.GetByUserIdAsync(userId);
            var response = _mapper.Map<List<PortfolioPostResponseDto>>(posts);
            return Ok(response);
        }


        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var post = await _service.GetByIdAsync(id);
            if (post == null) return NotFound(new ApiErrorResponse(404));

            var response = _mapper.Map<PortfolioPostResponseDto>(post);
            return Ok(response);
        }


        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] PortfolioPostDto portfolioPostDto)
        {
            if (portfolioPostDto.ImageFile == null || portfolioPostDto.ImageFile.Length == 0)
                return BadRequest("Image is required");

            var imageUrl = await _cloudinaryService.UploadImageAsync(portfolioPostDto.ImageFile);

            var post = _mapper.Map<PortfolioPost>(portfolioPostDto);
            post.Id = Guid.NewGuid();
            post.ImagePath = imageUrl;
            post.CreatedAt = DateTime.UtcNow;

            await _service.AddAsync(post);
            return Ok(new { message = "Post uploaded successfully.", imageUrl });
        }


        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PortfolioPostUpdateDto portfolioPostDto)
        {
            if (portfolioPostDto == null || string.IsNullOrWhiteSpace(portfolioPostDto.Description))
                return BadRequest(new ApiErrorResponse(400, "Description is required"));

            var post = await _service.GetByIdAsync(id);
            if (post == null) return NotFound(new ApiErrorResponse(404));

            // Map updated description only
            _mapper.Map(portfolioPostDto, post);

            await _service.UpdateAsync(post);
            return Ok(new { message = "Description updated successfully." });
        }


        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null) return BadRequest(new ApiErrorResponse(400));
            var post = await _service.GetByIdAsync(id);
            if (post == null) return NotFound(new ApiErrorResponse(400));

            await _cloudinaryService.DeleteImageAsync(post.ImagePath);
            await _service.DeleteAsync(id);

            return Ok(new { message = "Post and image deleted successfully." });
        }
    }
}
