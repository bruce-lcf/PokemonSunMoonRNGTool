using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static PKHeX.Util;

namespace PokemonSunMoonRNGTool
{
    public partial class Form1 : Form
    {
        private const string PATH_CONFIG = "config.txt";
        private const string PATH_TSV = "TSV.txt";
        private const string PATH_PARENTS = "parents.csv";
        private int[] other_tsv = new int[0];
        private string[] parents_list = new string[0];

        public string[,] pokedex =
        {
            { "カプ・コケコ", "70", "115", "85", "95", "75", "130"},
            { "カプ・テテフ", "70", "85", "75", "130", "115", "95"},
            { "カプ・ブルル", "70", "130", "115", "85", "95", "75"},
            { "カプ・レヒレ", "70", "75", "115", "95", "130", "85"},
            { "ソルガレオ", "137", "137", "107", "113", "89", "97"},
            { "ルナアーラ", "137", "113", "89", "137", "107", "97"},
            { "タイプ：ヌル", "95", "95", "95", "95", "95", "59"},
        };

        #region Translation
        private string[] natures;
        private string[] mezapa;
        private string[] items;
        private string[] species;
        public string[] msgstr;
        private readonly string[] genders = { "♂", "♀", "-" };
        private readonly string[] abilities = { "1", "2", "夢" };
        private static readonly string[] languages = { "ja", "en", "cn", "zh" };
        private static readonly string[] any = { "指定なし", "Any", "无限制", "無限制" };
        private static readonly string[] tempPID = { "仮性格値", "---", "伪性格值", "暫時性格值" };
        private static readonly string[] dream = { "夢", "H", "梦", "夢" };
        private static readonly string[] first = { "先", "Male", "父", "父" };
        private static readonly string[] second = { "後", "Female", "母", "母" };
        private static readonly string[] parent = { "親", " Parent", "方", "方" };
        private static readonly string[] only = { "のみ", "Only", "100%", "100%" };
        private static readonly string[] genderless = { "無性別", "Genderless", "无性别", "無性別" };
        private static readonly string[] main_langlist =
            {
                "日本語", // JPN
                "English", // ENG
                //"Français", // FRE
                //"Italiano", // ITA
                //"Deutsch", // GER
                //"Español", // SPA
                //"한국어", // KOR
                "简体中文", // CN
                "繁體中文", // ZH
                //"Português", // Portuguese
            };
        private string curlanguage;
        private string STR_ANY = "指定なし";
        private string STR_TEMP_PID = "仮性格値";
        private string STR_DREAM = "夢";
        private string STR_FIRST = "先";
        private string STR_SECOND = "後";
        private string STR_PARENT = "親";
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
            Properties.Settings.Default.lang = curlanguage;
            Properties.Settings.Default.Save();

            natures = getStringList("natures", curlanguage);
            mezapa = getStringList("types", curlanguage);
            items = getStringList("items", curlanguage);
            msgstr = getStringList("msgstr", curlanguage);
            species = getStringList("species", curlanguage);

            STR_ANY = any[l];
            STR_TEMP_PID = tempPID[l];
            STR_DREAM = dream[l];
            STR_FIRST = first[l];
            STR_SECOND = second[l];
            STR_PARENT = parent[l];
            STR_ONLY = only[l];
            STR_GENDERLESS = genderless[l];

            pre_Items.Items[0] = post_Items.Items[0] = ability.Items[0] = sex.Items[0] = ball.Items[0] = STR_ANY;
            pre_ability.Items[2] = post_ability.Items[2] = ability.Items[3] = abilities[2] = STR_DREAM;

            ball.Items[1] = STR_FIRST + STR_PARENT;
            ball.Items[2] = STR_SECOND + STR_PARENT;

            sex_ratio.Items[4] = "♂" + STR_ONLY;
            sex_ratio.Items[5] = "♀" + STR_ONLY;
            sex_ratio.Items[6] = STR_GENDERLESS;

            mezapaType.Items[0] = STR_ANY;
            St_mezapaType.Items[0] = STR_ANY;
            nature.Items[0] = STR_ANY;
            St_nature.Items[0] = STR_ANY;
            St_Synchro_nature.Items[0] = STR_ANY;

            for (int i = 0; i < items.Length; i++)
                pre_Items.Items[i + 1] = post_Items.Items[i + 1] = items[i];

            for (int i = 1; i < mezapa.Length - 1; i++)
                St_mezapaType.Items[i] = mezapaType.Items[i] = mezapa[i];

            for (int i = 0; i < natures.Length; i++)
                St_Synchro_nature.Items[i + 1] = St_nature.Items[i + 1] = nature.Items[i + 1] = natures[i];

            for (int i = 0; i < 4; i++)
                St_pokedex.Items[i] = species[785 + i];
            for (int i = 4; i < 6; i++)
                St_pokedex.Items[i] = species[791 + i - 4];
            St_pokedex.Items[6] = species[772];

        }

        private EggSearchSetting EgggetSettings()
        {
            int[] IVup = { (int)IVup1.Value, (int)IVup2.Value, (int)IVup3.Value, (int)IVup4.Value, (int)IVup5.Value, (int)IVup6.Value, };
            int[] IVlow = { (int)IVlow1.Value, (int)IVlow2.Value, (int)IVlow3.Value, (int)IVlow4.Value, (int)IVlow5.Value, (int)IVlow6.Value, };
            return new EggSearchSetting
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

        private StationarySearchSetting StationarygetSettings()
        {
            int[] IVup = { (int)St_IVup1.Value, (int)St_IVup2.Value, (int)St_IVup3.Value, (int)St_IVup4.Value, (int)St_IVup5.Value, (int)St_IVup6.Value, };
            int[] IVlow = { (int)St_IVlow1.Value, (int)St_IVlow2.Value, (int)St_IVlow3.Value, (int)St_IVlow4.Value, (int)St_IVlow5.Value, (int)St_IVlow6.Value, };
            int[] Status = { (int)St_stats1.Value, (int)St_stats2.Value, (int)St_stats3.Value, (int)St_stats4.Value, (int)St_stats5.Value, (int)St_stats6.Value, };

            return new StationarySearchSetting
            {
                Nature = St_nature.SelectedIndex - 1,
                HPType = St_mezapaType.SelectedIndex - 1,
                IVlow = IVlow,
                IVup = IVup,
                Status = Status,
                Skip = St_Invalid_Refine.Checked,
                Pokemon = St_pokedex.SelectedIndex,
                Lv = (int)St_Lv.Value
            };
        }

        private IDSearchSetting IDgetSettings()
        {
            string IDList = ID_List.Text;
            string PSV_List = ID_PSVList.Text;

            return new IDSearchSetting
            {
                ID_List = IDList,
                PSV_List = PSV_List,
                Skip = ID_Invalid_Refine.Checked
            };
        }

        private EggRNGSearch getEggRNGSettings()
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
                Heterogeneous = Heterogeneity.Checked,
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

        private StationaryRNGSearch getStationaryRNGSettings()
        {
            var rng = new StationaryRNGSearch
            {
                Synchro_Stat = St_Synchro_nature.SelectedIndex - 1,
                TSV = (int)St_TSV.Value,
                TypeNull = TypeNull.Checked,
                Valid_Blink = Valid_Blink.Checked
            };
            return rng;
        }

        private IDRNGSearch getIDRNGSettings()
        {
            var rng = new IDRNGSearch
            {
                Clock_CorrectionValue = (int)Clock_CorrectionValue.Value,
            };
            return rng;
        }


        private bool EggframeMatch(EggRNGSearch.EggRNGResult result, EggSearchSetting setting)
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

        private bool StationaryframeMatch(StationaryRNGSearch.StationaryRNGResult result, StationarySearchSetting setting)
        {
            setting.getStatus(result, setting);

            //ここで弾く
            if (setting.Skip)
                return true;

            if (St_shiny.Checked && !result.Shiny)
                return false;

            if (St_search_IV.Checked && !setting.validIVs(result.IVs))
                return false;

            if (St_search_Status.Checked && !setting.validStatus(result, setting))
                return false;

            if (!setting.mezapa_check(result.IVs))
                return false;

            if (St_SynchroOnly.Checked && !result.Synchronize)
                return false;

            if (setting.Nature != -1 && setting.Nature != result.Nature)
                return false;

            return true;
        }

        private bool IDframeMatch(IDRNGSearch.IDRNGResult result, IDSearchSetting setting)
        {
            System.IO.MemoryStream msID_List = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(setting.ID_List));
            System.IO.StreamReader srID = new System.IO.StreamReader(msID_List);
            System.IO.MemoryStream msPSV_List = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(setting.PSV_List));
            System.IO.StreamReader srPSV = new System.IO.StreamReader(msPSV_List);

            if (setting.Skip)
                return true;

            if (ID_shiny.Checked && setting.PSV_List != "")
            {
                while (!srPSV.EndOfStream)
                {
                    uint str = Convert.ToUInt32(srPSV.ReadLine());
                    if (result.TSV == str)
                    {
                        result.Shiny = true;
                        return true;
                    }
                }
                return false;
            }

            if (setting.ID_List != "")
            {
                while (!srID.EndOfStream)
                {
                    string str = string.Format("{0:D6}", srID.ReadLine());
                    string str2 = string.Format("{0:D6}", result.ID);

                    if (PerfectMatching.Checked && str2 == str)
                        return true;
                    if (PartialMatch.Checked && 0 <= str2.IndexOf(str))
                        return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }


        private DataGridViewRow getRow_Egg(int i, EggRNGSearch rng, EggRNGSearch.EggRNGResult result, DataGridView dgv)
        {
            var true_psv = rng.PIDRerolls > 0 ? result.PSV.ToString("d") : "-";
            string true_pid = International.Checked || omamori.Checked ? result.PID.ToString("X8") : STR_TEMP_PID;
            string true_nature = rng.Everstone ? (rng.Both_Everstone ? (result.BE_InheritParents == 0 ? STR_FIRST : STR_SECOND) : items[0]) : natures[result.Nature];

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dgv);
            row.SetValues(
                i, result.FramesUsed, result.Seed128,
                result.IVs[0], result.IVs[1], result.IVs[2], result.IVs[3], result.IVs[4], result.IVs[5],
                genders[result.Gender], abilities[result.Ability], true_nature,
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

        private DataGridViewRow getRow_Sta(int i, StationaryRNGSearch rng, StationaryRNGSearch.StationaryRNGResult result, DataGridView dgv)
        {
            int tolerance = (int)i - Convert.ToInt32(St_TargetFrame.Text);
            string true_nature = natures[result.Nature];
            var SynchronizeFlag = result.Synchronize ? "o" : "-";
            string[] status = new string[6];
            for (int j = 0; j < 6; j++)
                status[j] = St_Status_display.Checked ? result.p_Status[j].ToString() : "-";

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dgv);

            row.SetValues(
                i, tolerance,
                result.IVs[0], result.IVs[1], result.IVs[2], result.IVs[3], result.IVs[4], result.IVs[5],
                true_nature, SynchronizeFlag, status[0], status[1], status[2], status[3], status[4], status[5], result.PSV, result.Clock, result.row_r.ToString("X16")
                );

            if (result.Shiny)
            {
                row.DefaultCellStyle.BackColor = Color.LightCyan;
            }
            return row;
        }

        private DataGridViewRow getRow_ID(int i, IDRNGSearch rng, IDRNGSearch.IDRNGResult result, DataGridView dgv)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dgv);

            row.SetValues(
                i, String.Format("{0:D6}", result.ID), result.TSV, result.TID, result.SID, result.Clock
                );

            if (result.Shiny)
            {
                row.DefaultCellStyle.BackColor = Color.LightCyan;
            }
            return row;
        }

        private void EggSearch_Click(object sender, EventArgs e)
        {
            if (CheckValidity())
                EggSearch();
        }

        private void EggSearch()
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

            var setting = EgggetSettings();
            var rng = getEggRNGSettings();

            for (int i = 0; i < min; i++)
                tiny.nextState();
            for (int i = min; i <= max; i++, tiny.nextState())
            {
                //statusの更新
                tiny.status.CopyTo(st, 0);
                EggRNGSearch.EggRNGResult result = rng.Generate(st);

                if (!EggframeMatch(result, setting))
                    continue;
                list.Add(getRow_Egg(i, rng, result, k_dataGridView));
            }

            k_dataGridView.Rows.AddRange(list.ToArray());
            k_dataGridView.CurrentCell = null;
        }

        private void EggList_Search_Click(object sender, EventArgs e)
        {
            if (CheckValidity())
            {
                EggList_Search();
                EggList_calc_target();
            }
        }

        private void EggList_Search()
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

            var rng = getEggRNGSettings();
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
                    var row = getRow_Egg(i, rng, result, L_dataGridView);
                    list.Add(row);
                }
                // Continue adjacents
                rng.tiny.status.CopyTo(tiny.status, 0);
            }

            L_dataGridView.Rows.AddRange(list.ToArray());
            L_dataGridView.CurrentCell = null;
        }

        private void EggList_calc_target()
        {
            //Added function that shows how many number off eggs needed to receive and reject to advance foo number of frames
            int target = (int)Target_frame.Value;
            for (int co = 1; co < L_dataGridView.Rows.Count; co++)
            {
                if ((int)L_dataGridView[1, co].Value == target)
                {
                    Repeat_times.Text = msgstr[10] + $" {(int)L_dataGridView[0, co - 1].Value} " + msgstr[11];
                    break;
                }
                else if ((int)L_dataGridView[1, co].Value > target)
                {
                    Repeat_times.Text = msgstr[10] + $" {(int)L_dataGridView[0, co - 1].Value - 1} " + msgstr[11] + $" {target - (int)L_dataGridView[1, co - 1].Value} " + msgstr[12];
                    break;
                }
                else if (target > L_dataGridView.Rows.Count)
                {
                    Repeat_times.Text = msgstr[13];
                }
            }
        }

        private bool CheckValidity()
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
            else if (sex_ratio.SelectedIndex == 4 && !(post_ditto.Checked))
            {
                post_ditto.Checked = true;
                return true;
            }
            else if (sex_ratio.SelectedIndex == 6 && !(post_ditto.Checked || pre_ditto.Checked))
            {
                Error(msgstr[8]);
                post_ditto.Checked = true;
                return true;
            }
            else if (sex_ratio.SelectedIndex == 6 && pre_ditto.Checked)
            {
                Error(msgstr[9]);
                post_ditto.Checked = true;
                return true;
            }
            else
                return true;
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            St_dataGridView.DefaultCellStyle.Font = new Font("Consolas", 9);
            k_dataGridView.DefaultCellStyle.Font = new Font("Consolas", 9);
            L_dataGridView.DefaultCellStyle.Font = new Font("Consolas", 9);
            ID_dataGridView.DefaultCellStyle.Font = new Font("Consolas", 9);
            k_dataGridView.Columns[9].DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9);
            L_dataGridView.Columns[9].DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9);

            Type dgvtype = typeof(DataGridView);
            System.Reflection.PropertyInfo dgvPropertyInfo = dgvtype.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            dgvPropertyInfo.SetValue(St_dataGridView, true, null);
            dgvPropertyInfo.SetValue(k_dataGridView, true, null);
            dgvPropertyInfo.SetValue(L_dataGridView, true, null);
            dgvPropertyInfo.SetValue(ID_dataGridView, true, null);

            for (int i = 0; i < 17; i++)
            {
                mezapaType.Items.Add("");
                St_mezapaType.Items.Add("");
            }

            for (int i = 0; i < 26; i++)
            {
                nature.Items.Add("");
                St_nature.Items.Add("");
                St_Synchro_nature.Items.Add("");
            }

            for (int i = 0; i < pokedex.GetLength(0); i++)
            {
                St_pokedex.Items.Add("");
            }

            foreach (var cbItem in main_langlist)
                CB_MainLanguage.Items.Add(cbItem);

            string l = Properties.Settings.Default.lang;
            int lang = Array.IndexOf(languages, l);
            if (lang < 0) lang = Array.IndexOf(languages, "ja");

            CB_MainLanguage.SelectedIndex = lang;
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

            St_mezapaType.SelectedIndex = 0;
            St_nature.SelectedIndex = 0;
            St_Synchro_nature.SelectedIndex = 0;
            St_pokedex.SelectedIndex = 0;

            loadConfig();
            other_TSV.Enabled = loadTSV();
            Menu_ParentsList.Enabled = loadParents();

            St_IVlow1.Visible = true;
            St_IVlow2.Visible = true;
            St_IVlow3.Visible = true;
            St_IVlow4.Visible = true;
            St_IVlow5.Visible = true;
            St_IVlow6.Visible = true;

            St_IVup1.Visible = true;
            St_IVup2.Visible = true;
            St_IVup3.Visible = true;
            St_IVup4.Visible = true;
            St_IVup5.Visible = true;
            St_IVup6.Visible = true;

            St_stats1.Visible = false;
            St_stats2.Visible = false;
            St_stats3.Visible = false;
            St_stats4.Visible = false;
            St_stats5.Visible = false;
            St_stats6.Visible = false;

            St_pokedex.Enabled = false;
            St_Lv.Enabled = false;

            omamori.Checked = Properties.Settings.Default.omamori;
        }

        private void omamori_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.omamori = (sender as CheckBox).Checked;
            Properties.Settings.Default.Save();
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
                    Error("status[0]" + msgstr[14]);
                else if (!uint.TryParse(st1, NumberStyles.HexNumber, null, out s1))
                    Error("status[1]" + msgstr[14]);
                else if (!uint.TryParse(st2, NumberStyles.HexNumber, null, out s2))
                    Error("status[2]" + msgstr[14]);
                else if (!uint.TryParse(st3, NumberStyles.HexNumber, null, out s3))
                    Error("status[3]" + msgstr[14]);
                else if (!ushort.TryParse(tsvstr, out tsv))
                    Error("TSV" + msgstr[14]);
                else if (tsv > 4095)
                    Error("TSV" + msgstr[7]);
                else
                {
                    status3.Value = L_status3a.Value = s3;
                    status2.Value = L_status2a.Value = s2;
                    status1.Value = L_status1a.Value = s1;
                    status0.Value = L_status0a.Value = s0;
                    TSV.Value = tsv;
                    St_TSV.Value = tsv;
                }
            }
            else
            {
                Error(PATH_CONFIG + " " + msgstr[15] + "\n" + msgstr[16]);
            }
        }

        private bool loadTSV()
        {
            if (!File.Exists(PATH_TSV))
                return false;

            string[] list = File.ReadAllLines(PATH_TSV);
            int[] tsvs = new int[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                var v = list[i];
                int val;
                if (!int.TryParse(v, out val)) // not number
                {
                    string message = $"{i + 1}" + msgstr[17] + $"{v}" + msgstr[14];
                    Error(message);
                    return false;
                }
                if (0 > val || val > 4095)
                {
                    string message = $"{i + 1}" + msgstr[17] + $"{v}" + msgstr[7];
                    Error(message);
                    return false;
                }
                tsvs[i] = val;
            }

            other_tsv = tsvs;
            return true;
        }

        private bool loadParents()
        {
            if (!File.Exists(PATH_PARENTS))
                return false;

            string[] list = File.ReadAllLines(PATH_PARENTS);

            for (int i = 0; i < list.Length; i++)
            {
                var Data = list[i].Split(',');
                int value;

                if (Data.Length > 7)
                    return false;

                for (int j = 1; j < Data.Length; j++)
                {
                    if (!int.TryParse(Data[j], out value)) // not number
                    {
                        return false;
                    }
                    value = Convert.ToInt32(Data[j]);
                    if (0 > value || value > 31)
                    {
                        return false;
                    }
                }
            }

            parents_list = list;
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
                Error(msgstr[18]);
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
                Error(msgstr[19]);
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
                Error(msgstr[19]);
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
                Error(msgstr[18]);
            }
        }

        private void L_SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            L_dataGridView.SelectAll();
        }

        private void Change_ditto(object sender, EventArgs e)
        {
            if ((sender as CheckBox)?.Checked ?? false)
            {
                (sender == post_ditto ? pre_ditto : post_ditto).Checked = false;
                Heterogeneity.Enabled = false;
                Heterogeneity.Checked = true;
            }
            else
            {
                Heterogeneity.Checked = false;
                Heterogeneity.Enabled = true;
            }
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
                Error(PATH_CONFIG + msgstr[20]);
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
                Error(PATH_TSV + msgstr[20]);
            }
        }

        private void B_Parents_Click(object sender, EventArgs e)
        {
            var editor = new ParentsEntry(parents_list);
            editor.form1 = this;
            editor.ShowDialog();
        }

        private void mainMenuExit(object sender, EventArgs e)
        {
            Close();
        }

        private void Stationary_Search_Click(object sender, EventArgs e)
        {
            if (St_min.Value > St_max.Value)
                Error(msgstr[0]);
            else if (St_IVlow1.Value > St_IVup1.Value)
                Error(msgstr[1]);
            else if (St_IVlow2.Value > St_IVup2.Value)
                Error(msgstr[2]);
            else if (St_IVlow3.Value > St_IVup3.Value)
                Error(msgstr[3]);
            else if (St_IVlow4.Value > St_IVup4.Value)
                Error(msgstr[4]);
            else if (St_IVlow5.Value > St_IVup5.Value)
                Error(msgstr[5]);
            else if (St_IVlow6.Value > St_IVup6.Value)
                Error(msgstr[6]);
            else if (0 > St_TSV.Value || St_TSV.Value > 4095)
                Error("TSV" + msgstr[7]);
            else
                StationarySearch();
        }

        private void St_search_IV_CheckedChanged(object sender, EventArgs e)
        {
            if (St_search_IV.Checked)
            {
                St_IVlow1.Visible = true;
                St_IVlow2.Visible = true;
                St_IVlow3.Visible = true;
                St_IVlow4.Visible = true;
                St_IVlow5.Visible = true;
                St_IVlow6.Visible = true;

                St_IVup1.Visible = true;
                St_IVup2.Visible = true;
                St_IVup3.Visible = true;
                St_IVup4.Visible = true;
                St_IVup5.Visible = true;
                St_IVup6.Visible = true;

                St_stats1.Visible = false;
                St_stats2.Visible = false;
                St_stats3.Visible = false;
                St_stats4.Visible = false;
                St_stats5.Visible = false;
                St_stats6.Visible = false;

                St_Status_display.Visible = true;

                if (St_Status_display.Checked)
                {
                    St_pokedex.Enabled = true;
                    St_Lv.Enabled = true;
                }
                else
                {
                    St_pokedex.Enabled = false;
                    St_Lv.Enabled = false;
                }

            }
            else
            {
                St_IVlow1.Visible = false;
                St_IVlow2.Visible = false;
                St_IVlow3.Visible = false;
                St_IVlow4.Visible = false;
                St_IVlow5.Visible = false;
                St_IVlow6.Visible = false;

                St_IVup1.Visible = false;
                St_IVup2.Visible = false;
                St_IVup3.Visible = false;
                St_IVup4.Visible = false;
                St_IVup5.Visible = false;
                St_IVup6.Visible = false;

                St_stats1.Visible = true;
                St_stats2.Visible = true;
                St_stats3.Visible = true;
                St_stats4.Visible = true;
                St_stats5.Visible = true;
                St_stats6.Visible = true;

                St_Status_display.Visible = false;
                St_pokedex.Enabled = true;
                St_Lv.Enabled = true;
            }
        }

        private void StationarySearch()
        {
            uint InitialSeed = (uint)St_InitialSeed.Value;
            int min = (int)St_min.Value;
            int max = (int)St_max.Value;

            SFMT sfmt = new SFMT(InitialSeed);
            SFMT seed = new SFMT(InitialSeed);
            List<DataGridViewRow> list = new List<DataGridViewRow>();
            St_dataGridView.Rows.Clear();

            var setting = StationarygetSettings();
            var rng = getStationaryRNGSettings();

            for (int i = 0; i < min; i++)
                sfmt.NextUInt64();

            for (int i = min; i <= max; i++, sfmt.NextUInt64())
            {
                seed = (SFMT)sfmt.DeepCopy();
                StationaryRNGSearch.StationaryRNGResult result = rng.Generate(seed);

                if (!StationaryframeMatch(result, setting))
                    continue;
                list.Add(getRow_Sta(i, rng, result, St_dataGridView));
            }
            St_dataGridView.Rows.AddRange(list.ToArray());
            St_dataGridView.CurrentCell = null;
        }

        private void St_check_display_Click(object sender, EventArgs e)
        {
            if (St_Status_display.Checked)
            {
                St_pokedex.Enabled = true;
                St_Lv.Enabled = true;
            }
            else
            {
                St_pokedex.Enabled = false;
                St_Lv.Enabled = false;
            }
        }

        private void St_UpdateFrame_Click(object sender, EventArgs e)
        {
            UInt32 TargetFrame = Convert.ToUInt32(St_TargetFrame.Value);
            UInt32 TargetFrame_Range = Convert.ToUInt32(St_TargetFrame_Range.Text);
            UInt32 min = TargetFrame - TargetFrame_Range;
            UInt32 max = TargetFrame + TargetFrame_Range;

            St_min.Text = Convert.ToString(min);
            St_max.Text = Convert.ToString(max);

        }

        private void Get_Clock_Number(object sender, EventArgs e)
        {
            string str = ((Button)sender).Name;
            string number = str.Remove(0, str.IndexOf("Clock_") + 6);

            if (Clock_List.Text == "")
            {
                Clock_List.Text += Convert_Clock(number);
            }
            else
            {
                Clock_List.Text += "," + Convert_Clock(number);
            }

            var needle = Clock_List.Text.Split(',');
            if (Clock_List.Text.Where(c => c == ',').Count() >= 7)
            {
                var text = "";
                try
                {
                    var results = SFMTSeedAPI.request(Clock_List.Text);
                    if (results == null || results.Count() == 0)
                    {
                        text = "Not Found";
                    }
                    else
                    {
                        text = string.Join(" ", results.Select(r => r.seed));
                    }
                }
                catch (Exception exc)
                {

                    text = exc.Message;
                }
                finally
                {
                    TB_Candidate_InitSeed.Text = text;
                }
            }

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            Clock_List.Text = "";
        }

        private void Back_Click(object sender, EventArgs e)
        {
            string str = Clock_List.Text;
            if (Clock_List.Text != "")
            {
                if (str.LastIndexOf(",") != -1)
                {
                    str = str.Remove(str.LastIndexOf(","));
                }
                else
                {
                    str = "";
                }
            }
            Clock_List.Text = str;
        }

        private string Convert_Clock(string n)
        {
            int tmp = Convert.ToInt32(n);
            if (clock_end.Checked)
            {
                if (tmp >= 4)
                {
                    tmp -= 4;
                }
                else
                {
                    tmp += 13;
                }
                n = tmp.ToString();
            }
            return n;
        }

        private void Clock_CurrentFrame_Click(object sender, EventArgs e)
        {
            uint InitialSeed = (uint)Clock_InitialSeed.Value;
            int min = (int)Clock_min.Value;
            int max = (int)Clock_max.Value;
            if (Clock_SearchList.Text == "")
                return;
            string[] str = Clock_SearchList.Text.Split(',');
            try
            {
                int[] Clock_List = str.Select(s => int.Parse(s)).ToArray();
                int[] temp_List = new int[Clock_List.Length];

                SFMT sfmt = new SFMT(InitialSeed);
                SFMT seed = new SFMT(InitialSeed);
                bool flag;

                Search_Clock.Items.Clear();

                for (int i = 0; i < min; i++)
                    sfmt.NextUInt64();

                for (int i = min; i <= max; i++, sfmt.NextUInt64())
                {
                    flag = false;
                    seed = (SFMT)sfmt.DeepCopy();

                    for (int j = 0; j < Clock_List.Length; j++)
                        temp_List[j] = (int)(seed.NextUInt64() % 17);

                    if (temp_List.SequenceEqual(Clock_List))
                    {
                        flag = true;
                    }

                    if (flag)
                    {
                        Search_Clock.Items.Add(msgstr[21] + $"{i + Clock_List.Length - 1}" + msgstr[22] + $"{i + Clock_List.Length + 1}");
                    }

                }
            }
            catch
            {
            }
        }

        private void Calc_Frame_Click(object sender, EventArgs e)
        {
            uint InitialSeed = (uint)Calc_InitialSeed.Value;
            int min = (int)Calc_min.Value;
            int max = (int)Calc_max.Value;
            int NPC_n = (int)NPC.Value + 1;
            SFMT sfmt = new SFMT(InitialSeed);

            Calc_Output.Items.Clear();

            for (int i = 0; i < min; i++)
                sfmt.NextUInt64();

            int n_count = 0;

            int[] remain_frame = new int[NPC_n];
            int total_frame = 0;
            bool[] blink_flag = new bool[NPC_n];


            while (min + n_count < max)
            {
                //NPCの数だけ回す -- NPC Loop
                for (int i = 0; i < NPC_n; i++)
                {
                    if (remain_frame[i] > 0)
                        remain_frame[i]--;

                    if (remain_frame[i] == 0)
                    {
                        //まばたき中 -- Blinking
                        if (blink_flag[i])
                        {
                            if ((int)(sfmt.NextUInt64() % 3) == 0)
                            {
                                remain_frame[i] = 36;
                            }
                            else
                            {
                                remain_frame[i] = 30;
                            }
                            n_count++;
                            blink_flag[i] = false;
                        }
                        //非まばたき中 -- Not Blinking
                        else
                        {
                            if ((int)(sfmt.NextUInt64() % 128) == 0)
                            {
                                remain_frame[i] = 5;
                                blink_flag[i] = true;
                            }
                            n_count++;
                        }
                    }
                }
                total_frame++;
            }

            Calc_Output.Items.Add(msgstr[23] + $"：{(total_frame) * 2}");
        }

        private void ID_Search_Click(object sender, EventArgs e)
        {
            uint InitialSeed = (uint)ID_InitialSeed.Value;
            int min = (int)ID_min.Value;
            int max = (int)ID_max.Value;

            SFMT sfmt = new SFMT(InitialSeed);
            SFMT seed = new SFMT(InitialSeed);
            List<DataGridViewRow> list = new List<DataGridViewRow>();
            ID_dataGridView.Rows.Clear();

            var setting = IDgetSettings();
            var rng = getIDRNGSettings();

            for (int i = 0; i < min; i++)
                sfmt.NextUInt64();

            for (int i = min; i <= max; i++, sfmt.NextUInt64())
            {
                seed = (SFMT)sfmt.DeepCopy();
                IDRNGSearch.IDRNGResult result = rng.Generate(seed);

                if (!IDframeMatch(result, setting))
                    continue;
                list.Add(getRow_ID(i, rng, result, ID_dataGridView));
            }
            ID_dataGridView.Rows.AddRange(list.ToArray());
            ID_dataGridView.CurrentCell = null;
        }

        private void ChangePoke(object sender, EventArgs e)
        {
            TypeNull.Checked = (St_pokedex.SelectedIndex == 6);
            switch (St_pokedex.SelectedIndex)
            {
                case 3: NPC.Value = 1; break; // Tapu Fini
                case 6: NPC.Value = 8; break; // Type:Null
                default: NPC.Value = 0; break;
            }
        }
    }
}
