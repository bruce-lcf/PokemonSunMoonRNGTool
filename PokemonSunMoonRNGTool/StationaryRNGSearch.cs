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
        public bool AlwaysSynchro;
        public int Synchro_Stat;
        public bool Valid_Blink;

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

        public StationaryRNGResult Generate()
        {
            StationaryRNGResult st = new StationaryRNGResult();

            index = 0;
            //シンクロ -- Synchronize
            st.row_r = getrand();

            if (st.row_r % 100 >= 50)
                st.Synchronize = true;

            if (AlwaysSynchro)
                st.Synchronize = true;

            st.Clock = (int)(st.row_r % 17);

            //まばたき消費契機 -- maybe blinking process occurs 2 times for each character
            if (Valid_Blink)
                Advance(2);

            //謎の消費 -- Something
            Advance(60);

            //暗号化定数 -- Encryption Constant
            st.EC = (uint)(getrand() & 0xFFFFFFFF);

            //性格値 -- PID
            st.PID = (uint)(getrand() & 0xFFFFFFFF);
            st.PSV = ((st.PID >> 16) ^ (st.PID & 0xFFFF)) >> 4;

            if (st.PSV == TSV)
                st.Shiny = true;

            //V箇所 -- IV-31 Inheritance
            for (int i = 0; i < 3; i++)
            {
            repeat:
                st.InheritStats[i] = (uint)(getrand() % 6);

                // Scan for duplicate IV
                for (int k = 0; k < i; k++)
                    if (st.InheritStats[k] == st.InheritStats[i])
                        goto repeat;
            }

            //基礎個体値 -- Base IVs
            for (int j = 0; j < 3; j++)
                st.BaseIV[j] = (int)(getrand() & 0x1F);

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
            if (AlwaysSynchro)
                getrand();

            //性格 -- Nature
            st.Nature = (int)(getrand() % 25);
            if (Synchro_Stat >= 0 && st.Synchronize)
            {
                st.Nature = Synchro_Stat;
            }

            return st;
        }

        public static List<ulong> RandList;
        private int index;
        private ulong getrand()
        {
            return RandList[index++];
        }
        private void Advance(int d)
        {
            index += d;
        }
    }
}
