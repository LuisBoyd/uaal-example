using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities.editor;

namespace BlackBoard.editor
{
    public class BlackboardKeyView: VisualElement
    {
        public new class UxmlFactory: UxmlFactory<BlackboardKeyView, InspectorView.UxmlTraits>{}

        private const string BlackBoardKeyViewUxml = "Assets/Scripts/BlackBoard/editor/Uxml";       
        private const string BlackBoardKeyViewUss = "Assets/Scripts/BlackBoard/editor/Uxml";

        private Button addKeyBtn;
        private Button removeKeyBtn;
        private ListView KeyList;
        public Blackboard blackboard;

        public KeyValuePair<string, Type> currentSelected;
        

        public BlackboardKeyView()
        {
            VisualElement rootbtnView = new VisualElement();
            rootbtnView.style.flexDirection = FlexDirection.Row;
            rootbtnView.name = "RootBtnView";
            Add(rootbtnView);
            
            addKeyBtn = new Button();
            addKeyBtn.clickable.clicked += OnAddKeyBtn_Clicked;
            addKeyBtn.name = "KeyViewButton";
            addKeyBtn.text = "+";
            rootbtnView.Add(addKeyBtn);
            //Add button for menu
            
            removeKeyBtn = new Button();
            removeKeyBtn.clickable.clicked += OnRemoveKeyBtn_Clicked;
            removeKeyBtn.name = "KeyViewButton";
            removeKeyBtn.text = "-";
            rootbtnView.Add(removeKeyBtn);

            KeyList = new ListView();
            KeyList.name = "KeyListView";
            Add(KeyList);
        }

        ~BlackboardKeyView()
        {
            addKeyBtn.clickable.clicked -= OnAddKeyBtn_Clicked;
        }

        private void OnAddKeyBtn_Clicked()
        {
            // create the menu and add items to it
            GenericDropdownMenu menu = new GenericDropdownMenu();
 
            menu.AddItem("String", false, () => CreateStringKeyView(""));
            menu.AddItem("Float", false, () => CreateFloatKeyView(0.0f));
            menu.AddItem("Int", false, () => CreateIntKeyView(0));
            menu.AddItem("Bool", false, () => CreateBoolKeyView(false));
            menu.AddItem("Vector3", false, () => CreateVector3KeyView(Vector3.zero));
            menu.AddItem("GameObject", false, () => CreateGameObjectKeyView(default));
            menu.AddItem("Object", false, () => CreateObjectKeyView(default));
 
            // display the menu
            menu.DropDown(addKeyBtn.worldBound, addKeyBtn);
        }

        private void OnRemoveKeyBtn_Clicked()
        {
            if(currentSelected.Key == null)
                return;
            if(currentSelected.Value == typeof(int))
                blackboard.RemoveInt(currentSelected.Key);
            else if(currentSelected.Value == typeof(float))
                blackboard.RemoveFloat(currentSelected.Key);
            else if(currentSelected.Value == typeof(bool))
                blackboard.RemoveBool(currentSelected.Key);
            else if(currentSelected.Value == typeof(string))
                blackboard.RemoveString(currentSelected.Key);
            else if(currentSelected.Value == typeof(GameObject))
                blackboard.RemoveGameobject(currentSelected.Key);
            else if(currentSelected.Value == typeof(Vector3))
                blackboard.RemoveVector3(currentSelected.Key);
            else if(currentSelected.Value == typeof(object))
                blackboard.RemoveGeneric(currentSelected.Key);
            
            PoulateKeyView(blackboard);
        }
        
        private void CreateIntKeyView(int defaultValue)
        {
            blackboard.SetInt("New Int", defaultValue);
            PoulateKeyView(blackboard);
            EditorUtility.SetDirty(blackboard);
            AssetDatabase.SaveAssets();
        }
        private void CreateFloatKeyView(float defaultValue)
        {
            blackboard.SetFloat("New Float", defaultValue);
            PoulateKeyView(blackboard);
            EditorUtility.SetDirty(blackboard);
            AssetDatabase.SaveAssets();
        }
        private void CreateBoolKeyView(bool defaultValue)
        {
            blackboard.SetBool("New Bool", defaultValue);
            PoulateKeyView(blackboard);
            EditorUtility.SetDirty(blackboard);
            AssetDatabase.SaveAssets();
        }
        private void CreateStringKeyView(string defaultValue)
        {
            blackboard.SetString("New String", defaultValue);
            PoulateKeyView(blackboard);
            EditorUtility.SetDirty(blackboard);
            AssetDatabase.SaveAssets();
        }
        private void CreateGameObjectKeyView(GameObject defaultValue)
        {
            blackboard.SetGameobject("New Gameobject", defaultValue);
            PoulateKeyView(blackboard);
            EditorUtility.SetDirty(blackboard);
            AssetDatabase.SaveAssets();
        }
        private void CreateObjectKeyView(object defaultValue)
        {
            blackboard.SetGeneric("New Object", defaultValue);
            PoulateKeyView(blackboard);
            EditorUtility.SetDirty(blackboard);
            AssetDatabase.SaveAssets();
        }
        private void CreateVector3KeyView(Vector3 defaultValue)
        {
            blackboard.SetGeneric("New Object", defaultValue);
            PoulateKeyView(blackboard);
            EditorUtility.SetDirty(blackboard);
            AssetDatabase.SaveAssets();
        }
        
       

        public void PoulateKeyView(Blackboard blackboard)
        {
            this.blackboard = blackboard;
            int itemCount = blackboard.fieldName.Count;
            var items = new List<KeyValuePair<string, Type>>(itemCount);
            foreach (var fieldNameValue in blackboard.fieldName.Values)
            {
                items.Add(fieldNameValue);
            }
            KeyList.itemsSource = items;
            Func<VisualElement> makeItem = () => new BlackBoardKey();
            Action<VisualElement, int> bindItem = (v, i) =>
            {
                BlackBoardKey key = v as BlackBoardKey;
                if(key == null)
                    return;
                key.SetKey(items[i].Key,items[i].Value);
            };

            KeyList.makeItem = makeItem;
            KeyList.bindItem = bindItem;
            KeyList.selectionType = SelectionType.Single;

            KeyList.itemsChosen -= PopulateBlackBoardInspector;
            KeyList.itemsChosen += PopulateBlackBoardInspector;
            KeyList.selectionChanged -= PopulateBlackBoardInspector;
            KeyList.selectionChanged += PopulateBlackBoardInspector;
        }

        public void PopulateBlackBoardInspector(IEnumerable<object> blackboardKey)
        {
            currentSelected = (KeyValuePair<string, Type>)blackboardKey.First();
        }
        
    }
}