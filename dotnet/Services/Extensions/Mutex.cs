using System.Threading;
using System.Threading.Tasks;
namespace dotnet.Services.Extensions
{
    public class Mutex
    {
        public bool mutex { get;private set; } = false;
        public async Task Wait()
        {
            await Task.Run(() => 
            {
                while (this.mutex == true)
                {
                    Thread.Sleep(100);
                    System.Console.WriteLine(Thread.GetCurrentProcessorId());
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