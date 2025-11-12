# Delete the .quickpulse directory if it exists
Remove-Item -Path ".quickpulse" -Recurse -Force
# Recursively delete all .log files from the current directory
Get-ChildItem -Path . -Filter *.log -Recurse -Force | Remove-Item -Force -ErrorAction SilentlyContinue