using System.Net;
using FluentFTP;

namespace DotNet8.FluentFtpSample.Services;

public class FtpService
{
    private readonly IConfiguration _configuration;
    private readonly string _hostName = string.Empty;
    private readonly string _userName = string.Empty;
    private readonly string _password = string.Empty;
    private readonly AsyncFtpClient _ftp;

    public FtpService(IConfiguration configuration)
    {
        _configuration = configuration;
        _hostName = _configuration.GetSection("FtpCredentials")["FtpHostName"]!;
        _userName = _configuration.GetSection("FtpCredentials")["FtpUserName"]!;
        _password = _configuration.GetSection("FtpCredentials")["FtpPassword"]!;
        _ftp = new AsyncFtpClient
        {
            Host = _hostName,
            Credentials = new NetworkCredential(_userName, _password)
        };
    }

    #region Connect Async

    public async Task ConnectAsync()
    {
        var token = new CancellationToken();
        await _ftp.Connect(token);
    }

    #endregion

    #region Check Directory Exists Async

    public async Task<bool> CheckDirectoryExistsAsync(string directory)
    {
        try
        {
            var token = new CancellationToken();
            await _ftp.Connect(token);

            return await _ftp.DirectoryExists(directory, token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }

    #endregion

    #region Create Directory Async

    public async Task<bool> CreateDirectoryAsync(string directory)
    {
        try
        {
            var token = new CancellationToken();
            await _ftp.Connect(token);

            return await _ftp.CreateDirectory(directory, true, token);
        }
        catch (Exception ex)
        {
            if (ex.InnerException is not null)
            {
                Console.WriteLine(ex.InnerException);
            }
            Console.WriteLine(ex.ToString());
            throw;
        }
    }

    #endregion

    #region Upload File Async

    public async Task UploadFileAsync(IFormFile file, string directory)
    {
        var tempFilePath = Path.GetTempFileName();
        try
        {
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var token = new CancellationToken();
            await _ftp.Connect(token);
            Console.WriteLine("Connected to FTP server.");

            await _ftp.CreateDirectory(directory, token);
            Console.WriteLine($"Remote directory '{directory}' checked/created.");

            var remoteFilePath = Path.Combine(directory, file.FileName).Replace("\\", "/");

            var success = await _ftp.UploadFile(tempFilePath, remoteFilePath, token: token);
            Console.WriteLine("File Uploaded Successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            throw;
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
                Console.WriteLine("Temporary file deleted.");
            }
        }
    }

    #endregion

    #region Delete File Async

    #endregion
    public async Task DeleteFileAsync(string filePath)
    {
        try
        {
            var token = new CancellationToken();
            await _ftp.Connect(token);

            await _ftp.DeleteFile(filePath);
            Console.WriteLine("File Deleted Successfully!");
        }
        catch (Exception ex)
        {
            if (ex.InnerException is not null)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
            Console.WriteLine(ex.Message);
        }
    }
}
