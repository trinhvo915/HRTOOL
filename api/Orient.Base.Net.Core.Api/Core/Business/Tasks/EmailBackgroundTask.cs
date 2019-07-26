using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orient.Base.Net.Core.Api.Core.Business.IoC;
using Orient.Base.Net.Core.Api.Core.DataAccess.Repositories.Base;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Business.Services;

namespace Orient.Base.Net.Core.Api.Core.Business.Tasks
{
    public class EmailBackgroundTask : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;

        public EmailBackgroundTask(ILogger<EmailBackgroundTask> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Email Background Task is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Email Background Task is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}

