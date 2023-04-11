using UnityEngine;

namespace Item
{
    [CreateAssetMenu(menuName = "Scriptable object/Item2")]
    public class ItemData : ScriptableObject
    {
        [Header("Only UI")] 
        public bool stackable = true;
        public uint nbItemByStack;

        [Header("Both")] 
        public string name;
        public string description;
        public Sprite image;
        public GameObject prefab;
    }
}
