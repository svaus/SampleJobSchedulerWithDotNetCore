namespace SampleJobSchedulerWithDotNetCore
{
    public interface ISchedulerService
    {
        void Start();
        void Shutdown();
    }
}