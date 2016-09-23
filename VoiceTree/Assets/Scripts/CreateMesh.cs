using UnityEngine;
using System.Collections;
using UnityEditor;

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
    /*----------------------------------------------------------- drawBranch */
    public void drawPrism()
    {
        //制御
        if (TopRadius < 0) TopRadius = 0;
        if (BottomRadius < 0) BottomRadius = 0;
        if (Divition < 3) Divition = 3;

        //宣言
        int num_triangles = ((cnt_True(onTopPolygon, onBottomPolygon) + 2) * this.Divition) * 3;
        int cnt_triangles;
        int[] newTriangles = new int[num_triangles];
        Vector3[] newVertices = new Vector3[(this.Divition + 1) * 2];
        Vector2[] newUV = new Vector2[(this.Divition + 1) * 2];
        mesh = new Mesh();


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
        for (int i = this.Divition + 1; i < newVertices.Length; i++)
        {
            newVertices[i] = new Vector3(
                Pivot.x + Mathf.Cos((float)(i - (this.Divition + 1)) / this.Divition * 2.0f * Mathf.PI) * BottomRadius,
                Pivot.y,
                Pivot.z + Mathf.Sin((float)(i - (this.Divition + 1)) / this.Divition * 2.0f * Mathf.PI) * BottomRadius
                );
        }
        newVertices[newVertices.Length - 1] = new Vector3(Pivot.x, Pivot.y, Pivot.z);


        /*------------------------------------------------------- 図形の描画 */
        //--- 側面.
        for (int i = 0; i < this.Divition; i++)
        {
            //--- UVの指定.

            //上部正多角形の一辺を辺として持つ三角形
            newTriangles[(6 * i) + 0] = i;
            newTriangles[(6 * i) + 1] = (i + 1) % Divition;
            newTriangles[(6 * i) + 2] = i + (Divition + 1);
            //下部正多角形の一辺を辺として持つ三角形
            newTriangles[(6 * i) + 3] = ((i + 1) % Divition) + (Divition + 1);
            newTriangles[(6 * i) + 4] = i + (Divition + 1);
            newTriangles[(6 * i) + 5] = (i + 1) % Divition;
        }
        cnt_triangles = Divition * 2;
        //--- 上円.
        if (onTopPolygon)
        {
            //UVの指定

            //三角形ごとの頂点指定
            for (int i = 0; i < this.Divition; i++)
            {
                newTriangles[(cnt_triangles * 3) + (3 * i) + 0] = this.Divition;
                newTriangles[(cnt_triangles * 3) + (3 * i) + 1] = (i + 1) % (this.Divition);
                newTriangles[(cnt_triangles * 3) + (3 * i) + 2] = (i + 0) % (this.Divition);
            }
            cnt_triangles += Divition;
        }
        //--- 下円.
        if (onBottomPolygon)
        {
            //UVの指定

            //三角形ごとの頂点指定
            for (int i = 0; i < this.Divition; i++)
            {
                newTriangles[(cnt_triangles * 3) + (3 * i) + 0] = newVertices.Length - 1;
                newTriangles[(cnt_triangles * 3) + (3 * i) + 1] = (i + 0) % (this.Divition) + (this.Divition + 1);
                newTriangles[(cnt_triangles * 3) + (3 * i) + 2] = (i + 1) % (this.Divition) + (this.Divition + 1);
            }
        }


        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshFilter>().sharedMesh.name = "Prism";
    }

    /*=======================================================================*/
    // Use this for initialization
    void Start()
    {
        //初期化
        Pivot = new Vector3(0, 0, 0);
        Height = 1.0f;
        TopRadius = 0.5f;
        BottomRadius = 1.0f;
        Divition = 6;
        onTopPolygon = false;
        onBottomPolygon = false;

        drawPrism();

        AssetDatabase.CreateAsset(mesh, "Assets/" + mesh.name + ".asset");
        AssetDatabase.SaveAssets();
    }
    /*=======================================================================*/
    // Update is called once per frame
    void Update()
    {
        drawPrism();
    }
}

