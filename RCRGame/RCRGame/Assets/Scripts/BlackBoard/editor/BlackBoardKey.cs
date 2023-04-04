using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace BlackBoard.editor
{
    public class BlackBoardKey : VisualElement
    {
        //Load the blackBoard key field.
        public Type keyType;
        public string fieldName;

        private const string backGroundUssClass = "background-fieldname";
        private const string TypeIconUssClass = "TypeIcon";
        private const string FieldNameUssClass = "FieldName";

        private TextElement fieldname;
        private VisualElement Icon;
        private VisualElement BackGround;

        public BlackBoardKey()
        {
            //add visual element
             BackGround = new VisualElement();
            BackGround.name = backGroundUssClass;
            Add(BackGround);

             Icon = new VisualElement();
            Icon.name = TypeIconUssClass;
            BackGround.Add(Icon);

             fieldname = new TextElement();
            fieldname.name = FieldNameUssClass;
            BackGround.Add(fieldname);

        }

        private void removeUssClasses()
        {
            RemoveFromClassList("int");
            RemoveFromClassList("float");
            RemoveFromClassList("bool");
            RemoveFromClassList("string");
            RemoveFromClassList("GameObject");
            RemoveFromClassList("Vector3");
            RemoveFromClassList("object");
        }

        public void SetKey(string fieldname,System.Type type)
        {
            removeUssClasses();
            this.fieldname.text = fieldname;
            
            if(type == typeof(int))
                AddToClassList("int");
            else if(type == typeof(float))
                AddToClassList("float");
            else if(type == typeof(bool))
                AddToClassList("bool");
            else if(type == typeof(string))
                AddToClassList("string");
            else if(type == typeof(GameObject))
                AddToClassList("GameObject");
            else if(type == typeof(Vector3))
                AddToClassList("Vector3");
            else if(type == typeof(object))
                AddToClassList("object");
            
            Debug.Log("");
        }
    }
}