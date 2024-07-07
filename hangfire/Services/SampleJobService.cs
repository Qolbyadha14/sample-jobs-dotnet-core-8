namespace hangfire_jobs.Services
{
    public class SampleJobService
    {
        public void ProcessBackgroundJob()
        {
            Console.WriteLine("Background job executed at: " + DateTime.Now);
        }

        public void ProcessCronJob()
        {
            Console.WriteLine("Cron job executed at: " + DateTime.Now);
        }
    }
}
