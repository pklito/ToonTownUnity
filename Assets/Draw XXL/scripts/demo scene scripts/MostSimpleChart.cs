namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Internal Not For Manual Creation/Most Simple Chart Example")]
    public class MostSimpleChart : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            DrawCharts.premadeChart0.AddValue(transform.position.y);
            DrawCharts.premadeChart0.Draw();
        }
    }
}


