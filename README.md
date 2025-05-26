# FileSystemMcp

## Index

- [Features](#features)
- [Available Tools](#available-tools)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Build and Run](#build-and-run)
  - [Running in VSCode](#running-in-vscode)
- [Project Structure](#project-structure)
- [License](#license)
- [Contributing](#contributing)

## Description

This is a demo project to exercise the new Model Context Protocol (MCP) SDK for C#. It demonstrates how to build a simple file system server tool using the MCP SDK, providing basic file and directory operations such as listing directory contents, reading file contents, and retrieving the current working directory.

## Features

- List directory contents with metadata
- Read the contents of files
- Get the current working directory
- Built with extensibility and logging using Microsoft.Extensions

## Available Tools

The following tools are available in the FileSystemMcp server:

- **GetCurrentDirectory**: Returns the current working directory path.
- **ListDirectoryContents**: Lists the contents of a directory, returning entries with metadata.
- **ReadFileContents**: Reads the contents of a file as a string.
- **WriteFileContents**: Writes content to a file, creating or overwriting it.
- **DeleteFileOrDirectory**: Deletes a file or directory (recursively for directories).
- **CreateDirectory**: Creates a new directory at the specified path.
- **MoveFileOrDirectory**: Moves a file or directory to a new location, with optional overwrite.
- **CopyFile**: Copies a file to a new location.
- **FileOrDirectoryExists**: Checks if a file or directory exists at the specified path.
- **GetFileOrDirectorySize**: Gets the size (in bytes) of a file or directory (recursive for directories).
- **GetFileOrDirectoryLastModified**: Gets the last modified date and time of a file or directory.
- **GetFileSystemPath**: Gets the file system path of the running tool.
- **CreateSymlink**: Creates a symlink to a file or directory.

## Getting Started

### Prerequisites

- .NET 10.0 or later
- [MCP SDK for C#](https://github.com/modelcontextprotocol/csharp-sdk)

### Build and Run

1. Clone the repository:

   ```sh
   git clone https://github.com/dougcunha/FileSystemMcp.git
   cd FileSystemMcp
   ```

2. Build the project:

   ```sh
   dotnet build
   ```

3. Run the project:

   ```sh
   dotnet run --project FileSystemMcp/FileSystemMcp.csproj
   ```

### Running in VSCode

To configure and run this project as an MCP server inside VSCode, follow these steps:

Add the configuration to the mcp.json file:

```json
{
    "servers": {
        "filesystem-mcp": {
            "type": "stdio",
            "command": "dotnet",
            "args": ["run", "--project", "FileSystemMcp\\FileSystemMcp.csproj"]
        }
    }
}
```

## Project Structure

- `FileSystemMcp/` - Main project directory
- `Models/Entry.cs` - Model for file system entries
- `FileSystemServerTool.cs` - Main server tool implementation
- `Program.cs` - Entry point

## License

See [LICENSE](LICENSE) for details.

## Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

---
This project is for demonstration purposes and is not intended for production use.
