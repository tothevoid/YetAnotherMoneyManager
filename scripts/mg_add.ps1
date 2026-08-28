param(
    [Parameter(Mandatory = $true)]
    [string]$MigrationName
)

dotnet ef migrations add $MigrationName --project ./server/src/Infrastructure --startup-project ./server/src/WebApi

if ($LASTEXITCODE -ne 0) {
    Write-Error "An error occurred while creating the migration."
    exit $LASTEXITCODE
}

Write-Host "Migration '$MigrationName' was successfully created."