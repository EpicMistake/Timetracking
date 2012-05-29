using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ProjectCreator {

    public partial class CreatorView : Form, CreatorInterface {
        private List<ComboBox> participantBoxes;
        private List<TextBox> stageBoxes;
        private List<DateTimePicker> dateFromBoxes;
        private List<DateTimePicker> dateToBoxes;
        private TabPage _planPage;
        public CreatorView() {
            InitializeComponent();
            participantBoxes = new List<ComboBox>();
            participantBoxes.Add(participant1);
            participantBoxes.Add(participant2);
            participantBoxes.Add(participant3);
            participantBoxes.Add(participant4);

            stageBoxes = new List<TextBox>();
            #region filling stageBoxes array
            stageBoxes.Add(stageBox1);
            stageBoxes.Add(stageBox2);
            stageBoxes.Add(stageBox3);
            stageBoxes.Add(stageBox4);
            stageBoxes.Add(stageBox5);
            stageBoxes.Add(stageBox6);
            stageBoxes.Add(stageBox7);
            stageBoxes.Add(stageBox8);
            stageBoxes.Add(stageBox9);
            stageBoxes.Add(stageBox10);
            stageBoxes.Add(stageBox11);
            stageBoxes.Add(stageBox12);
            stageBoxes.Add(stageBox13);
            stageBoxes.Add(stageBox14);
            stageBoxes.Add(stageBox15);
            stageBoxes.Add(stageBox16);
            stageBoxes.Add(stageBox17);
            #endregion
            dateFromBoxes = new List<DateTimePicker>();
            #region filling dateFromBoxes array
            dateFromBoxes.Add(planDateFrom1);
            dateFromBoxes.Add(planDateFrom2);
            dateFromBoxes.Add(planDateFrom3);
            dateFromBoxes.Add(planDateFrom4);
            dateFromBoxes.Add(planDateFrom5);
            dateFromBoxes.Add(planDateFrom6);
            dateFromBoxes.Add(planDateFrom7);
            dateFromBoxes.Add(planDateFrom8);
            dateFromBoxes.Add(planDateFrom9);
            dateFromBoxes.Add(planDateFrom10);
            dateFromBoxes.Add(planDateFrom11);
            dateFromBoxes.Add(planDateFrom12);
            dateFromBoxes.Add(planDateFrom13);
            dateFromBoxes.Add(planDateFrom14);
            dateFromBoxes.Add(planDateFrom15);
            dateFromBoxes.Add(planDateFrom16);
            dateFromBoxes.Add(planDateFrom17);
            #endregion
            dateToBoxes = new List<DateTimePicker>();
            #region filling dateToBoxes array
            dateToBoxes.Add(planDateTo1);
            dateToBoxes.Add(planDateTo2);
            dateToBoxes.Add(planDateTo3);
            dateToBoxes.Add(planDateTo4);
            dateToBoxes.Add(planDateTo5);
            dateToBoxes.Add(planDateTo6);
            dateToBoxes.Add(planDateTo7);
            dateToBoxes.Add(planDateTo8);
            dateToBoxes.Add(planDateTo9);
            dateToBoxes.Add(planDateTo10);
            dateToBoxes.Add(planDateTo11);
            dateToBoxes.Add(planDateTo12);
            dateToBoxes.Add(planDateTo13);
            dateToBoxes.Add(planDateTo14);
            dateToBoxes.Add(planDateTo15);
            dateToBoxes.Add(planDateTo16);
            dateToBoxes.Add(planDateTo17);
            #endregion

            foreach(DateTimePicker box in dateFromBoxes) {
                box.Checked = false;
            }
            foreach(DateTimePicker box in dateToBoxes) {
                box.Enabled = false;
            }

            _planPage = tabPage2;
            tabControl.TabPages.Remove(tabPage2);
        }

        #region interfaces implementions
        public void setClient(string client) {
            clientBox.Text = client;
        }
        public void setProjectId(int id) {
            projectIdBox.Text = id.ToString();
        }
        public void setParticipants(List<ControlMember> players) {
            players.Insert(0, new ControlMember(-1, ""));
            foreach(ComboBox box in participantBoxes) {
                var data = players.Select(player => new { Id = player.id, Text = player.name }).ToList();
                box.ValueMember = "Id";
                box.DisplayMember = "Text";
                box.DataSource = data;
            }
        }
        private void dateTimeChecks(object sender, EventArgs e) {
            DateTimePicker box = (DateTimePicker)sender;
            int index = dateFromBoxes.IndexOf(box);
            if(box.Checked) {
                dateToBoxes[index].Enabled = true;
            } else {
                dateToBoxes[index].Enabled = false;
            }
        }
        public void setStages(List<ControlMember> stages) {
            for(int i = 0; i < stageBoxes.Count; i++ ){
                stageBoxes[i].Dispose();
                dateFromBoxes[i].Dispose();
                dateToBoxes[i].Dispose();
            }
            stageBoxes.Clear();
            dateFromBoxes.Clear();
            dateToBoxes.Clear();
            for(int i = 0; i < stages.Count; i++) {
                TextBox box = new TextBox();
                box.Parent = tabPage2;
                box.Text = stages[i].name;
                box.Left = 6;
                box.Top = 6 + i * box.Height;
                box.Width = 268;
                DateTimePicker fromDate = new DateTimePicker();
                fromDate.Parent = tabPage2;
                fromDate.Left = box.Left + box.Width;
                fromDate.Width = 200;
                fromDate.Top = 6 + i * box.Height;
                fromDate.Height = box.Height;
                fromDate.ShowCheckBox = true;
                fromDate.Checked = false;
                fromDate.MouseDown += new MouseEventHandler(dateTimeChecks);
                DateTimePicker toDate = new DateTimePicker();
                toDate.Parent = tabPage2;
                toDate.Left = box.Left + box.Width + fromDate.Width;
                toDate.Width = 200;
                toDate.Top = 6 + i * box.Height;
                toDate.Height = box.Height;
                toDate.Enabled = false;
                stageBoxes.Add(box);
                dateToBoxes.Add(toDate);
                dateFromBoxes.Add(fromDate);
            }
            /*var data = stages.Select(stage => new { Id = stage.id, Text = stage.name }).ToList();
            int i = 0;
            foreach(TextBox box in stageBoxes) {
                box.Dispose();
                //box.Text = stages[i].name;
                //i++;
            }*/
        }
        public void reload() {
            if(OnStart != null) {
                dateBox.Clear();
                projectIdBox.Clear();
                projectBox.Clear();
                startDateBox.Clear();
                isPlanBox.Checked = false;
                OnStart(this, null);
            }
        }
        public void setClients(List<ControlMember> clients) {
            var data = clients.Select(client => new { Id = client.id, Text = client.name }).ToList();
            clientBox.ValueMember = "Id";
            clientBox.DisplayMember = "Text";
            clientBox.DataSource = data;
        }
        public void setRequestTypes(List<ControlMember> requestTypes) {
            var data = requestTypes.Select(request => new { Id = request.id, Text = request.name }).ToList();
            requestBox.ValueMember = "Id";
            requestBox.DisplayMember = "Text";
            requestBox.DataSource = data;
        }
        public void setProjectTypes(List<ControlMember> projectTypes) {
            var data = projectTypes.Select(projectType => new { Id = projectType.id, Text = projectType.name }).ToList();
            typeBox.ValueMember = "Id";
            typeBox.DisplayMember = "Text";
            typeBox.DataSource = data;
        }
        public void setStartDate(string date) {
            startDateBox.Text = date;
        }
        public void setLetterDate(string date) {
            dateBox.Text = date;
        }
        public void setProjectName(string name) {
            projectBox.Text = name;
        }
        public void addFile(string file) {
            //listBox1.Items.Add(file);
        }
        public event EventHandler<EventArgs> OnCreate;
        public event EventHandler<DragEventArgs> OnDrag;
        public event EventHandler<DragEventArgs> OnDrop;
        public event EventHandler<EventArgs> OnStart;
        #endregion

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }
        private void createButton_Click(object sender, EventArgs e) {
            bool isEmpty = true;
            foreach(ComboBox box in participantBoxes) {
                if((int)box.SelectedValue != -1) {
                    isEmpty = false;
                    break;
                }
            }
            if(isEmpty) {
                MessageBox.Show("Specify at least one participant");
                return;
            }
            if(OnCreate != null) {

                if(clientBox.Text == "" || projectBox.Text == "" || typeBox.Text == "" || dateBox.Text == "" || requestBox.Text == null) {
                    MessageBox.Show("Please, fill all text boxes.");
                    return;
                }
                ProjectCreator.ProjectInfo info = new ProjectInfo();
                if(clientBox.SelectedValue == null) info.clientId = -1;
                else {
                    info.clientId = (int)clientBox.SelectedValue;
                }
                info.clientName = clientBox.Text;
                info.projectId = Int32.Parse(projectIdBox.Text);
                info.projectName = projectBox.Text;
                info.projectStartDate = startDateBox.Text;
                info.letterDate = dateBox.Text;
                info.projectType = (int)typeBox.SelectedValue;
                info.projectTypeName = typeBox.Text;
                info.requestType = (int)requestBox.SelectedValue;
                info.requestTypeName = requestBox.Text;
                info.participants = new List<int>();
                info.deadline = DateTime.Parse(deadlineBox.Text).ToShortDateString();
                foreach(ComboBox box in participantBoxes) {
                    if((int)box.SelectedValue >= 0) {
                        info.participants.Add((int)box.SelectedValue);
                    }
                }
                if(isPlanBox.Checked) {
                    info.stagesDate = new Dictionary<int, KeyValuePair<string, string>>();
                    for(int i = 0; i < stageBoxes.Count; i++) {
                        if(dateFromBoxes[i].Checked == false) continue;
                        info.stagesDate[i + 1] = new KeyValuePair<string, string>(DateTime.Parse(dateFromBoxes[i].Text).ToShortDateString(), DateTime.Parse(dateToBoxes[i].Text).ToShortDateString());
                    }
                }
                OnCreate(sender, new CreatorEvent(info));
            }
        }
        private void creatorView_DragEnter(object sender, DragEventArgs e) {
            if(OnDrag != null) {
                OnDrag(sender, e);
            }
        }
        private void creatorView_DragDrop(object sender, DragEventArgs e) {
            if(OnDrop != null) {
                OnDrop(sender, e);
            }
        }
        private void CreatorView_Shown(object sender, EventArgs e) {
            if(OnStart != null) {
                OnStart(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            if(OnDrop != null) {
                OnDrop(sender, null);
            }
        }
        private void tabPage2_MouseEnter(object sender, EventArgs e) {
            tabPage2.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            if(isPlanBox.Checked) {
                tabControl.TabPages.Add(_planPage);
                tabControl.SelectedTab = _planPage;
            } else {
                tabControl.TabPages.Remove(_planPage);
            }
        }

        private void isPlanBox_CheckStateChanged(object sender, EventArgs e) { 
        }

        private void planDateFrom1_MouseDown(object sender, MouseEventArgs e) {
            
        }
    }
}
