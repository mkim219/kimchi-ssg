# kimchi-ssg

[![Nuget](https://img.shields.io/nuget/v/Kimchi-ssg)](https://www.nuget.org/packages/Kimchi-ssg/1.0.3)

Static Site Generator Open Source Development

## Built with
-   C#
-   .NET 5.0
-   [Html Agility Pack](https://html-agility-pack.net/) 
-   [xUnit](https://xunit.net/) for testing
-   [moq](https://github.com/moq/moq) for testing

## Installation on Your local

1.  Clone the reposistory
2.  Change solution configuration into `release` and build solution in Visual Studio
3.  Open the cmd or terminal
4.  Type command `dotnet tool install --global --add-source <project_root_path>/bin/release kimchi-ssg.exe`
5.  Check installation `dotnet tool list --global`

## Without Installation

1.   Clone repository https://github.com/mkim219/kimchi-ssg.git
2.   Open "kimchi-ssg.csproj" C# project file
3.   Build the project with ctrl+shift+B (if you Windows user) with either `debug` or `release` mode
4.   Go to directory `\bin\Release\net5.0`, you can find `kimchi-ssg.exe` or `/bin/Debug/net5.0` for linux
5.   Run command prompt and change directory where `kimchi-ssg.exe` (windows) or `./kimichi-ssg` (Linux) locates at


## Release
- Via NuGet
    - Before submit the code, make sure you have changed `<version>{version}</version` in `kimchi-ssg.csproj`
    - GitHub Action will process the release for NuGet

## Option

-   `--i --input <arguments>`: Input your text or Markdown file and folder to convert html
-   `--version`: Show version information
-   `-?, -h, --help`: Show help and usage information
-   `-c, --config <json file>`: Run with options on json file
-   `-f or --format`: Fix the format
-   `-l or --lint`: Fix the lint 

## Author
[Minsu Kim](https://github.com/mkim219)

## License
[MIT](https://github.com/mkim219/kimchi-ssg/blob/main/LICENSE)

