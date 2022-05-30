using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TetrisManager : MonoBehaviour
{
    #region Singleton Initilization

    public static TetrisManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    #endregion

    private GameObject geometryT, geometryI, geometryL, geometryJ,geometryO;
    private Material red, yellow, blue, grey;
    private IEnumerator coroutine;
    public List<GameObject> otherCubes = new List<GameObject>();
    public GameObject nextGeometry;
    public bool gameOver = false;

    void Start()
    {
        InitializeGeometry();
        LoadMaterials();
        StartCoroutine(SpawnGeometry(2));
    }

    void InitializeGeometry()
    {
        geometryT = Resources.Load("Prefabs/TetrisGeometry/Geometry_T") as GameObject;
        geometryJ = Resources.Load("Prefabs/TetrisGeometry/Geometry_J") as GameObject;
        geometryL = Resources.Load("Prefabs/TetrisGeometry/Geometry_L") as GameObject;
        geometryI = Resources.Load("Prefabs/TetrisGeometry/Geometry_I") as GameObject;
        geometryO = Resources.Load("Prefabs/TetrisGeometry/Geometry_O") as GameObject;

    }

    void LoadMaterials()
    {
        red = Resources.Load("Materials/RedOfBlood") as Material;
        yellow = Resources.Load("Materials/YellowOfThunder") as Material;
        blue = Resources.Load("Materials/BlueOfMisery") as Material;
        grey = Resources.Load("Materials/GreyOfDeath") as Material;
    }

    private GameObject GetRandomGeomtry()
    {
        int num = Random.Range(1, 6);
        if (num == 1)
            return geometryT;
        if (num == 2)
            return geometryI;
        if (num == 3)
            return geometryL;
        if (num == 4)
            return geometryJ;
        if (num == 5)
            return geometryO;

        return geometryI;
    }

    private Material GetRandomMaterial()
    {
        int num = Random.Range(1, 4);
        if (num == 1)
            return red;
        if (num == 2)
            return yellow;
        if (num == 3)
            return blue;

        return red;
    }

    private void PaintCubes(GameObject geometry)
    {
        Material m = GetRandomMaterial();
        foreach (Transform index in geometry.transform)
        {
            index.gameObject.GetComponent<MeshRenderer>().material = m;
        }
    }

    //Spawns random geometry every T seconds
    public IEnumerator SpawnGeometry(int t = 0)
    {
        if (!gameOver)
        {

            yield return new WaitForSeconds(t);
            DestroyAndPushDown();
            GameObject geom = new GameObject();
            PlayerAnalytics.Instance.AddShapesCreated();
            if (nextGeometry == null)
            {
                nextGeometry = Instantiate(GetRandomGeomtry());
                nextGeometry.transform.position = new Vector3(-3.5f, 4, -2);
                nextGeometry.transform.rotation = Quaternion.Euler(0, 0, 90);
                geom = Instantiate(GetRandomGeomtry());
                geom.AddComponent<GeometryMovement>();
                PaintCubes(geom);
            }
            else
            {
                if (nextGeometry.name.Contains("Geometry_I"))
                    geom = Instantiate(geometryI);
                else if (nextGeometry.name.Contains("Geometry_J"))
                    geom = Instantiate(geometryJ);
                else if (nextGeometry.name.Contains("Geometry_L"))
                    geom = Instantiate(geometryL);
                else if (nextGeometry.name.Contains("Geometry_T"))
                    geom = Instantiate(geometryT);
                else if (nextGeometry.name.Contains("Geometry_O"))
                    geom = Instantiate(geometryO);
                else
                    Debug.LogError($"Error: none geometry found!");
                geom.AddComponent<GeometryMovement>();
                PaintCubes(geom);

                Destroy(nextGeometry);
                nextGeometry = Instantiate(GetRandomGeomtry());
                nextGeometry.transform.position = new Vector3(-4.5f, 4, -2);
                nextGeometry.transform.rotation = Quaternion.Euler(0, 0, 90);
            }

            PlayerAnalytics.Instance.AddCurrentShapes(geom.transform.childCount);
        }

        yield return null;
    }

    private void DestroyAndPushDown() //do for more than one y.
    {
        int shapesDestroyed = 0;
        bool pushDown = false;
        gameOver = true;
        List<int> levels = CheckForCompletion();
        foreach (var index in levels)
        {
            if (index == 16)
            {
                pushDown = true;
                for (int i = 0; i < otherCubes.Count; i++)
                {
                    if (otherCubes[i].transform.position.y == (0.5))
                    {
                        Destroy(otherCubes[i]);
                        shapesDestroyed++;
                    }
                }
                PlayerAnalytics.Instance.AddShapesDestroyed(shapesDestroyed);
                StartCoroutine(DeleteAfterFrame());
            }
        }

        foreach (var index in levels)
        {
            if (index == 0)
                gameOver = false;
        }

        if (gameOver)
        {
            foreach (GameObject cube in otherCubes)
            {
                cube.GetComponent<MeshRenderer>().material.color = Color.gray;
            }

            var audioComp = GetComponent<AudioSource>();
            audioComp.clip = Resources.Load("Sounds/Death", typeof(AudioClip)) as AudioClip;
            audioComp.loop = false;
            audioComp.PlayOneShot(audioComp.clip);
            Destroy(nextGeometry);
            Destroy(this);
        }
        if (pushDown)
        {
            foreach (var index in otherCubes)
            {
                index.transform.position += new Vector3(0, -1, 0);
            }
            
            PlayerAnalytics.Instance.AddShapesDestroyed(16);
            PlayerAnalytics.Instance.AddCurrentShapes(-16);
        }
    }

    private List<int> CheckForCompletion()
    {
        List<int> levels = new List<int>(8);
        for (int i = 0; i < 8; i++)
        {
            levels.Add(0);
        }
        
        foreach (GameObject index in otherCubes)
        {
                levels[(int) index.transform.position.y/1] += 1;
        }
        
        return levels;
    }

    IEnumerator DeleteAfterFrame()
    {
        yield return new WaitForEndOfFrame();

        for(var i = otherCubes.Count - 1; i > -1; i--)
        {
            if (otherCubes[i] == null)
                otherCubes.RemoveAt(i);
        }
    }
}