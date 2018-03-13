Write-Host "Cleaning artifacts"
if(Test-Path .\ImageBlendingAlgorithms\**\artifacts) { Remove-Item .\ImageBlendingAlgorithms\**\artifacts -Force -Recurse }
Write-Host "Done cleaning artifacts"

cd ImageBlendingAlgorithms

Write-Host "Restoring packages"
nuget restore -recursive
Write-Host "Done restoring packages"

Write-Host "Building solution"
dotnet build ImageBlendingAlgorithms.sln
Write-Host "Done building solution"

Write-Host "Executing tests"
dotnet test .\Tests\IBALibTest.csproj -c Release
Write-Host "Done executing tests"

cd ..

$revision = $env:APPVEYOR_BUILD_NUMBER
Write-Host "Packaging project"
dotnet pack .\ImageBlendingAlgorithms\**\ -c Release -o .\artifacts --version-suffix=$revision
Write-Host "Done packaging projects"
