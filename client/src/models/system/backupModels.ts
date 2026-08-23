export interface BackupValidationResult {
    isValid: boolean;
    isEncrypted: boolean;
    errorMessage?: string;
}

export interface RestoreBackupResult {
    success: boolean;
    message?: string;
}
