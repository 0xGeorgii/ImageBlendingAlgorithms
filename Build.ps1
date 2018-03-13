cd ImageBlendingAlgorithms

Write-Host "Cleaning artifacts"
if(Test-Path .\IBALib\artifacts) { Remove-Item .\IBALib\artifacts -Force -Recurse }
if(Test-Path .\Processing\artifacts) { Remove-Item .\Processing\artifacts -Force -Recurse }
Write-Host "Done cleaning artifacts"

Write-Host "Restoring packages"
nuget restore -recursive
Write-Host "Done restoring packages"

Write-Host "Building solution"
dotnet build ImageBlendingAlgorithms.sln
Write-Host "Done building solution"

Write-Host "Executing tests"
dotnet test .\Tests\IBALibTest.csproj -c Release
Write-Host "Done executing tests"

$revision = $env:APPVEYOR_BUILD_NUMBER
Write-Host "Packaging project"
dotnet pack .\IBALib -c Release -o .\artifacts --version-suffix=$revision
dotnet pack .\Processing -c Release -o .\artifacts --version-suffix=$revision
Write-Host "Done packaging projects"
