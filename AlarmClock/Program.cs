using System;
using System.Media;
using System.Threading;

namespace AlarmClock
{
    class Program
    {
        private static int[,] ALARM_TRIGGER_TIME =
        {
            //HH, MM, SS
            {  2, 20, 00 },
            { 10, 20, 00 },
            { 18, 20, 00 }
        };

        private static void Main(string[] args)
        {
            const string SOUND_PATH = "alarm.wav";

            try
            {
                using (SoundPlayer sp = new SoundPlayer(SOUND_PATH))
                {
                    sp.LoadAsync();
                    Console.WriteLine("ALARM ARMED!");

                    while (true)
                    {
                        int ActiveTriggerTime = -1;

                        while ((ActiveTriggerTime = GetActiveTriggerTime()) < 0)
                        {
                            Thread.Sleep(100);
                        }
                        
                        sp.PlayLooping();
                        Console.WriteLine("ALARM TRIGGERED! {0}:{1}:{2}", ALARM_TRIGGER_TIME[ActiveTriggerTime, 0],
                                                                            ALARM_TRIGGER_TIME[ActiveTriggerTime, 1],
                                                                            ALARM_TRIGGER_TIME[ActiveTriggerTime, 2]);

                        Console.ReadKey();
                        
                        sp.Stop();
                        Console.WriteLine("ALARM STOPPED!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.HResult + Environment.NewLine +
                                    ((ex.Message    == null || ex.Message               == string.Empty) ? string.Empty : ("Message: " + ex.Message        + Environment.NewLine)) +
                                    ((ex.Source     == null || ex.Source                == string.Empty) ? string.Empty : ("Source: " + ex.Source          + Environment.NewLine)) +
                                    ((ex.Data       == null || ex.Data.ToString()       == string.Empty) ? string.Empty : ("Data: " + ex.Data              + Environment.NewLine)) +
                                    ((ex.TargetSite == null || ex.TargetSite.ToString() == string.Empty) ? string.Empty : ("Target Site: " + ex.TargetSite + Environment.NewLine)) +
                                    ((ex.HelpLink   == null || ex.HelpLink              == string.Empty) ? string.Empty : ("Help Link: " + ex.HelpLink)));
            }

            Console.ReadKey();
        }

        private static int GetActiveTriggerTime()
        {
            DateTime dtNow = DateTime.Now;

            for (int i = 0; i < ALARM_TRIGGER_TIME.GetLength(0); i++)
            {
                if (dtNow.Hour == ALARM_TRIGGER_TIME[i, 0] &&
                    dtNow.Minute == ALARM_TRIGGER_TIME[i, 1] &&
                    dtNow.Second == ALARM_TRIGGER_TIME[i, 2])
                    return i;
            }

            return -1;
        }
    }
}
