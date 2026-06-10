dotnet ef migrations remove --project ./server/Infrastructure --startup-project ./server/WebApi

if ($LASTEXITCODE -ne 0) {
    Write-Error "An error occurred while removing the migration."
    exit $LASTEXITCODE
}

Write-Host "Migration was successfully removed."