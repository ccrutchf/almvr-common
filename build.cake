#tool nuget:?package=Nerdbank.GitVersioning&version=2.1.23

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
	version = StartPowershellFile("./tools/Nerdbank.GitVersioning.2.1.23/tools/Get-Version.ps1")[1].BaseObject;

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

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
