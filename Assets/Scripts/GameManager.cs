using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Camera Controls")]
    public Camera mainCamera;
    public Slider slider;

    [Header("Tiles")]
    public Tile deadTile;
    public Tile aliveTile;
    public Tilemap nextState;
    public Tilemap currentState;

    [Header("Pattern Scriptable Asset")]
    public Pattren pattren;

    [Header("Time")]
    public float intervalTime = 0.04f;

    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> cellsToCheck;

    public int population { get; private set; }
    public int iterations { get; private set; }
    public float time { get; private set; }

    private void Awake()
    {
        aliveCells = new HashSet<Vector3Int>();
        cellsToCheck = new HashSet<Vector3Int>();
    }

    private void Start()
    {
        SetPattern(pattren);
        slider.minValue = 50;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        mainCamera.orthographicSize = value;
    }

    private void SetPattern(Pattren pattren)
    {
        Clear();

        Vector2Int center = pattren.GetCenter();

        for (int i = 0; i < pattren.cells.Length; i++)
        {
            Vector3Int cell = (Vector3Int)(pattren.cells[i] - center);
            currentState.SetTile(cell, aliveTile);
            aliveCells.Add(cell);
        }

        population = aliveCells.Count;
    }

    private void Clear()
    {
        aliveCells.Clear();
        cellsToCheck.Clear();
        currentState.ClearAllTiles();
        nextState.ClearAllTiles();
        population = 0;
        iterations = 0;
        time = 0f;
    }

    private void OnEnable()
    {
        StartCoroutine(Simulate());
    }

    private IEnumerator Simulate()
    {
        var interval = new WaitForSeconds(intervalTime);
        yield return interval;

        while (enabled)
        {
            UpdateState();

            population = aliveCells.Count;
            iterations++;
            time += intervalTime;

            yield return interval;
        }
    }

    private void UpdateState()
    {
        cellsToCheck.Clear();
        
        foreach (Vector3Int cell in aliveCells)  // getting cells to check
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    cellsToCheck.Add(cell + new Vector3Int(x, y));
                }
            }
        }
        
        foreach (Vector3Int cell in cellsToCheck) // transition cells to the next state
        {
            int neighbors = CountNeighbors(cell);
            bool alive = IsAlive(cell);

            if (!alive && neighbors == 3)
            {
                nextState.SetTile(cell, aliveTile);
                aliveCells.Add(cell);
            }
            else if (alive && (neighbors < 2 || neighbors > 3))
            {
                nextState.SetTile(cell, deadTile);
                aliveCells.Remove(cell);
            }
            else
            {
                nextState.SetTile(cell, currentState.GetTile(cell));
            }
        }

        (nextState, currentState) = (currentState, nextState);
        nextState.ClearAllTiles();
    }

    private int CountNeighbors(Vector3Int cell)
    {
        int count = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighbor = cell + new Vector3Int(x, y);

                if (x == 0 && y == 0)
                {
                    continue;
                }
                else if (IsAlive(neighbor))
                {
                    count++;
                }
            }
        }

        return count;
    }

    private bool IsAlive(Vector3Int cell)
    {
        return currentState.GetTile(cell) == aliveTile;
    }
}
