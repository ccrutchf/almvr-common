#addin "Cake.Incubator"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define dirs
var buildDir = Directory("./build") + Directory(configuration);

// Define files.
var slnFile = File("./src/AlmVR.Common.sln");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Build")
    .IsDependentOn("Clean")
	.Does(() =>
{
	var dotNetCoreSettings = new DotNetCoreBuildSettings
	{
		Configuration = configuration
	};
	
	DotNetCoreBuild(slnFile, dotNetCoreSettings);
});

Task("Publish")
	.IsDependentOn("Build")
	.WithCriteria(() => !string.IsNullOrWhiteSpace(EnvironmentVariable("NUGET_API_KEY")))
	.Does(() =>
{
	var settings = new DotNetCoreNuGetPushSettings
	{
		ApiKey = EnvironmentVariable("NUGET_API_KEY"),
		Source = "https://www.myget.org/F/alm-vr/api/v3/index.json"
	};

	DotNetCoreNuGetPush($"./build/{configuration}/AlmVR.Common.Models.1.0.1-g076b41e065.nupkg", settings);
});

Task("Publish-Server")
	.Does(() =>
{
     var settings = new DotNetCorePublishSettings
     {
         Configuration = "Release",
         OutputDirectory = "./src/AlmVR.Server/artifacts/"
     };

     DotNetCorePublish("./src/AlmVR.Server/AlmVR.Server.sln", settings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
