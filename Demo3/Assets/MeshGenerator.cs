using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    #region 싱글톤
    public static MeshGenerator instance = null;
    private void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; //내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else
        {
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
        }
    }
    #endregion
    #region 엘립스변수
    float M = 1000;
    float p = 2;
    public float rounding = 1;
    int num_point = 15;
    int num_layer = 15;
    float scale = 0.1f;

    public string name;
    public float topA, topB, bottomA, bottomB,
        rX, rY,
        heightT, heightB,
        rotateAngle = 0,
        rotate,
        rotateValue = 0,
        centerX, centerY, centerZ;
    public Material[] material;
    private int block_num=0;
    
    #endregion
    #region 함수
    double deg2rad(double degree)
    {
        return degree * Math.PI / 180;
    }
    double sgn(double val)
    {
        if (val > 0.0) return 1.0;
        if (val < 0.0) return -1.0;
        return 0.0;
    }
    float CT(float t, float vertical_r)
    {
        float n = Math.Abs(vertical_r) + 1.0f;
        float ct = (vertical_r >= 0) ? (float)((Math.Pow(1.0f - Math.Pow((t), (n)), 1.0f / (n)) - 1.0f + (t))) :
            (float)-((Math.Pow(1.0f - Math.Pow((1.0f - (t)), (n)), 1.0f / (n)) - (t)));
        return ct;
    }
    int createMesh(ref int[] triangles)
    {
        int count = 0;
        // Mesh는 겉면을 시계방향으로 통일해줘야한다.

        // Top Mesh
        for (int i = 0; i < num_point - 2; i++)
        {
            triangles[count] = ((num_layer - 1) * num_point); count++;
            triangles[count] = ((num_layer - 1) * num_point) + (i + 2); count++;
            triangles[count] = ((num_layer - 1) * num_point) + (i + 1); count++;
        }
        /*
         Middle Mesh
         
         number of k in plane : numPoints.
         normal case :
         k₂----k₂+1
         |  \   |
         |   \  |
         k₁----k₁+1
         ◺ : k₁		k₂		k₁+1
         ◹ : k₂+1	k₁+1    k₂

         last one :
         k₂----k₂-(numPoints - 1)
         |  \   |
         |   \  |
         k₁----k₁-(numPoints - 1)
         ◺ : k₁		              k₂		          k₁-(numPoints - 1)
         ◹ : k₂-(numPoints - 1)   k₁-(numPoints - 1)  k₂		
        */
        int K1, K2;
        for (int i = 0; i < num_layer - 1; i++)
        {
            for (int j = 0; j < num_point - 1; j++)
            {
                K1 = ((i * num_point) + j);
                K2 = (((i + 1) * num_point) + j);

                triangles[count] = K1; count++;
                triangles[count] = K2; count++;
                triangles[count] = K1 + 1; count++;

                triangles[count] = K2 + 1; count++;
                triangles[count] = K1 + 1; count++;
                triangles[count] = K2; count++;
            }
            K1 = ((i * num_point) + (num_point - 1));
            K2 = (((i + 1) * num_point) + (num_point - 1));
            triangles[count] = K1; count++;
            triangles[count] = K2; count++;
            triangles[count] = K1 - (num_point - 1); count++;

            triangles[count] = K2 - (num_point - 1); count++;
            triangles[count] = K1 - (num_point - 1); count++;
            triangles[count] = K2; count++;

        }
        // Bottom Mesh
        for (int i = 0; i < num_point - 2; i++)
        {
            triangles[count] = 0; count++;
            triangles[count] = (i + 1); count++;
            triangles[count] = (i + 2); count++;
        }
        return count;
    }
    void createVertex(float top_a, float top_b, float bottom_a, float bottom_b, float n, float r_x, float r_y, float height_top, float height_bottom,
      Vector3[] position)
    {
        
        int height = num_layer - 1;
        int count = 0;
        float a, b;
        float delta_a = top_a - bottom_a, delta_b = top_b - bottom_b;
        rotateAngle = 0;
        for (int i = 0; i <= height; i++)
        {
            float t = (float)i / (float)height;
            if (i == 1)
            {
                t = 0.0001f;
            }
            if(i== height - 1)
            {
                t = 0.9999f;
            }
               
            if (delta_a >= 0)
            {
                a = (delta_a) * t + bottom_a + Math.Abs(delta_a) * CT(1.0f - t, r_x);
            }
            else
            {
                a = (delta_a) * t + bottom_a + Math.Abs(delta_a) * CT(t, r_x);
            }
            if (delta_b >= 0)
            {
                b = (delta_b) * t + bottom_b + Math.Abs(delta_b) * CT(1.0f - t, r_y);
            }
            else
            {
                b = (delta_b) * t + bottom_b + Math.Abs(delta_b) * CT(t, r_y);
            }

            for (int j = 0; j < num_point; j++)
            {
                float angle = (float)j / (float)num_point * 360.0f;
                float angleXY = (float)(angle * Math.PI / 180.0f);
                float NewX = (float)((Math.Pow(Math.Abs(Math.Cos(angleXY)), 2.0 / n) * a * sgn(Math.Cos(angleXY))) * scale);
                float NewY = ((height_top - height_bottom) * t) * scale;
                float NewZ = (float)((Math.Pow(Math.Abs(Math.Sin(angleXY)), 2.0 / n) * b * sgn(Math.Sin(angleXY))) * scale);
                RotateVertex(ref NewX, ref NewZ, rotateAngle);
                position[count] = new Vector3(NewX, NewY, NewZ);
                count++;
            }
            rotateAngle += rotateValue;
        }
        rotateAngle = 0;
    }
    public void RotateVertex(ref float x,ref float z,double degree)
    {
        double radian = deg2rad(degree);
        double x2 = Math.Cos(radian) * x - Math.Sin(radian) * z;
        double z2 = Math.Sin(radian) * x + Math.Cos(radian) * z;
        x = (float)x2;
        z = (float)z2;
        
    }
    public void deleteBlock()
    {
        GameObject sup = GameObject.Find(name);
        Destroy(sup);
    }
    public GameObject createBlock()
    {
        name = string.Format("B" + block_num);
        float n = (float)(M / ((M / 2 - 1) * Math.Pow(rounding, p) + 1.0f));
        int all_numpoints = (int)(num_point * num_layer);
        Vector3[] vertices = new Vector3[all_numpoints];
        int[] triangles = new int[(all_numpoints-2)*6];
        Vector3[] Normal = new Vector3[all_numpoints];

        GameObject sup = new GameObject(name);
        Mesh mesh = new Mesh();
        MeshFilter mf = sup.AddComponent<MeshFilter>();
        MeshRenderer mr = sup.AddComponent<MeshRenderer>();
        Material mt = new Material(Shader.Find("Standard"));
        createVertex(topA, topB, bottomA, bottomB, n, rX, rY,
              heightT, heightB, vertices);
        createMesh(ref triangles);
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.Optimize();

        mf.sharedMesh = mesh;
        mr.material = mt;
        //mr.material = material[9];

        sup.AddComponent<BoxCollider>();

        //if (CopyAndPaste.instance.top < File_Parser.instance.loaddata.MBlockStore[name].geometry.top.location[2] * scale)
        //{
        //    CopyAndPaste.instance.top = File_Parser.instance.loaddata.MBlockStore[name].geometry.top.location[2] * scale;
        //}
        //BlockStore.instance.blockstore.Add(sup);
        block_num++;
        return sup;
    }
    public GameObject modifyBlock()
    {
        float n = (float)(M / ((M / 2 - 1) * Math.Pow(rounding, p) + 1.0f));
        int all_numpoints = (int)(num_point * num_layer);
        Vector3[] vertices = new Vector3[all_numpoints];
        int[] triangles = new int[(all_numpoints - 2) * 6];
        Vector3[] Normal = new Vector3[all_numpoints];

        GameObject sup = new GameObject(name);
        Mesh mesh = new Mesh();
        MeshFilter mf = sup.AddComponent<MeshFilter>();
        MeshRenderer mr = sup.AddComponent<MeshRenderer>();
        Material mt = new Material(Shader.Find("Standard"));
        createVertex(topA, topB, bottomA, bottomB, n, rX, rY,
              heightT, heightB, vertices);
        createMesh(ref triangles);
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.Optimize();

        mf.sharedMesh = mesh;
        mr.material = mt;
        //mr.material = material[9];

        sup.AddComponent<BoxCollider>();

        //if (CopyAndPaste.instance.top < File_Parser.instance.loaddata.MBlockStore[name].geometry.top.location[2] * scale)
        //{
        //    CopyAndPaste.instance.top = File_Parser.instance.loaddata.MBlockStore[name].geometry.top.location[2] * scale;
        //}
        //BlockStore.instance.blockstore.Add(sup);
        return sup;
    }
    public GameObject previewBlock()
    {

        float n = (float)(M / ((M / 2 - 1) * Math.Pow(rounding, p) + 1.0f));
        int all_numpoints = (int)(num_point * num_layer);
        Vector3[] vertices = new Vector3[all_numpoints];
        int[] triangles = new int[(all_numpoints - 2) * 6];
        Vector3[] Normal = new Vector3[all_numpoints];

        GameObject sup = new GameObject("preview");
        Mesh mesh = new Mesh();
        MeshFilter mf = sup.AddComponent<MeshFilter>();
        MeshRenderer mr = sup.AddComponent<MeshRenderer>();
        Material mt = new Material(Shader.Find("Standard"));
        createVertex(topA, topB, bottomA, bottomB, n, rX, rY,
              heightT, heightB, vertices);
        createMesh(ref triangles);
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.Optimize();

        mf.sharedMesh = mesh;
        mr.material = mt;
        //mr.material = material[9];

        sup.AddComponent<BoxCollider>();

        //if (CopyAndPaste.instance.top < File_Parser.instance.loaddata.MBlockStore[name].geometry.top.location[2] * scale)
        //{
        //    CopyAndPaste.instance.top = File_Parser.instance.loaddata.MBlockStore[name].geometry.top.location[2] * scale;
        //}
        //BlockStore.instance.blockstore.Add(sup);
        return sup;
    }
    public void translateBlock(GameObject sup)
    {
        sup.transform.Translate(centerX * scale, centerY * scale, centerZ * scale);
    }
    public void rotateBlock(GameObject sup)
    {
        Vector3 angle = new Vector3(0, rotate, 0);
        sup.transform.eulerAngles = angle;
    }
    #endregion



}
