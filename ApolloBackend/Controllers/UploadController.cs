using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using ApolloBackend.Models.DTOs;

namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        // Add these allowed extensions and content types
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        private readonly string[] _allowedContentTypes = {
            "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp"
        };

        // FTP Server settings
        private const string FtpServer = "localhost";
        private const int FtpPort = 21;
        private const string FtpFolder = "images";
        
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] FileModel fileModel)
        {
            try
            {
                // Validate file exists
                if (fileModel.file == null || fileModel.file.Length == 0)
                {
                    return BadRequest(new { Message = "No file was uploaded." });
                }

                // Validate file is an image
                string fileExtension = Path.GetExtension(fileModel.file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(fileExtension) ||
                    !_allowedContentTypes.Contains(fileModel.file.ContentType))
                {
                    return BadRequest(new { Message = "Only image files are allowed." });
                }

                // Generate a unique filename to avoid conflicts
                string uniqueFileName = $"{fileModel.FileName}";
                
                // Create a temporary file to process the image
                string tempFilePath = Path.GetTempFileName();
                
                try
                {
                    // First save to temp file
                    using (var stream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await fileModel.file.CopyToAsync(stream);
                    }
                    
                    // Process image metadata if needed
                    try
                    {
                        using (var img = Image.FromFile(tempFilePath))
                        {
                            // This forces proper metadata to be saved
                            img.Save(tempFilePath, img.RawFormat);
                        }
                    }
                    catch
                    {
                        // If image processing fails, we still have the original file
                    }
                    
                    // Upload to FTP server
                    string ftpUrl = $"ftp://{FtpServer}:{FtpPort}/{FtpFolder}/{uniqueFileName}";
                    
                    // Create FTP request
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    
                    // Anonymous login (no credentials)
                    request.Credentials = new NetworkCredential("", "");
                    
                    // Set up FTP request properties
                    request.UseBinary = true;
                    request.UsePassive = true;
                    request.KeepAlive = false;
                    
                    // Read the file and upload to FTP
                    using (FileStream fileStream = System.IO.File.OpenRead(tempFilePath))
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        await fileStream.CopyToAsync(requestStream);
                    }
                    
                    // Get the FTP response
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        // Check if upload was successful
                        if (response.StatusCode != FtpStatusCode.ClosingData)
                        {
                            return BadRequest(new { 
                                Message = "Error uploading to FTP server.", 
                                Status = response.StatusDescription 
                            });
                        }
                    }
                    
                    return Ok(new
                    {
                        FileName = uniqueFileName,
                        FilePath = ftpUrl,
                        FileSize = fileModel.file.Length
                    });
                }
                finally
                {
                    // Clean up temp file
                    if (System.IO.File.Exists(tempFilePath))
                    {
                        System.IO.File.Delete(tempFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error while uploading the file.", Error = ex.Message });
            }
        }
    }
}