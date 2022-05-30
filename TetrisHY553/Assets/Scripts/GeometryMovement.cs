using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryMovement : MonoBehaviour
{
    [SerializeField] private List<Vector3> cubesPositions = new List<Vector3>();

    private enum KeyPressed
    {
        Left,
        Right,
        Up,
        Down,
        RotateY,
        RotateZ,
        Dropping
    };

    private void Start()
    {
        foreach (Transform index in transform)
        {
            cubesPositions.Add(index.position);
        }

        StartCoroutine(DropDownEveryTime(4));

    }

    IEnumerator DropDownEveryTime(int t)
    {
        yield return new WaitForSeconds(t);
        
        transform.position += new Vector3(0, -1, 0);
        UpdateVectorPos();
        if (CheckAndMove(KeyPressed.Dropping) == false)
        {
            transform.position += new Vector3(0, 1, 0);
            UpdateVectorPos();
            DeleteComponentAndFather();
            PlayerAnalytics.Instance.AddPlayerMove();
        }

        yield return DropDownEveryTime(t);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CheckAndMove(KeyPressed.Left))
            {
                transform.position += new Vector3(1.0f, 0, 0);
                UpdateVectorPos();
                PlayerAnalytics.Instance.AddPlayerMove();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CheckAndMove(KeyPressed.Right))
            {
                transform.position += new Vector3(-1.0f, 0, 0);
                UpdateVectorPos();
                PlayerAnalytics.Instance.AddPlayerMove();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CheckAndMove(KeyPressed.Up))
            {
                //Debug.Log($"Can move Up!!");
                transform.position += new Vector3(0, 0, -1.0f);
                UpdateVectorPos();
                PlayerAnalytics.Instance.AddPlayerMove();
            }
            //else
                //Debug.Log($"Cannot move Up =(");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (CheckAndMove(KeyPressed.Down))
            {
                transform.position += new Vector3(0, 0, 1.0f);
                UpdateVectorPos();
                PlayerAnalytics.Instance.AddPlayerMove();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Quaternion quaternion = Quaternion.Euler(0, 90, 0);
            transform.rotation *= quaternion;
            UpdateVectorPos();
            if (CheckAndMove(KeyPressed.RotateY) == false)
            {
                quaternion = Quaternion.Euler(0, -90, 0);
                transform.rotation *= quaternion;
                UpdateVectorPos();
            }
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            Quaternion quaternion = Quaternion.Euler(0, 0, 90);
            transform.rotation *= quaternion;
            UpdateVectorPos();
            if (CheckAndMove(KeyPressed.RotateZ) == false)
            {
                quaternion = Quaternion.Euler(0, 0, -90);
                transform.rotation *= quaternion;
                UpdateVectorPos();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position += new Vector3(0, -1, 0);
            UpdateVectorPos();
            if (CheckAndMove(KeyPressed.Dropping) == false)
            {
                transform.position += new Vector3(0, 1, 0);
                UpdateVectorPos();
                DeleteComponentAndFather();
                PlayerAnalytics.Instance.AddPlayerMove();
            }
        }

        if (TetrisManager.Instance.gameOver)
        {
            Destroy(gameObject);
        }
    }

    bool CheckAndMove(KeyPressed key)
    {
        bool flag = true;
        foreach (Vector3 cubePos in cubesPositions)
        {
            switch (key)
            {
                case KeyPressed.Left:
                    foreach (GameObject index in TetrisManager.Instance.otherCubes)
                    {
                        if (index.transform.position.x == cubePos.x + 1 && index.transform.position.z == cubePos.z && index.transform.position.y == cubePos.y)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (cubePos.x + 1 == 2.5)
                        flag = false;
                    break;
                case KeyPressed.Right:
                    foreach (GameObject index in TetrisManager.Instance.otherCubes)
                    {
                        if ( index.transform.position.x == cubePos.x - 1 && index.transform.position.z == cubePos.z && index.transform.position.y == cubePos.y)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (cubePos.x - 1 == -2.5)
                        flag = false;
                    break;
                case KeyPressed.Down:
                    foreach (GameObject index in TetrisManager.Instance.otherCubes)
                    {
                        if ( index.transform.position.z == cubePos.z + 1 && index.transform.position.x == cubePos.x && index.transform.position.y == cubePos.y)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (cubePos.z + 1 == 2.5)
                        flag = false;
                    break;
                case KeyPressed.Up:
                    foreach (GameObject index in TetrisManager.Instance.otherCubes)
                    {
                        if ( index.transform.position.z == cubePos.z - 1 && index.transform.position.x == cubePos.x  && index.transform.position.y == cubePos.y)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (cubePos.z - 1 == -2.5)
                        flag = false;
                    break;
                case KeyPressed.RotateY:
                    foreach (GameObject index in TetrisManager.Instance.otherCubes)
                    {
                        if ( index.transform.position == cubePos)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (cubePos.x <= -2.5)
                        flag = false;
                    else if (cubePos.x >= 2.5)
                        flag = false;
                    else if (cubePos.z >= 2.5)
                        flag = false;
                    else if (cubePos.z <= -2.5)
                        flag = false;
                    break;
                case KeyPressed.RotateZ:
                    foreach (GameObject index in TetrisManager.Instance.otherCubes)
                    {
                        if ( index.transform.position == cubePos)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (cubePos.x <= -2.5)
                        flag = false;
                    else if (cubePos.x >= 2.5)
                        flag = false;
                    else if (cubePos.z >= 2.5)
                        flag = false;
                    else if (cubePos.z <= -2.5)
                        flag = false;
                    else if (cubePos.y >= 7.5)
                        flag = false;
                    else if (cubePos.y <= -0.5)
                        flag = false;
                    break;
                case KeyPressed.Dropping:
                    if (cubePos.y == -0.5)
                        flag = false;
                    foreach (GameObject index in TetrisManager.Instance.otherCubes)
                    {
                        var tr = index.transform;
                        if (tr.position == cubePos)
                            flag = false;
                    }

                    break;
                default:
                    break;
            }
        }

        return flag;
    }

    void DeleteComponentAndFather()
    {
        Destroy(GetComponent<GeometryMovement>());
        TetrisManager.Instance.StartCoroutine(TetrisManager.Instance.SpawnGeometry());
        //Detach children components
        foreach (Transform index in transform)
        {
            TetrisManager.Instance.otherCubes.Add(index.gameObject);
        }
    }

    void UpdateVectorPos()
    {
        cubesPositions = new List<Vector3>();
        foreach (Transform index in transform)
        {
            Vector3 rounded = index.position;
            rounded.x = Mathf.Round(rounded.x * 10.0f) * 0.1f;
            rounded.y = Mathf.Round(rounded.y * 10.0f) * 0.1f;
            rounded.z = Mathf.Round(rounded.z * 10.0f) * 0.1f;
            cubesPositions.Add(rounded);
        }
    }
}