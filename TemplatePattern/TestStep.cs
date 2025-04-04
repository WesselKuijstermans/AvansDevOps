using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.TemplatePattern
{
    public class TestStep: IPipelineStep
    {
        public void Execute()
        {
            int i = 1;
            while (i < 10)
            {
                // sleep for a tenth of a second
                System.Threading.Thread.Sleep(100);
                Console.WriteLine("Running test " + i);
                // sleep for a random amount of time between 0 and 1 second
                Random random = new Random();
                System.Threading.Thread.Sleep(random.Next(0, 1000));
                Console.WriteLine("Test " + i + " succeeded.");
                i++;
            }

        }
    }
}
