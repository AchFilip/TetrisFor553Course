using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnalytics : MonoBehaviour
{
    #region Singleton Initilization
    public static PlayerAnalytics Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    #endregion

    [SerializeField]
    private int shapesCreated, currentShapes, totalPlayerMoves, shapesDestroyed, combos;
    [SerializeField]
    private Text shapesCreatedUI, currentShapesUI, totalPlayerMovesUI, shapesDestroyedUI, combosUI;
    
    public int GetPlayerMoves()
    {
        return totalPlayerMoves;
    }

    public void AddPlayerMove()
    {
        totalPlayerMoves += 1;
        RefreshInformationUI();
    }

    public int GetShapesCreated()
    {
        return shapesCreated;
    }

    public void AddShapesCreated()
    {
        shapesCreated += 1;
        RefreshInformationUI();
    }

    public int GetShapesDestroyed()
    {
        return shapesDestroyed;
    }
    
    public void AddShapesDestroyed(int shapes)
    {
        shapesDestroyed += shapes;
        RefreshInformationUI();
    }

    public void AddCurrentShapes(int currShapes)
    {
        currentShapes += currShapes;
        RefreshInformationUI();
    }

    private void RefreshInformationUI()
    {
        shapesCreatedUI.text = shapesCreated.ToString();
        currentShapesUI.text = currentShapes.ToString();
        totalPlayerMovesUI.text = totalPlayerMoves.ToString();
        shapesDestroyedUI.text = shapesDestroyed.ToString();
        combosUI.text = combos.ToString();
    }
    public void Start()
    {
        shapesCreated = currentShapes = totalPlayerMoves = shapesDestroyed = combos = 0;
        var textValuesTransform = transform.Find("TextValues");
        
        shapesCreatedUI = textValuesTransform.Find("ShapesCreated").GetComponent<Text>();
        currentShapesUI = textValuesTransform.Find("CurrentShapes").GetComponent<Text>();
        totalPlayerMovesUI = textValuesTransform.Find("Total Player Moves").GetComponent<Text>();
        shapesDestroyedUI = textValuesTransform.Find("Shapes Destroyed").GetComponent<Text>();
        combosUI = textValuesTransform.Find("Combos").GetComponent<Text>();
    }
}
