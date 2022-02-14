using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace PCShutDownTime
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            String lang = System.Globalization.CultureInfo.CurrentCulture.Name;

            InitializeComponent();
        }
        public const string TimeF = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 开始任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Opentask_Click(object sender, RoutedEventArgs e)
        {
            Opentask.IsEnabled = false;
            FinishTask.IsEnabled = true;
            LockSlider();
            var  thistime = DateTime.Now;
             string time= thistime.ToString(TimeF);
            OpenTaskTime.Text = time;
            int hour = int.Parse(textBox_hour.Text);
            int min = int.Parse(textBox_min.Text);
            int sec = int.Parse(textBox_sec.Text);
            var endtime = thistime.AddHours(hour).AddMinutes(min).AddSeconds(sec);
            EstimateTime.Text = endtime.ToString(TimeF);
            StarAsync(endtime).GetAwaiter() ;
        }
        private void LockSlider() {
            Slider_hour.IsEnabled = false;
            Slider_min.IsEnabled = false;
            Slider_sec.IsEnabled = false;

        }
        private void UnLockSlider()
        {
            Slider_hour.IsEnabled = true;
            Slider_min.IsEnabled = true;
            Slider_sec.IsEnabled = true;
        }
        /// <summary>
        /// 结束任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinishTask_Click(object sender, RoutedEventArgs e)
        {
            Opentask.IsEnabled = true;
            FinishTask.IsEnabled = false;
            sched.Clear();
            UnLockSlider();
            OpenTaskTime.Text = "请开始任务";
            EstimateTime.Text = "未知";
        }
        IScheduler sched = null;
        public async Task StarAsync(DateTime endtime)
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
             sched = await schedFact.GetScheduler();
            await sched.Clear();
            //2.启动 scheduler
            await sched.Start();

            // 3.创建 job
            IJobDetail job = JobBuilder.Create<MyTaskScheduJob>()
                   .WithIdentity("job1", "group1")
                    .Build();

            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
             .WithIdentity("trigger1", "group1")
             .StartAt(endtime)
                .Build();

            // 5.使用trigger规划执行任务job
            await sched.ScheduleJob(job, trigger);
        }
    }
}
