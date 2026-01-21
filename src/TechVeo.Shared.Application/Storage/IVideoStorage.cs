using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TechVeo.Shared.Application.Storage;

public interface IVideoStorage
{
    Task<bool> DeleteSnapshotsAsync(string zipKey, CancellationToken cancellationToken = default);
    Task<bool> DeleteVideoAsync(string videoKey, CancellationToken cancellationToken = default);
    Task<Stream> DownloadVideoAsync(string videoKey, CancellationToken cancellationToken = default);
    Task<Dictionary<string, Stream>> ExtractSnapshotsFromZipAsync(string zipKey, CancellationToken cancellationToken = default);
    Task<string> GetVideoDownloadUrlAsync(string videoKey, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    Task<string> UploadSnapshotsAsZipAsync(IEnumerable<(Stream Stream, string FileName)> snapshots, string zipFileName, CancellationToken cancellationToken = default);
    Task<string> UploadVideoAsync(Stream videoStream, string fileName, CancellationToken cancellationToken = default);
}
