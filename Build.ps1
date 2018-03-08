if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

cd ImageBlendingAlgorithms

dotnet restore

MSBuild.exe ImageBlendingAlgorithms.sln /p:Configuration=Release /p:Platform="Any CPU"

dotnet test .\Tests\IBALibTest.csproj -c Release
$revision = '0.0.' + $env:APPVEYOR_BUILD_NUMBER
dotnet pack .\IBALib -c Release -o .\artifacts --version=$revision
