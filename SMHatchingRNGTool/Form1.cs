using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static PKHeX.Util;

namespace SMHatchingRNGTool
{
    public partial class Form1 : Form
    {
        private const string PATH_CONFIG = "config.txt";
        private const string PATH_TSV = "TSV.txt";
        private int[] other_tsv = new int[0];

        #region Translation
        private string[] natures;
        private string[] mezapa;
        private string[] msgstr;
        private readonly string[] genders = { "♂", "♀", "-" };
        private readonly string[] abilities = { "1", "2", "夢" };
        private static readonly string[] languages = {"ja", "en", "zh"};
        private static readonly string[] any = {"指定なし", "Any", "无限制"};
        private static readonly string[] tempPID = {"仮性格値", "---", "伪性格值"};
        private static readonly string[] dream = {"夢", "H", "梦"};
        private static readonly string[] first = {"先", "First", "父"};
        private static readonly string[] second = {"後", "Second", "母"};
        private static readonly string[] parent = {"親", " Parent", "方"};
        private static readonly string[] everstone = {"変わらず", "Everstone", "不变之石"};
        private static readonly string[] destiny = { "赤い糸", "Destiny Knot", "红线"};
        private static readonly string[] only = { "のみ", " Only", " 100%"};
        private static readonly string[] genderless = { "無性別", "Genderless", "无性别"};
        private static readonly string[] main_langlist =
            {
                "日本語", // JPN
                "English", // ENG
                //"Français", // FRE
                //"Italiano", // ITA
                //"Deutsch", // GER
                //"Español", // SPA
                //"한국어", // KOR
                "中文", // CHN
                //"Português", // Portuguese
            };
        private string curlanguage;
        private string STR_ANY = "指定なし";
        private string STR_TEMP_PID = "仮性格値";
        private string STR_DREAM = "夢";
        private string STR_FIRST = "先";
        private string STR_SECOND = "後";
        private string STR_PARENT = "親";
        private string STR_EVERSTONE = "変わらず";
        private string STR_DESTINY = "赤い糸";
        private string STR_ONLY = "のみ";
        private string STR_GENDERLESS = "無性別";
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void changeLanguage(object sender, EventArgs e)
        {
            Menu_Options.DropDown.Close();
            int l = CB_MainLanguage.SelectedIndex;
            string lang = languages[l];
            if (lang == curlanguage)
                return;

            curlanguage = lang;
            TranslateInterface(this, curlanguage); // Translate the UI to language.

            natures = getStringList("natures", curlanguage);
            mezapa = getStringList("types", curlanguage);
            msgstr = getStringList("msgstr", curlanguage);

            STR_ANY = any[l];
            STR_TEMP_PID = tempPID[l];
            STR_DREAM = dream[l];
            STR_FIRST = first[l];
            STR_SECOND = second[l];
            STR_PARENT = parent[l];
            STR_EVERSTONE = everstone[l];
            STR_DESTINY = destiny[l];
            STR_ONLY = only[l];
            STR_GENDERLESS = genderless[l];

            pre_Items.Items[0] = post_Items.Items[0] = ability.Items[0] = sex.Items[0] = ball.Items[0] = STR_ANY;
            pre_ability.Items[2] = post_ability.Items[2] = ability.Items[3] = abilities[2] = STR_DREAM;

            ball.Items[1] = STR_FIRST + STR_PARENT;
            ball.Items[2] = STR_SECOND + STR_PARENT;
            pre_Items.Items[1] = post_Items.Items[1] = STR_EVERSTONE;
            pre_Items.Items[2] = post_Items.Items[2] = STR_DESTINY;

            sex_ratio.Items[4] = "♂" + STR_ONLY;
            sex_ratio.Items[5] = "♀" + STR_ONLY;
            sex_ratio.Items[6] = STR_GENDERLESS;

            mezapaType.Items[0] = STR_ANY;
            nature.Items[0] = STR_ANY;
            for (int i = 1; i < mezapa.Length - 1; i++)
                mezapaType.Items[i] = mezapa[i];

            for (int i = 0; i < natures.Length; i++)
                nature.Items[i + 1] = natures[i];
        }

        private SearchSetting getSettings()
        {
            int[] IVup = { (int)IVup1.Value, (int)IVup2.Value, (int)IVup3.Value, (int)IVup4.Value, (int)IVup5.Value, (int)IVup6.Value, };
            int[] IVlow = { (int)IVlow1.Value, (int)IVlow2.Value, (int)IVlow3.Value, (int)IVlow4.Value, (int)IVlow5.Value, (int)IVlow6.Value, };
            return new SearchSetting
            {
                Nature = nature.SelectedIndex - 1,
                Ability = ability.SelectedIndex - 1,
                Gender = sex.SelectedIndex - 1,
                HPType = mezapaType.SelectedIndex - 1,
                IVlow = IVlow,
                IVup = IVup,
                Ball = ball.SelectedIndex - 1,
                Skip = Invalid_Refine.Checked,
            };
        }

        private EggRNGSearch getRNGSettings()
        {
            int[] pre_parent = { (int)pre_parent1.Value, (int)pre_parent2.Value, (int)pre_parent3.Value, (int)pre_parent4.Value, (int)pre_parent5.Value, (int)pre_parent6.Value, };
            int[] post_parent = { (int)post_parent1.Value, (int)post_parent2.Value, (int)post_parent3.Value, (int)post_parent4.Value, (int)post_parent5.Value, (int)post_parent6.Value, };
            int sex_threshold = 0;
            switch (sex_ratio.SelectedIndex)
            {
                case 0: sex_threshold = 126; break;
                case 1: sex_threshold = 30; break;
                case 2: sex_threshold = 63; break;
                case 3: sex_threshold = 189; break;
                case 4: sex_threshold = 0; break;
                case 5: sex_threshold = 252; break;
            }

            var rng = new EggRNGSearch
            {

                GenderRatio = sex_threshold,
                GenderRandom = sex_ratio.SelectedIndex < 4,
                GenderMale = sex_ratio.SelectedIndex == 4,
                GenderFemale = sex_ratio.SelectedIndex == 5,
                International = International.Checked,
                ShinyCharm = omamori.Checked,
                Heterogeneous = pre_ditto.Checked || post_ditto.Checked || Heterogeneity.Checked,
                Both_Everstone = pre_Items.SelectedIndex == 1 && post_Items.SelectedIndex == 1,
                Everstone = pre_Items.SelectedIndex == 1 || post_Items.SelectedIndex == 1,
                DestinyKnot = pre_Items.SelectedIndex == 2 || post_Items.SelectedIndex == 2,
                PowerItems = pre_Items.SelectedIndex > 2 || post_Items.SelectedIndex > 2,
                Both_PowerItems = pre_Items.SelectedIndex > 2 && post_Items.SelectedIndex > 2,
                MalePowerStat = pre_Items.SelectedIndex - 3,
                FemalePowerStat = post_Items.SelectedIndex - 3,
                ParentAbility = (!post_ditto.Checked ? post_ability : pre_ability).SelectedIndex,
                ConciderTSV = k_TSV_shiny.Checked,
                SearchOtherTSV = other_TSV.Checked,

                TSV = (int)TSV.Value,
                pre_parent = pre_parent,
                post_parent = post_parent,
            };
            rng.Initialize();
            return rng;
        }

        private bool frameMatch(EggRNGSearch.EggRNGResult result, SearchSetting setting)
        {
            //ここで弾く
            if (setting.Skip)
                return true;

            if (!(International.Checked || omamori.Checked) && shiny.Checked)
                return false;

            if (!other_TSV.Checked)
            {
                if (shiny.Checked && !result.Shiny)
                    return false;
            }
            else
            {
                if (International.Checked || omamori.Checked)
                    result.Shiny = other_tsv.Any(item => result.PSV == item);
                if (!result.Shiny)
                    return false;
            }
            if (!setting.validIVs(result.IVs))
                return false;
            if (!setting.mezapa_check(result.IVs))
                return false;

            if (setting.Nature != -1 && setting.Nature != result.Nature)
                return false;
            if (setting.Ability != -1 && setting.Ability != result.Ability)
                return false;
            if (setting.Gender != -1 && setting.Gender != result.Gender)
                return false;
            if (setting.Ball != -1 && setting.Ball != result.Ball)
                return false;
            return true;
        }

        private DataGridViewRow getRow(int i, EggRNGSearch rng, EggRNGSearch.EggRNGResult result, DataGridView dgv)
        {
            var true_psv = rng.PIDRerolls > 0 ? result.PSV.ToString("d") : "-";
            string true_pid = International.Checked || omamori.Checked ? result.PID.ToString("X8") : STR_TEMP_PID;

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dgv);
            row.SetValues(
                i, result.FramesUsed, result.Seed128,
                result.IVs[0], result.IVs[1], result.IVs[2], result.IVs[3], result.IVs[4], result.IVs[5],
                genders[result.Gender], abilities[result.Ability], rng.Everstone ? STR_EVERSTONE : natures[result.Nature],
                true_pid, true_psv, result.EC.ToString("X8"), result.row_r.ToString("X8")
                );

            for (int k = 0; k < result.InheritStats.Length; k++)
            {
                var color = result.InheritParents[k] == 0 ? pre.ForeColor : post.ForeColor;
                row.Cells[3 + (int)result.InheritStats[k]].Style.ForeColor = color;
            }
            if (result.Shiny)
            {
                row.DefaultCellStyle.BackColor = Color.LightCyan;
            }
            return row;
        }

        private void search_Click(object sender, EventArgs e)
        {
            if (s_min.Value > s_max.Value)
                Error(msgstr[0]);
            else if (IVlow1.Value > IVup1.Value)
                Error(msgstr[1]);
            else if (IVlow2.Value > IVup2.Value)
                Error(msgstr[2]);
            else if (IVlow3.Value > IVup3.Value)
                Error(msgstr[3]);
            else if (IVlow4.Value > IVup4.Value)
                Error(msgstr[4]);
            else if (IVlow5.Value > IVup5.Value)
                Error(msgstr[5]);
            else if (IVlow6.Value > IVup6.Value)
                Error(msgstr[6]);
            else if (0 > TSV.Value || TSV.Value > 4095)
                Error("TSV" + msgstr[7]);
            else if (sex_ratio.SelectedIndex == 6 && !(post_ditto.Checked || pre_ditto.Checked))
                Error(msgstr[8]);
            else
                search();
        }

        private void search()
        {
            int min = (int)s_min.Value;
            int max = (int)s_max.Value;

            uint[] st =
            {
                (uint)status0.Value,
                (uint)status1.Value,
                (uint)status2.Value,
                (uint)status3.Value,
            };

            uint[] status = { st[0], st[1], st[2], st[3] };
            var tiny = new TinyMT(status, new TinyMTParameter(0x8f7011ee, 0xfc78ff1f, 0x3793fdff));

            List<DataGridViewRow> list = new List<DataGridViewRow>();
            k_dataGridView.Rows.Clear();

            var setting = getSettings();
            var rng = getRNGSettings();

            for (int i = 0; i < min; i++)
                tiny.nextState();
            for (int i = min; i <= max; i++, tiny.nextState())
            {
                //statusの更新
                tiny.status.CopyTo(st, 0);
                EggRNGSearch.EggRNGResult result = rng.Generate(st);

                if (!frameMatch(result, setting))
                    continue;
                list.Add(getRow(i, rng, result, k_dataGridView));
            }

            k_dataGridView.Rows.AddRange(list.ToArray());
            k_dataGridView.CurrentCell = null;
        }

        private void List_search_Click(object sender, EventArgs e)
        {
            if (s_min.Value > s_max.Value)
                Error(msgstr[0]);
            else if (IVlow1.Value > IVup1.Value)
                Error(msgstr[1]);
            else if (IVlow2.Value > IVup2.Value)
                Error(msgstr[2]);
            else if (IVlow3.Value > IVup3.Value)
                Error(msgstr[3]);
            else if (IVlow4.Value > IVup4.Value)
                Error(msgstr[4]);
            else if (IVlow5.Value > IVup5.Value)
                Error(msgstr[5]);
            else if (IVlow6.Value > IVup6.Value)
                Error(msgstr[6]);
            else if (0 > TSV.Value || TSV.Value > 4095)
				Error("TSV" + msgstr[7]);
            else if (sex_ratio.SelectedIndex == 6 && !(post_ditto.Checked || pre_ditto.Checked))
                Error(msgstr[8]);
            else
                EggList_search();
                EggList_cal_target();
        }

        private void EggList_search()
        {
            int min = (int)n_min.Value;
            int max = (int)n_max.Value;

            uint[] st =
            {
                (uint)L_status0a.Value,
                (uint)L_status1a.Value,
                (uint)L_status2a.Value,
                (uint)L_status3a.Value,
            };

            uint[] status = { st[0], st[1], st[2], st[3] };
            var tiny = new TinyMT(status, new TinyMTParameter(0x8f7011ee, 0xfc78ff1f, 0x3793fdff));

            List<DataGridViewRow> list = new List<DataGridViewRow>();
            L_dataGridView.Rows.Clear();

            var rng = getRNGSettings();
            int frameCount = 0;
            for (int i = 1; i <= max; i++)
            {
                //statusの更新
                tiny.status.CopyTo(st, 0);
                EggRNGSearch.EggRNGResult result = rng.Generate(st);
                int ctr = result.FramesUsed;
                result.FramesUsed = frameCount;
                frameCount += ctr;

                if (i >= min)
                {
                    var row = getRow(i, rng, result, L_dataGridView);
                    list.Add(row);
                }
                // Continue adjacents
                rng.tiny.status.CopyTo(tiny.status, 0);
            }

            L_dataGridView.Rows.AddRange(list.ToArray());
            L_dataGridView.CurrentCell = null;
        }

        private void EggList_cal_target()
        {
            //Added function that shows how many number off eggs needed to receive and reject to advance foo number of frames
            int target = (int)Target_frame.Value;
            for (int co = 1; co < L_dataGridView.Rows.Count; co++)
            {
                if ((int)L_dataGridView[1, co].Value == target)
                {
                    Repeat_times.Text = msgstr[9] + $" {(int)L_dataGridView[0, co - 1].Value} " + msgstr[10];
                    break;
                }
                else if ((int)L_dataGridView[1, co].Value > target)
                {
                    Repeat_times.Text = msgstr[9] + $" {(int)L_dataGridView[0, co - 1].Value - 1} " + msgstr[10] + $" {target - (int)L_dataGridView[1, co - 1].Value} " + msgstr[11];
                    break;
                }
                else if (target > L_dataGridView.Rows.Count)
                {
                    Repeat_times.Text = msgstr[12];
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            k_dataGridView.DefaultCellStyle.Font = new Font("Consolas", 9);
            L_dataGridView.DefaultCellStyle.Font = new Font("Consolas", 9);
            k_dataGridView.Columns[9].DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9);
            L_dataGridView.Columns[9].DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9);

            Type dgvtype = typeof(DataGridView);
            System.Reflection.PropertyInfo dgvPropertyInfo = dgvtype.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            dgvPropertyInfo.SetValue(k_dataGridView, true, null);
            dgvPropertyInfo.SetValue(L_dataGridView, true, null);

            for (int i = 0; i < 17; i++)
                mezapaType.Items.Add("");
            for (int i = 0; i < 26; i++)
                nature.Items.Add("");

            foreach (var cbItem in main_langlist)
                CB_MainLanguage.Items.Add(cbItem);
            CB_MainLanguage.SelectedIndex = 0;
            changeLanguage(null, null);

            pre_Items.SelectedIndex = 0;
            post_Items.SelectedIndex = 0;
            mezapaType.SelectedIndex = 0;
            nature.SelectedIndex = 0;
            ability.SelectedIndex = 0;
            pre_ability.SelectedIndex = 0;
            post_ability.SelectedIndex = 0;
            sex.SelectedIndex = 0;
            sex_ratio.SelectedIndex = 0;
            ball.SelectedIndex = 0;

            loadConfig();
            other_TSV.Enabled = loadTSV();
        }

        private void loadConfig()
        {
            if (File.Exists(PATH_CONFIG))
            {
                string[] list = File.ReadAllLines(PATH_CONFIG);
                if (list.Length != 5)
                    return;

                string st3 = list[0];
                string st2 = list[1];
                string st1 = list[2];
                string st0 = list[3];
                string tsvstr = list[4];
                ushort tsv;
                uint s3, s2, s1, s0;


                if (!uint.TryParse(st0, NumberStyles.HexNumber, null, out s0))
					Error("status[0]"+ msgstr[13]);
                else if (!uint.TryParse(st1, NumberStyles.HexNumber, null, out s1))
                    Error("status[1]"+ msgstr[13]);
                else if (!uint.TryParse(st2, NumberStyles.HexNumber, null, out s2))
                    Error("status[2]"+ msgstr[13]);
                else if (!uint.TryParse(st3, NumberStyles.HexNumber, null, out s3))
                    Error("status[3]"+ msgstr[13]);
                else if (!ushort.TryParse(tsvstr, out tsv))
                    Error("TSV"+ msgstr[13]);
                else if (tsv > 4095)
                    Error("TSV"+ msgstr[7]);
                else
                {
                    status3.Value = L_status3a.Value = s3;
                    status2.Value = L_status2a.Value = s2;
                    status1.Value = L_status1a.Value = s1;
                    status0.Value = L_status0a.Value = s0;
                    TSV.Value = tsv;
                }
            }
            else
            {
                Error(PATH_CONFIG + msgstr[14] +"\n" + msgstr[15]);
            }
        }

        private bool loadTSV()
        {
            if (!File.Exists(PATH_TSV))
                return false;

            //test.txtを1行ずつ読み込んでいき、末端(何もない行)までwhile文で繰り返す
            string[] list = File.ReadAllLines(PATH_TSV);
            int[] tsvs = new int[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                var v = list[i];
                int val;
                if (!int.TryParse(v, out val)) // not number
                {
                    string message = $"{i + 1}" + msgstr[16] + $"{v}" + msgstr[13];
                    Error(message);
                    return false;
                }
                if (0 > val || val > 4095)
                {
                    string message = $"{i + 1}" + msgstr[16] + $"{v}" + msgstr[7];
                    Error(message);
                    return false;
                }
                tsvs[i] = val;
            }

            other_tsv = tsvs;
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
                Clipboard.SetDataObject(k_dataGridView.GetClipboardContent());
            }
            catch (ArgumentNullException)
            {
                Error(msgstr[17]);
            }
        }

        private void NumericUpDown_Enter(object sender, EventArgs e)
        {
            NumericUpDown NumericUpDown = sender as NumericUpDown;
            NumericUpDown.Select(0, NumericUpDown.Text.Length);
        }

        private void NumericUpDown_Check(object sender, CancelEventArgs e)
        {
            // > http://msdn.microsoft.com/en-us/library/system.windows.forms.numericupdown.hexadecimal.aspx
            // > When the Hexadecimal property is set to true,
            // > the Maximum property should be set to Int32.
            // > MaxValue and the Minimum property should be set to Int32.MinValue.
            // maximum of numeric is 0x7FFFFFFF?
             
            NumericUpDown NumericUpDown = sender as NumericUpDown;
            Control ctrl = NumericUpDown;
            if (ctrl == null)
                return;
            if (!string.IsNullOrEmpty(NumericUpDown.Text))
                return;
            foreach (var box in ((NumericUpDown)ctrl).Controls.OfType<TextBox>())
            {
                // クリップボードへコピー
                box.Undo();
                break;
            }
        }

        private void Send2List(object sender, EventArgs e)
        {
            try
            {
                var seed = (string)k_dataGridView.CurrentRow.Cells[2].Value;
                string[] Data = seed.Split(',');
                L_status3a.Value = Convert.ToUInt32(Data[0], 16);
                L_status2a.Value = Convert.ToUInt32(Data[1], 16);
                L_status1a.Value = Convert.ToUInt32(Data[2], 16);
                L_status0a.Value = Convert.ToUInt32(Data[3], 16);
            }
            catch (NullReferenceException)
            {
                Error(msgstr[18]);
            }
        }

        private void Send2SearchSeed(object sender, EventArgs e)
        {
            try
            {
                var seed = (string)k_dataGridView.CurrentRow.Cells[2].Value;
                string[] Data = seed.Split(',');
                status3.Value = Convert.ToUInt32(Data[0], 16);
                status2.Value = Convert.ToUInt32(Data[1], 16);
                status1.Value = Convert.ToUInt32(Data[2], 16);
                status0.Value = Convert.ToUInt32(Data[3], 16);
            }
            catch (NullReferenceException)
            {
                Error(msgstr[18]);
            }
        }

        private void L_copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(L_dataGridView.GetClipboardContent());
            }
            catch (ArgumentNullException)
            {
                Error(msgstr[17]);
            }
        }

        private void L_SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            L_dataGridView.SelectAll();
        }

        private void Change_ditto(object sender, EventArgs e)
        {
            if ((sender as CheckBox)?.Checked ?? false)
                (sender == post_ditto ? pre_ditto : post_ditto).Checked = false;
        }

        private void Change_color(object sender, EventArgs e)
        {
            // Invert Colors
            if (pre.ForeColor == Color.Red)
            {
                pre.ForeColor = Color.DodgerBlue;
                post.ForeColor = Color.Red;
            }
            else
            {
                pre.ForeColor = Color.Red;
                post.ForeColor = Color.DodgerBlue;
            }
            pre_parent1.ForeColor = pre.ForeColor;
            pre_parent2.ForeColor = pre.ForeColor;
            pre_parent3.ForeColor = pre.ForeColor;
            pre_parent4.ForeColor = pre.ForeColor;
            pre_parent5.ForeColor = pre.ForeColor;
            pre_parent6.ForeColor = pre.ForeColor;

            post_parent1.ForeColor = post.ForeColor;
            post_parent2.ForeColor = post.ForeColor;
            post_parent3.ForeColor = post.ForeColor;
            post_parent4.ForeColor = post.ForeColor;
            post_parent5.ForeColor = post.ForeColor;
            post_parent6.ForeColor = post.ForeColor;
        }

        private void B_SaveConfig_Click(object sender, EventArgs e)
        {
            string[] lines =
            {
                status3.Text,
                status2.Text,
                status1.Text,
                status0.Text,
                TSV.Text,
            };
            try
            {
                File.WriteAllLines(PATH_CONFIG, lines);
            }
            catch
            {
                Error(PATH_CONFIG+msgstr[19]);
            }
        }

        private void ConsiderTSVcheck(object sender, EventArgs e)
        {
            k_TSV_shiny.Checked = L_TSV_shiny.Checked = (sender as CheckBox)?.Checked ?? false;
        }

        private void B_TSV_Click(object sender, EventArgs e)
        {
            var editor = new TSVEntry(other_tsv);
            editor.ShowDialog();
            other_tsv = editor.other_tsv;

            try
            {
                File.WriteAllLines(PATH_TSV, other_tsv.Select(i => i.ToString()).ToArray());
            }
            catch
            {
                Error(PATH_TSV+msgstr[19]);
            }
        }

		private void mainMenuExit(object sender, EventArgs e)
		{
			Close();
		}
    }
}
