using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.UserData;
using System.Collections.Generic;

namespace Buzz
{
    public struct Slot
    {
        public DateTime Start;
        public DateTime End;
        public Slot(DateTime s, DateTime e)
        {
            Start = s;
            End = e;
        }
    }

    public class AppointmentManager
    {
        public AppointmentManager()
        {
        }

        public void GetFreeTime()
        {
            Appointments appts = new Appointments();

            //Identify the method that runs after the asynchronous search completes.
            appts.SearchCompleted += new EventHandler<AppointmentsSearchEventArgs>(Appointments_SearchCompleted);

            DateTime start = DateTime.Now;

            //Hard Code to 7 days
            DateTime end = start.AddDays(7);

            //Hard limit
            int max = 5;

            //Start the asynchronous search.
            appts.SearchAsync(start, end, max, "Appointments Test #1");
        }


        private void Appointments_SearchCompleted(object sender, AppointmentsSearchEventArgs e)
        {
            List<Slot> freeTimes = null;

            try
            {
                //Bind the results to the user interface.
                //AppointmentResultsData.DataContext = e.Results;
                var appts = e.Results;

                List<Slot> busyTimes = new List<Slot>();
                freeTimes = new List<Slot>();

                //Add logic to stop all day events?
                foreach (var appt in appts)
                {
                    busyTimes.Add(new Slot(appt.StartTime, appt.EndTime));
                }

                DateTime startTimeOfDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 8, 0, 0);
                DateTime endTimeOfDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 0, 0);

                var slotBegin = startTimeOfDay;
                var slotEnd = endTimeOfDay < busyTimes[0].Start ? endTimeOfDay : busyTimes[0].Start;

                int endCount = 0;
                freeTimes.Add(new Slot(startTimeOfDay, slotEnd));
                for (int startCount = 1; startCount < busyTimes.Count; startCount++)
                {
                    startTimeOfDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, busyTimes[startCount].Start.Day, 8, 0, 0);
                    endTimeOfDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, busyTimes[endCount].End.Day, 23, 0, 0);

                    if (startTimeOfDay > busyTimes[endCount].End)
                        slotBegin = startTimeOfDay;
                    else
                        slotBegin = busyTimes[endCount].End;

                    if (endTimeOfDay < busyTimes[startCount].Start)
                        slotEnd = endTimeOfDay;
                    else
                        slotEnd = busyTimes[startCount].Start;


                    endCount++;
                    freeTimes.Add(new Slot(slotBegin, slotEnd));

                }
                freeTimes.Add(new Slot(busyTimes[busyTimes.Count - 1].End, endTimeOfDay));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("");
            }

            FreeTimesAvailable(freeTimes);
        }

        public delegate void FreeTimesAvailableDelegate(List<Slot> FreeTimes);
        public event FreeTimesAvailableDelegate FreeTimesAvailable;
    }
}
