using UnityEngine;

namespace Classes.Elements
{
    [CreateAssetMenu(fileName = "New Element", menuName = "Elements/Element", order = 0)]
    public class Element : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private Color color;
        [SerializeField] private string description;

        public Sprite Icon => icon;
        public Color Color => color;
        public string Description => description;
    }
}