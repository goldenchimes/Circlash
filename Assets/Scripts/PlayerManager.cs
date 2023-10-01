using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    SceneRestarter sceneRestarter;

    [SerializeField]
    Transform field;

    [SerializeField]
    GameObject computerPrefab;

    [SerializeField]
    float cellSize = 2.0f;

    [SerializeField]
    int playerCount = 5;

    int _where = -1;

    int where
    {
        get => _where;
        set
        {
            if (value >= transform.childCount)
            {
                value = 0;
            }
            else if (value < 0)
            {
                value = transform.childCount - 1;
            }
            _where = value;
            player.TakeTurn();
        }
    }

    bool gameOver
    {
        get
        {
            int activeCount = 0;
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    if (child.GetComponent<Human>())
                    {
                        return false;
                    }
                    activeCount++;
                }
            }
            return activeCount > 1;
        }
    }

    bool won
    {
        get
        {
            if (gameOver)
            {
                foreach (Transform child in transform)
                {
                    if (child.gameObject.activeSelf && child.GetComponent<Human>())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    Player player
    {
        get => transform.GetChild(where).GetComponent<Player>();
    }

    void Awake()
    {
        int rows = (int) (field.lossyScale.y / cellSize);
        int columns = (int) (field.lossyScale.x / cellSize);
        int cellCount = rows * columns;
        int[,] cells = new int[cellCount, 2];
        float halfFieldHeight = field.lossyScale.y / 2.0f;
        float halfFieldWidth = field.lossyScale.x / 2.0f;
        float halfCellSize = cellSize / 2.0f;
        Transform[] players = new Transform[playerCount];

        for (int row = 0; row < rows; row++)
        {
            int rowI = row * columns;
            for (int column = 0; column < columns; column++)
            {
                int cellI = rowI + column;
                cells[cellI, 0] = row;
                cells[cellI, 1] = column;
            }
        }

        for (int i = cellCount - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int rowValue = cells[i, 0];
            int columnValue = cells[i, 1];
            cells[i, 0] = cells[j, 0];
            cells[i, 1] = cells[j, 1];
            cells[j, 0] = rowValue;
            cells[j, 1] = columnValue;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            players[i] = transform.GetChild(i);
        }

        for (int i = transform.childCount; i < playerCount; i++)
        {
            players[i] = Instantiate(computerPrefab, transform).transform;
        }

        for (int i = 0; i < players.Length; i++)
        {
            Transform child = players[i];
            Vector3 position = child.position;
            position.x = (cellSize * cells[i, 0]) - halfFieldWidth + halfCellSize;
            position.y = (cellSize * cells[i, 1]) - halfFieldHeight + halfCellSize;
            child.position = position;
            child.up = -child.position;
        }

        NextTurn();
    }

    void Start()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Player>().manager = this;
        }
    }

    public void NextTurn()
    {
        List<ConstraintSource> sources = new List<ConstraintSource>();
        PositionConstraint constraint = Camera.main.GetComponent<PositionConstraint>();
        ConstraintSource source = new ConstraintSource();

        if (gameOver)
        {
            Vector3 position = Camera.main.transform.position;

            position.x = player.transform.position.x;
            position.y = player.transform.position.y;
            Camera.main.transform.position = position;

            sceneRestarter.Restart(won);
        }
        else
        {
            where++;

            source.sourceTransform = player.transform;
            source.weight = 1.0f;

            sources.Add(source);
        }

        constraint.SetSources(sources);
    }
}
