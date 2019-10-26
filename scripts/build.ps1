# https://www.appveyor.com/docs/environment-variables/

# Script static variables
$buildDir = $env:APPVEYOR_BUILD_FOLDER
$buildNumber = $env:APPVEYOR_BUILD_VERSION

$projectDir = $buildDir + "\";
$projectFile = $projectDir + "\$env:APPVEYOR_PROJECT_NAME.csproj";
$nugetFile = $projectDir + $env:APPVEYOR_PROJECT_NAME + "." + $buildNumber + ".nupkg";
$configFile = $projectDir + "appsettings.json";

# Prepairing configuration file
Write-Host "Prepairing configuration file" -ForegroundColor Green
(Get-Content $configFile -Raw).Replace('NATS_SERVER_NAME',$env:NATS_SERVER_NAME) | 
 Set-Content $configFile 
(Get-Content $configFile -Raw).Replace('NATS_SERVER_PORT',$env:NATS_SERVER_PORT) | 
 Set-Content $configFile 

# Display .Net Core version
Write-Host "Checking .NET Core version" -ForegroundColor Green
& dotnet --version

# Restore the main project
Write-Host "Restoring project" -ForegroundColor Green
& dotnet restore --source "http://servicelab.tk:5555/v3/index.json" --source "https://api.nuget.org/v3/index.json"

# Publish the project
Write-Host "Publish the project" -ForegroundColor Green
& dotnet publish --configuration Release -r linux-x64 --self-contained true


# Compress artifact
$pathToCompress = $projectDir+"\bin\Release\*";
$compressedArtifactName	= $env:APPVEYOR_PROJECT_NAME +"."+ $env:APPVEYOR_BUILD_VERSION + ".zip";

Write-Host "Generate zip artifact" -ForegroundColor Green
Compress-Archive -Path $pathToCompress -CompressionLevel Fastest -DestinationPath "compressed"
Push-AppveyorArtifact "compressed.zip" -FileName $compressedArtifactName

# Done
Write-Host "Done!" -ForegroundColor Green