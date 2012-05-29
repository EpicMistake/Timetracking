using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace ProjectCreator {
    
    class CreatorControl {
        CreatorInterface _view;
        CreatorModel _model;
        public CreatorControl(CreatorInterface view) {
            _view = view;
            _view.OnCreate += new EventHandler<EventArgs>(OnCreate);
            _view.OnDrop += new EventHandler<DragEventArgs>(OnDrop);
            _view.OnDrag += new EventHandler<DragEventArgs>(OnDrag);
            _view.OnStart += new EventHandler<EventArgs>(OnStart);
            //_model = new CreatorModel(@"D:\_documents\_projects\ProjectCreator\Projects");
            _model = new CreatorModel(@"O:\DCM\3_Transport_studies\0_PROJECTS");
        }
        void OnStart(object sender, EventArgs e) {
            if(_model.open()) {
                _view.setClients(_model.getClients());
                _view.setProjectTypes(_model.getProjectTypes());
                _view.setRequestTypes(_model.getRequestTypes());
                _view.setParticipants(_model.getParticipants());
                _view.setStages(_model.getStages());
                _model.close();
            }            
        }
        public void OnCreate(object sender, EventArgs e) {
            CreatorEvent cEvent = (CreatorEvent)e;
            try {
                if(_model.open()) {
                    _model.create(cEvent.data);
                    _model.close();
                }
            } catch(SystemException ex) {

            } finally {
                _view.reload();
            }
        }
        public void OnDrop(object sender, DragEventArgs e) {
            if(_model.open()) {
                _model.getActiveLetter();
                _view.setProjectId(_model.getProjectId());
                _view.setLetterDate(_model.getLetterDate());
                _view.setStartDate(_model.getStartDate());
                _view.setProjectName(_model.getProjectName());
                _model.close();
            }
        }
        public void OnDrag(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.All;
        }
    }
}
