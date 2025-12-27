# Delete the .quickcheckr directory
Remove-Item ".quickcheckr" -Recurse -Force -ErrorAction SilentlyContinue
# Recursively delete all .report files from the current directory
Get-ChildItem -Path . -Filter *.qr -Recurse -Force | Remove-Item -Force -ErrorAction SilentlyContinue
Get-ChildItem -Path . -Filter *.qv -Recurse -Force | Remove-Item -Force -ErrorAction SilentlyContinue
Get-ChildItem -Path . -Filter *.ql -Recurse -Force | Remove-Item -Force -ErrorAction SilentlyContinue