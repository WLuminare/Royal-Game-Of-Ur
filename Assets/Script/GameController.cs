using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public enum GameState
    {
        STARTING, PLAYING, GAME_OVER
    }

    public enum PlayerNumber
    {
        ONE, TWO
    }

    public enum StartingPlayer
    {
        ONE, TWO, RANDOM
    }

    public enum TurnState
    {
        WAITING_FOR_ROLL, WAITING_FOR_MOVE, MOVING, ENDED
    }

    public enum DiceType
    {
        VIRTUAL, VIRTUAL2
    }

    public static GameController obj;
    public static StartingPlayer FirstPlayer = StartingPlayer.ONE;
	public static DiceType TypeOfDice = DiceType.VIRTUAL;

    public List<List<Tile>> Map;

    public Sprite TileBase;
    public Sprite TileRollAgain;
    public Sprite TileSafe;
    public Sprite TileStart;
    public Sprite TileEnd;
    public Sprite CoinHeads;
    public Sprite CoinTails;
    public Button RollButtons;
    public RollsDisplay RollsDisplays;
    public RollButton RollButtonsToReset;

    public GameState State;
    public TurnState Turn;

    public Player Player1;
    public Player Player2;

    public Player CurrentPlayer;
    public PlayerNumber CurrentPlayerNumber;

    public float GameStartTimer = 0.5f;


    public int RolledValue = 0;

    public GameObject DiceVirtual;
    public GameObject DiceVirtual2;

    public TextMeshProUGUI CurrentPlayerText;
	public TextMeshProUGUI RedCompletedText;
	public TextMeshProUGUI BlueCompletedText;
	public TextMeshProUGUI RedOnBoardText;
	public TextMeshProUGUI BlueOnBoardText;
	public TextMeshProUGUI RedOnHandText;
	public TextMeshProUGUI BlueOnHandText;

    public NotificationText NotificationText;
    public static int NumRedPieceCompleted;
	public static int NumBluePieceCompleted;
	public static int NumRedPieceOnBoard;
	public static int NumBluePieceOnBoard;
	public static int NumRedPieceOnHand;
	public static int NumBluePieceOnHand;
    RollZonkSound RZ;
    private void Awake()
    {
        obj = this;

		NumRedPieceCompleted = 0;
		NumBluePieceCompleted = 0;
		NumRedPieceOnBoard = 0;
		NumBluePieceOnBoard = 0;
		NumRedPieceOnHand = 7;
		NumBluePieceOnHand = 7;

        Map = new List<List<Tile>>();
        for (int i = 0; i < 3; i++)
        {
            Map.Add(new List<Tile>());
            for (int j = 0; j < 8; j++)
            {
                Map[i].Add(null);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        State = GameState.STARTING;
        List<Player> players = new List<Player> { Player1, Player2 };
        foreach (Player player in players)
        {
            foreach (Piece piece in player.Pieces)
            {
                piece.SetUp(player);
            }
            player.PlayerTurnIndicator.SetUp(player);
        }
        RollButtons.interactable = false;
        SetDiceDisplay();
        RollsDisplays.Hide();

        RZ = FindObjectOfType<RollZonkSound>();
        gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (State == GameState.STARTING)
        {
            GameStartTimer -= Time.deltaTime;
            if (GameStartTimer <= 0)
            {
                State = GameState.PLAYING;
                PlayerNumber playerToStart = PlayerNumber.ONE;
                switch (FirstPlayer)
                {
                    case StartingPlayer.ONE:
                        playerToStart = PlayerNumber.ONE;
                        break;
                    case StartingPlayer.TWO:
                        playerToStart = PlayerNumber.TWO;
                        break;
                    case StartingPlayer.RANDOM:
                        if (Random.Range(0f, 1f) < 0.5f)
                        {
                            playerToStart = PlayerNumber.ONE;
                        }
                        else
                        {
                            playerToStart = PlayerNumber.TWO;
                        }
                        break;
                }
                StartTurn(playerToStart);
            }

        }

		CheckPlayerPieceStatus ();
    }

	public void CheckPlayerPieceStatus()
	{
		int RedPieceCompleted = 0;
		int BluePieceCompleted = 0;
		int RedPieceOnBoard = 0;
		int BluePieceOnBoard = 0;
		int RedPieceOnHand = 7;
		int BluePieceOnHand = 7;

		foreach (Piece piece in Player1.Pieces) {
			if (piece.IsDone) {
				RedPieceCompleted++;
			} else {
				if (piece.CurrentTile != null && (piece.CurrentTile.Type == Tile.TileType.NORMAL
					|| piece.CurrentTile.Type == Tile.TileType.ROLL_AGAIN || piece.CurrentTile.Type == Tile.TileType.SAFE)) {
					RedPieceOnBoard++;
				} else if (piece.CurrentTile != null && piece.CurrentTile.Type == Tile.TileType.START) {
					RedPieceOnHand++;
				}
			}
		}

		foreach (Piece piece in Player2.Pieces) {
			if (piece.IsDone) {
				BluePieceCompleted++;
			} else {
				if (piece.CurrentTile != null && (piece.CurrentTile.Type == Tile.TileType.NORMAL
					|| piece.CurrentTile.Type == Tile.TileType.ROLL_AGAIN || piece.CurrentTile.Type == Tile.TileType.SAFE)) {
					BluePieceOnBoard++;
				} else if (piece.CurrentTile != null && piece.CurrentTile.Type == Tile.TileType.START) {
					BluePieceOnHand++;
				}
			}
		}

		NumRedPieceOnBoard = RedPieceOnBoard;
		NumRedPieceOnHand = RedPieceOnHand - RedPieceOnBoard - RedPieceCompleted;
		NumRedPieceCompleted = RedPieceCompleted;

		NumBluePieceOnBoard = BluePieceOnBoard;
		NumBluePieceOnHand = BluePieceOnHand - BluePieceOnBoard - BluePieceCompleted;
		NumBluePieceCompleted = BluePieceCompleted;

		RedCompletedText.SetText ("Completed\t: " + NumRedPieceCompleted.ToString());
		BlueCompletedText.SetText ("Completed\t: " + NumBluePieceCompleted.ToString());
		RedOnBoardText.SetText ("On Board\t: " + NumRedPieceOnBoard.ToString());
		BlueOnBoardText.SetText ("On Board\t: " + NumBluePieceOnBoard.ToString());
		RedOnHandText.SetText ("Unused\t: " + NumRedPieceOnHand.ToString());
		BlueOnHandText.SetText ("Unused\t: " + NumBluePieceOnHand.ToString());
	}

    public void NextTurn()
    {
        CheckGameOver();
        switch (CurrentPlayerNumber)
        {
            case PlayerNumber.ONE:
                StartTurn(PlayerNumber.TWO);
                break;
            case PlayerNumber.TWO:
                StartTurn(PlayerNumber.ONE);
                break;
        }
        RollsDisplays.Hide();
        RollButtonsToReset.Clear();
    }
    public void CheckGameOver()
    {
        List<Player> players = new List<Player> { Player1, Player2 };
        foreach (Player player in players)
        {
            bool hasWon = true;
            foreach (Piece piece in player.Pieces)
            {
                hasWon &= piece.IsDone;
            }

            if (hasWon)
            {
                Win(player.PlayerNumber);
            }
        }
    }
    public void Win(PlayerNumber playerNumber)
    {
        State = GameState.GAME_OVER;
        switch (playerNumber)
        {
            case PlayerNumber.ONE:
                SceneManager.LoadScene("GameOver1");
                break;
            case PlayerNumber.TWO:
                SceneManager.LoadScene("GameOver2");
                break;
        }
    }

    public void StartTurn(PlayerNumber player)
    {
        Player1.PlayerTurnIndicator.SetOff();
        Player2.PlayerTurnIndicator.SetOff();

        switch (player)
        {
		case PlayerNumber.ONE:
			CurrentPlayerNumber = PlayerNumber.ONE;
			CurrentPlayer = Player1;
			CurrentPlayerText.SetText ("Red Turn");
            Player1.PlayerTurnIndicator.SetOn();
            break;
        case PlayerNumber.TWO:
            CurrentPlayerNumber = PlayerNumber.TWO;
            CurrentPlayer = Player2;
			CurrentPlayerText.SetText ("Blue Turn");
            Player2.PlayerTurnIndicator.SetOn();
            break;
        }

        SetTurnState(TurnState.WAITING_FOR_ROLL);
        
        RollButtons.image.color = CurrentPlayer.ColorPale;
    }

    public void Rolled(int amount)
    {
        RolledValue = amount;
        RollsDisplays.Show();

        if (amount == 0)
        {
            RZ.PlaySoundZonk();
            ShowMessage(CurrentPlayerNumber, "Rolled a 0, next turn.");
            NextTurn();
        }
        else
        {
            bool movePossible = false;
            foreach (Piece piece in CurrentPlayer.Pieces)
            {
                movePossible |= (piece.CanMoveTimes(RolledValue) != null);
            }

            if (!movePossible)
            {
                ShowMessage(CurrentPlayerNumber, "No move possible, next turn.");
                NextTurn();
            }
            else
            {
                SetTurnState(TurnState.WAITING_FOR_MOVE);
            }
        }
    }
    public void ShowMessage(PlayerNumber recipientPlayer, string message)
    {
        NotificationText.gameObject.SetActive(true);
        NotificationText.ShowMessage(recipientPlayer, message);
    }
    public void INPUT_Rolled(int amount)
    {
        Rolled(amount);
    }

    public void SetTurnState(TurnState turnState)
    {
        Turn = turnState;
        switch (Turn)
        {
            case TurnState.WAITING_FOR_ROLL:
                RollsDisplays.Hide();
                RollButtons.interactable = true;
                break;
            case TurnState.WAITING_FOR_MOVE:
                RollButtons.interactable = false;
                foreach (Piece piece in CurrentPlayer.Pieces)
                {
                    piece.SetActiveIfCanMove();
                }
                break;
            case TurnState.MOVING:
                RollButtons.interactable = false;
                
                foreach (Piece piece in CurrentPlayer.Pieces)
                {
                    piece.SetInactive();
                }
                break;
            case TurnState.ENDED:
                
                    RollButtons.interactable = false;
                
                NextTurn();
                break;
        }
    }

    public void SetDiceDisplay()
    {
        DiceVirtual.SetActive(false);

        switch (TypeOfDice)
        {
            case DiceType.VIRTUAL:
                DiceVirtual.SetActive(true);
                break;
            case DiceType.VIRTUAL2:
                DiceVirtual.SetActive(true);
                break;
        }
    }
}
