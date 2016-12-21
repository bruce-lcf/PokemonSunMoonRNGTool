using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMHatchingRNGTool
{
    public partial class Form1 : Form
    {

        #region deta
        string[] natures =
        {
            "がんばりや", "さみしがり", "ゆうかん", "いじっぱり",
            "やんちゃ", "ずぶとい", "すなお", "のんき", "わんぱく",
            "のうてんき", "おくびょう", "せっかち", "まじめ", "ようき",
            "むじゃき", "ひかえめ", "おっとり", "れいせい", "てれや",
            "うっかりや", "おだやか", "おとなしい",
            "なまいき", "しんちょう", "きまぐれ"
        };


        object[,] mezapa =
        {
            {25, "指定なし"},
            {0, "格闘"},
            {1, "飛行"},
            {2, "毒"},
            {3, "地面"},
            {4, "岩"},
            {5, "虫"},
            {6, "ゴースト"},
            {7, "鋼"},
            {8, "炎"},
            {9, "水"},
            {10, "草"},
            {11, "電気"},
            {12, "エスパー"},
            {13, "氷"},
            {14, "ドラゴン"},
            {15, "悪"},
        };
        #endregion

        #region グローバル変数
        List<string> other_tsv = new List<string>();
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private bool IVcheck(int[] IV, int[] IVup, int[] IVlow)
        {
            for (int i = 0; i < 6; i++)
            {
                if (!(IVlow[i] <= IV[i]) || !(IV[i] <= IVup[i])) return false;
            }
            return true;
        }

        private void k_search_Click(object sender, EventArgs e)
        {
            if (!(Regex.IsMatch(status0.Text, "^[0-9a-fA-F]{0,8}$")))
            {
                MessageBox.Show("status[0]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!(Regex.IsMatch(status1.Text, "^[0-9a-fA-F]{0,8}$")))
            {
                MessageBox.Show("status[1]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!(Regex.IsMatch(status2.Text, "^[0-9a-fA-F]{0,8}$")))
            {
                MessageBox.Show("status[2]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!(Regex.IsMatch(status3.Text, "^[0-9a-fA-F]{0,8}$")))
            {
                MessageBox.Show("status[3]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (status0.Text == "")
            {
                MessageBox.Show("status[0]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (status1.Text == "")
            {
                MessageBox.Show("status[1]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (status2.Text == "")
            {
                MessageBox.Show("status[2]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (status3.Text == "")
            {
                MessageBox.Show("status[3]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Regex.IsMatch(s_min.Text, "[^0-9]+$"))
            {
                MessageBox.Show("消費数の下限に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Regex.IsMatch(s_max.Text, "[^0-9]+$"))
            {
                MessageBox.Show("消費数の上限に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (s_min.Text == "")
            {
                MessageBox.Show("消費数の下限が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (s_max.Text == "")
            {
                MessageBox.Show("消費数の上限が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt64(s_min.Text) > Convert.ToInt64(s_max.Text))
            {
                MessageBox.Show("消費数が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow1.Text) > Convert.ToInt32(IVup1.Text))
            {
                MessageBox.Show("Hの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow2.Text) > Convert.ToInt32(IVup2.Text))
            {
                MessageBox.Show("Aの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow3.Text) > Convert.ToInt32(IVup3.Text))
            {
                MessageBox.Show("Bの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow4.Text) > Convert.ToInt32(IVup4.Text))
            {
                MessageBox.Show("Cの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow5.Text) > Convert.ToInt32(IVup5.Text))
            {
                MessageBox.Show("Dの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow6.Text) > Convert.ToInt32(IVup6.Text))
            {
                MessageBox.Show("Sの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!(Regex.IsMatch(TSV.Text, "^[0-9]{0,5}$")))
            {
                MessageBox.Show("TSVに不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (0 > Convert.ToInt32(TSV.Text) || Convert.ToInt32(TSV.Text) > 4095)
            {
                MessageBox.Show("TSVの上限下限が閾値を超えています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if(sex_ratio.SelectedIndex == 6 && !(post_ditto.Checked || pre_ditto.Checked))
            {
                MessageBox.Show("無性別ポケモンに対し、メタモンが選択されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                kotai_search();
            }
        }

        private void kotai_search()
        {
            #region 宣言
            int min = Convert.ToInt32(s_min.Text);
            int max = Convert.ToInt32(s_max.Text);
            int u_Type = (int)mezapa[mezapaType.SelectedIndex, 0];
            string u_ability = ability.Text;
            string u_sex = sex.Text;
            string u_ball = ball.Text;
            int count;
            string row_iden_oya = "", row_sex = "", seed = "";
            string[] row_iden = { "H", "A", "B", "C", "D", "S" };
            UInt32 r = 0x0, pid = 0x0, encryption_key = 0x0;
            string true_pid = "";
            UInt32 LID = 0x0;
            UInt32 HID = 0x0;
            UInt32 psv = 0x0;
            string true_psv = "";
            bool shiny_flag, for_flag;
            #endregion

            #region 遺伝
            int iden_loop = 0;
            if (pre_Items.Text == "赤い糸" || post_Items.Text == "赤い糸") iden_loop = 5;
            else iden_loop = 3;

            string[] iden_oya_box = new string[iden_loop];
            UInt32[] iden_box = new UInt32[iden_loop];
            string p_ability = "", p_nature = "", p_sex, p_ball;
            #endregion

            #region stats
            int[] IV = new int[6];
            int[] IVup = new int[6];
            int[] IVlow = new int[6];
            int[] pre_parent = new int[6];
            int[] post_parent = new int[6];
            UInt32[] st = new UInt32[4];

            IVup[0] = Convert.ToInt32(IVup1.Text);
            IVup[1] = Convert.ToInt32(IVup2.Text);
            IVup[2] = Convert.ToInt32(IVup3.Text);
            IVup[3] = Convert.ToInt32(IVup4.Text);
            IVup[4] = Convert.ToInt32(IVup5.Text);
            IVup[5] = Convert.ToInt32(IVup6.Text);
            IVlow[0] = Convert.ToInt32(IVlow1.Text);
            IVlow[1] = Convert.ToInt32(IVlow2.Text);
            IVlow[2] = Convert.ToInt32(IVlow3.Text);
            IVlow[3] = Convert.ToInt32(IVlow4.Text);
            IVlow[4] = Convert.ToInt32(IVlow5.Text);
            IVlow[5] = Convert.ToInt32(IVlow6.Text);

            pre_parent[0] = Convert.ToInt32(pre_parent1.Text);
            pre_parent[1] = Convert.ToInt32(pre_parent2.Text);
            pre_parent[2] = Convert.ToInt32(pre_parent3.Text);
            pre_parent[3] = Convert.ToInt32(pre_parent4.Text);
            pre_parent[4] = Convert.ToInt32(pre_parent5.Text);
            pre_parent[5] = Convert.ToInt32(pre_parent6.Text);
            post_parent[0] = Convert.ToInt32(post_parent1.Text);
            post_parent[1] = Convert.ToInt32(post_parent2.Text);
            post_parent[2] = Convert.ToInt32(post_parent3.Text);
            post_parent[3] = Convert.ToInt32(post_parent4.Text);
            post_parent[4] = Convert.ToInt32(post_parent5.Text);
            post_parent[5] = Convert.ToInt32(post_parent6.Text);

            st[0] = Convert.ToUInt32(status0.Text, 16);
            st[1] = Convert.ToUInt32(status1.Text, 16);
            st[2] = Convert.ToUInt32(status2.Text, 16);
            st[3] = Convert.ToUInt32(status3.Text, 16);

            UInt32 tsv = Convert.ToUInt32(TSV.Text);
            #endregion

            #region 性別閾値
            int sex_threshold = 0;
            if (sex_ratio.SelectedIndex == 0)
            {
                sex_threshold = 126;
            }
            else if (sex_ratio.SelectedIndex == 1)
            {
                sex_threshold = 31;
            }
            else if (sex_ratio.SelectedIndex == 2)
            {
                sex_threshold = 63;
            }
            else if (sex_ratio.SelectedIndex == 3)
            {
                sex_threshold = 189;
            }
            else if (sex_ratio.SelectedIndex == 4)
            {
                sex_threshold = 0;
            }
            else if (sex_ratio.SelectedIndex == 5)
            {
                sex_threshold = 252;
            }
            #endregion

            var status = new uint[4] { st[0], st[1], st[2], st[3] };
            var tiny = new TinyMT(status, new TinyMTParameter(0x8f7011ee, 0xfc78ff1f, 0x3793fdff));

            List<DataGridViewRow> list = new List<DataGridViewRow>();
            k_dataGridView.Rows.Clear();

            for (int i = 0; i <= max; i++)
            {
                shiny_flag = false;
                //statusの更新
                for (int j = 0; j <= 3; j++) st[j] = tiny.status[j];

                r = tiny.temper();
                seed = tiny.status[3].ToString("X") + "," + tiny.status[2].ToString("X") + "," + tiny.status[1].ToString("X") + "," + tiny.status[0].ToString("X");
                //生の乱数列からの性別と遺伝箇所
                row_sex = (r % 252 < sex_threshold) ? "♀" : "♂";
                row_iden_oya = (r % 2 == 0) ? "先" : "後";

                //計算
                cal(st, out IV, out iden_box, out iden_oya_box, out p_sex, out p_ability, out p_nature, out pid, out encryption_key, out count, out p_ball);

                for (int j = 0; j < iden_loop; j++)
                {
                    int value = (int)iden_box[j];
                    IV[value] = (iden_oya_box[j].ToString() == "先") ? pre_parent[value] : post_parent[value];
                }

                HID = pid >> 16;
                LID = pid & 0xFFFF;
                psv = (HID ^ LID) / 0x10;
                if (!(International.Checked || omamori.Checked)) true_psv = "-";
                else true_psv = psv.ToString("d");
                if (!(International.Checked || omamori.Checked) && shiny.Checked) goto ExitIF;
                //ここで弾く
                if (!Invalid_Refine.Checked)
                {
                    if (!other_TSV.Checked)
                    {
                        if (shiny.Checked && !(psv == tsv)) goto ExitIF;
                        if (psv == tsv) shiny_flag = true;
                    }
                    else
                    {
                        for_flag = false;
                        if (International.Checked || omamori.Checked)
                        {
                            foreach (var item in other_tsv)
                            {
                                if (psv == Convert.ToInt32(item))
                                {
                                    for_flag = true;
                                    shiny_flag = true;
                                    break;
                                }
                            }
                        }
                        if(!for_flag) goto ExitIF;
                    }
                    if (!IVcheck(IV, IVup, IVlow)) goto ExitIF;
                    if (u_Type != 25)
                    {
                        if (!mezapa_check(IV, u_Type)) goto ExitIF;
                    }
                    if (u_ability != "指定なし")
                    {
                        if (u_ability != p_ability) goto ExitIF;
                    }
                    if (u_sex != "指定なし")
                    {
                        if (u_sex != p_sex) goto ExitIF;
                    }
                    if (u_ball != "指定なし")
                    {
                        if (u_ball != p_ball) goto ExitIF;
                    }
                }

                if (pre_Items.Text == "変わらず" || post_Items.Text == "変わらず") p_nature = "変わらず";
                if (!(International.Checked || omamori.Checked)) true_pid = "仮性格値";
                else true_pid = pid.ToString("X");

                if (i >= min)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(this.k_dataGridView);
                    row.SetValues(new object[] { i, seed, IV[0], IV[1], IV[2], IV[3], IV[4], IV[5], p_sex, p_ability, p_nature, true_pid, encryption_key.ToString("X"), count, true_psv,
                        r.ToString("X"), (r % 32).ToString("d"), row_iden[r % 6], row_iden_oya, natures[r % 25], row_sex });

                    for (int k = 0; k < iden_loop; k++)
                    {
                        if (pre.ForeColor == Color.DodgerBlue)
                        {
                            if (iden_oya_box[k] == "先") row.Cells[2 + (int)iden_box[k]].Style.ForeColor = Color.DodgerBlue;
                            else row.Cells[2 + (int)iden_box[k]].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            if (iden_oya_box[k] == "先") row.Cells[2 + (int)iden_box[k]].Style.ForeColor = Color.Red;
                            else row.Cells[2 + (int)iden_box[k]].Style.ForeColor = Color.DodgerBlue;
                        }

                    }
                    if (shiny_flag)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCyan;
                    }
                    list.Add(row);
                }
            ExitIF:;

                tiny.nextState();
            }

            k_dataGridView.Rows.AddRange(list.ToArray());
            k_dataGridView.CurrentCell = null;
        }

        private void List_search_Click(object sender, EventArgs e)
        {
            if (!(Regex.IsMatch(L_status0.Text, "^[0-9a-fA-F]{0,8}$")))
            {
                MessageBox.Show("status[0]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!(Regex.IsMatch(L_status1.Text, "^[0-9a-fA-F]{0,8}$")))
            {
                MessageBox.Show("status[1]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!(Regex.IsMatch(L_status2.Text, "^[0-9a-fA-F]{0,8}$")))
            {
                MessageBox.Show("status[2]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!(Regex.IsMatch(L_status3.Text, "^[0-9a-fA-F]{0,8}$")))
            {
                MessageBox.Show("status[3]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (L_status0.Text == "")
            {
                MessageBox.Show("status[0]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (L_status1.Text == "")
            {
                MessageBox.Show("status[1]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (L_status2.Text == "")
            {
                MessageBox.Show("status[2]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (L_status3.Text == "")
            {
                MessageBox.Show("status[3]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Regex.IsMatch(n_min.Text, "[^0-9]+$"))
            {
                MessageBox.Show("受け取り回数の下限に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Regex.IsMatch(n_max.Text, "[^0-9]+$"))
            {
                MessageBox.Show("受け取り回数上限に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (n_min.Text == "")
            {
                MessageBox.Show("受け取り回数の下限が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (n_max.Text == "")
            {
                MessageBox.Show("受け取り回数の上限が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt64(n_min.Text) > Convert.ToInt64(n_max.Text))
            {
                MessageBox.Show("受け取り回数が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow1.Text) > Convert.ToInt32(IVup1.Text))
            {
                MessageBox.Show("Hの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow2.Text) > Convert.ToInt32(IVup2.Text))
            {
                MessageBox.Show("Aの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow3.Text) > Convert.ToInt32(IVup3.Text))
            {
                MessageBox.Show("Bの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow4.Text) > Convert.ToInt32(IVup4.Text))
            {
                MessageBox.Show("Cの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow5.Text) > Convert.ToInt32(IVup5.Text))
            {
                MessageBox.Show("Dの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Convert.ToInt32(IVlow6.Text) > Convert.ToInt32(IVup6.Text))
            {
                MessageBox.Show("Sの個体値が 下限 ＞上限 になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!(Regex.IsMatch(TSV.Text, "^[0-9]{0,5}$")))
            {
                MessageBox.Show("TSVに不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (0 > Convert.ToInt32(TSV.Text) || Convert.ToInt32(TSV.Text) > 4095)
            {
                MessageBox.Show("TSVの上限下限が閾値を超えています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (sex_ratio.SelectedIndex == 6 && !(post_ditto.Checked || pre_ditto.Checked))
            {
                MessageBox.Show("無性別ポケモンに対し、メタモンが選択されていません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                FukaList_search();
            }
        }

        private void FukaList_search()
        {
            #region 宣言
            int min = Convert.ToInt32(n_min.Text);
            int max = Convert.ToInt32(n_max.Text);
            string seed = "";
            UInt32 r = 0x0, pid = 0x0, encryption_key = 0x0;
            UInt32 LID = 0x0;
            UInt32 HID = 0x0;
            UInt32 psv = 0x0;
            string true_psv = "";
            string true_pid = "";
            int count = 0, pre_count = 0;
            bool shiny_flag;

            int International_loop = 0;
            int omamori_loop = 0;
            if (International.Checked) International_loop = 6;
            if (omamori.Checked) omamori_loop = 2;
            #endregion

            #region 遺伝
            int iden_loop = 0;
            if (pre_Items.Text == "赤い糸" || post_Items.Text == "赤い糸") iden_loop = 5;
            else iden_loop = 3;

            string[] iden_oya_box = new string[iden_loop];
            UInt32[] iden_box = new UInt32[iden_loop];
            string p_ability = "", p_nature = "", p_sex = "", p_ball = "";
            #endregion

            #region stats
            int[] IV = new int[6];
            int[] IVup = new int[6];
            int[] IVlow = new int[6];
            int[] pre_parent = new int[6];
            int[] post_parent = new int[6];
            UInt32[] st = new UInt32[4];

            IVup[0] = Convert.ToInt32(IVup1.Text);
            IVup[1] = Convert.ToInt32(IVup2.Text);
            IVup[2] = Convert.ToInt32(IVup3.Text);
            IVup[3] = Convert.ToInt32(IVup4.Text);
            IVup[4] = Convert.ToInt32(IVup5.Text);
            IVup[5] = Convert.ToInt32(IVup6.Text);
            IVlow[0] = Convert.ToInt32(IVlow1.Text);
            IVlow[1] = Convert.ToInt32(IVlow2.Text);
            IVlow[2] = Convert.ToInt32(IVlow3.Text);
            IVlow[3] = Convert.ToInt32(IVlow4.Text);
            IVlow[4] = Convert.ToInt32(IVlow5.Text);
            IVlow[5] = Convert.ToInt32(IVlow6.Text);

            pre_parent[0] = Convert.ToInt32(pre_parent1.Text);
            pre_parent[1] = Convert.ToInt32(pre_parent2.Text);
            pre_parent[2] = Convert.ToInt32(pre_parent3.Text);
            pre_parent[3] = Convert.ToInt32(pre_parent4.Text);
            pre_parent[4] = Convert.ToInt32(pre_parent5.Text);
            pre_parent[5] = Convert.ToInt32(pre_parent6.Text);
            post_parent[0] = Convert.ToInt32(post_parent1.Text);
            post_parent[1] = Convert.ToInt32(post_parent2.Text);
            post_parent[2] = Convert.ToInt32(post_parent3.Text);
            post_parent[3] = Convert.ToInt32(post_parent4.Text);
            post_parent[4] = Convert.ToInt32(post_parent5.Text);
            post_parent[5] = Convert.ToInt32(post_parent6.Text);

            st[0] = Convert.ToUInt32(L_status0.Text, 16);
            st[1] = Convert.ToUInt32(L_status1.Text, 16);
            st[2] = Convert.ToUInt32(L_status2.Text, 16);
            st[3] = Convert.ToUInt32(L_status3.Text, 16);

            UInt32 tsv = Convert.ToUInt32(TSV.Text);
            #endregion

            #region 性別閾値
            int sex_threshold = 0;
            if (sex_ratio.SelectedIndex == 0)
            {
                sex_threshold = 126;
            }
            else if (sex_ratio.SelectedIndex == 1)
            {
                sex_threshold = 31;
            }
            else if (sex_ratio.SelectedIndex == 2)
            {
                sex_threshold = 63;
            }
            else if (sex_ratio.SelectedIndex == 3)
            {
                sex_threshold = 189;
            }
            else if (sex_ratio.SelectedIndex == 4)
            {
                sex_threshold = 0;
            }
            else if (sex_ratio.SelectedIndex == 5)
            {
                sex_threshold = 252;
            }
            else if (sex_ratio.SelectedIndex == 6)
            {
                sex_threshold = 300;
            }
            #endregion

            List<DataGridViewRow> list = new List<DataGridViewRow>();
            L_dataGridView.Rows.Clear();

            var status = new uint[4] { st[0], st[1], st[2], st[3] };
            var tiny = new TinyMT(status, new TinyMTParameter(0x8f7011ee, 0xfc78ff1f, 0x3793fdff));

            for (int i = 1; i <= max; i++)
            {
                shiny_flag = false;
                seed = tiny.status[3].ToString("X") + "," + tiny.status[2].ToString("X") + "," + tiny.status[1].ToString("X") + "," + tiny.status[0].ToString("X");
                //最初の消費
                r = tiny.temper();
                tiny.nextState();
                count++;

                //性別
                if (sex_ratio.SelectedIndex < 4)
                {
                    r = tiny.temper();
                    p_sex = (r % 252 < sex_threshold) ? "♀" : "♂";
                    tiny.nextState();
                    count++;
                }
                if (sex_ratio.SelectedIndex > 3)
                {
                    p_sex = (r % 252 < sex_threshold) ? "♀" : "♂";
                }
                if (sex_threshold == 300) p_sex = "-";

                //性格
                r = tiny.temper();
                p_nature = natures[r % 25];
                tiny.nextState();
                count++;

                //両親変わらず
                if (pre_Items.Text == "変わらず" & post_Items.Text == "変わらず")
                {
                    r = tiny.temper();
                    tiny.nextState();
                    count++;
                }

                //特性
                r = tiny.temper();
                int value = (int)(r % 100);
                if (!(post_ditto.Checked || pre_ditto.Checked))
                {
                    if (post_ability.Text == "1")
                    {
                        if (value < 80) p_ability = "1";
                        else p_ability = "2";
                    }
                    if (post_ability.Text == "2")
                    {
                        if (value < 20) p_ability = "1";
                        else p_ability = "2";
                    }
                    if (post_ability.Text == "夢")
                    {
                        if (value < 20) p_ability = "1";
                        else if (value < 40) p_ability = "2";
                        else p_ability = "夢";
                    }
                }
                else
                {
                    if (pre_ditto.Checked)
                    {
                        if (post_ability.Text == "1")
                        {
                            if (value < 80) p_ability = "1";
                            else p_ability = "2";
                        }
                        if (post_ability.Text == "2")
                        {
                            if (value < 20) p_ability = "1";
                            else p_ability = "2";
                        }
                        if (post_ability.Text == "夢")
                        {
                            if (value < 20) p_ability = "1";
                            else if (value < 40) p_ability = "2";
                            else p_ability = "夢";
                        }
                    }
                    else
                    {
                        if (pre_ability.Text == "1")
                        {
                            if (value < 80) p_ability = "1";
                            else p_ability = "2";
                        }
                        if (pre_ability.Text == "2")
                        {
                            if (value < 20) p_ability = "1";
                            else p_ability = "2";
                        }
                        if (pre_ability.Text == "夢")
                        {
                            if (value < 20) p_ability = "1";
                            else if (value < 40) p_ability = "2";
                            else p_ability = "夢";
                        }
                    }
                }
                tiny.nextState();
                count++;

                //最初の遺伝箇所
                int iden_count = 0;
                bool flag;

                while (true)
                {
                    flag = true;
                    r = tiny.temper();
                    for (int k = 0; k < iden_count; k++)
                    {
                        if (iden_box[k] == r % 6)
                        {
                            r = tiny.temper();
                            tiny.nextState();
                            count++;
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            r = tiny.temper();
                            string iden_oya = (r % 2 == 0) ? "先" : "後";
                            tiny.nextState();

                            if (j == 0)
                            {
                                iden_box[iden_count] = r % 6;
                            }
                            else
                            {
                                iden_oya_box[iden_count] = iden_oya;
                                iden_count++;
                            }
                            count++;
                        }
                    }

                    if (iden_count == iden_loop) break;
                }

                //基礎個体値
                for (int j = 0; j < 6; j++)
                {
                    r = tiny.temper();
                    IV[j] = (int)(r % 32);
                    tiny.nextState();
                    count++;
                }

                //暗号化定数
                r = tiny.temper();
                encryption_key = r;
                tiny.nextState();
                count++;

                //性格値判定    
                LID = 0x0;
                HID = 0x0;

                if (!(International.Checked || omamori.Checked))
                {
                    r = tiny.temper();
                    pid = r;
                }
                else
                {
                    for (int j = 0; j < (omamori_loop + International_loop); j++)
                    {
                        r = tiny.temper();
                        pid = r;

                        HID = pid >> 16;
                        LID = pid & 0xFFFF;
                        tiny.nextState();
                        count++;
                        if (L_TSV_shiny.Checked & (((HID ^ LID) / 0x10) == tsv))
                        {
                            shiny_flag = true;
                            break;
                        }
                    }
                }

                //ボール消費
                
                if (!(post_ditto.Checked || pre_ditto.Checked || Heterogeneity.Checked))
                {
                    r = tiny.temper();
                    p_ball = (r % 100 >= 50) ? "先親" : "後親";
                    tiny.nextState();
                    count++;
                }

                //何かの消費
                tiny.nextState();
                count++;

                if (i >= min)
                {
                    for (int j = 0; j < iden_loop; j++)
                    {
                        value = (int)iden_box[j];
                        IV[value] = (iden_oya_box[j].ToString() == "先") ? pre_parent[value] : post_parent[value];
                    }

                    HID = pid >> 16;
                    LID = pid & 0xFFFF;
                    psv = (HID ^ LID) / 0x10;
                    if (!(International.Checked || omamori.Checked)) true_psv = "-";
                    else true_psv = psv.ToString("d");

                    if (!(International.Checked || omamori.Checked)) true_pid = "仮性格値";
                    else true_pid = pid.ToString("X");

                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(this.L_dataGridView);
                    row.SetValues(new object[] { i, pre_count, seed, IV[0], IV[1], IV[2], IV[3], IV[4], IV[5], p_sex, p_ability, p_nature, true_pid, true_psv, encryption_key.ToString("X") });

                    for (int k = 0; k < iden_loop; k++)
                    {
                        if (pre.ForeColor == Color.DodgerBlue)
                        {
                            if (iden_oya_box[k] == "先") row.Cells[3 + (int)iden_box[k]].Style.ForeColor = Color.DodgerBlue;
                            else row.Cells[3 + (int)iden_box[k]].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            if (iden_oya_box[k] == "先") row.Cells[3 + (int)iden_box[k]].Style.ForeColor = Color.Red;
                            else row.Cells[3 + (int)iden_box[k]].Style.ForeColor = Color.DodgerBlue;
                        }

                    }
                    if (shiny_flag)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCyan;
                    }
                    list.Add(row);
                }
                pre_count = count;
            }

            L_dataGridView.Rows.AddRange(list.ToArray());
            L_dataGridView.CurrentCell = null;
        }

        private void cal(UInt32[] st, out int[] IV, out UInt32[] iden_box, out string[] iden_oya_box, out string p_sex, out string p_ability, out string p_nature, out UInt32 pid, out UInt32 encryption_key, out int count, out string p_ball)
        {
            #region 宣言その他もろもろ
            UInt32 r;
            count = 0;
            p_ability = "";
            p_nature = "";
            p_sex = "";
            p_ball = "";
            pid = 0x0;
            encryption_key = 0x0;
            IV = new int[] { 0, 0, 0, 0, 0, 0 };

            int International_loop = 0;
            int omamori_loop = 0;
            if (International.Checked) International_loop = 6;
            if (omamori.Checked) omamori_loop = 2;
            #endregion

            var status = new uint[4] { st[0], st[1], st[2], st[3] };
            var tiny = new TinyMT(status, new TinyMTParameter(0x8f7011ee, 0xfc78ff1f, 0x3793fdff));

            #region 性別閾値
            int sex_threshold = 0;
            if (sex_ratio.SelectedIndex == 0)
            {
                sex_threshold = 126;
            }
            else if (sex_ratio.SelectedIndex == 1)
            {
                sex_threshold = 32;
            }
            else if (sex_ratio.SelectedIndex == 2)
            {
                sex_threshold = 63;
            }
            else if (sex_ratio.SelectedIndex == 3)
            {
                sex_threshold = 189;
            }
            else if (sex_ratio.SelectedIndex == 4)
            {
                sex_threshold = 0;
            }
            else if (sex_ratio.SelectedIndex == 5)
            {
                sex_threshold = 252;
            }
            else if (sex_ratio.SelectedIndex == 6)
            {
                sex_threshold = 300;
            }
            #endregion

            #region 遺伝箇所
            int iden_loop = 0;
            if (pre_Items.Text == "赤い糸" || post_Items.Text == "赤い糸") iden_loop = 5;
            else iden_loop = 3;

            if (iden_loop == 3)
            {
                iden_box = new UInt32[] { 0, 0, 0 };
                iden_oya_box = new string[] { "", "", "" };
            }
            else
            {
                iden_box = new UInt32[] { 0, 0, 0, 0, 0 };
                iden_oya_box = new string[] { "", "", "", "", "" };
            }
            #endregion

            //最初の消費
            r = tiny.temper();
            tiny.nextState();
            count++;

            //性別
            if (sex_ratio.SelectedIndex < 4) 
            {
                r = tiny.temper();
                p_sex = (r % 252 < sex_threshold) ? "♀" : "♂";
                tiny.nextState();
                count++;
            }
            if (sex_ratio.SelectedIndex > 3) 
            {
                p_sex = (r % 252 < sex_threshold) ? "♀" : "♂";
            }
            if (sex_threshold == 300) p_sex = "-";

            //性格
            r = tiny.temper();
            p_nature = natures[r % 25];
            tiny.nextState();
            count++;

            //両親変わらず
            if (pre_Items.Text == "変わらず" & post_Items.Text == "変わらず")
            {
                r = tiny.temper();
                tiny.nextState();
                count++;
            }


            //特性
            r = tiny.temper();
            int value = (int)(r % 100);
            if (!(post_ditto.Checked || pre_ditto.Checked))
            {
                if (post_ability.Text == "1")
                {
                    if (value < 80) p_ability = "1";
                    else p_ability = "2";
                }
                if (post_ability.Text == "2")
                {
                    if (value < 20) p_ability = "1";
                    else p_ability = "2";
                }
                if (post_ability.Text == "夢")
                {
                    if (value < 20) p_ability = "1";
                    else if (value < 40) p_ability = "2";
                    else p_ability = "夢";
                }
            }
            else
            {
                if (pre_ditto.Checked)
                {
                    if (post_ability.Text == "1")
                    {
                        if (value < 80) p_ability = "1";
                        else p_ability = "2";
                    }
                    if (post_ability.Text == "2")
                    {
                        if (value < 20) p_ability = "1";
                        else p_ability = "2";
                    }
                    if (post_ability.Text == "夢")
                    {
                        if (value < 20) p_ability = "1";
                        else if (value < 40) p_ability = "2";
                        else p_ability = "夢";
                    }
                }
                else
                {
                    if (pre_ability.Text == "1")
                    {
                        if (value < 80) p_ability = "1";
                        else p_ability = "2";
                    }
                    if (pre_ability.Text == "2")
                    {
                        if (value < 20) p_ability = "1";
                        else p_ability = "2";
                    }
                    if (pre_ability.Text == "夢")
                    {
                        if (value < 20) p_ability = "1";
                        else if (value < 40) p_ability = "2";
                        else p_ability = "夢";
                    }
                }
            }
            tiny.nextState();
            count++;

            //最初の遺伝箇所
            int iden_count = 0;
            bool flag;

            while (true)
            {
                flag = true;
                r = tiny.temper();
                for (int k = 0; k < iden_count; k++)
                {
                    if (iden_box[k] == r % 6)
                    {
                        r = tiny.temper();
                        tiny.nextState();
                        count++;
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        r = tiny.temper();
                        string iden_oya = (r % 2 == 0) ? "先" : "後";
                        tiny.nextState();

                        if (j == 0)
                        {
                            iden_box[iden_count] = r % 6;
                        }
                        else
                        {
                            iden_oya_box[iden_count] = iden_oya;
                            iden_count++;
                        }
                        count++;
                    }
                }

                if (iden_count == iden_loop) break;
            }

            //基礎個体値
            for (int j = 0; j < 6; j++)
            {
                r = tiny.temper();
                IV[j] = (int)(r % 32);
                tiny.nextState();
                count++;
            }
            //暗号化定数

            r = tiny.temper();
            encryption_key = r;
            tiny.nextState();
            count++;

            //性格値判定
            UInt32 tsv = Convert.ToUInt32(TSV.Text);
            UInt32 LID = 0x0;
            UInt32 HID = 0x0;

            if (!(International.Checked || omamori.Checked))
            {
                r = tiny.temper();
                pid = r;
            }
            else
            {
                for (int j = 0; j < (omamori_loop + International_loop); j++)
                {
                    r = tiny.temper();
                    pid = r;

                    HID = pid >> 16;
                    LID = pid & 0xFFFF;
                    tiny.nextState();
                    count++;
                    if(!other_TSV.Checked)
                    {
                        if ((shiny.Checked || k_TSV_shiny.Checked) && (((HID ^ LID) / 0x10) == tsv)) break;
                    }
                }
            }
            //ボール消費
            if (!(post_ditto.Checked || pre_ditto.Checked　|| Heterogeneity.Checked))
            {
                r = tiny.temper();
                p_ball = (r % 100 >= 50) ? "先親" : "後親";
                tiny.nextState();
                count++;
            }

            //something
            r = tiny.temper();
            tiny.nextState();
            count++;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            k_dataGridView.DefaultCellStyle.Font = new Font("Consolas", 9);
            k_dataGridView.Columns[20].DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9);
            k_dataGridView.Columns[8].DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9);
            L_dataGridView.DefaultCellStyle.Font = new Font("Consolas", 9);
            L_dataGridView.Columns[9].DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9);

            System.Type dgvtype = typeof(DataGridView);
            System.Reflection.PropertyInfo dgvPropertyInfo = dgvtype.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            dgvPropertyInfo.SetValue(k_dataGridView, true, null);
            dgvPropertyInfo.SetValue(L_dataGridView, true, null);

            for (int i = 0; i < mezapa.GetLength(0); i++)
            {
                mezapaType.Items.Add(mezapa[i, 1]);
            }

            for (int co = 15; co < 20; co++)
            {
                k_dataGridView.Columns[co].DefaultCellStyle.BackColor = Color.Gainsboro;
            }

            pre_Items.SelectedIndex = 0;
            post_Items.SelectedIndex = 0;
            mezapaType.SelectedIndex = 0;
            ability.SelectedIndex = 0;
            pre_ability.SelectedIndex = 0;
            post_ability.SelectedIndex = 0;
            sex.SelectedIndex = 0;
            sex_ratio.SelectedIndex = 0;
            ball.SelectedIndex = 0;

            #region config読み込み
            if (System.IO.File.Exists("config.txt"))
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("config.txt"))
                {
                    string line = "";
                    List<string> list = new List<string>();

                    for (int i = 0; i < 5; i++)
                    {
                        line = file.ReadLine();
                        list.Add(line);
                    }

                    string st3 = list[0];
                    string st2 = list[1];
                    string st1 = list[2];
                    string st0 = list[3];
                    string tsv = list[4];

                    if (!(Regex.IsMatch(st0, "^[0-9a-fA-F]{0,8}$")))
                    {
                        MessageBox.Show("status[0]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (!(Regex.IsMatch(st1, "^[0-9a-fA-F]{0,8}$")))
                    {
                        MessageBox.Show("status[1]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (!(Regex.IsMatch(st2, "^[0-9a-fA-F]{0,8}$")))
                    {
                        MessageBox.Show("status[2]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (!(Regex.IsMatch(st3, "^[0-9a-fA-F]{0,8}$")))
                    {
                        MessageBox.Show("status[3]に不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (st0 == "")
                    {
                        MessageBox.Show("status[0]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (st1 == "")
                    {
                        MessageBox.Show("status[1]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (st2 == "")
                    {
                        MessageBox.Show("status[2]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (st3 == "")
                    {
                        MessageBox.Show("status[3]が空白になっています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (!(Regex.IsMatch(tsv, "^[0-9]{0,5}$")))
                    {
                        MessageBox.Show("TSVに不正な値が含まれています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (0 > Convert.ToInt32(tsv) || Convert.ToInt32(tsv) > 4095)
                    {
                        MessageBox.Show("TSVの上限下限が閾値を超えています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        status3.Text = st3;
                        status2.Text = st2;
                        status1.Text = st1;
                        status0.Text = st0;
                        L_status3.Text = st3;
                        L_status2.Text = st2;
                        L_status1.Text = st1;
                        L_status0.Text = st0;
                        TSV.Text = tsv;
                    }
                }
            }
            else
            {
                MessageBox.Show("config.txtが存在しません。\nデフォルトの設定を読み込みます。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            #endregion

            #region 複数TSV読み込み
            if (System.IO.File.Exists("TSV.txt"))
            {
                bool flag = false;
                using (System.IO.StreamReader file = new System.IO.StreamReader("TSV.txt"))
                {
                    string line = "";
                    List<string> list = new List<string>();

                    //test.txtを1行ずつ読み込んでいき、末端(何もない行)までwhile文で繰り返す
                    while ((line = file.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                    foreach (var item in list.Select((v, i) => new { v, i }))
                    {
                        if (!(Regex.IsMatch(item.v, "^[0-9]{0,5}$")))
                        {
                            string message = string.Format("{0}番目のTSV:{1}に不正な値が含まれています。", item.i + 1, item.v);
                            MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            flag = true;
                            goto ExitIF;
                        }
                        else if (0 > Convert.ToInt32(item.v) || Convert.ToInt32(item.v) > 4095)
                        {
                            string message = string.Format("{0}番目のTSV:{1}が上限下限が閾値を超えています。", item.i + 1, item.v);
                            MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            flag = true;
                            goto ExitIF;
                        }
                    }

                    other_tsv = list;

                ExitIF:;
                }
                if (flag)
                {
                    other_TSV.Enabled = false;
                }
            }
            else
            {
                other_TSV.Enabled = false;
            }

            #endregion
        }

        private bool mezapa_check(int[] IV, int u_Type)
        {
            int[] bits = new int[6];
            double Type = 0;
            double number = ((IV[0] & 1) + 2 * (IV[1] & 1) + 4 * (IV[2] & 1) + 8 * (IV[5] & 1) + 16 * (IV[3] & 1) + 32 * (IV[4] & 1)) * 15 / 63;
            Type = Math.Floor(number);
            if (u_Type != Type) return false;
            return true;
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            k_dataGridView.SelectAll();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(this.k_dataGridView.GetClipboardContent());
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("選択されていません");
            }
        }

        private void NumericUpDown_Enter(object sender, EventArgs e)
        {
            NumericUpDown NumericUpDown = sender as NumericUpDown;
            NumericUpDown.Select(0, NumericUpDown.Text.Length);
        }

        private void NumericUpDown_Check(object sender, CancelEventArgs e)
        {
            NumericUpDown NumericUpDown = sender as NumericUpDown;
            Control ctrl = NumericUpDown;
            if (NumericUpDown.Text == "")
            {
                if (ctrl is NumericUpDown)
                {
                    foreach (Control c in ((NumericUpDown)ctrl).Controls)
                    {
                        if (c is TextBox)
                        {
                            // クリップボードへコピー
                            ((TextBox)c).Undo();
                            break;
                        }
                    }
                }
            }
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.SelectAll();
        }

        private void Initial_textBox_Check(object sender, CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "" | Regex.IsMatch(textBox.Text, "[^0-9a-fA-F]+$"))
            {
                textBox.Undo();
            }
        }

        private void Send2List(object sender, EventArgs e)
        {
            string seed = "";
            try
            {
                seed = (string)k_dataGridView.CurrentRow.Cells[1].Value;
                string[] Data = seed.Split(',');
                L_status3.Text = Data[0];
                L_status2.Text = Data[1];
                L_status1.Text = Data[2];
                L_status0.Text = Data[3];
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("検索結果からseedを選択して下さい");
            }
        }

        private void L_copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(this.L_dataGridView.GetClipboardContent());
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("選択されていません");
            }
        }

        private void L_SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            L_dataGridView.SelectAll();
        }

        private void pre_ditto_Click(object sender, EventArgs e)
        {
            if (post_ditto.Checked)
            {
                post_ditto.Checked = false;
            }
        }

        private void post_ditto_Click(object sender, EventArgs e)
        {
            if (pre_ditto.Checked)
            {
                pre_ditto.Checked = false;
            }
        }

        private void Change_color(object sender, EventArgs e)
        {
            if (pre.ForeColor == Color.Red)
            {
                pre.ForeColor = Color.DodgerBlue;
                pre_parent1.ForeColor = Color.DodgerBlue;
                pre_parent2.ForeColor = Color.DodgerBlue;
                pre_parent3.ForeColor = Color.DodgerBlue;
                pre_parent4.ForeColor = Color.DodgerBlue;
                pre_parent5.ForeColor = Color.DodgerBlue;
                pre_parent6.ForeColor = Color.DodgerBlue;

                post.ForeColor = Color.Red;
                post_parent1.ForeColor = Color.Red;
                post_parent2.ForeColor = Color.Red;
                post_parent3.ForeColor = Color.Red;
                post_parent4.ForeColor = Color.Red;
                post_parent5.ForeColor = Color.Red;
                post_parent6.ForeColor = Color.Red;
            }
            else
            {
                pre.ForeColor = Color.Red;
                pre_parent1.ForeColor = Color.Red;
                pre_parent2.ForeColor = Color.Red;
                pre_parent3.ForeColor = Color.Red;
                pre_parent4.ForeColor = Color.Red;
                pre_parent5.ForeColor = Color.Red;
                pre_parent6.ForeColor = Color.Red;

                post.ForeColor = Color.DodgerBlue;
                post_parent1.ForeColor = Color.DodgerBlue;
                post_parent2.ForeColor = Color.DodgerBlue;
                post_parent3.ForeColor = Color.DodgerBlue;
                post_parent4.ForeColor = Color.DodgerBlue;
                post_parent5.ForeColor = Color.DodgerBlue;
                post_parent6.ForeColor = Color.DodgerBlue;
            }
        }
    }
}
