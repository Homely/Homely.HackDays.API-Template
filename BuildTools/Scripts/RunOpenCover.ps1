Param(
    [string]$sourcesDirectory, #the root of your project
    [string]$testAssembly, #the file pattern describing test assemblies to look for
    [string]$testFiltercriteria="", #test filter criteria (as in Run Visual Studio Tests task)
    [string]$openCoverFilters="+[*]*" #OpenCover-specific filters
)

. $PSScriptRoot\vsts-task-lib\LongPathFunctions.ps1
. $PSScriptRoot\vsts-task-lib\TraceFunctions.ps1
. $PSScriptRoot\vsts-task-lib\LegacyFindFunctions.ps1

# resolve test assembly files (copied from VSTest.ps1)
$testAssemblyFiles = @()
# check for solution pattern
if ($testAssembly.Contains("*") -Or $testAssembly.Contains("?"))
{
    Write-Host "Pattern found in solution parameter. Calling Find-Files."
    Write-Host "Calling Find-Files with pattern: $testAssembly"    
    $testAssemblyFiles = Find-Files -LegacyPattern $testAssembly -LiteralDirectory $sourcesDirectory
    Write-Host "Found files: $testAssemblyFiles"
}
else
{
    Write-Host "No Pattern found in solution parameter."
    $testAssembly = $testAssembly.Replace(';;', "`0") # Borrowed from Legacy File Handler
    foreach ($assembly in $testAssembly.Split(";"))
    {
        $testAssemblyFiles += ,($assembly.Replace("`0",";"))
    }
}

# build test assebly files string for vstest
$testFilesString = ""
foreach ($file in $testAssemblyFiles) {
    $testFilesString = $testFilesString + " ""$file"""
}

Write-Host $MyInvocation.MyCommand ": Removing old testresults"
Remove-Item -Path $PSScriptRoot\TestResults -Recurse -Force -ErrorAction SilentlyContinue 

Write-Host $MyInvocation.MyCommand ": Running OpenCover using vstest.console.exe"
Start-Process "$PSScriptRoot\..\Packages\OpenCover.4.6.519\OpenCover.Console.exe" -wait -NoNewWindow -ArgumentList "-register:user -filter:""$OpenCoverFilters"" -target:""%VS140COMNTOOLS%\..\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"" -targetargs:""$testFilesString /TestCaseFilter:$testFiltercriteria /logger:trx"" -output:OpenCover.xml -mergebyhash" -WorkingDirectory $PSScriptRoot

Write-Host $MyInvocation.MyCommand ": Converting Code Coverage result to Cobertura format"
Start-Process "$PSScriptRoot\..\Packages\OpenCoverToCoberturaConverter.0.2.6.0\tools\OpenCoverToCoberturaConverter.exe" -Wait -NoNewWindow -ArgumentList "-input:""$PSScriptRoot\OpenCover.xml"" -output:""$PSScriptRoot\Cobertura.xml"" -sources:""$sourcesDirectory"""

Write-Host $MyInvocation.MyCommand ": Generating HTML report"
Start-Process "$PSScriptRoot\..\Packages\ReportGenerator.2.5.6\tools\ReportGenerator.exe" -Wait -NoNewWindow -ArgumentList "-reports:""$PSScriptRoot\OpenCover.xml"" -targetdir:""$PSScriptRoot\CoverageReport"""