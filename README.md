# AlmVR (Common Libraries)
[![Build status](https://ci.appveyor.com/api/projects/status/667xv6tv84ciw3y5/branch/master?svg=true)](https://ci.appveyor.com/project/ccrutchf/almvr-common/branch/master)

A virtual reality (VR) application life cycle (ALM) management utility written for the Google Daydream and targeting Trello.

## Demo
[![AlmVR Demo](http://img.youtube.com/vi/dSCv77CD3rA/0.jpg)](http://www.youtube.com/watch?v=dSCv77CD3rA)

## What?
This repository contains the common components of AlmVR. The common components are the data and event models used by the [server](https://github.com/ccrutchf/almvr-server) and the [client](https://github.com/ccrutchf/almvr-client). They represent abstract models which can be used for various ALM platforms (ie Trello).

## How?
The common components are a `.NET Standard` library published as a `NuGet` package published to [this](https://ci.appveyor.com/nuget/almvr-common-ivwn4jqfduci) feed, which represents [artifacts](https://ci.appveyor.com/project/ccrutchf/almvr-common/branch/master/artifacts) published by `AppVeyor`.

### Builds
The common components (as well as the remainder of AlmVR) is built using `Cake Build` run in `AppVeyor`.  The build executes the following on every commit that is pushed to GitHub:
1. A clean of all of the build files.
2. Generate the version number using [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning).
3. Build the common package using the `dotnet` CLI.
4. `AppVeyor` is configured to automatical pick up the package and publish it as an artifact.

**NOTE:** Builds of this project can be run on `Windows` only as `Cake` depends on `PowerShell` to get the version number.

## Why?
This package was pulled into its own git repository so that it could be shared between the [client](https://github.com/ccrutchf/almvr-client) and the [server](https://github.com/ccrutchf/almvr-server). By publishing the package to [AppVeyor's artifacts feed](https://ci.appveyor.com/project/ccrutchf/almvr-common/branch/master/artifacts), `NuGet` is able to handle downloading and updating the library in both the client and server projects. During development time, Visual Studio handles this process; during build time, `Cake Build` is responsible for this via the `dotnet` CLI. Both Visual Studio and the `dotnet` CLI know to find this feed by looking at each project's respective `NuGet.config` file located under the `src` folder.

## Struggles
* Necessity to share models between server and client - we created this project and pushed it to `NuGet` so that we had a straightforward way to share the models between the projects without copying code.
* Location to push the package to - we looked at a few places to push the NuGet package before settling on the artifacts for `AppVeyor`.

## How to build
The following software is required to build this project:
* Visual Studio 2017
* .NET Core 2.1 SDK
* PowerShell 5.1/PowerShell 6 Core/Bash (minimum one)

Execute the `build.ps1` or `build.sh` file found in the root of the repository. This builds the `NuGet` package which can then be installed.
