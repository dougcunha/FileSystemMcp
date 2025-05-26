using System.IO.Abstractions;

namespace FileSystemMcp.Models;

/// <summary>
/// Represents a file system entry, which can be a file or a directory.
/// This record contains properties such as Path, Size, LastModified, and provides methods to access
/// </summary>
/// <param name="Path">The file system path of the entry.</param>
/// <param name="LastModified">The last modified date and time of the entry.</param>
public class Entry
{
    /// <summary>
    /// The file system path of the entry.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Gets the name of the file or directory represented by this entry.
    /// This is the last segment of the path, excluding the directory structure.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the directory path of the entry.
    /// This is the path to the parent directory containing the file or directory.
    /// </summary>
    public string Directory { get; }

    /// <summary>
    /// Gets the extension of the file represented by this entry.
    /// If the entry is a directory, this will return an empty string.
    /// </summary>
    public string Extension { get; }

    /// <summary>
    /// Indicates whether the entry is a directory.
    /// This property checks if the path exists and is a directory.
    /// </summary>
    public bool IsDirectory { get; }

    /// <summary>
    /// Indicates whether the entry is a file.
    /// This property checks if the path exists and is a file.
    /// </summary>
    public bool IsFile { get; }

    /// <summary>
    /// Gets the size of the file represented by this entry.
    /// If the entry is a directory, this will return 0.
    /// </summary>
    public long Size { get; }

    /// <summary>
    /// Gets the last modified date and time of the entry.
    /// </summary>
    public DateTime LastModified { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entry"/> class, retrieving all metadata at construction.
    /// </summary>
    /// <param name="fileSystem">The file system abstraction to use.</param>
    /// <param name="path">The file system path of the entry.</param>
    public Entry(IFileSystem fileSystem, string path)
    {
        Path = path;
        Name = System.IO.Path.GetFileName(path);
        Directory = System.IO.Path.GetDirectoryName(path) ?? string.Empty;
        Extension = System.IO.Path.GetExtension(path);
        IsDirectory = fileSystem.Directory.Exists(path);
        IsFile = fileSystem.File.Exists(path);
        Size = IsFile ? fileSystem.FileInfo.New(path).Length : 0;
        LastModified = IsFile ? fileSystem.FileInfo.New(path).LastWriteTime : DateTime.MinValue;
    }
}