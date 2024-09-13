namespace PinfoBackend.Cpu
{
	public interface ICpuManager
	{
		double GetCpuLoadPercentage();
	}
	public class CpuManager : ICpuManager
	{
		public static string CpuArchitecture => System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString();

		public double GetCpuLoadPercentage()
		{
			string loadAvgContent = File.ReadAllText("/host/proc/loadavg");
			string[] loadAvgValues = loadAvgContent.Split(' ');
			double oneMinuteLoadAvg = double.Parse(loadAvgValues[0]);

			int cpuCoreCount = 0;
			string[] cpuInfoLines = File.ReadAllLines("/host/proc/cpuinfo");
			foreach (string line in cpuInfoLines)
			{
				if (line.StartsWith("processor"))
				{
					cpuCoreCount++;
				}
			}

			double cpuLoadPercentage = (oneMinuteLoadAvg / cpuCoreCount) * 100;

			return cpuLoadPercentage;
		}
	}
}