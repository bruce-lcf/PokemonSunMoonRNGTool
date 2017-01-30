using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonSunMoonRNGTool
{
    class StationaryRNGSearch
    {
        // Search Settings
        public int TSV;
        public bool TypeNull;
        public int Synchro_Stat;

        public class StationaryRNGResult
        {
            public readonly int[] BaseIV = new int[3];
            public uint[] InheritStats = new uint[3];
            public int Nature;
            public int Clock;
            public uint PID, EC, PSV;
            public UInt64 row_r;
            public int[] IVs;
            public int[] p_Status;
            public bool Shiny;
            public bool Synchronize;
        }

        public StationaryRNGResult Generate(SFMT sfmt)
        {
            StationaryRNGResult st = new StationaryRNGResult();

            //シンクロ -- Synchronize
            st.row_r = sfmt.NextUInt64();
            if (st.row_r % 100 >= 50)
                st.Synchronize = true;

            st.Clock = (int)(st.row_r % 17);

            //謎の消費 -- Something
            for (int i = 0; i < 60; i++)
                sfmt.NextUInt64();

            //暗号化定数 -- Encryption Constant
            st.EC = (uint)(st.row_r % 0x100000000);

            //性格値 -- PID
            st.PID = (uint)(sfmt.NextUInt64() % 0x100000000);
            st.PSV = ((st.PID >> 16) ^ (st.PID & 0xFFFF)) >> 4;

            if (st.PSV == TSV)
                st.Shiny = true;

            //V箇所 -- IV-31 Inheritance
            for (int i = 0; i < 3; i++)
            {
            repeat:
                st.InheritStats[i] = (uint)(sfmt.NextUInt64() % 6);

                // Scan for duplicate IV
                for (int k = 0; k < i; k++)
                    if (st.InheritStats[k] == st.InheritStats[i])
                        goto repeat;
            }

            //基礎個体値 -- Base IVs
            for (int j = 0; j < 3; j++)
                st.BaseIV[j] = (int)(sfmt.NextUInt64() & 0x1F);

            //個体値処理
            int[] IV = new int[6] { 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 3; i++)
                IV[st.InheritStats[i]] = 31;

            for (int i = 0, k = 0; i < 6; i++) 
            {
                if (IV[i] != 31)
                {
                    IV[i] = st.BaseIV[k];
                    k++;
                    if (k == 3) break;
                }
            }
            st.IVs = (int[])IV.Clone();

            //謎消費 -- Something
            if(TypeNull)
                sfmt.NextUInt64();

            //性格 -- Nature
            st.Nature = (int)(sfmt.NextUInt64() % 25);
            if (Synchro_Stat >= 0 && st.Synchronize)
            {
                st.Nature = Synchro_Stat;
            }

            return st;
        }
    }
}
