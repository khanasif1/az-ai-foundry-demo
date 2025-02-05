# check_venv.ps1

# Check if the VIRTUAL_ENV environment variable is set
if ($env:VIRTUAL_ENV) {
    Write-Output "Python virtual environment is enabled."
    Write-Output "Virtual environment path: $env:VIRTUAL_ENV"
} else {
    Write-Output "Python virtual environment is not enabled."
}