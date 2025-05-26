using FileSystemMcp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Trace);
});

builder.Services.AddSingleton<IFileSystem, FileSystem>();
builder.Services.AddSingleton<FileSystemServerTool>();

builder.Logging.AddConsole
(
    consoleLogOptions =>
    {
        consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
    }
);

var server = builder.Services
    .AddMcpServer(opt =>
    {
        opt.ServerInstructions = """
        This is a file system server. 
        Use the FileSystemServerTool to interact with the file system. 
        You can list directories, read files, and perform other file system operations.
        """;
    })
    .WithStdioServerTransport(); 

server.WithTools<FileSystemServerTool>();

await builder.Build().RunAsync();

