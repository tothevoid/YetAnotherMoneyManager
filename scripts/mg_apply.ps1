dotnet ef database update --project ./server/src/Infrastructure --startup-project ./server/src/WebApi

if ($LASTEXITCODE -ne 0) {
    Write-Error "An error occurred while applying the migration."
    exit $LASTEXITCODE
}

Write-Host "Migration was successfully applied."