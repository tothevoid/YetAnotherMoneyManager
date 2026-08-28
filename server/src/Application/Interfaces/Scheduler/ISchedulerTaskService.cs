using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Application.DTO.Scheduler;

namespace MoneyManager.Application.Interfaces.Scheduler
{
    public interface ISchedulerTaskService
    {
        Task<IEnumerable<ScheduledTaskDefinitionDto>> GetNotScheduledTasksAsync();

        Task<IEnumerable<ScheduledTaskDto>> GetAllTasksAsync();

        Task<ScheduledTaskDto> GetTaskByNameAsync(string taskName);

        Task<ScheduledTaskDto> CreateTaskAsync(CreateScheduledTaskDto dto);

        Task<ScheduledTaskDto> UpdateScheduleAsync(string taskName, UpdateScheduleDto dto);

        Task<ScheduledTaskDto> ToggleTaskStatusAsync(string taskName, bool isEnabled);

        Task<bool> DeleteTaskAsync(string taskName);
    }
}
