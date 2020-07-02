using SAL_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShineALight
{
    public partial class PresetsEdit : Form
    {
        private PictureBox selectedColor = null;
        private Dictionary<PictureBox, SAL_Core.Color> colorDict = new Dictionary<PictureBox, SAL_Core.Color>();
        private int y = 5;
        public Dictionary<string, EffectPreset> PresetList { get; private set; }
        private string selectedPreset = String.Empty; 

        public PresetsEdit(Dictionary<string, EffectPreset> presetList)
        {
            InitializeComponent();
            Design.Apply(this);
            PresetList = new Dictionary<string, EffectPreset>(presetList);
            foreach (var name in Enum.GetNames(typeof(EffectTypes))) typeSelect.Items.Add(name);
            FillListBox();
            LoadPreset();
        }

        private void FillListBox()
        {
            foreach(var preset in PresetList.Keys)
            {
                presetListBox.Items.Add(preset);
            }
            if (presetListBox.Items.Count > 0) presetListBox.SelectedIndex = 0;
        }

        private void LoadPreset()
        {
            if (presetListBox.SelectedItem == null) return;
            selectedPreset = (string)presetListBox.SelectedItem;
            var preset = PresetList[selectedPreset];
            nameBox.Text = selectedPreset;
            typeSelect.SelectedItem = preset.Type.ToString();
            y = 5;
            colorsPanel.Controls.Clear();
            colorDict.Clear();
            selectedColor = null;
            foreach(var color in preset.ColorList)
            {
                AddColor(color);
            }
        }

        private void AddColor(SAL_Core.Color color)
        {
            var pic = new PictureBox();
            pic.BackColor = color.ToSystemColor();
            pic.Click += Pic_Click;
            pic.DoubleClick += Pic_DoubleClick;
            colorsPanel.Controls.Add(pic);
            pic.Size = new Size(250, 25);
            pic.Location = new Point(5, y);
            pic.BorderStyle = BorderStyle.FixedSingle;
            y += 35;
            colorDict.Add(pic, color);
        }

        private void Pic_DoubleClick(object sender, EventArgs e)
        {
            using(ColorDialog dialog = new ColorDialog())
            {
                dialog.FullOpen = true;
                dialog.ShowDialog(this);
                if (dialog.Color == System.Drawing.Color.Black) return;
                var pict = sender as PictureBox;
                pict.BackColor = dialog.Color;
                colorDict[pict] = new SAL_Core.Color(dialog.Color);
            }
        }

        private void Pic_Click(object sender, EventArgs e)
        {
            if ((PictureBox)sender == selectedColor) return;
            //if(selectedColor != null) selectedColor.BorderStyle = BorderStyle.FixedSingle;
            if(selectedColor != null) selectedColor.Size = new Size(250, 25);
            selectedColor = sender as PictureBox;
            //selectedColor.BorderStyle = BorderStyle.Fixed3D;
            selectedColor.Size = new Size(280, 25);
        }

        private void PresetListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedPreset) && PresetList.ContainsKey(selectedPreset)) PresetList[selectedPreset].ColorList = colorDict.Values.ToList();
            LoadPreset();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (!PresetList.ContainsKey("New Preset"))
            {
                PresetList.Add("New Preset", new EffectPreset());
                presetListBox.Items.Add("New Preset");
                presetListBox.SelectedItem = "New Preset";
            }
            else
            {
                for (int i = 1; ; i++)
                {
                    if (!PresetList.ContainsKey("New Preset" + i))
                    {
                        PresetList.Add("New Preset" + i, new EffectPreset());
                        presetListBox.Items.Add("New Preset" + i);
                        presetListBox.SelectedItem = "New Preset" + i;
                        break;
                    }
                }
            }
            LoadPreset();
        }

        private void NameBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void NameBox_Leave(object sender, EventArgs e)
        {
            if(nameBox.Text != (string)presetListBox.SelectedItem)
            {
                if (!PresetList.ContainsKey(nameBox.Text))
                {
                    PresetList.Add(nameBox.Text, PresetList[selectedPreset]);
                    PresetList.Remove(selectedPreset);
                    selectedPreset = nameBox.Text;
                    presetListBox.Items[presetListBox.SelectedIndex] = nameBox.Text;
                    presetListBox.Update();
                }
                else
                {
                    MessageBox.Show("There is already a preset with that name", "Warning");
                }
            }
        }

        private void TypeSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            PresetList[(string)presetListBox.SelectedItem].Type = (EffectTypes)Enum.Parse(typeof(EffectTypes), typeSelect.Text);
            while(colorDict.Count < MinColors)
            {
                AddColor(Colors.RED);
            }
        }

        private void ColorAddBtn_Click(object sender, EventArgs e)
        {
            AddColor(Colors.RED);
        }

        private void ColorDelBtn_Click(object sender, EventArgs e)
        {
            if(selectedColor != null)
            {
                if(colorDict.Count > MinColors)
                {
                    colorDict.Remove(selectedColor);
                    y = 5;
                    colorsPanel.Controls.Clear();
                    selectedColor = null;
                    foreach (var kvp in colorDict)
                    {
                        kvp.Key.BackColor = kvp.Value.ToSystemColor();
                        kvp.Key.Click += Pic_Click;
                        kvp.Key.DoubleClick += Pic_DoubleClick;
                        colorsPanel.Controls.Add(kvp.Key);
                        kvp.Key.Size = new Size(250, 25);
                        kvp.Key.Location = new Point(5, y);
                        kvp.Key.BorderStyle = BorderStyle.FixedSingle;
                        y += 35;
                    }
                }
                else
                {
                    MessageBox.Show("This effect type has a minimum colors of " + MinColors);
                }
            }
            else
            {
                MessageBox.Show("Please select a color");
            }
        }

        private int MinColors
        {
            get
            {
                var type = (EffectTypes)Enum.Parse(typeof(EffectTypes), typeSelect.Text);
                switch (type)
                {
                    case EffectTypes.Rainbow:
                    case EffectTypes.Cycle:
                    case EffectTypes.Fire:
                    case EffectTypes.Sweep:
                        return 2;
                    case EffectTypes.Breathing:
                    case EffectTypes.Flash:
                    case EffectTypes.Static:
                    case EffectTypes.Beam:
                    case EffectTypes.Load:
                        return 1;
                    default:
                        return 2;
                }
            }
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedPreset) && PresetList.ContainsKey(selectedPreset)) PresetList[selectedPreset].ColorList = colorDict.Values.ToList();
            NameBox_Leave(this, new EventArgs());
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void DelBtn_Click(object sender, EventArgs e)
        {
            if(PresetList.Count > 1)
            {
                var presetName = selectedPreset;
                PresetList.Remove(presetName);
                if (presetListBox.SelectedIndex != 0) presetListBox.SelectedIndex = 0;
                else presetListBox.SelectedIndex = 1;
                presetListBox.Items.Remove(presetName);
                presetListBox.Update();
            }
            else
            {
                MessageBox.Show("There has to be at least one preset");
            }
        }
    }
}
