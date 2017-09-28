using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBaseLibrary
{
    public abstract class XtraSched_LessThanDayInterval : TimeScale
    {// Holds the scale increment, in minutes.
        int scaleValue;
        TimeSpan scaleValueTime;
        int HoraIni;
        int HoraFim;

        public XtraSched_LessThanDayInterval(int ScaleValue)
        {
            scaleValue = ScaleValue;
            scaleValueTime = TimeSpan.FromMinutes(ScaleValue);
        }

        public XtraSched_LessThanDayInterval(int ScaleValue, int hIni, int hFim)
        {
            scaleValue = ScaleValue;
            scaleValueTime = TimeSpan.FromMinutes(ScaleValue);

            HoraIni = hIni;
            HoraFim = hFim;
        }

        // Gets the start of the first time interval.
        protected virtual int FirstIntervalStart(DateTime date)
        {
            return HoraIni * 60;
        }

        // Gets the start of the last time interval.
        protected virtual int LastIntervalStart(DateTime date)
        {
            return HoraFim * 60 - scaleValue;
        }

        // Gets the value used to order the scales.
        protected override TimeSpan SortingWeight
        {
            get { return TimeSpan.FromMinutes(scaleValue + 1); }
        }

        public override DateTime Floor(DateTime date)
        {

            // Performs edge calculations.
            if (date == DateTime.MinValue || date == DateTime.MaxValue)
                return RoundToScaleInterval(date);

            // Rounds down to the last interval in the previous date.
            if (date.TimeOfDay.TotalMinutes < FirstIntervalStart(date))
                return RoundToVisibleIntervalEdge(date.AddDays(-1), LastIntervalStart(date));

            // Rounds down to the last interval in the current date.
            if (date.TimeOfDay.TotalMinutes > LastIntervalStart(date))
                return RoundToVisibleIntervalEdge(date, LastIntervalStart(date));

            // Rounds down to the scale node.
            return RoundToScaleInterval(date);
        }

        protected DateTime RoundToVisibleIntervalEdge(DateTime dateTime, int minutes)
        {
            return dateTime.Date.AddMinutes(minutes);
        }
        protected DateTime RoundToScaleInterval(DateTime date)
        {
            return DevExpress.XtraScheduler.Native.DateTimeHelper.Floor(date, TimeSpan.FromMinutes(scaleValue));
        }
        // Checks for edge conditions.
        protected override bool HasNextDate(DateTime date)
        {
            return date <= (DateTime.MaxValue - scaleValueTime);
        }

        public override DateTime GetNextDate(DateTime date)
        {
            if (HasNextDate(date))
            {
                return (date.TimeOfDay.TotalMinutes > LastIntervalStart(date) - scaleValue) ?
                    RoundToVisibleIntervalEdge(date.AddDays(1), FirstIntervalStart(date.AddDays(1))) : date.AddMinutes(scaleValue);
            }
            else return date;
        }
    }

    public class CustomMinutesScale : XtraSched_LessThanDayInterval
    {
        const int myScaleValue = 30;
        public CustomMinutesScale()
            : base(myScaleValue)
        {
        }
        public CustomMinutesScale(int Scale)
            : base(Scale)
        {
        }

        public CustomMinutesScale(int Scale, int Ini, int Fim)
            : base(Scale, Ini, Fim)
        {

        }

        protected override string DefaultDisplayFormat
        {
            get { return "HH:mm"; }
        }

    }

    public class CustomHourScale : XtraSched_LessThanDayInterval
    {
        const int myScaleValue = 60;
        public CustomHourScale()
            : base(myScaleValue)
        {
        }

        public CustomHourScale(int Scale, int Ini, int Fim)
            : base(Scale, Ini, Fim)
        {
        }


        protected override string DefaultDisplayFormat
        {
            get { return "hh:mm"; }
        }

    }

    public class CustomTimeScaleDay : XtraSched_LessThanDayInterval
    {

        const int myScaleValue = 1440;
        public CustomTimeScaleDay()
            : base(myScaleValue)
        {
        }

        public CustomTimeScaleDay(int Scale, int Ini, int Fim)
            : base(Scale, Ini, Fim)
        {
        }

        public override DateTime Floor(DateTime date)
        {

            // Performs edge calculations.
            if (date == DateTime.MinValue)
                return date.AddMinutes(FirstIntervalStart(date));

            DateTime start = date.Date;

            // Rounds down to the previous date.
            if (date.TimeOfDay.TotalMinutes < FirstIntervalStart(start))
                start = start.AddDays(-1);


            // Rounds down to the scale node.
            return start.AddMinutes(FirstIntervalStart(start));
        }

        protected override string DefaultDisplayFormat
        {
            get { return "dd/MM/yyyy"; }
        }


    }


}
