using DotNet8.FluentFtpSample.Models;
using DotNet8.FluentFtpSample.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8.FluentFtpSample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly FtpService _ftpService;

    public BlogController(FtpService ftpService)
    {
        _ftpService = ftpService;
    }

    [HttpGet]
    public async Task<IActionResult> CheckDirectoryExists(string directory)
    {
        try
        {
            bool isExist = await _ftpService.CheckDirectoryExistsAsync(directory);
            return Ok(isExist);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateDirectory(string directory)
    {
        try
        {
            bool isCreateSuccessful = await _ftpService.CreateDirectoryAsync(directory);
            return Ok(isCreateSuccessful);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost("UploadFile")]
    public async Task<IActionResult> UploadFile([FromForm] BlogRequestModel requestModel)
    {
        try
        {
            await _ftpService.UploadFileAsync(requestModel.File, "testing");
            return Ok(requestModel);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFile()
    {
        try
        {
            await _ftpService.DeleteFileAsync("/testing/profile.png");
            return Ok();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
