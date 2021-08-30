using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransferEmployeeAndJobService.ReqModels;

namespace TransferEmployeeAndJobService
{
    class Listener : BackgroundService
    {
        private EmployeeRequestDBContext _contextReq;

        public Listener()
        {
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("listener log");
                try
                {
                    

                    await Task.Delay(5000, stoppingToken);
                }
                catch (Exception e)
                {
                    await LogErrors(DateTime.Now.ToString() + Environment.NewLine + e.Message + Environment.NewLine + Environment.NewLine);
                }

            }
        }

        private async static Task LogErrors(string error)
        {
            using (StreamWriter writetext = new StreamWriter("log.txt", true))
            {
                await writetext.WriteLineAsync(error);
            }
        }
    }
}
