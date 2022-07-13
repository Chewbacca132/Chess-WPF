using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class GameOverEventArgs : EventArgs
    {
        public State state;
        public GameOverEventArgs(State state)
        {
            this.state = state;
        }
    }
}
