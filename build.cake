#tool nuget:?package=GitVersion.CommandLine&version=4.0.0
#tool nuget:?package=vswhere&version=2.6.7
#addin nuget:?package=Cake.Figlet&version=1.2.0
#addin nuget:?package=Cake.Plist&version=0.5.0
#addin nuget:?package=Cake.Git&version=0.19.0

var sln = new FilePath("XamForms.Controls.Calendar.sln");

var coreProj = new FilePath("./XamForms.Controls.Calendar/XamForms.Controls.Calendar.csproj");
var iOSProj = new FilePath("./XamForms.Controls.Calendar.iOS/XamForms.Controls.Calendar.iOS.csproj");
var droidProj = new FilePath("./XamForms.Controls.Calendar.Droid/XamForms.Controls.Calendar.Droid.csproj");

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var verbosityArg = Argument("verbosity", "Minimal");
var prereleaseTools = Argument<bool>("prereleasetools", false);
var verbosity = Verbosity.Minimal;
var outputDirArgument = Argument("outputDir", "./artifacts");
var outputDir = new DirectoryPath(outputDirArgument);
var gitVersionLog = new FilePath("./gitversion.log");
var nugetPackagesDir = new DirectoryPath("./nuget/packages");

GitVersion versionInfo = null;

Setup(context =>
{
    EnsureDirectoryExists(outputDir);

    versionInfo = context.GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
        OutputType = GitVersionOutput.Json,
        LogFilePath = gitVersionLog
    });

    var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

    Information(Figlet("Calendar"));
    Information("Building version {0} {1}, ({2}, {3}) using version {4} of Cake.",
        versionInfo.SemVer,
        versionInfo.BuildMetaData,
        configuration,
        target,
        cakeVersion);   

    verbosity = Verbosity.Normal;
});

Task("Clean")
    .Does(() =>
{    
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
    CleanDirectories(nugetPackagesDir.FullPath);    
    CleanDirectory(outputDir.FullPath);

    EnsureDirectoryExists(outputDir);

    MoveFileToDirectory(gitVersionLog, outputDir);
});

Task("CleanUntracked")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var repoPath = MakeAbsolute(new DirectoryPath("./"));
    Information("Cleaning untracked files in: {0}", repoPath.FullPath.ToString());
    GitClean(repoPath);
});

FilePath msBuildPath;
Task("ResolveBuildTools")
    .WithCriteria(() => IsRunningOnWindows())
    .Does(() =>
{
    Information("ResolveBuildTools");

    var vsWhereSettings = new VSWhereLatestSettings
    {
        IncludePrerelease = prereleaseTools,
        Requires = "Component.Xamarin"
    };

    var vsLatest = VSWhereLatest(vsWhereSettings);
    msBuildPath = (vsLatest == null)
        ? null
        : vsLatest.CombineWithFilePath("./MSBuild/Current/Bin/MSBuild.exe");

    if (msBuildPath != null)
        Information("Found MSBuild at {0}", msBuildPath.ToString());
});

Task("RestorePackages")
    .IsDependentOn("ResolveBuildTools")
    .Does(() =>
{
    NuGetRestore(sln, new NuGetRestoreSettings { NoCache = true });

    //TODO figure out why it is not working
    // var settings = GetDefaultBuildSettings()
    //     .WithTarget("Restore");
    // MSBuild(sln, settings);
});

Task("BuildAndroid")
    .IsDependentOn("Clean")
    .IsDependentOn("ResolveBuildTools")
    .IsDependentOn("RestorePackages")
    .IsDependentOn("Build")
    .Does(() =>
{  
    var settings = GetDefaultBuildSettings()
        .WithProperty("Version", versionInfo.SemVer)
        .WithProperty("PackageVersion", versionInfo.SemVer)
        .WithProperty("InformationalVersion", versionInfo.InformationalVersion)
        .WithTarget("Build");

    MSBuild(droidProj, settings);
});

Task("BuildiOS")
    .IsDependentOn("Clean")
    .IsDependentOn("ResolveBuildTools")
    .IsDependentOn("RestorePackages")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = GetDefaultBuildSettings()
        .WithProperty("Version", versionInfo.SemVer)
        .WithProperty("PackageVersion", versionInfo.SemVer)
        .WithProperty("InformationalVersion", versionInfo.InformationalVersion)
        .WithTarget("Build");

    MSBuild(iOSProj, settings);
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("ResolveBuildTools")
    .IsDependentOn("RestorePackages")
    .Does(() =>
{
    var settings = GetDefaultBuildSettings()
        .WithProperty("Version", versionInfo.SemVer)
        .WithProperty("PackageVersion", versionInfo.SemVer)
        .WithProperty("InformationalVersion", versionInfo.InformationalVersion)
        .WithTarget("Build");

    MSBuild(coreProj, settings);
});

Task("NugetPack")
    .IsDependentOn("Build")
    .IsDependentOn("BuildAndroid")    
    .IsDependentOn("BuildiOS")    
    .Does(() =>
{
    var version = string.IsNullOrEmpty(versionInfo.PreReleaseLabel)? versionInfo.MajorMinorPatch : $"{versionInfo.MajorMinorPatch}-{versionInfo.PreReleaseLabel}";

    Information($"Making version: {version}");

    NuGetPack("./XamForms.Controls.Calendar.nuspec", new NuGetPackSettings{
            Version = version, 
            Description = "TBD", 
            Verbosity = NuGetVerbosity.Detailed,
        });
});

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("BuildAndroid")
    .IsDependentOn("BuildiOS")
    .IsDependentOn("NugetPack")
    .Does(() => {});

RunTarget(target);

MSBuildSettings GetDefaultBuildSettings()
{
    var settings = new MSBuildSettings
    {
        Configuration = configuration,
        ToolPath = msBuildPath,
        Verbosity = verbosity,
        ArgumentCustomization = args => args.Append("/m"),
        ToolVersion = MSBuildToolVersion.VS2019
    };

    return settings;
}