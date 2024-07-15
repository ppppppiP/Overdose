using System;
using System.Collections.Generic;

namespace BlackBoardSystem
{
    public interface IExpert
    {
        int GetInstance(Blackboard blackboard);
        void Execute(Blackboard blackboard);
    }
}
