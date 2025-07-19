namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Internal Not For Manual Creation/Multiple Lines Chart Example")]
    public class MultipleLinesChart : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            DrawCharts.premadeChart0.AddValue(transform.position.y, "Position");
            DrawCharts.premadeChart0.AddValue(GetComponent<Rigidbody>().linearVelocity.y, "Velocity");
            DrawCharts.premadeChart0.AddValue(Time.deltaTime, "Delta Time");

            DrawCharts.premadeChart0.Draw();
        }
    }
}


