using System.Threading;

namespace CreditCards.UITests
{
    //Slow down browser interactions.
    internal static class DemoHelper
    {
        public static void Pause(int secondsToPause = 3000)
        {
            Thread.Sleep(secondsToPause);
        }
    }
}
