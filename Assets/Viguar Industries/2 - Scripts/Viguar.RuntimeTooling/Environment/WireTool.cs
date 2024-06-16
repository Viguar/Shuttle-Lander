using UnityEngine;
using UnityEditor;

namespace Viguar.RuntimeTooling.Environment.Wire
{
    [ExecuteInEditMode]
    public class WireTool : MonoBehaviour
    {

        [Range(0f, 1f)]
        [SerializeField]
        private float wireWidth = 0.1f;
        [SerializeField]
        private Material wireMaterial;
        [SerializeField]
        private bool loopWire;
        [Range(1, 15)]
        [SerializeField]
        private int subdivisions = 1;
        [Range(-3f, 3f)]
        [SerializeField]
        private float[] curvatureParameters;
        [SerializeField]
        private bool isAnimatedWire;

        private LineRenderer lineRenderer;
        private Transform[] positions;

        private void Update()
        {

#if UNITY_EDITOR

            if (!EditorApplication.isPlaying)
            {

                FindReferences();

                ConfigureWire();


                DrawWire();
            }


#endif

            if (isAnimatedWire)
            {
                DrawWire();
            }
        }

        void FixedUpdate()
        {

        }

        #region Wire

        private void FindReferences()
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }

            gameObject.name = "Cable";//Rename the Container
            int n = gameObject.transform.childCount;

            while (n < 2)
            {
                GameObject g = new GameObject();
                g.transform.SetParent(gameObject.transform);
                g.transform.position += new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                n++;
            }

            if (loopWire)
            {
                positions = new Transform[n + 1];
                for (int i = 0; i < n; i++)
                {
                    positions[i] = gameObject.transform.GetChild(i);
                }
                positions[n] = gameObject.transform.GetChild(0);
            }
            else
            {
                positions = new Transform[n];

                for (int i = 0; i < n; i++)
                {
                    positions[i] = gameObject.transform.GetChild(i);
                }
            }

            //Rename the Childs
            for (int i = 0; i < n; i++)
            {
                positions[i].name = "Wire Point " + (i + 1).ToString();
            }

            if (curvatureParameters == null)
            {
                curvatureParameters = new float[n];
            }
            else
            {
                int k = n - 1;
                if (loopWire)
                {
                    k = n;
                }

                if (curvatureParameters.Length != k)
                {
                    float[] aux = curvatureParameters;
                    curvatureParameters = new float[k];
                    int j = 0;
                    if (aux.Length > k)
                    {
                        j = k;
                    }
                    else
                    {
                        j = aux.Length;
                    }
                    for (int i = 0; i < j; i++)
                    {
                        curvatureParameters[i] = aux[i];
                    }
                }
            }

        }

        private void ConfigureWire()
        {
            lineRenderer.material = wireMaterial;
            lineRenderer.startWidth = wireWidth;
            lineRenderer.endWidth = wireWidth;

        }

        private void DrawWire()
        {
            int lineVertices = positions.Length;
            if (subdivisions > 1)
            {
                int n = positions.Length;

                lineRenderer.positionCount = subdivisions * (n - 1);

                int vertexIndex = 0;

                //Draw the hanging wire
                for (int k = 0; k < positions.Length - 1; k++)
                {
                    float distance = Vector3.Distance(positions[k].position, positions[k + 1].position);
                    float deltaX = distance / (subdivisions);

                    float y0 = positions[k].position.y;
                    float x1 = distance / 2;
                    float y1;
                    if (positions[k].position.y < positions[k + 1].position.y)
                    {
                        y1 = positions[k].position.y + curvatureParameters[k];
                    }
                    else
                    {
                        y1 = positions[k + 1].position.y + curvatureParameters[k];
                    }

                    float x2 = distance;
                    float y2 = positions[k + 1].position.y;

                    float b = (y2 - y0 - ((x2 * x2) * (y1 - y0)) / (x1 * x1)) / (x2 - (x2 * x2 / x1));
                    float a = (y1 - y0 - b * x1) / (x1 * x1);

                    float c = y0;//Always

                    for (int i = 0; i < subdivisions; i++)
                    {
                        //draw the curve
                        Vector3 pos = Vector3.Lerp(positions[k].position, positions[k + 1].position, deltaX * i / distance);
                        pos.y = a * ((deltaX * i) * (deltaX * i)) + b * deltaX * i + c;
                        lineRenderer.SetPosition(vertexIndex, pos);

                        vertexIndex++;
                    }
                }

                if (!loopWire)
                {
                    lineRenderer.positionCount = lineRenderer.positionCount + 1;
                    lineRenderer.SetPosition(vertexIndex, positions[positions.Length - 1].position);
                }
            }
            else
            {

                lineRenderer.positionCount = lineVertices;
                for (int i = 0; i < lineVertices; i++)
                {
                    lineRenderer.SetPosition(i, positions[i].position);
                }
            }
            lineRenderer.loop = loopWire;

        }

        public LineRenderer GetLineRenderer()
        {
            return lineRenderer;
        }

        #endregion
    }
}
