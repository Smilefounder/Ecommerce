using Kooboo.Commerce.Data.Folders;
using Kooboo.Commerce.Recommendations.Engine.Jobs;
using Kooboo.Commerce.Recommendations.Models;
using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Recommendations.Controllers
{
    public class ConfigController : CommerceController
    {
        public ActionResult Jobs()
        {
            var schedules = Schedulers.Get(CurrentInstance.Name).Schedules.ToList();
            return View(schedules);
        }

        public ActionResult JobConfig(string jobName)
        {
            var schedule = Schedulers.Get(CurrentInstance.Name).GetSchedule(jobName);
            return JsonNet(new JobConfigModel
            {
                JobName = jobName,
                Interval = (int)schedule.Interval.TotalMinutes,
                StartHour = schedule.StartTime.Hour,
                StartMinute = schedule.StartTime.Mintue
            }).UsingClientConvention();
        }

        [HttpPost]
        public void JobConfig(JobConfigModel model)
        {
            Kooboo.Commerce.Recommendations.Engine.Jobs.JobConfig.Update(CurrentInstance.Name, new JobConfig
            {
                Interval = TimeSpan.FromMinutes(model.Interval),
                StartTime = new TimeOfDay(model.StartHour, model.StartMinute)
            });

            var scheduler = Schedulers.Get(CurrentInstance.Name);
            scheduler.Reschedule(model.JobName, TimeSpan.FromMinutes(model.Interval), new TimeOfDay(model.StartHour, model.StartMinute));
        }

        public ActionResult Weights()
        {
            return View();
        }
    }
}
