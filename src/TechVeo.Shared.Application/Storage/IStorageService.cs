using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TechVeo.Shared.Application.Storage;

public interface IStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);

    Task<Stream> DownloadAsync(string fileKey, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string fileKey, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string fileKey, CancellationToken cancellationToken = default);

    Task<string> GetPresignedUrlAsync(string fileKey, TimeSpan expiration, CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> ListFilesAsync(string prefix = "", CancellationToken cancellationToken = default);
}
