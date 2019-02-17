var projectName = "Webnotes";
var solution = "./" + projectName + ".sln";
var target = Argument("target", "Default");
var framework = "netstandard2.0";
var targetPath = Argument("out", "./dist/");
var configuration = Argument("configuration", "Release");
var distDirectory = Directory(targetPath);
var temproryDirectory = Directory(Context.Environment.GetSpecialPath(SpecialPath.LocalTemp).Combine("webnotes_build_local_tmp").ToString());
//var branch = Argument("branch", EnvironmentVariable("BUIL_BRANCH"));


Information($"Running target {target} in configuration {configuration} framwork {target} to {distDirectory}");
Information($"Temp folder: {temproryDirectory}");

// Deletes the contents of the Artifacts folder if it contains anything from a previous build.
Task("Clean")
    .Does(() =>
    {
        CleanDirectory(distDirectory);
    });

// Run dotnet restore to restore all package references.
Task("Restore")
    .Does(() =>
    {
        DotNetCoreRestore("./src/WebnoteBlazor");
    });

Task("Publish")
    .Does(() =>
    {
        var settings = new DotNetCorePublishSettings
        {
            Framework = framework,
            Configuration = configuration,
            OutputDirectory = distDirectory,
            NoDependencies = false,
            NoRestore = true,
            NoWorkingDirectory = true,
            //WorkingDirectory = distDirectory
            //ArgumentCustomization = args => newArgs != null ? args.Append(newArgs) : args
        };
        DotNetCorePublish("./src/WebnoteBlazor", settings);
        Information("Publish finished");
        CleanDirectory(temproryDirectory);
    });

Task("Build")
    .Does(() =>
    {
        var settings = new DotNetCoreBuildSettings
        {
            Framework = framework,
            Configuration = configuration,
            OutputDirectory = distDirectory,
            NoDependencies = false,
            NoRestore = true,
        };
        DotNetCoreBuild("./src/WebnoteBlazor",  settings);
        Information("Main build finished");
        CleanDirectory(temproryDirectory);
    });

public DotNetCoreBuildSettings BuildSettings(string framework, string configuration, string dist, string newArgs = null)
{
    var settings = new DotNetCoreBuildSettings
        {
            Framework = framework,
            Configuration = configuration,
            OutputDirectory = dist,
            ArgumentCustomization = args => newArgs != null ? args.Append(newArgs) : args
        };
    return settings;
}

public class AH_Module
{
    public string ProjectPath {get;set;}
    public string ModulePath {get;set;}
}

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

// A meta-task that runs all the steps to Build the app
Task("BuildAndPublish")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Publish");
Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

//var files = GetFiles("./dist/*.dll");
//foreach(var file in files)
//   Information("File: {0}", file);
