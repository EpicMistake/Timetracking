using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;
namespace ProjectCreator {
    class Data {
        private string _client;
        private string _projectName;
        private bool _isTLA;
        private string _date;
        private string _type;
        private string _msgFileName;
        public List<string> files;
        public Data() {
            files = new List<string>();
        }
        #region setters/getters
        public string msgFileName {
            get { return _msgFileName; }
            set { _msgFileName = value; }
        }
        public string date {
            get { return _date; }
            set { _date = value; }
        }
        public string type {
            get { return _type; }
            set { _type = value; }
        }
        public bool isTLA {
            get { return _isTLA; }
            set { _isTLA = value; }
        }
        public string projectName {
            get { return _projectName; }
            set { _projectName = value; }
        }
        public string client {
            get { return _client; }
            set { _client = value; }
        }
        #endregion
    }
    class CreatorModel {
        private string _startDate;
        private string _root;
        private string _defaultRoot;
        private string _projectName;
        #region setters/getters
        public string projectName { get; set; }
        public string clientName { get; set; }
        public string date { get; set; }
        public string root { get; set; }
        public string defaultRoot { get; set; }
        #endregion

        private DataBaseManager dbManager;
        private List<string> _files;
        public CreatorModel(string root) {
            _root = root;
            _defaultRoot = root;
            _files = new List<string>();
            dbManager = new DataBaseManager();
        }
        public string getProjectName() {
            return _projectName;
        }
        public bool open() {
            return dbManager.open();
        }
        public bool close() {
            return dbManager.close();
        }
        public void create(ProjectInfo data) {
            string ifErrorDelete = null ;
            try {
                if(_startDate == null) _startDate = DateTime.Now.ToShortDateString();
                _root += @"\" + data.projectTypeName + @"\" + data.clientName + @"\";
                ifErrorDelete = _root;
                Directory.CreateDirectory(_root);
                _root += _startDate + "_" + data.requestTypeName + "_" + data.projectName + @"\";
                data.path = _root;
                Directory.CreateDirectory(_root);
                string tenders = _root + "Tender task_" + _startDate + "_" + data.requestTypeName + "_" + data.projectName;
                Directory.CreateDirectory(tenders);
                Directory.CreateDirectory(_root + "Calculation_" + _startDate + "_" + data.requestTypeName + "_" + data.projectName);
                Directory.CreateDirectory(_root + "Offers_" + _startDate + "_" + data.requestTypeName + "_" + data.projectName);
                Directory.CreateDirectory(_root + "Validation_" + _startDate + "_" + data.requestTypeName + "_" + data.projectName);
                foreach(string file in _files) { 
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + @"temp\" + file, tenders + @"\" + file, true);
                }
                
                dbManager.createProject(data);
                System.Diagnostics.Process.Start(_root);
                _root = _defaultRoot;
            } catch(SystemException ex) {
                this.close();
                System.Windows.Forms.MessageBox.Show(ex.Message);
                if(ifErrorDelete != null) Directory.Delete(ifErrorDelete, true);
                _root = _defaultRoot;
                throw ex;
            }
        }
        public List<ControlMember> getClients() {
            Dictionary<int, string> clients = dbManager.getValues("SELECT * FROM Clients");
            List<ControlMember> to_return = new List<ControlMember>();
            foreach(int id in clients.Keys) {
                ControlMember obj = new ControlMember(id, clients[id]);
                to_return.Add(obj);
            }
            return to_return;
        }
        public List<ControlMember> getStages() {
            Dictionary<int, string> stages = dbManager.getValues("SELECT * FROM StageNames");
            List<ControlMember> to_return = new List<ControlMember>();
            foreach(int id in stages.Keys) {
                ControlMember obj = new ControlMember(id, stages[id]);
                to_return.Add(obj);
            }
            return to_return;
        }
        public List<ControlMember> getParticipants() {
            Dictionary<int, string> players = dbManager.getValues("SELECT * FROM Participants");
            List<ControlMember> to_return = new List<ControlMember>();
            foreach(int id in players.Keys) {
                ControlMember obj = new ControlMember(id, players[id]);
                to_return.Add(obj);
            }
            return to_return;
        }
        public int getProjectId() {
            return dbManager.generateID("Projects");
        }
        public List<ControlMember> getRequestTypes() {
            Dictionary<int, string> types = dbManager.getValues("SELECT * FROM RequestTypes");
            List<ControlMember> to_return = new List<ControlMember>();
            foreach(int id in types.Keys) {
                ControlMember obj = new ControlMember(id, types[id]);
                to_return.Add(obj);
            }
            return to_return;
        }
        public string getLetterDate() {
            return _startDate; 
        }
        public string getStartDate() {
            return DateTime.Now.Date.ToShortDateString();
        }
        public List<ControlMember> getProjectTypes() {
            Dictionary<int, string> types = dbManager.getValues("SELECT * FROM ProjectTypes");
            List<ControlMember> to_return = new List<ControlMember>();
            foreach(int id in types.Keys) {
                ControlMember obj = new ControlMember(id, types[id]);
                to_return.Add(obj);
            }
            return to_return;
        }
        public void getActiveLetter() {
            Outlook.Application app = new Outlook.Application();
            Outlook.Selection items = app.ActiveExplorer().Selection;
            foreach(object mail in items) {
                Outlook.MailItem im = (Outlook.MailItem)mail;
                _projectName = im.Subject.Replace(":", "");
                _startDate = im.CreationTime.Date.ToShortDateString();

                Directory.CreateDirectory("temp");
                foreach(Outlook.Attachment attach in im.Attachments) {
                    attach.SaveAsFile(AppDomain.CurrentDomain.BaseDirectory + @"temp\" + attach.FileName);
                    _files.Add(attach.FileName);
                }
                im.SaveAs(AppDomain.CurrentDomain.BaseDirectory + @"temp\" + im.Subject.Replace(":", "") + ".msg");
                _files.Add(im.Subject.Replace(":", "") + ".msg");
            }
        }
    }
}
