using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PCShutDownTime
{
    public class MyTaskScheduJob : IJob
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
          await   Task.Run(()=> {
              //Console.WriteLine(DateTime.Now.ToString(MainWindow.TimeF)) ;
              //Console.WriteLine("你好");
             Process.Start("c:/windows/system32/shutdown.exe", "-s -t 0");
            });
        }
    }
}
