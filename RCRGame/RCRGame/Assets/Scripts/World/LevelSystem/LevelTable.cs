using System;
using System.Collections.Generic;
using System.Linq;
using RCRCoreLib.Core;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace World.LevelSystem
{
    public class LevelTable
    {
        [TableList(IsReadOnly = true, AlwaysExpanded = false), ShowInInspector]
        private readonly List<LevelWrapper> allLevels;

        public LevelInfo this[int index]
        {
            get { return this.allLevels[index].Level; }
        }

        public LevelTable(IEnumerable<LevelInfo> levels)
        {
            this.allLevels =
                levels.Select(x => new LevelWrapper(x)).ToList();
        }

        private void refreshTable(IEnumerable<LevelInfo> levels)
        {
            allLevels.Clear();
            foreach (LevelInfo levelInfo in levels)
                allLevels.Add(new LevelWrapper(levelInfo));
        }
        
        [HorizontalGroup("BinaryRow")]
        [Button("Serialize", ButtonSizes.Medium)]
        public void SerializeConfig()
        {
            ObjectSerializerCreator.ShowDialog("Assets/DataObjects/Levels", allLevels.Select(x => x.Level).ToList());
            GUIUtility.ExitGUI();
        }

        [HorizontalGroup("BinaryRow")]
        [Button("Deserialize", ButtonSizes.Medium)]
        public void DeserializeConfig()
        {
            List<LevelInfo> gotlevel = ObjectDeserializerCreator.ShowDialog<List<LevelInfo>>("Assets/DataObjects/Levels");
            if (gotlevel != null)
            {
                refreshTable(gotlevel);
            }
            GUIUtility.ExitGUI();
        }

        private class LevelWrapper
        {
            private LevelInfo level;
            
            public LevelInfo Level
            {
                get { return this.level; }
            }
            
            public LevelWrapper(LevelInfo level)
            {
                this.level = level;
            }

            [TableColumnWidth(50, true)]
            [ShowInInspector]
            [OdinSerialize]
            public string Name
            {
                get { return this.Level.Name; }
                set
                {
                    this.level.Name = value;
                    EditorUtility.SetDirty(this.Level);
                }
            }

            [TableColumnWidth(50, true)] [ShowInInspector]
            [OdinSerialize]
            public int levelId
            {
                get { return this.level.LevelID; }
                set
                {
                    this.level.LevelID = value;
                    EditorUtility.SetDirty(this.level);
                }
                
            }
            
            [TableColumnWidth(100, true)] [ShowInInspector]
            [OdinSerialize]
            public int ExpirenceToLevel
            {
                get { return this.level.ExpirenceToLevelUp; }
                set
                {
                    this.level.ExpirenceToLevelUp = value;
                    EditorUtility.SetDirty(this.level);
                }
                
            }
        }
    }
}