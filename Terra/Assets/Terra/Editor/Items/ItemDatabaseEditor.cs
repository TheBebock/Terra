using Terra.Extensions;
using Terra.Itemization;
using Terra.Itemization.Abstracts.Definitions;
using UnityEditor;
using UnityEngine;

namespace Terra.Editor.Items
{

    [CustomEditor(typeof(ItemsDatabase))]
    public class ItemsDatabaseEditor : UnityEditor.Editor
    {
        private ItemsDatabase _itemsDatabase;

        // This variable will hold the input value for the new item name
        private string _itemName = "";

        private ItemType _selectedItemType = ItemType.None;

        // OnEnable is called when the editor is loaded
        private void OnEnable()
        {
            // Get the target object, which is an instance of ItemsDatabase
            _itemsDatabase = (ItemsDatabase)target;
        }

        // This is called when the inspector is drawn
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);

            // Input field for the item name
            GUILayout.Label("Enter Item Name");
            _itemName = EditorGUILayout.TextField(_itemName, GUILayout.Width(300));

            _itemName = _itemName.RemoveWhiteSpace();

            _selectedItemType = (ItemType)EditorGUILayout.EnumPopup(_selectedItemType, GUILayout.Width(150));
            GUILayout.BeginHorizontal();

            // Add item button
            if (GUILayout.Button("Add Item", GUILayout.Width(100), GUILayout.Height(50)))
            {

                if (!string.IsNullOrEmpty(_itemName))
                {
                    _itemsDatabase.AddItem(_itemName, _selectedItemType);
                    _itemName = "";

                    // Mark the object as dirty to ensure the changes are saved and visible
                    EditorUtility.SetDirty(_itemsDatabase);
                }
                else
                {
                    Debug.LogWarning("Item name cannot be empty!");
                }
            }

            // Space between buttons
            GUILayout.Space(100);

            // Remove item button
            if (GUILayout.Button("Remove Item", GUILayout.Width(100), GUILayout.Height(50)))
            {
                if (!string.IsNullOrEmpty(_itemName))
                {
                    _itemsDatabase.RemoveItem(_itemName);
                    _itemName = "";

                    // Mark the object as dirty to ensure the changes are saved and visible
                    EditorUtility.SetDirty(_itemsDatabase);
                }
                else
                {
                    Debug.LogWarning("Item name cannot be empty!");
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            // Display the default fields
            base.OnInspectorGUI();

            // Force a repaint to update the inspector
            Repaint();
        }
    }
}