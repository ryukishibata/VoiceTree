using UnityEngine;
using System.Collections;

public class CreateMesh : MonoBehaviour
{

    public Vector3 Pivot = new Vector3(0, 0, 0);
    public float Height;
    public float TopRadius;
    public float BottomRadius;
    public int Divition;

    private Mesh mesh;

    // Use this for initialization
    void Start()
    {
        //初期化

        mesh = new Mesh();
        Vector3[] newVertices = new Vector3[(this.Divition + 1) * 2];
        Vector2[] newUV = new Vector2[(this.Divition + 1) * 2];
        int[] newTriangles = new int[(this.Divition * 2) * 3];

        // 上円頂点座標の指定.
        newVertices[0] = new Vector3(0, 0, 0);
        for(int i = 1; i <= this.Divition + 1; i++)
        {
            newVertices[i] = new Vector3(
                Mathf.Cos((float)(i - 1) / this.Divition * 2.0f * Mathf.PI) * TopRadius,
                0,
                Mathf.Sin((float)(i - 1) / this.Divition * 2.0f * Mathf.PI) * TopRadius
                );
        }
        // 下円頂点座標の指定.
        for (int i = 0; i < this.Divition + 1; i++)
        {
            newVertices[i] = new Vector3(
                Mathf.Cos((float)i / this.Divition * 2.0f * Mathf.PI) * TopRadius,
                0.0f,
                Mathf.Sin((float)i / this.Divition * 2.0f * Mathf.PI) * TopRadius
                );
        }
        newVertices[this.Divition] = new Vector3(0, 0, 0);




        // UVの指定 (頂点数と同じ数を指定すること).
        //newUV[0] = new Vector2(0.0f, 0.0f);
        //newUV[1] = new Vector2(0.0f, 1.0f);
        //newUV[2] = new Vector2(1.0f, 1.0f);
        //newUV[3] = new Vector2(1.0f, 0.0f);
        //newUV[4] = new Vector2(0.0f, 0.0f);


        /*---------------------- 三角形ごとの頂点インデックスを指定. */
        // 上円の頂点インデックスを指定
        for (int i = 0; i < this.Divition; i++){
            newTriangles[(3*i)+0] = 0;
            newTriangles[(3*i)+1] = (i + 0) % (this.Divition) + 1;
            newTriangles[(3*i)+2] = (i + 1) % (this.Divition) + 1;
        }
        // 下円の頂点インデックスを指定
        for (int i = 0; i < this.Divition; i++)
        {
            newTriangles[(3 * i)] = i % this.Divition;
            newTriangles[(3 * i) + 1] = (i + 1) % this.Divition;
            newTriangles[(3 * i) + 2] = this.Divition;
        }


        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
    }

    // Update is called once per frame
    void Update()
    {

    }
}

