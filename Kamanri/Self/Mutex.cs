using System.Threading;
using System.Threading.Tasks;
namespace Kamanri.Self
{
	public class Mutex
	{
		public bool mutex { get;private set; } = false;

		private int WaitTime = 5;

		public Mutex(){}

		public Mutex(int waitTime)
		{
			WaitTime = waitTime;
		}

		public Mutex(bool initialMutex = false)
		{
			this.mutex = initialMutex;
		}
		public async Task Wait()
		{
			await Task.Run(() => 
			{
				while (this.mutex == true)
				{
					Thread.Sleep(WaitTime);
				}
				this.mutex = true;
			});

		}
		
		public void Signal()
		{
			this.mutex = false;
		}
	 
	}
}