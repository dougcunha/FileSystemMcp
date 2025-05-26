# FileSystemMcp

This is a demo project to exercise the new Model Context Protocol (MCP) SDK for C#. It demonstrates how to build a simple file system server tool using the MCP SDK, providing basic file and directory operations such as listing directory contents, reading file contents, and retrieving the current working directory.

## Features

- List directory contents with metadata
- Read the contents of files
- Get the current working directory
- Built with extensibility and logging using Microsoft.Extensions

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
