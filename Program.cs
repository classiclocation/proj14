using System.IO.Compression;

var directory = args.FirstOrDefault();

if (string.IsNullOrWhiteSpace(directory))
{
    Console.Write("Enter the folder path: ");
    directory = Console.ReadLine();
}

if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
{
    Console.WriteLine("Folder was not found.");
    return;
}

var files = Directory
    .GetFiles(directory)
    .Where(fileName => !fileName.EndsWith(".gz", StringComparison.OrdinalIgnoreCase));

await Task.WhenAll(files.Select(CompressFileAsync));

Console.WriteLine("Compression completed.");

static async Task CompressFileAsync(string fileName)
{
    await using var source = File.OpenRead(fileName);
    await using var destination = File.Create(fileName + ".gz");
    await using var gzip = new GZipStream(destination, CompressionMode.Compress);

    await source.CopyToAsync(gzip);
}
