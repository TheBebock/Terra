using Terra.Extensions;
using Terra.Itemization;
using Terra.Itemization.Abstracts.Definitions;
using Terra.Itemization.Items.Definitions;
using UnityEditor;
using UnityEngine;

namespace Terra.Editor
{

    [CustomEditor(typeof(ItemsDatabase))]
    public class ItemsDatabaseEditor : UnityEditor.Editor
    {
        private ItemsDatabase itemsDatabase;

        // This variable will hold the input value for the new item name
        private string itemName = "";

        private ItemType selectedItemType = ItemType.None;

        // OnEnable is called when the editor is loaded
        private void OnEnable()
        {
            // Get the target object, which is an instance of ItemsDatabase
            itemsDatabase = (ItemsDatabase)target;
        }

        // This is called when the inspector is drawn
        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);

            // Input field for the item name
            GUILayout.Label("Enter Item Name");
            itemName = EditorGUILayout.TextField(itemName, GUILayout.Width(300));

            itemName = itemName.RemoveWhiteSpace();

            selectedItemType = (ItemType)EditorGUILayout.EnumPopup(selectedItemType, GUILayout.Width(150));
            GUILayout.BeginHorizontal();

            // Add item button
            if (GUILayout.Button("Add Item", GUILayout.Width(100), GUILayout.Height(50)))
            {

                if (!string.IsNullOrEmpty(itemName))
                {
                    itemsDatabase.AddItem(itemName, selectedItemType);
                    itemName = "";

                    // Mark the object as dirty to ensure the changes are saved and visible
                    EditorUtility.SetDirty(itemsDatabase);
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
                if (!string.IsNullOrEmpty(itemName))
                {
                    itemsDatabase.RemoveItem(itemName);
                    itemName = "";

                    // Mark the object as dirty to ensure the changes are saved and visible
                    EditorUtility.SetDirty(itemsDatabase);
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