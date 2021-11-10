using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockData : MonoBehaviour
{
    #region 싱글톤
    public static BlockData instance = null;
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
    //블록의 이름을 저장하는 List
    public List<string> MBlockRank = new List<string>();
    //블록의 데이터를 저장하는 Dictionary
    public Dictionary<string, Block> MBlockStore = new Dictionary<string, Block>();
    public class Block
    {
        public float topA, topB, bottomA, bottomB;
        public float rX, rY;
        public float heightT, heightB;
        public float rounding;
        public int material;
        public float centerX, centerY, centerZ;
        public float rotate;
    }

    public void deleteBlockList(string name)
    {
        MBlockStore.Remove(name);
        for (int i = 0; i < MBlockRank.Count; i++)
        {
            if (MBlockRank[i] == name)
            {
                MBlockRank.RemoveAt(i);
            }
        }
    }
    public void addBlockList(string name)
    {
        MBlockStore.Add(name, new Block()
        {
            material = 30,
            topA = MeshGenerator.instance.topA,
            topB = MeshGenerator.instance.topB,
            bottomA = MeshGenerator.instance.bottomA,
            bottomB = MeshGenerator.instance.bottomB,
            rX = MeshGenerator.instance.rX,
            rY = MeshGenerator.instance.rY,
            heightT = MeshGenerator.instance.heightT,
            heightB = MeshGenerator.instance.heightB,
            rounding = MeshGenerator.instance.rounding,
            centerX = MeshGenerator.instance.centerX,
            centerY = MeshGenerator.instance.centerY,
            centerZ = MeshGenerator.instance.centerZ,
            rotate = MeshGenerator.instance.rotate
        });
        MBlockRank.Add(name);
    }
    public void updateBlockList(string name)
    {
        MBlockStore[name].topA = MeshGenerator.instance.topA;
        MBlockStore[name].topB = MeshGenerator.instance.topB;

        MBlockStore[name].bottomA = MeshGenerator.instance.bottomA;
        MBlockStore[name].bottomB = MeshGenerator.instance.bottomB;

        MBlockStore[name].rX = MeshGenerator.instance.rX;
        MBlockStore[name].rY = MeshGenerator.instance.rY;

        MBlockStore[name].heightT = MeshGenerator.instance.heightT;
        MBlockStore[name].heightB = MeshGenerator.instance.heightB;

        MBlockStore[name].rounding = MeshGenerator.instance.rounding;
        MBlockStore[name].centerX = MeshGenerator.instance.centerX;
        MBlockStore[name].centerY = MeshGenerator.instance.centerY;
        MBlockStore[name].centerZ = MeshGenerator.instance.centerZ;
        MBlockStore[name].rotate = MeshGenerator.instance.rotate;

    }

}

