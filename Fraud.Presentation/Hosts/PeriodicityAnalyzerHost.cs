using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Fraud.Infrastructure.Repository;
using Fraud.Interactor.Transactions;
using Fraud.UseCase.Transactions;
using Microsoft.Extensions.Hosting;

namespace Fraud.Presentation.Hosts
{
    public class PeriodicityAnalyzerHost : IHostedService, IDisposable
    {
        private readonly ITransactionAnalyzer _transactionAnalyzer;
        private readonly ITransactionRepository _transactionRepository;
        
        private readonly int _intervalInMillis = TimeSpan.FromMinutes(1).Milliseconds; // 1 minute
        private Timer _timer;
        
        public PeriodicityAnalyzerHost(ITransactionRepository transactionRepository, PeriodicityAnalyzer periodicityAnalyzer)
        {
            _transactionRepository = transactionRepository;
            _transactionAnalyzer = periodicityAnalyzer;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Timer will fire 10 seconds after start
            _timer = new Timer(DoWork, null, 10000, Timeout.Infinite);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);       
            return Task.CompletedTask;
        }
        
        private async void DoWork(object state)
        {
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                var dateFrom = DateTimeOffset.Now.Date;
                var dateTo = DateTimeOffset.Now.AddDays(1).AddHours(23).AddMinutes(0).AddSeconds(0);
                var transactions = await _transactionRepository.FindByDateRange("", dateFrom, dateTo);
                _transactionAnalyzer.AnalyzeTransactions(transactions);
            }
            finally
            {
                // Reset timer after long running operation
                _timer.Change(Math.Max(0, _intervalInMillis - watch.ElapsedMilliseconds), Timeout.Infinite);  
            }
        }  

        public void Dispose()
        {
            _transactionRepository?.Dispose();
            _timer?.Dispose();
        }
    }
}