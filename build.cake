#tool nuget:?package=Nerdbank.GitVersioning

#addin "Cake.Incubator"
#addin "Cake.Powershell"

using System.Linq;

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

dynamic version;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Git-Versioning")
	.Does(() =>
{
	version = StartPowershellFile("./tools/Addins/Nerdbank.GitVersioning.2.1.23/tools/Get-Version.ps1")[1].BaseObject;

	Information($"Version number: \"{version.AssemblyInformationalVersion}\".");

	var script = @"
if (Get-Command ""Update-AppveyorBuild"" -errorAction SilentlyContinue)
{{
    Update-AppveyorBuild -Version {0}
}}";

	StartPowershellScript(string.Format(script, version.AssemblyInformationalVersion));
});

Task("Build")
    .IsDependentOn("Clean")
	.IsDependentOn("Git-Versioning")
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

	var nugetPackages = GetFiles($"./build/{configuration}/*.nupkg");
	foreach (var package in nugetPackages)
	{
		DotNetCoreNuGetPush(package.ToString(), settings);
	}
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
