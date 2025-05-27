using UnityEngine;

namespace Terra.Extensions
{
    public static class  GameObjectExtension
    {
        public static void SetLayer(this GameObject gameObject, LayerMask layer, bool includeChildren = false)
        {
            if ((layer.value & (layer.value - 1)) != 0)
            {
                Debug.LogError($"LayerMask contains multiple layers. Only one layer can be assigned to a GameObject {gameObject.name}.");
                return;
            }
            
            int layerIndex = Mathf.RoundToInt(Mathf.Log(layer.value, 2));
            gameObject.layer = layerIndex;


            if (includeChildren)
            {
                Transform[] children = gameObject.GetComponentsInChildren<Transform>();
                for (int i = 0; i < children.Length; i++)
                {
                    children[i].gameObject.layer = layerIndex;
                }
            }
        }
    }
}
