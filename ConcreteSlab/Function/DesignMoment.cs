using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossStruc.ConcreteSlab.Function
{
    public class DesignMoment
    {
        public static (int, int, int, int) SlabDesignMoment(int Mx, int My, int Mxy)
        {
            int MdxBot; int MdyBot;
            int MdxTop; int MdyTop;

            // For bottom design moment
            MdxBot = Mx - Math.Abs(Mxy);
            MdyBot = My - Math.Abs(Mxy);

            if (MdxBot > 0 && MdyBot > 0)
            {
                MdxBot = 0;
                MdyBot = 0;
            }
            else if ((MdxBot > 0 || MdyBot > 0))
            {
                if (MdxBot > 0)
                {
                    MdxBot = 0;
                    MdyBot = Convert.ToInt32(My - Math.Abs((double)Math.Pow(Mxy, 2) / Mx));
                }
                if (MdyBot > 0)
                {
                    MdxBot = Convert.ToInt32(Mx - Math.Abs((double)Math.Pow(Mxy, 2) / My));
                    MdyBot = 0;
                }
            }

            // For top design moment
            MdxTop = Mx + Math.Abs(Mxy);
            MdyTop = My + Math.Abs(Mxy);

            if (MdxTop < 0 && MdyTop < 0)
            {
                MdxTop = 0;
                MdyTop = 0;
            }
            else if (MdxTop < 0 || MdyTop < 0)
            {
                if (MdxTop < 0)
                {
                    MdxTop = 0;
                    MdyTop = Convert.ToInt32(My + Math.Abs((double)Math.Pow(Mxy, 2) / Mx));
                }
                if (MdyTop < 0)
                {
                    MdxTop = Convert.ToInt32(Mx + Math.Abs((double)Math.Pow(Mxy, 2) / My));
                    MdyTop = 0;
                }
            }

            return (MdxBot, MdyBot, MdxTop, MdyTop);
        }
    }
}
