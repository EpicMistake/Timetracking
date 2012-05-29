using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectCreator {
    class CreatorEvent : EventArgs {
        private ProjectInfo _data;
        public CreatorEvent(ProjectInfo data) {
            _data = data;
        }
        public ProjectInfo data {
            get { return _data; }
        }
    }
}
