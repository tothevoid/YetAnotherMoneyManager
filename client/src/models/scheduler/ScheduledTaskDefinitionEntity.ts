export interface ScheduledTaskDefinitionEntity {
    taskName: string;
    displayName: string;
    description: string;
    category: string;
    defaultCronExpression: string;
}

export interface ScheduledTaskDefinitionEntityResponse {
    taskName: string;
    displayName: string;
    description: string;
    category: string;
    defaultCronExpression: string;
}

export interface CreateScheduledTaskEntityRequest {
    taskName: string;
    cronExpression: string;
    isEnabled: boolean;
}
