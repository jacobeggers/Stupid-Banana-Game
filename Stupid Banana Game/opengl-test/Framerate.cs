using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opengl_test
{
    public static class Framerate
    {
        private static long prevTime = 0;
        public static int getFramerate()
        {
            long currentTime = DateTime.Now.Ticks;
            int final = (int)(10000000 / (currentTime - prevTime));
            prevTime = currentTime;
            if (final == 0)
            {
                final = 1;
            }
            return final;
        }
    }
}
