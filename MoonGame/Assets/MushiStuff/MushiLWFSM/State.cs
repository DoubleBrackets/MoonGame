using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MushiLWFSM
{
    public interface State
    {
        public void Update();
        public void FixedUpdate();
        public void EnterState();
        public void ExitState();
    }
}

