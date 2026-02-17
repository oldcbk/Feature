using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaHero : MonoBehaviour
{
    public class State
    {
        public string m_name;

        public State m_exitState;

        public class Transition
        {

            public State m_state;
        }
    }
}
