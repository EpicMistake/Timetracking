using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectCreator {
    interface CreatorInterface {
        void setClient(string client);
        void setProjectName(string name);
        void setLetterDate(string date);
        void setStartDate(string date);
        void addFile(string file);
        void setProjectId(int id);
        void setParticipants(List<ControlMember> players);
        void setClients(List<ControlMember> clients);
        void setProjectTypes(List<ControlMember> projectTypes);
        void setRequestTypes(List<ControlMember> requestTypes);
        void setStages(List<ControlMember> stages);
        void reload();

        event EventHandler<EventArgs> OnCreate;
        event EventHandler<EventArgs> OnStart;
        event EventHandler<System.Windows.Forms.DragEventArgs> OnDrop;
        event EventHandler<System.Windows.Forms.DragEventArgs> OnDrag;
    }
}
