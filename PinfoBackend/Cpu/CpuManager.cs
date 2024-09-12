namespace PinfoBackend.Cpu
{
	public static class CpuManager
	{
		public static string CpuArchitecture => System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString();

		public static double GetCpuLoadPercentage()
		{
			// Read the 1-minute load average from /proc/loadavg
			string loadAvgContent = File.ReadAllText("/host/proc/loadavg");
			string[] loadAvgValues = loadAvgContent.Split(' ');
			double oneMinuteLoadAvg = double.Parse(loadAvgValues[0]);

			// Get the number of CPU cores from /proc/cpuinfo
			int cpuCoreCount = 0;
			string[] cpuInfoLines = File.ReadAllLines("/host/proc/cpuinfo");
			foreach (string line in cpuInfoLines)
			{
				if (line.StartsWith("processor"))
				{
					cpuCoreCount++;
				}
			}

			// Calculate the CPU load percentage
			double cpuLoadPercentage = (oneMinuteLoadAvg / cpuCoreCount) * 100;

			return cpuLoadPercentage;
		}
	}
}