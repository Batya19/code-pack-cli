using System.CommandLine;
using System.Text;

RootCommand rootCommand = new RootCommand("CLI tool for bundling code files");

// Create Bundle Command
var bundleCommand = new Command("bundle", "Bundle code files into a single file");

// Create RSP Command
var createRspCommand = new Command("create-rsp", "Create a response file with bundle command");


// Language Option
var languageOption = new Option<string>("--language", "Programming languages to include (use 'all' for all files)")
{
    IsRequired = true
};
languageOption.AddAlias("--l");

// Output Option
var outputOption = new Option<FileInfo>("--output", "Output bundle file path");
outputOption.AddAlias("--o");

// Note Option
var noteOption = new Option<bool>("--note", "Include source code location as comments");
noteOption.AddAlias("--n");

// Sort Option

var sortOption = new Option<string>("--sort", () => "name", "Sort order (name/type)");
sortOption.AddAlias("--s");

// Remove Empty Lines Option
var removeEmptyLinesOption = new Option<bool>("--remove-empty-lines", "Remove empty lines from source code");
removeEmptyLinesOption.AddAlias("--r");

// Author Option
var authorOption = new Option<string>("--author", "Name of the file creator");
authorOption.AddAlias("-a");


bundleCommand.AddOption(languageOption);
bundleCommand.AddOption(outputOption);
bundleCommand.AddOption(noteOption);
bundleCommand.AddOption(sortOption);
bundleCommand.AddOption(removeEmptyLinesOption);
bundleCommand.AddOption(authorOption);


void BundleFiles(string language, FileInfo output, bool note,
    string sort, bool removeEmptyLines, string author)
{
    // Validate output path
    if (output == null)
    {
        throw new ArgumentException("Output file path is required.");
    }

    try
    {
        // Create directory if it doesn't exist
        output.Directory?.Create();
    }
    catch (Exception)
    {
        throw new Exception("Unable to create output directory.");
    }

    // Get current directory
    var currentDir = Directory.GetCurrentDirectory();

    // Get all code files based on language
    var files = GetSourceFiles(currentDir, language);

    // Sort files
    files = SortFiles(files, sort);

    // Create bundle content
    var content = new StringBuilder();

    // Add author if provided
    if (!string.IsNullOrEmpty(author))
    {
        content.AppendLine($"// Author: {author}");
        content.AppendLine();
    }

    foreach (var file in files)
    {
        var fileContent = File.ReadAllText(file);

        if (note)
        {
            var relativePath = Path.GetRelativePath(currentDir, file);
            content.AppendLine($"// Source: {relativePath}");
        }

        if (removeEmptyLines)
        {
            fileContent = RemoveEmptyLines(fileContent);
        }

        content.AppendLine(fileContent);
        content.AppendLine();
    }

    // Write to output file
    File.WriteAllText(output.FullName, content.ToString());
    Console.WriteLine($"Bundle created successfully at: {output.FullName}");
}

bundleCommand.SetHandler((string language, FileInfo output, bool note,
    string sort, bool removeEmptyLines, string author) =>
{
    try
    {
        BundleFiles(language, output, note, sort, removeEmptyLines, author);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}, languageOption, outputOption, noteOption, sortOption, removeEmptyLinesOption, authorOption);

void CreateResponseFile()
{
    var responses = new Dictionary<string, string>();

    Console.WriteLine("Creating response file for bundle command.");
    Console.WriteLine("Please provide the following information:");

    responses["language"] = GetUserInput("Enter programming languages (or 'all'):",
        input => !string.IsNullOrWhiteSpace(input));

    responses["output"] = GetUserInput("Enter output file path:",
        input => !string.IsNullOrWhiteSpace(input));

    responses["note"] = GetUserInput("Include source code location as comments? (true/false):",
        input => bool.TryParse(input, out _));

    responses["sort"] = GetUserInput("Enter sort order (name/type):",
        input => input == "name" || input == "type");

    responses["remove-empty-lines"] = GetUserInput("Remove empty lines? (true/false):",
        input => bool.TryParse(input, out _));

    responses["author"] = GetUserInput("Enter author name (optional):",
        input => true);  // Optional, so any input is valid

    // Create response file content
    var command = new StringBuilder("bundle");
    foreach (var response in responses)
    {
        if (!string.IsNullOrEmpty(response.Value))
        {
            command.Append($" --{response.Key} {response.Value}");
        }
    }

    var rspFileName = "bundle-command.rsp";
    File.WriteAllText(rspFileName, command.ToString());
    Console.WriteLine($"Response file created: {rspFileName}");
    Console.WriteLine($"Run with: dotnet @{rspFileName}");
}

createRspCommand.SetHandler(() =>
{
    try
    {
        CreateResponseFile();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
});

string GetUserInput(string prompt, Func<string, bool> validator)
{
    string input;
    do
    {
        Console.WriteLine(prompt);
        input = Console.ReadLine()?.Trim() ?? "";

        if (!validator(input))
        {
            Console.WriteLine("Invalid input. Please try again.");
        }
    } while (!validator(input));

    return input;
}

string[] GetSourceFiles(string directory, string language)
{
    var extensions = language.ToLower() == "all"
        ? new[] { ".cs", ".java", ".py", ".js", ".cpp", ".h" }
        : GetExtensionsForLanguage(language);

    var allFiles = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
    var filteredFiles = new List<string>();

    foreach (var file in allFiles)
    {
        var extension = Path.GetExtension(file).ToLower();
        if (extensions.Contains(extension) && !file.Contains("\\bin\\") && !file.Contains("\\debug\\"))
        {
            filteredFiles.Add(file);
        }
    }

    return filteredFiles.ToArray();
}

string[] GetExtensionsForLanguage(string language)
{
    return language.ToLower() switch
    {
        "csharp" or "c#" => new[] { ".cs" },
        "java" => new[] { ".java" },
        "python" => new[] { ".py" },
        "javascript" => new[] { ".js" },
        "cpp" or "c++" => new[] { ".cpp", ".h" },
        _ => throw new ArgumentException($"Unsupported language: {language}")
    };
}

string[] SortFiles(string[] files, string sortOrder)
{
    switch (sortOrder.ToLower())
    {
        case "name":
            Array.Sort(files, (a, b) => Path.GetFileName(a).CompareTo(Path.GetFileName(b)));
            break;
        case "type":
            Array.Sort(files, (a, b) =>
            {
                var compareExt = Path.GetExtension(a).CompareTo(Path.GetExtension(b));
                if (compareExt == 0)
                {
                    return Path.GetFileName(a).CompareTo(Path.GetFileName(b));
                }
                return compareExt;
            });
            break;
    }
    return files;
}

string RemoveEmptyLines(string content)
{
    var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    var nonEmptyLines = new List<string>();

    foreach (var line in lines)
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            nonEmptyLines.Add(line);
        }
    }

    return string.Join(Environment.NewLine, nonEmptyLines);
}

rootCommand.AddCommand(bundleCommand);
rootCommand.AddCommand(createRspCommand);


rootCommand.InvokeAsync(args);