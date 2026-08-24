export enum ScheduledTaskExecutionStatus {
    Unknown = 0,
    Success = 1,
    Failed = 2,
    Running = 3
}

export enum ScheduledTaskTriggerSource {
    Scheduled = 1,
    Manual = 2
}

export interface ScheduledTaskEntity {
    taskName: string;
    displayName: string;
    description: string;
    cronExpression: string;
    isEnabled: boolean;
    nextExecutionUtc?: Date;
    lastExecutionUtc?: Date;
    lastExecutionStatus: ScheduledTaskExecutionStatus;
    lastExecutionDurationMs?: number;
    category: string;
}

export interface ScheduledTaskEntityResponse {
    taskName: string;
    displayName: string;
    description: string;
    cronExpression: string;
    isEnabled: boolean;
    nextExecutionUtc?: string;
    lastExecutionUtc?: string;
    lastExecutionStatus: ScheduledTaskExecutionStatus;
    lastExecutionDurationMs?: number;
    category: string;
}

export interface UpdateScheduleEntityRequest {
    cronExpression?: string;
    isEnabled?: boolean;
}
