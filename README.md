# AlmVR (Common Libraries)
[![Build status](https://ci.appveyor.com/api/projects/status/667xv6tv84ciw3y5/branch/master?svg=true)](https://ci.appveyor.com/project/ccrutchf/almvr-common/branch/master)

A virtual reality (VR) application life cycle (ALM) management utility written for the Google Daydream and targeting Trello.

## What?
This repository contains the common components of AlmVR.  The common components are the data and event models used by the [server](https://github.com/ccrutchf/almvr-server) and the [client](https://github.com/ccrutchf/almvr-client).  They represent abstract models which can be used for various ALM products (ie Trello).

## How?
The common components are a `.NET Standard` library published as a NuGet Package published at [this](https://ci.appveyor.com/nuget/almvr-common-ivwn4jqfduci) feed, which represents [artifacts](https://ci.appveyor.com/project/ccrutchf/almvr-common/branch/master/artifacts) published by AppVeyor.

### Builds
The server (as well as the remainder of AlmVR) is built using Cake Build run in AppVeyor.  The build executes the following on every commit that is pushed to GitHub:
1. A clean of all of the build files.
2. Generate the version number using [Nerdbank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning).
3. Build the common package using the dotnet CLI.
4. AppVeyor is configured to automatical pick up the package and publish it as an artifact.

**NOTE:** Builds of this project can be run on Windows only as Cake depends on PowerShell to get the version number.

## Why?
This package was pulled into its own git repository so that it could be shared between the [client](https://github.com/ccrutchf/almvr-client) and the [server](https://github.com/ccrutchf/almvr-server).  By publishing the package to [AppVeyor's artifacts feed](https://ci.appveyor.com/project/ccrutchf/almvr-common/branch/master/artifacts), NuGet is able to handle downloading and updating the library in both the client and server projects.  During development time, Visual Studio handles this process; during build time, Cake Build is responsible for this via the dotnet CLI.  Both Visual Studio and the dotnet CLI know to find this feed by looking at each project's respective `NuGet.config` file located under the `src` folder.
