using UnityEngine;

public class SquashControl : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    Material material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        material = meshRenderer.material;
    }

    public void startSquash(float amount)
    {
        material.SetFloat("_SquashAmount", amount);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
