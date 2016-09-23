using UnityEngine;
using System.Collections;

public class CreateMesh : MonoBehaviour
{

    public Vector3 Pivot;
    public float Height;
    public float TopRadius;
    public float BottomRadius;
    public int Divition;
    public bool onTopPolygon;
    public bool onBottomPolygon;


    private Mesh mesh;

    /*------------------------------------------------------------- cnt_True */
    int cnt_True(bool a, bool b)
    {
        if (a && b) return 2;
        else if (a || b) return 1;
        else return 0;
    }
    /*=======================================================================*/
    // Use this for initialization
    void Start()
    {
        //初期化

        mesh = new Mesh();
        Vector3[] newVertices = new Vector3[(this.Divition + 1) * 2];
        Vector2[] newUV = new Vector2[(this.Divition + 1) * 2];
        int num_triangles = ((cnt_True(onTopPolygon, onBottomPolygon) + 2) * 4) * 3;
        Debug.Log(num_triangles);
        int[] newTriangles = new int[num_triangles];


        /*------------------------------------------------ 各頂点座標の指定 */
        //--- 上円.
        for (int i = 0; i < this.Divition; i++)
        {
            newVertices[i] = new Vector3(
                Pivot.x + Mathf.Cos((float)(i) / this.Divition * 2.0f * Mathf.PI) * TopRadius,
                Pivot.y + Height,
                Pivot.z + Mathf.Sin((float)(i) / this.Divition * 2.0f * Mathf.PI) * TopRadius
                );
        }
        newVertices[this.Divition] = new Vector3(Pivot.x, Pivot.y + Height, Pivot.z);

        //--- 下円.
        for (int i = this.Divition+1; i < newVertices.Length; i++)
        {
            newVertices[i] = new Vector3(
                Pivot.x + Mathf.Cos((float)(i - (this.Divition + 1)) / this.Divition * 2.0f * Mathf.PI) * BottomRadius,
                Pivot.y,
                Pivot.z + Mathf.Sin((float)(i - (this.Divition + 1)) / this.Divition * 2.0f * Mathf.PI) * BottomRadius
                );
        }
        newVertices[newVertices.Length - 1] = new Vector3(Pivot.x, Pivot.y, Pivot.z);
        Debug.Log(newVertices.Length);

        for (int i = 0; i< newVertices.Length; i++)
        {
            Debug.Log(i + ":" + newVertices[i]);
        }

        /*------------------------------------------------------- 図形の描画 */
        //--- 側面.
        for(int i = 0; i < this.Divition; i++)
        {
                newTriangles[(6 * i) + 0] = i;
                newTriangles[(6 * i) + 1] = (i+1) % Divition;
                newTriangles[(6 * i) + 2] = i + (Divition + 1);


                newTriangles[(6 * i) + 3] = (i + 1) % Divition + (Divition + 1);
                newTriangles[(6 * i) + 4] = i + (Divition + 1);
                newTriangles[(6 * i) + 5] = (i + 1) % Divition;
        }
        ////--- 上円.
        //if (onTopPolygon)
        //{
        //    /*----------------------------------------------------- UVの指定 */
        //
        //    /*--------------------------- 三角形ごとの頂点インデックスの指定 */
        //    for (int i = 0; i < this.Divition; i++)
        //    {
        //        newTriangles[(3 * i) + 0] = this.Divition;
        //        newTriangles[(3 * i) + 1] = (i + 1) % (this.Divition);
        //        newTriangles[(3 * i) + 2] = (i + 0) % (this.Divition);
        //    }
        //}
        ////--- 下円.
        //if (onBottomPolygon)
        //{
        //    /*----------------------------------------------------- UVの指定 */
        //
        //    /*--------------------------- 三角形ごとの頂点インデックスの指定 */
        //    for (int i = this.Divition; i < this.Divition * 2; i++)
        //    {
        //        newTriangles[(3 * i) + 0] = newVertices.Length - 1;
        //        newTriangles[(3 * i) + 1] = (i + 0) % (this.Divition) + (this.Divition + 1);
        //        newTriangles[(3 * i) + 2] = (i + 1) % (this.Divition) + (this.Divition + 1);
        //    }
        //}


        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update()
    {

    }
}

