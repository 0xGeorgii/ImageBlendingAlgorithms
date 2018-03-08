if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }
cd ImageBlendingAlgorithms
dotnet restore
dotnet test .\IBALibTest -c Release
$revision = '0.0.' + $env:APPVEYOR_BUILD_NUMBER
dotnet pack .\src\IBALib -c Release -o .\artifacts --version=$revision
