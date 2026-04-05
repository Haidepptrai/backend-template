using Amazon.S3;
using Amazon.S3.Model;
using Client.WebApi.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace Client.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileUploadController : ControllerBase
{
    private readonly string _bucketName;
    private readonly IAmazonS3 _s3Client;

    public FileUploadController(IAmazonS3 s3Client, IOptions<AwsSettings> awsSettings)
    {
        _s3Client = s3Client;
        _bucketName = awsSettings.Value.BucketName;
    }


    /// <summary>
    /// Uploads a file to the S3 bucket.
    /// Optimized for LocalStack compatibility by disabling chunked encoding.
    /// </summary>
    [HttpPost("upload")]
    public async Task<IActionResult> UploadObject(IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("No file provided or file is empty.");

        try
        {
            await using var stream = file.OpenReadStream();
            var objectKey = Guid.NewGuid().ToString();

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey,
                InputStream = stream,
                ContentType = file.ContentType,
            };

            var response = await _s3Client.PutObjectAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return Ok(new
                {
                    Message = "File uploaded successfully.",
                    Key = objectKey,
                });
            }

            return StatusCode((int)response.HttpStatusCode, "Upload failed at the storage provider.");
        }
        catch (AmazonS3Exception ex)
        {
            return StatusCode((int)ex.StatusCode, $"AWS S3 Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Downloads an object from S3 and returns it as a file stream.
    /// </summary>
    [HttpGet("download/{key}")]
    public async Task<IActionResult> GetObject(string key)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            using var response = await _s3Client.GetObjectAsync(request);

            // We copy the response stream to a MemoryStream because the 'response' object 
            // is disposed when this method returns, which would close the underlying stream.
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return File(memoryStream, response.Headers.ContentType, key);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound($"Object '{key}' not found in bucket '{_bucketName}'.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving file: {ex.Message}");
        }
    }
}