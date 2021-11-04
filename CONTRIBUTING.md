### Usage
1. `git clone https://github.com/mkim219/kimchi-ssg.git`
2. Open `kimchi-ssg.sln`
3. build solution with Release mode
4. Go to `bin\Release\net5.0`
5. You can find `Kimchi-ssg.exe` executable file

### Formatter 
```
kimchi-ssg -f
``` 
This command automatically check that your local machine has [dotnet-format](https://github.com/dotnet/format). If your machine already has `dotnet-format`, it will run `Formatter`. If not, automatically install `dotnet-format` and run `Formatter`.

### Linter
```
kimchi-ssg -l
```
The [StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers) install with building solution. Run `kimchi-ssg -l` for removing lint and smell of your code.

### Editor/IDE Integration
In git repository, you can find `kimchi-ssg.vssettings`.
You can import this file to have same development environment. On your `Visual Studio`, you can import the setting via 
```
Tools --> Import and Export setting 
--> Import selected environment settings 
--> Browse kimchi-ssg.vssettings
``` 