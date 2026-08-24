const basicUrl = 'api/Scheduler';

export const getAttachmentDownloadUrl = (attachmentId: string): string => {
    return `/${basicUrl}/journal/attachments/${attachmentId}/download`;
};
