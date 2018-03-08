if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

cd ImageBlendingAlgorithms

nuget restore -recursive

MSBuild.exe ImageBlendingAlgorithms.sln /p:Configuration=Release /p:Platform="Any CPU"

dotnet test .\Tests\IBALibTest.csproj -c Release
$revision = $env:APPVEYOR_BUILD_NUMBER
dotnet pack .\IBALib /p:Platform="Any CPU" -c Release -o .\artifacts /p:PackageVersion=0.0.1 --version-suffix=$revision
