# Get the root directory of the project
$rootDir = Get-Location

# Define the build configurations to check
$config = "Release"
#$config = "Debug"

# Get all project directories
$projectDirs = @("MathProcessor","TextProcessor","CommandValidator","CommandSource","CommandMonitor")
#Get-ChildItem -Path $rootDir -Directory
foreach ($projectDir in $projectDirs) {
        $exePath = Join-Path -Path $rootDir -ChildPath $projectDir
        $exePath = Join-Path $exePath -ChildPath "bin\$config\net8.0\$($projectDir).exe"
        if (Test-Path $exePath) {
            Write-Host "Running $exePath"
            Start-Process -FilePath $exePath
            #-NoNewWindow -Wait
        }else{
            Write-Host "Could not find $exePath"
        }
}
Start-Sleep -Seconds 30