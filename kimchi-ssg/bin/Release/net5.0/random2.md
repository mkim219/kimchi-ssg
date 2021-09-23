# kimchi-ssg random 2

Static Site Generator Open Source Development

## Built with
- C#
- .NET 5.0
- [command-line-api](https://github.com/dotnet/command-line-api)
- [Html Agility Pack](https://html-agility-pack.net/) 

## Installation

1. Clone the reposistory
2. Open the cmd 
3. Move to `./kimchi-ssg/bin/release`, you can find "kimchi-ssg.1.0.0.nupkg" nuget package file
4. Type command `dotnet tool install --global --add-source {your project directory including kimchi-ssg.1.0.0.nupkg} kimchi-ssg` 
5. Check installation `dotnet tool list --global`

## Without Installation

1. Clone [repository](https://github.com/mkim219/kimchi-ssg) 
2. Open "kimchi-ssg.csproj" C# project file
3. Build the project with ctrl+shift+B (if you Windows user)
4. Go to directory "\bin\Release\net5.0", you can find kimchi-ssg.exe
5. Run command prompt and change directory where kimchi-ssg.exe locates at



## Option

- `--i --input <arguments>` Input your text file to convert html
- `--version` Show version information
- `-?, -h, --help` Show help and usage information

## Author
[Minsu Kim](https://github.com/mkim219)

## License
[MIT](https://github.com/mkim219/kimchi-ssg/blob/main/LICENSE)

