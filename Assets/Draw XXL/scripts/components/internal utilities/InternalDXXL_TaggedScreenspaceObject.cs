namespace DrawXXL
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct InternalDXXL_TaggedScreenspaceObject
    {
        [SerializeField] public GameObject previousGameobject;
        [SerializeField] public GameObject gameobject;
        [SerializeField] public Color color;
        [SerializeField] public string text;

        public void TryUseSeededColorFromGameobjectID()
        {
            if (gameobject != null)
            {
                if (previousGameobject == null)
                {
                    color = SeededColorGenerator.ColorOfGameobjectID(gameobject);
                }
                else
                {
                    if (gameobject != previousGameobject)
                    {
                        color = SeededColorGenerator.ColorOfGameobjectID(gameobject);
                    }
                }
            }
            previousGameobject = gameobject;
        }

    }
}
