using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using TechVeo.Shared.Application.Storage;

namespace TechVeo.Shared.Infra.Storage;

public class VideoStorage : IVideoStorage
{
    private readonly IStorageService _storageService;

    public VideoStorage(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<string> UploadVideoAsync(Stream videoStream, string fileName, CancellationToken cancellationToken = default)
    {
        var contentType = GetVideoContentType(fileName);
        return await _storageService.UploadAsync(videoStream, fileName, contentType, cancellationToken);
    }

    public async Task<string> UploadSnapshotsAsZipAsync(
        IEnumerable<(Stream Stream, string FileName)> snapshots,
        string zipFileName,
        CancellationToken cancellationToken = default)
    {
        using var zipStream = new MemoryStream();

        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var (stream, fileName) in snapshots)
            {
                var entry = archive.CreateEntry(fileName, CompressionLevel.Optimal);

                using var entryStream = entry.Open();
                await stream.CopyToAsync(entryStream, cancellationToken);

                stream.Position = 0;
            }
        }

        zipStream.Position = 0;

        return await _storageService.UploadAsync(zipStream, zipFileName, "application/zip", cancellationToken);
    }

    public async Task<Stream> DownloadVideoAsync(string videoKey, CancellationToken cancellationToken = default)
    {
        return await _storageService.DownloadAsync(videoKey, cancellationToken);
    }

    public async Task<Dictionary<string, Stream>> ExtractSnapshotsFromZipAsync(
        string zipKey,
        CancellationToken cancellationToken = default)
    {
        var zipStream = await _storageService.DownloadAsync(zipKey, cancellationToken);
        var snapshots = new Dictionary<string, Stream>();

        try
        {
            using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);

            foreach (var entry in archive.Entries)
            {
                if (string.IsNullOrEmpty(entry.Name))
                    continue;

                var memoryStream = new MemoryStream();
                using var entryStream = entry.Open();
                await entryStream.CopyToAsync(memoryStream, cancellationToken);
                memoryStream.Position = 0;

                snapshots[entry.Name] = memoryStream;
            }
        }
        catch
        {
            foreach (var stream in snapshots.Values)
            {
                stream?.Dispose();
            }
            throw;
        }

        return snapshots;
    }

    public async Task<bool> DeleteVideoAsync(string videoKey, CancellationToken cancellationToken = default)
    {
        return await _storageService.DeleteAsync(videoKey, cancellationToken);
    }

    public async Task<bool> DeleteSnapshotsAsync(string zipKey, CancellationToken cancellationToken = default)
    {
        return await _storageService.DeleteAsync(zipKey, cancellationToken);
    }

    public async Task<string> GetVideoDownloadUrlAsync(
        string videoKey,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        var defaultExpiration = expiration ?? TimeSpan.FromHours(1);
        return await _storageService.GetPresignedUrlAsync(videoKey, defaultExpiration, cancellationToken);
    }

    private static string GetVideoContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".mp4" => "video/mp4",
            ".avi" => "video/x-msvideo",
            ".mov" => "video/quicktime",
            ".wmv" => "video/x-ms-wmv",
            ".flv" => "video/x-flv",
            ".webm" => "video/webm",
            ".mkv" => "video/x-matroska",
            ".m4v" => "video/x-m4v",
            _ => "application/octet-stream"
        };
    }
}
