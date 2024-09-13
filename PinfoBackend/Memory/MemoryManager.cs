using Serilog;

namespace PinfoBackend.Memory
{
	public interface IMemoryManager
	{
		MemInfoDto GetMemoryLoad();
	}
	
	public class MemoryManager : IMemoryManager
	{
		public MemInfoDto GetMemoryLoad()
		{
			try
			{
				string[] lines = File.ReadAllLines("/host/proc/meminfo");

				// Variables to store memory information
				long totalMemoryKB = 0;
				long availableMemoryKB = 0;
				
				foreach (string line in lines)
				{
					if (line.StartsWith("MemTotal:"))
					{
						totalMemoryKB = ParseMemInfoLine(line);
					}
					else if (line.StartsWith("MemAvailable:"))
					{
						availableMemoryKB = ParseMemInfoLine(line);
					}

					// Break out once we have both values
					if (totalMemoryKB > 0 && availableMemoryKB > 0)
					{
						break;
					}
				}

				// Calculate used memory
				long usedMemoryKB = totalMemoryKB - availableMemoryKB;

				// Convert from KB to MB
				double totalMemoryMB = totalMemoryKB / 1024.0;
				double usedMemoryMB = usedMemoryKB / 1024.0;

				return new MemInfoDto()
				{
					CurrentlyUsed = usedMemoryMB,
					Total = totalMemoryMB
				};
			}
			catch (Exception e)
			{
				Log.Error(e.Message);
				throw;
			}
		}
		
		private static long ParseMemInfoLine(string line)
		{
			// The line format is: "MemTotal:      16390084 kB"
			string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			return long.Parse(parts[1]); // The second element is the value in kB
		}
	}
}