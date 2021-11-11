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

### Unit Test
There are two ways you can have unit test
1. using `cmd`
  - Go to directory where `UnitTest.csproj` is
  - Run with `dotnet test` for entire unit testing
3. Via Visual Studio 2019
  - Open the `UnitTest.csproj` file
  - Open the Test explorer on Visual Studio 2019 
  - You will see sets of unit testing function on `UnitTest.cs` and also in Test Explorer
  - Click `Run all Test in Views` button for entire testing or Click individual unit test function on Test Explorer and click `Run` button 


