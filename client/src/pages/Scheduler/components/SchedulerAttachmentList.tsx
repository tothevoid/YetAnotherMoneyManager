import React from 'react';
import { Box, Button, Flex, HStack, Icon, Text, VStack } from '@chakra-ui/react';
import { useTranslation } from 'react-i18next';
import { MdAttachFile, MdDownload, MdInsertDriveFile } from 'react-icons/md';
import { ScheduledTaskAttachmentEntity } from '../../../models/scheduler/ScheduledTaskAttachmentEntity';
import { getAttachmentDownloadUrl } from '../../../api/scheduler/schedulerAttachmentApi';
import { formatFileSize } from '../../../shared/utilities/formatters/fileFormatter';

interface SchedulerAttachmentListProps {
    attachments?: ScheduledTaskAttachmentEntity[];
    variant?: 'boxed' | 'plain';
}

export const SchedulerAttachmentList: React.FC<SchedulerAttachmentListProps> = ({
    attachments = [],
    variant = 'boxed'
}) => {
    const { t } = useTranslation();

    if (!attachments || attachments.length === 0) {
        return null;
    }

    const items = (
        <VStack align="stretch" gap={2}>
            {attachments.map((att) => (
                <Flex
                    key={att.id}
                    justify="space-between"
                    align="center"
                    p={2.5}
                    borderRadius="md"
                    backgroundColor={variant === 'boxed' ? 'background_secondary' : 'background_primary'}
                    borderWidth="1px"
                    borderColor="border_primary"
                >
                    <HStack gap={2.5} overflow="hidden">
                        <Icon fontSize="20px" color="action_primary" flexShrink={0}>
                            <MdInsertDriveFile />
                        </Icon>
                        <VStack align="flex-start" gap={0} overflow="hidden">
                            <Text fontSize="xs" fontWeight="medium" color="text_primary" truncate>
                                {att.fileName}
                            </Text>
                            <Text fontSize="2xs" color="text_secondary">
                                {formatFileSize(att.fileSizeBytes)}
                            </Text>
                        </VStack>
                    </HStack>

                    <a
                        href={getAttachmentDownloadUrl(att.id)}
                        download={att.fileName}
                        style={{ textDecoration: 'none', flexShrink: 0 }}
                        onClick={(e) => e.stopPropagation()}
                    >
                        <Button
                            size="xs"
                            variant="outline"
                            color="text_primary"
                            borderColor="border_primary"
                        >
                            <Icon mr={1}><MdDownload /></Icon>
                            {t('scheduler_journal_download')}
                        </Button>
                    </a>
                </Flex>
            ))}
        </VStack>
    );

    if (variant === 'plain') {
        return items;
    }

    return (
        <Box
            p={3.5}
            borderRadius="md"
            backgroundColor="background_primary"
            borderWidth="1px"
            borderColor="border_primary"
        >
            <HStack gap={1.5} mb={2} color="text_primary">
                <Icon color="action_primary"><MdAttachFile /></Icon>
                <Text fontSize="xs" fontWeight="semibold">
                    {t('scheduler_journal_attachments')} ({attachments.length})
                </Text>
            </HStack>
            {items}
        </Box>
    );
};
