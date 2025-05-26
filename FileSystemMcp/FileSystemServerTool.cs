using System.ComponentModel;
using System.IO.Abstractions;
using FileSystemMcp.Models;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace FileSystemMcp;

[McpServerToolType]
public class FileSystemServerTool(IFileSystem fileSystem, ILogger<FileSystemServerTool> logger)
{
    /// <summary>
    /// Gets the current working directory.
    /// </summary>
    /// <returns>The current working directory path.</returns>
    [McpServerTool, Description("Gets the current working directory.")]
    public string GetCurrentDirectory()
        => fileSystem.Directory.GetCurrentDirectory();

    /// <summary>
    /// Lists the contents of a directory.
    /// </summary>
    /// <param name="path">The path of the directory to list.</param>
    /// <returns>A list of file and directory names in the specified directory.</returns>
    [McpServerTool, Description("Lists the contents of a directory returning a list of entries with their metadata.")]
    public IEnumerable<Entry?> ListDirectoryContents(string path)
    {
        foreach (var entryPath in fileSystem.Directory.GetFiles(path))
        {
            Entry? entry = null;
            try
            {
                entry = new Entry(fileSystem, entryPath);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to create entry for file: {Path}", entryPath);
            }

            yield return entry;
        }
    }

    /// <summary>
    /// Reads the contents of a file.
    /// </summary>
    /// <param name="path">The path of the file to read.</param>
    /// <returns>The contents of the file as a string.</returns>
    [McpServerTool, Description("Reads the contents of a file.")]
    public string ReadFileContents(string path)
        => fileSystem.File.Exists(path)
            ? fileSystem.File.ReadAllText(path)
            : $"The file '{path}' does not exist.";

    /// <summary>
    /// Writes content to a file.
    /// </summary>
    /// <param name="path">The path of the file to write to.</param>
    /// <param name="content">The content to write to the file.</param>
    [McpServerTool, Description("Writes content to a file.")]
    public bool WriteFileContents(string path, string content)
    {
        try
        {
            fileSystem.File.WriteAllText(path, content);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to write file: {Path}", path);

            return false;
        }
    }

    /// <summary>
    /// Deletes a file or directory.
    /// </summary>
    /// <param name="path">The path of the file or directory to delete.</param>
    /// <returns>True if the deletion was successful, otherwise false.</returns>
    [McpServerTool, Description("Deletes a file or directory.")]
    public bool DeleteFileOrDirectory(string path)
    {
        try
        {
            if (fileSystem.Directory.Exists(path))
            {
                fileSystem.Directory.Delete(path, true);
            }
            else if (fileSystem.File.Exists(path))
            {
                fileSystem.File.Delete(path);
            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete file or directory: {Path}", path);

            return false;
        }
    }

    /// <summary>
    /// Creates a new directory.
    /// </summary>
    /// <param name="path">The path of the directory to create.</param>
    /// <returns>True if the directory was created successfully, otherwise false.</returns>
    [McpServerTool, Description("Creates a new directory.")]
    public bool CreateDirectory(string path)
    {
        try
        {
            fileSystem.Directory.CreateDirectory(path);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create directory: {Path}", path);

            return false;
        }
    }

    /// <summary>
    /// Moves a file or directory to a new location.
    /// </summary>
    /// <param name="sourcePath">The current path of the file or directory.</param>
    /// <param name="destinationPath">The new path where the file or directory should be moved.</param>
    /// <returns>True if the move was successful, otherwise false.</returns>
    [McpServerTool, Description("Moves a file or directory to a new location.")]
    public bool MoveFileOrDirectory(string sourcePath, string destinationPath, bool overwrite = false)
    {
        try
        {
            if (fileSystem.Directory.Exists(sourcePath))
            {
                if (fileSystem.Directory.Exists(destinationPath))
                {
                    if (overwrite)
                    {
                        fileSystem.Directory.Delete(destinationPath, true);
                    }
                    else
                    {
                        logger.LogWarning("Destination directory already exists: {DestinationPath}", destinationPath);

                        return false;
                    }
                }
                fileSystem.Directory.Move(sourcePath, destinationPath);
            }
            else if (fileSystem.File.Exists(sourcePath))
            {
                fileSystem.File.Move(sourcePath, destinationPath, overwrite);
            }
            else
            {
                logger.LogWarning("Source path does not exist: {SourcePath}", sourcePath);

                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to move file or directory from {Source} to {Destination}", sourcePath, destinationPath);
            return false;
        }
    }

    /// <summary>
    /// Copies a file or directory to a new location.
    /// </summary>
    /// <param name="sourcePath">The current path of the file or directory.</param>
    /// <param name="destinationPath">The new path where the file or directory should be copied.</param>
    /// <returns>True if the copy was successful, otherwise false.</returns>
    [McpServerTool, Description("Copies a file to a new location.")]
    public bool CopyFile(string sourcePath, string destinationPath)
    {
        try
        {
            if (fileSystem.File.Exists(sourcePath))
            {
                fileSystem.File.Copy(sourcePath, destinationPath);
            }
            else
            {
                logger.LogWarning("Source path does not exist: {SourcePath}", sourcePath);

                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to copy file from {Source} to {Destination}", sourcePath, destinationPath);
            return false;
        }
    }

    /// <summary>
    /// Checks if a file or directory exists at the specified path.
    /// </summary>
    /// <param name="path">The path to check for existence.</param>
    /// <returns>True if the file or directory exists, otherwise false.</returns>
    [McpServerTool, Description("Checks if a file or directory exists at the specified path.")]
    public bool FileOrDirectoryExists(string path)
    {
        try
        {
            return fileSystem.Directory.Exists(path) || fileSystem.File.Exists(path);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to check existence of {Path}", path);

            return false;
        }
    }

    /// <summary>
    /// Gets the size of a file or directory.
    /// </summary>
    /// <param name="path">The path to the file or directory.</param>
    /// <returns>The size of the file or directory in bytes, or -1 if it does not exist.</returns>
    [McpServerTool, Description("Gets the size of a file or directory.")]
    public long GetFileOrDirectorySize(string path)
    {
        try
        {
            if (fileSystem.Directory.Exists(path))
            {
                return fileSystem.DirectoryInfo.New(path).GetFiles("*", SearchOption.AllDirectories)
                    .Sum(file => file.Length);
            }
            else if (fileSystem.File.Exists(path))
            {
                return fileSystem.FileInfo.New(path).Length;
            }
            else
            {
                logger.LogWarning("Path does not exist: {Path}", path);

                return -1;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get size of {Path}", path);

            return -1;
        }
    }

    /// <summary>
    /// Gets the last modified date and time of a file or directory.
    /// </summary>
    /// <param name="path">The path to the file or directory.</param>
    /// <returns>The last modified date and time, or null if the path does not exist.</returns>
    [McpServerTool, Description("Gets the last modified date and time of a file or directory.")]
    public DateTime? GetFileOrDirectoryLastModified(string path)
    {
        try
        {
            if (fileSystem.Directory.Exists(path))
            {
                return fileSystem.Directory.GetLastAccessTime(path);
            }
            else if (fileSystem.File.Exists(path))
            {
                return fileSystem.File.GetLastAccessTime(path);
            }
            else
            {
                logger.LogWarning("Path does not exist: {Path}", path);

                return null;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get last modified date of {Path}", path);

            return null;
        }
    }

    /// <summary>
    /// Gets the file system path of the current tool.
    /// </summary>
    /// <returns>The file system path of the current tool.</returns>
    [McpServerTool, Description("Gets the file system path of the current tool.")]
    public string? GetFileSystemPath()
        => fileSystem.Path.GetDirectoryName(GetType().Assembly.Location);


    /// <summary>
    /// Create a symlink to a file or directory.
    /// </summary>
    /// <param name="sourcePath">The path of the file or directory to link to.</param>
    /// <param name="linkPath">The path where the symlink should be created.</param>
    /// <returns>True if the symlink was created successfully, otherwise false.</returns>
    [McpServerTool, Description("Creates a symlink to a file or directory.")]
    public bool CreateSymlink(string sourcePath, string linkPath)
    {
        try
        {
            var sourcePathExists = fileSystem.File.Exists(sourcePath) || fileSystem.Directory.Exists(sourcePath);

            if (!sourcePathExists)
            {
                logger.LogWarning("Source path does not exist: {SourcePath}", sourcePath);

                return false;
            }

            var isFile = fileSystem.File.Exists(sourcePath);
            var isDirectory = fileSystem.Directory.Exists(sourcePath);

            if (fileSystem.File.Exists(linkPath) || fileSystem.Directory.Exists(linkPath))
            {
                logger.LogWarning("Link path already exists: {LinkPath}", linkPath);

                return false;
            }

            // Create the symlink
            if (isDirectory)
                fileSystem.Directory.CreateSymbolicLink(linkPath, sourcePath);
            else
                fileSystem.File.CreateSymbolicLink(linkPath, sourcePath);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create symlink from {Source} to {Link}", sourcePath, linkPath);
            
            return false;
        }
    }
}