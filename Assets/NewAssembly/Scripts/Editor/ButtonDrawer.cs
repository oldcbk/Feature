using UnityEditor;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ButtonAttribute))]
public class ButtonDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var btn = new Button()
        {
            text = (attribute as ButtonAttribute).MethodName
        };
        var targetObject = property.serializedObject.targetObject;
        var methodInfo = targetObject.GetType().GetMethod(btn.text);
        btn.clicked += () => methodInfo.Invoke(targetObject, null);
        return btn;
    }
}
