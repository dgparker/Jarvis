using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace Jarvis
{
    class Program
    {

        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        ///
        /// Where all the magic Happens!!
        ///
        static void Main(string[] args)
        {

            List<string> cpuMaxedOutMessages = new List<string>();
            cpuMaxedOutMessages.Add("WARNING: Holy crap your CPU is about to catch fire!");
            cpuMaxedOutMessages.Add("WARNING: oh my god you should not run your cpu that hard");
            cpuMaxedOutMessages.Add("WARNING: RED ALERT RED ALERT RED ALERT RED ALERT RED ALERT I FARTED");
            cpuMaxedOutMessages.Add("WARNING: STOP DOWNLOADING THE PORN IT'S MAXING ME OUT");



            //This will pull the current CPU load in percentage
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfCpuCount.NextValue();

            //This will pull the current memory in Megabytes
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available Mbytes");
            perfMemCount.NextValue();

            //This will pull the system uptime (in seconds)
            PerformanceCounter perfUptimeCount = new PerformanceCounter("System", "System Up Time");
            perfUptimeCount.NextValue();

            //Greets the user
            
            Speak("Welcome to Jarvis version one point Oh!", VoiceGender.Male);

            TimeSpan uptimeSpan = TimeSpan.FromSeconds(perfUptimeCount.NextValue());

            string systemUptimeMessage = string.Format("The current system up time is {0} days {1} hours {2} minutes {3} seconds",
                (int)uptimeSpan.TotalDays,
                (int)uptimeSpan.Hours,
                (int)uptimeSpan.Minutes,
                (int)uptimeSpan.Seconds            
                );

            //Tell the user what current system uptime is
           Speak(systemUptimeMessage, VoiceGender.Male);


            // Infinite While Loop
            while (true)
            {
                // Get the current performance counter values
                int currentCpuPercentage = (int)perfCpuCount.NextValue();
                int currentAvailableMemory = (int)perfMemCount.NextValue();

                //Every 1 second print the CPU and memory load to the screen
                Console.WriteLine("CPU LOAD        : {0}%", currentCpuPercentage);
                Console.WriteLine("Available Memory: {0}MB", currentAvailableMemory);

                if(currentCpuPercentage > 80)
                {
                    if (currentCpuPercentage == 100)
                    {
                        string cpuLoadVocalMessage = String.Format("WARNING: Holy crap your CPU is about to catch fire!", currentCpuPercentage);
                        Speak(cpuLoadVocalMessage, VoiceGender.Female);
                    }
                    else
                    {
                        string cpuLoadVocalMessage = String.Format("The current CPU load is {0} percent", currentCpuPercentage);
                        Speak(cpuLoadVocalMessage, VoiceGender.Female);
                    }

                }

                if(currentAvailableMemory < 1024)
                {
                    string memAvailableVocalMessage = String.Format("You currently have {0} megabytes of memory available", currentAvailableMemory);
                    Speak(memAvailableVocalMessage, VoiceGender.Female);
                }                 


                Thread.Sleep(1000);
            }
        }

        //custom speak method for choosing a voice
        public static void Speak(string message, VoiceGender voiceGender)
        {
            synth.SelectVoiceByHints(voiceGender);
            synth.Speak(message);
        }
        public static void Speak(string message, VoiceGender voiceGender, int rate)
        {
            synth.Rate = rate;
            Speak(message, voiceGender);
        }
    }
}
