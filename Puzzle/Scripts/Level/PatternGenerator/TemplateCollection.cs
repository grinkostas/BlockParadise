using System.Collections.Generic;
using GameCore.Puzzle.Scripts.Field.Patterns;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Level
{
    [CreateAssetMenu(menuName = "Collections/Templates")]
    public class TemplateCollection : ScriptableObject
    {
        public List<TemplatePattern> templates = new();
    }
}