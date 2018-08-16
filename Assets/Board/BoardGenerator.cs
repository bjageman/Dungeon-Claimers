using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour {

	[Header("Board Settings")]
	[SerializeField] int boardWidth;
	[SerializeField] int boardHeight;
	[SerializeField] float startX = 0;
	[SerializeField] float startY = 0;
	[SerializeField] bool useFogOfWar = true;

	[SerializeField] MonsterTribe[] monsters;

	[Header("Spawn Tiles")]
	[SerializeField] GameObject defaultTile;
	[SerializeField] GameObject fogOfWar;
	[SerializeField] GameObject claimOverlay;


	[SerializeField] Sprite EmptyBlock;
	[SerializeField] Sprite DeadEndPathUp;
	[SerializeField] Sprite DeadEndPathDown;
	[SerializeField] Sprite DeadEndPathLeft;
	[SerializeField] Sprite DeadEndPathRight;
	[SerializeField] Sprite StraightPathUpDown;
	[SerializeField] Sprite StraightPathLeftRight;
	[SerializeField] Sprite CurvedPathUpLeft;
	[SerializeField] Sprite CurvedPathUpRight;
	[SerializeField] Sprite CurvedPathDownRight;
	[SerializeField] Sprite CurvedPathDownLeft;
	[SerializeField] Sprite ForkedPathUp;
	[SerializeField] Sprite ForkedPathDown;
	[SerializeField] Sprite ForkedPathLeft;
	[SerializeField] Sprite ForkedPathRight;	
	[SerializeField] Sprite FourWayPath;
	
	Room[,] board;

	public static BoardGenerator instance;

	public Room GetRoom(int x, int y){
		if (x >= 0 && x < boardWidth && y >= 0 && y < boardHeight){
			return board[x,y];
		}
		return null;
	}

	void Awake(){
		if (FindObjectsOfType<BoardGenerator>().Length > 1){
			Destroy(this);
		}else{
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		board = new Room[boardWidth, boardHeight];
		CreateBoard();
	}
	
	private void CreateBoard(){
		
		for(int x = 0; x < boardWidth; x++){
			for(int y = 0; y < boardHeight; y++){
				Room room = CreateNewRoom(x,y);
				board[x,y] = room; 
			}
		}
		TraverseDungeon(board[0,0]);
		TraverseDungeon(board[0, boardHeight - 1]);
		TraverseDungeon(board[boardWidth - 1, 0]);
		TraverseDungeon(board[boardWidth - 1, boardHeight - 1]);
		SetTilesOnBoard();
		AddPlayersToBoard();
	}

    private void AddPlayersToBoard()
    {
        HeroController[] players = FindObjectsOfType<HeroController>();
		int[][] startPositions = new int[][]{
			new int[2]{0,0}, new int[2]{boardWidth - 1, boardHeight - 1}, new int[2]{0, boardHeight - 1}, new int[2]{boardWidth - 1, 0}, 
		};
		int i = 0;
		foreach ( HeroController player in players ){
			if (i > 3){ return;}
			int[] position = startPositions[i];
			i++;
			player.SetHeroOnBoard(position[0], position[1]);
			Destroy(GetRoom(position[0],position[1]).GetTribe().gameObject);
		}
	}

    private Room CreateNewRoom(int x, int y)
    {
        GameObject newTile = CreateNewTile(x, y);
        Room room = new Room();
        room.tile = newTile;
		room.claimOverlay = CreateClaimOverlay(newTile);
		room.SetTribe(CreateMonster(newTile));
		if (useFogOfWar){
			room.fog = createFogOfWar(newTile);
		}	
        room.x = x;
        room.y = y;
        return room;
    }

    private GameObject CreateClaimOverlay(GameObject newTile)
    {
        GameObject claimTile = Instantiate(claimOverlay, newTile.transform.position, newTile.transform.rotation);
		claimTile.gameObject.SetActive(false);
		return claimTile;
    }

    private GameObject CreateNewTile(int x, int y)
    {
        Vector2 offset = defaultTile.GetComponent<SpriteRenderer>().bounds.size;
        GameObject newTile = Instantiate(defaultTile, new Vector3(startX + (offset.x * x), startY + (offset.y * y), 0), defaultTile.transform.rotation);
        newTile.transform.parent = transform;
        SpriteRenderer newTileSR = newTile.GetComponent<SpriteRenderer>();
        newTileSR.sprite = EmptyBlock;
        newTileSR.sortingOrder = -1;
        newTileSR.sortingLayerName = "Dungeon";
        return newTile;
    }

	private MonsterTribe CreateMonster(GameObject newTile)
    {
		MonsterTribe monster = monsters[UnityEngine.Random.Range(0, monsters.Length)];
        GameObject monsterObj = Instantiate(monster.gameObject, newTile.transform.position, Quaternion.identity, transform);
		return monsterObj.GetComponent<MonsterTribe>();
    }

	private GameObject createFogOfWar(GameObject newTile)
    {
        return Instantiate(fogOfWar, newTile.transform.position, newTile.transform.rotation);
    }

    private void TraverseDungeon(Room currentRoom, Room previousRoom = null){
		currentRoom.isVisited = true;
		int x = currentRoom.x;
		int y = currentRoom.y;
		List<Vector2Int> paths = new List<Vector2Int>();
		//Traverse to next room and add a pathway to this room
		if (previousRoom != null && !currentRoom.pathWays.Contains(previousRoom)){
			currentRoom.pathWays.Add(previousRoom);
		}
		if(currentRoom.y < boardHeight - 1 && board[x,y+1].isVisited == false){
			paths.Add(Vector2Int.up);
		}
		if(currentRoom.x < boardWidth - 1 && board[x + 1,y].isVisited == false){
			paths.Add(Vector2Int.right);
		}
		if(currentRoom.y > 0 && board[x, y - 1].isVisited == false){
			paths.Add(Vector2Int.down);
		}
		if(currentRoom.x > 0 && board[x - 1, y].isVisited == false){
			paths.Add(Vector2Int.left);
		}
		if (paths.Count > 0)
        {
            int branches = 1;
			if (paths.Count > 1)
			{
				//Create a random number of branches
				branches = UnityEngine.Random.Range(1, paths.Count + 1);
			}
			for (int i = 0; i < branches; i++){
				Vector2Int direction = paths[UnityEngine.Random.Range(0, paths.Count)];
				Room nextRoom = board[x + direction.x ,y + direction.y];
				if (!currentRoom.pathWays.Contains(nextRoom)){
					currentRoom.pathWays.Add(nextRoom);
				}
				TraverseDungeon(nextRoom, currentRoom);
			}
			
        }
    }

    private void SetTilesOnBoard()
    {
		for(int x = 0; x < boardWidth; x++){
			for(int y = 0; y < boardHeight; y++){
				Room room = board[x,y];
				List<Vector2Int> directions = new List<Vector2Int>();
				foreach(Room pathWay in room.pathWays){
					directions.Add(new Vector2Int(
						pathWay.x - room.x,
						pathWay.y - room.y
					));
				}
				if (directions.Count == 1){
					if (directions[0] == Vector2Int.up){
						room.tile.GetComponent<SpriteRenderer>().sprite = DeadEndPathUp;
					}
					else if (directions[0] == Vector2Int.right){
						room.tile.GetComponent<SpriteRenderer>().sprite = DeadEndPathRight;
					}
					else if (directions[0] == Vector2Int.down){
						room.tile.GetComponent<SpriteRenderer>().sprite = DeadEndPathDown;
					}
					else if (directions[0] == Vector2Int.left){
						room.tile.GetComponent<SpriteRenderer>().sprite = DeadEndPathLeft;
					}else{
						Debug.LogWarning("Unable to read tile direction: " + room);
					}
				}
				//TODO Make this one shorter
				if (directions.Count == 2){
					if (directions.Contains(Vector2Int.up) && directions.Contains(Vector2Int.down)){
						room.tile.GetComponent<SpriteRenderer>().sprite = StraightPathUpDown;
					} else if (directions.Contains(Vector2Int.left) && directions.Contains(Vector2Int.right)){
						room.tile.GetComponent<SpriteRenderer>().sprite = StraightPathLeftRight;
					} else if (directions.Contains(Vector2Int.up) && directions.Contains(Vector2Int.left)){
						room.tile.GetComponent<SpriteRenderer>().sprite = CurvedPathUpLeft;
					} else if (directions.Contains(Vector2Int.up) && directions.Contains(Vector2Int.right)){
						room.tile.GetComponent<SpriteRenderer>().sprite = CurvedPathUpRight;
					} else if (directions.Contains(Vector2Int.down) && directions.Contains(Vector2Int.left)){
						room.tile.GetComponent<SpriteRenderer>().sprite = CurvedPathDownLeft;
					} else if (directions.Contains(Vector2Int.down) && directions.Contains(Vector2Int.right)){
						room.tile.GetComponent<SpriteRenderer>().sprite = CurvedPathDownRight;
					} else{
						Debug.LogWarning("Can't determine 2 path direction");
					}
				}
				if (directions.Count == 3){
					List<Vector2Int> possibleDirections = new List<Vector2Int>{Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
					foreach (Vector2Int direction in directions){
						possibleDirections.Remove(direction);
					}
					if (possibleDirections[0] == Vector2Int.up){
						room.tile.GetComponent<SpriteRenderer>().sprite = ForkedPathDown;
					}
					else if (possibleDirections[0] == Vector2Int.right){
						room.tile.GetComponent<SpriteRenderer>().sprite = ForkedPathLeft;
					}
					else if (possibleDirections[0] == Vector2Int.down){
						room.tile.GetComponent<SpriteRenderer>().sprite = ForkedPathUp;
					}
					else if (possibleDirections[0] == Vector2Int.left){
						room.tile.GetComponent<SpriteRenderer>().sprite = ForkedPathRight;
					}else{
						Debug.LogWarning("Unable to read tile direction: " + room);
					}
				}
				if (directions.Count == 4){
					room.tile.GetComponent<SpriteRenderer>().sprite = FourWayPath;
				}
			}
		}
        
    }
}
