using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orient.Base.Net.Core.Api.Core.Business.IoC;
using Orient.Base.Net.Core.Api.Core.Business.Models.Jobs;
using Orient.Base.Net.Core.Api.Core.Business.Services;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.Business.Tasks
{
    public class BackgroundTask : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private bool _check;
        private int _timeCheck;

        public BackgroundTask(ILogger<BackgroundTask> logger)
        {
            _logger = logger;
            _check = true;
            _timeCheck = 1;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Email and Job Background Task is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        private async Task JobBackgroundTask()
        {
            _check = false;
            var _jobService = IoCHelper.GetInstance<IJobService>();
            var _jobRepository = IoCHelper.GetInstance<IRepository<Job>>();
            var jobs = await _jobRepository.GetAll()
                            .Include(x => x.Reporter)
                            .Include(x => x.UserInJobs)
                                .ThenInclude(UserInJobs => UserInJobs.User)
                            .Include(x => x.Comments)
                                .ThenInclude(comment => comment.User)
                            .Include(x => x.JobInCategories)
                                .ThenInclude(JobInCategories => JobInCategories.Category)
                            .Include(x => x.AttachmentInJobs)
                                .ThenInclude(AttachmentInJobs => AttachmentInJobs.Attachment)
                            .Include(x => x.StepInJobs)
                            .Where(x => (!x.RecordDeleted) && (x.DateRepeat > 0 && !x.IsDisable && !x.IsProcess))
                                .ToListAsync();

            foreach (var job in jobs)
            {
                if (DateTime.UtcNow > job.DateStart)
                {
                    var responseModel = await _jobService.CreateJobAsync(job.ReporterId, new JobManageModel(job));
                    if (responseModel.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        job.IsProcess = true;
                        await _jobRepository.UpdateAsync(job);
                    }
                }
            }
            _logger.LogInformation("Job Background Task is working.");
        }

        private async Task EmailBackgroundTask()
        {
            //get data from table emailQueue
            var _emailQueueRepository = IoCHelper.GetInstance<IRepository<EmailQueue>>();

            var _emailService = IoCHelper.GetInstance<IEmailService>();

            var emailQueues = await _emailQueueRepository.GetAll()
                        .Where(x => (x.SentOn == null && x.SentTries <= 5))
                        .ToListAsync();

            if (emailQueues != null)
            {
                foreach (var emailQueue in emailQueues)
                {
                    var response = await _emailService.SendEmail(emailQueue);
                    emailQueue.SentTries += 1;
                    if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                    {
                        emailQueue.SentOn = DateTime.Now;
                    }
                    await _emailQueueRepository.UpdateAsync(emailQueue);
                }
            }
            _logger.LogInformation("Email Background Task is working.");
        }

        private async void DoWork(object state)
        {
            await EmailBackgroundTask();
            // 60 / 5 = 12, one day have 24h => 24*12 = 288
            if (_check || _timeCheck == 288)
            {
                await JobBackgroundTask();
                if (_timeCheck == 288)
                    _timeCheck = 1;
            }
            else
            {
                _timeCheck++;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Email and Job Background Task is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
