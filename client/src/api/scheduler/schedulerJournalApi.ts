import { GetJournalQueryRequest, ScheduledTaskJournalEntity, ScheduledTaskJournalEntityResponse } from '../../models/scheduler/ScheduledTaskJournalEntity';
import { ScheduledTaskExecutionStatus, ScheduledTaskTriggerSource } from '../../models/scheduler/ScheduledTaskEntity';
import { PaginationConfig } from '../../shared/models/PaginationConfig';
import { prepareScheduledTaskJournal } from './schedulerJournalApiMapping';
import { getAllEntitiesByConfig, getPagination } from '../basicApi';

const basicUrl = 'api/Scheduler';

export const getScheduledTaskJournal = async (
    query: GetJournalQueryRequest
): Promise<ScheduledTaskJournalEntity[]> => {
    return await getAllEntitiesByConfig<GetJournalQueryRequest, ScheduledTaskJournalEntityResponse>(`${basicUrl}/journal`, query)
        .then((responses) => (responses || []).map(prepareScheduledTaskJournal));
};

export const getJournalPagination = async (
    taskName?: string,
    status?: ScheduledTaskExecutionStatus,
    triggerSource?: ScheduledTaskTriggerSource
): Promise<PaginationConfig | void> => {
    const params = new URLSearchParams();
    if (taskName && taskName !== 'All') params.append('taskName', taskName);
    if (status !== undefined) params.append('status', status.toString());
    if (triggerSource !== undefined) params.append('triggerSource', triggerSource.toString());

    const queryString = params.toString();
    const url = queryString ? `${basicUrl}/journal/pagination?${queryString}` : `${basicUrl}/journal/pagination`;
    return await getPagination(url);
};
