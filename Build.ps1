if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }
exec { & dotnet restore }
exec { & dotnet test .\IBALibTest -c Release }
$revision = '0.0.' + $env:APPVEYOR_BUILD_NUMBER
exec { & dotnet pack .\src\IBALib -c Release -o .\artifacts --version=$revision }
