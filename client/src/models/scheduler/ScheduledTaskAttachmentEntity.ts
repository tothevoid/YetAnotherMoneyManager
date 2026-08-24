export interface ScheduledTaskAttachmentEntity {
    id: string;
    occurrenceId: string;
    fileName: string;
    storagePath: string;
    contentType: string;
    fileSizeBytes: number;
    createdAt: Date;
}

export interface ScheduledTaskAttachmentEntityResponse {
    id: string;
    occurrenceId: string;
    fileName: string;
    storagePath: string;
    contentType: string;
    fileSizeBytes: number;
    createdAt: string;
}
