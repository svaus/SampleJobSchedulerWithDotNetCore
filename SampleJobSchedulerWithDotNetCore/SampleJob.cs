using System.Threading.Tasks;
using log4net;
using Quartz;

namespace SampleJobSchedulerWithDotNetCore
{
    public class SampleJob : IJob
    {
        private readonly ILog _log;

        public SampleJob(ILog log)
        {
            _log = log;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _log.Info("Execute Job is working");

            // Write background job logic. All the business logic goes here.

            await Task.FromResult(0);
        }
    }
}