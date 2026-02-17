using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ButtonAttribute : PropertyAttribute
{
    public string MethodName
    {
        get => m_methodName;
        private set { m_methodName = value; }
    }

    string m_methodName;
    public ButtonAttribute(string methodName = "Invoke")
    {
        m_methodName = methodName;
    }
}
