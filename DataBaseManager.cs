using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Xml.Serialization;

namespace ProjectCreator {
    public class ControlMember {
        public int id;
        public string name;
        public ControlMember(int id, string name) {
            this.id = id;
            this.name = name;
        }
    }
    class ProjectInfo {
        private int _projectId;
        private string _clientName;
        private int _clientId;
        private string _projectName;
        private int _projectType;
        private int _requestType;
        private string _letterDate;
        private string _projectStartDate;
        private int _pilot;
        private string _requestTypeName;
        private string _projectTypeName;
        private Dictionary<int, KeyValuePair<string, string>> _stagesDate;
        private List<int> _participants;
        private string _path;
        private string _deadline;

        public string deadline {
            get { return _deadline; }
            set { _deadline = value; }
        }


        public string path {
            get { return _path; }
            set { _path = value; }
        }


        #region setters/getters
        public int clientId {
            get { return _clientId; }
            set { _clientId = value; }
        }
        public Dictionary<int, KeyValuePair<string, string>> stagesDate {
            get { return _stagesDate; }
            set { _stagesDate = value; }
        }
        public int pilot {
            get { return _pilot; }
            set { _pilot = value; }
        }
        public string projectStartDate {
            get { return _projectStartDate; }
            set { _projectStartDate = value; }
        }        
        public string letterDate {
            get { return _letterDate; }
            set { _letterDate = value; }
        }
        public int requestType {
            get { return _requestType; }
            set { _requestType = value; }
        }
        public int projectType {
            get { return _projectType; }
            set { _projectType = value; }
        }        
        public string projectName {
            get { return _projectName; }
            set { _projectName = value; }
        }
        public string clientName {
            get { return _clientName; }
            set { _clientName = value; }
        }
        public int projectId {
            get { return _projectId; }
            set { _projectId = value; }
        }
        public string projectTypeName {
            get { return _projectTypeName; }
            set { _projectTypeName = value; }
        }

        public string requestTypeName {
            get { return _requestTypeName; }
            set { _requestTypeName = value; }
        }
        public List<int> participants {
            get { return _participants; }
            set { _participants = value; }
        }
        #endregion
    }
    class DataBaseManager {
        private OleDbConnection cnn;

        private string cnnString;
        public DataBaseManager() {
            cnn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;data source=D:\_documents\_projects\Timetracking\StudiesProjects.accdb;");
            
        }
        public bool open() {
            try {
                cnn.Open();
            } catch(SystemException e) {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                return false;
            }
            return true;
        }
        public bool close() {
            try {
                cnn.Close();
            } catch(SystemException e) {
                System.Windows.Forms.MessageBox.Show(e.ToString());
                return false;
            }
            return true;
        }
        public Dictionary<int, string> getValues(string query, int id = 0, int value = 1) {
            Dictionary<int, string> to_return = new Dictionary<int, string>();
            //cnn.Open();
            OleDbCommand cmd = new OleDbCommand(query, cnn);
            OleDbDataReader reader = cmd.ExecuteReader();
            while(reader.Read()) {
                to_return[(int)reader.GetInt32(id)] = reader.GetString(value);
            }
            //cnn.Close();
            return to_return;
        }
        public int generateID(string table) {
            //cnn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT TOP 1 id FROM " + table + " ORDER BY id DESC", cnn);
            OleDbDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int to_return = reader.GetInt32(0) + 1;
            //cnn.Close();
            return to_return;
        }
        public Dictionary<int, string> getParticipants() {
            Dictionary<int, string> to_return = new Dictionary<int, string>();
           // cnn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Participants", cnn);
            OleDbDataReader reader = cmd.ExecuteReader();
            while(reader.Read()) {
                to_return[(int)reader.GetInt32(0)] = reader.GetString(1) + " " + reader.GetString(2);
            }
            //cnn.Close();
            return to_return;
        }
        public void createProject(ProjectInfo info) {
            //cnn.Open();
            int clientId = info.clientId;
            int projectId = info.projectId;
            string insertStatement;
            OleDbCommand insertCommand;
            OleDbTransaction transaction = cnn.BeginTransaction();

            try {
                if(clientId == -1) { //!(new OleDbCommand("SELECT * FROM Clients WHERE id=" + clientId.ToString(), cnn).ExecuteReader().HasRows)) {
                    insertStatement = "INSERT INTO Clients "
                             + "(clientName) "
                             + "VALUES (@clientName)";
                    insertCommand = new OleDbCommand(insertStatement, cnn);
                    insertCommand.Transaction = transaction;
                    //insertCommand.Parameters.Add("@id", OleDbType.Integer).Value = clientId;
                    insertCommand.Parameters.Add("@clientName", OleDbType.Char).Value = info.clientName;
                    insertCommand.ExecuteNonQuery();
                    insertCommand.CommandText = "Select @@Identity";
                    clientId = (int)insertCommand.ExecuteScalar();
                }

                if(info.stagesDate != null) {
                    foreach(int id in info.stagesDate.Keys) {
                        insertStatement = "INSERT INTO Stages "
                              + "(projectId, stageId, plannedStartDate, plannedEndDate, roundNumber)"
                              + "VALUES (@projectId, @stageId, @plannedStartDate, @plannedEndDate, @roundNumber)";
                        insertCommand = new OleDbCommand(insertStatement, cnn);
                        insertCommand.Transaction = transaction;
                        insertCommand.Parameters.Add("@projectId", OleDbType.Integer).Value = projectId;
                        insertCommand.Parameters.Add("@stageId", OleDbType.Integer).Value = id;
                        insertCommand.Parameters.Add("@plannedStartDate", OleDbType.Date).Value = info.stagesDate[id].Key;
                        insertCommand.Parameters.Add("@plannedEndDate", OleDbType.Date).Value = info.stagesDate[id].Value;
                        insertCommand.Parameters.Add("@roundNumber", OleDbType.Integer).Value = 1;
                        insertCommand.ExecuteNonQuery();
                    }
                }
                foreach(int player in info.participants) {
                    insertStatement = "INSERT INTO ProjectParticipants "
                        + "(projectId, participantId)"
                        + "VALUES (@projectId, @participantId)";
                    insertCommand = new OleDbCommand(insertStatement, cnn);
                    insertCommand.Transaction = transaction;
                    insertCommand.Parameters.Add("@projectId", OleDbType.Integer).Value = projectId;
                    insertCommand.Parameters.Add("@participantId", OleDbType.Integer).Value = player;
                    insertCommand.ExecuteNonQuery();
                }

                insertStatement = "INSERT INTO Projects "
                         + "(id, projectName, currentStageId, responsibleId, projectType, requestType, clientId, isClosed, pathToFolder, deadline, startDate) "
                         + "VALUES (@id, @projectName, @currentStageId, @responsibleId, @projectType, @requestType, @clientId, @isClosed, @pathToFolder, @deadline, @startDate)";

                insertCommand = new OleDbCommand(insertStatement, cnn);
                insertCommand.Transaction = transaction;
                insertCommand.Parameters.Add("@id", OleDbType.Integer).Value = projectId;
                insertCommand.Parameters.Add("@projectName", OleDbType.Char).Value = info.projectName;
                insertCommand.Parameters.Add("@currentStageId", OleDbType.Integer).Value = 1;                
                insertCommand.Parameters.Add("@responsibleId", OleDbType.Integer).Value = info.pilot;
                insertCommand.Parameters.Add("@projectType", OleDbType.Integer).Value = info.projectType;
                insertCommand.Parameters.Add("@requestType", OleDbType.Integer).Value = info.requestType;
                insertCommand.Parameters.Add("@clientId", OleDbType.Integer).Value = clientId;
                insertCommand.Parameters.Add("@isClosed", OleDbType.Integer).Value = 0;
                insertCommand.Parameters.Add("@pathToFolder", OleDbType.Char).Value = info.path;
                insertCommand.Parameters.Add("@deadline", OleDbType.Date).Value = info.deadline;
                insertCommand.Parameters.Add("@startDate", OleDbType.Date).Value = info.projectStartDate;
                insertCommand.ExecuteNonQuery();
                transaction.Commit();
                //cnn.Close();
            } catch(SystemException e) {
                transaction.Rollback();
                throw e;
            }
        }
    }
}
