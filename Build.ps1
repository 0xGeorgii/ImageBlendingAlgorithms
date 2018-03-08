function Exec  
{
    [CmdletBinding()]
    param(
        [Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
        [Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }
exec { & dotnet restore }
exec { & dotnet test .\IBALibTest -c Release }
$revision = '0.0.' + $env:APPVEYOR_BUILD_NUMBER
exec { & dotnet pack .\src\IBALib -c Release -o .\artifacts --version=$revision }
