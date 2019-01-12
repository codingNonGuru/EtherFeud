using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace EtherGame
{
    public class BattleState : MonoBehaviour
    {
        [SerializeField]
		Transform floor = null;

        [SerializeField]
        GameObject fighterPrefab = null;

        Transform mainCamera = null;

        float azimuth = 150.0f;

        float inclination = 40.0f; 

        float distance = 5.0f;

        List<Fighter> fighters = new List<Fighter>();

        Fighter playerFighter = null;

        bool hasRefreshedCommands = false;

        void Awake()
        {
            BattleModule.OnPlayerJoined -= HandlePlayerJoined;
            BattleModule.OnPlayerJoined += HandlePlayerJoined;

            BattleModule.OnCommandsRefreshed -= HandleCommandsRefreshed;
            BattleModule.OnCommandsRefreshed += HandleCommandsRefreshed;
        }

        void Start()
        {
            mainCamera = Camera.main.transform;

            GenerateTerrain();

            InstantiatePlayer();
        }

        void Update()
        {
            RefreshCamera();

            if(!hasRefreshedCommands)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                hasRefreshedCommands = false;
            }
        }

        void IssueCommands()
        {
            if(Input.GetKey(KeyCode.W))
            {
                BattleModule.IssueCommand("moveForward");
            }
            else if(Input.GetKey(KeyCode.S))
            {
                BattleModule.IssueCommand("moveBackwards");
            }
            
            if(Input.GetKey(KeyCode.A))
            {
                BattleModule.IssueCommand("turnLeftwards");
            }
            else if(Input.GetKey(KeyCode.D))
            {
                BattleModule.IssueCommand("turnRightwards");
            }

            if(Input.GetKey(KeyCode.Space))
            {
                BattleModule.IssueCommand("shoot");
            }
        }

        void RefreshCamera()
        {
            /*var direction = new Vector3(
                Mathf.Cos(azimuth) * Mathf.Sin(inclination),
                Mathf.Cos(inclination), 
                Mathf.Sin(azimuth) * Mathf.Sin(inclination));

            mainCamera.position = -direction * 300.0f;

            mainCamera.LookAt(floor);*/

            var direction = -playerFighter.transform.forward * 3.0f;
            direction += floor.up * 1.0f;

            direction.Normalize();

            mainCamera.position = playerFighter.transform.position + direction * distance;

            mainCamera.LookAt(playerFighter.transform);

            mainCamera.position += floor.up * 2.0f;
        }

        void InstantiatePlayer()
        {
            var playerFighterInstance = Instantiate(fighterPrefab, Vector3.zero, Quaternion.identity);

            playerFighter = playerFighterInstance.GetComponent<Fighter>();
            if(playerFighter == null)
                return;

            playerFighter.Initialize(new HumanController());

            fighters.Add(playerFighter);
        }

        void InstantiateDummy()
        {
            var dummyPosition = new Vector3(10.0f, 0.0f, 10.0f);
            var dummyFighterInstance = Instantiate(fighterPrefab, dummyPosition, Quaternion.identity);

            var dummyFighter = dummyFighterInstance.GetComponent<Fighter>();
            if(dummyFighter == null)
                return;

            fighters.Add(dummyFighter);
        }

        void GenerateTerrain()
        {
            var config = new ProceduralToolkit.Examples.LowPolyTerrainGenerator.Config();
            float terrainSide = 100.0f;
            config.terrainSize = new Vector3(terrainSide, 1.0f, terrainSide);
            config.gradient = new Gradient();

            var terrainMeshDraft = ProceduralToolkit.Examples.LowPolyTerrainGenerator.TerrainDraft(config);
            for(int index = 0; index < terrainMeshDraft.vertexCount; ++index)
            {
                var vertex = terrainMeshDraft.vertices[index];
                terrainMeshDraft.vertices[index] = new Vector3(
                    (vertex.x - terrainSide * 0.5f) * 0.25f, 
                    (vertex.y - 0.5f) * 40.0f, 
                    (vertex.z - terrainSide * 0.5f) * 0.25f
                    );
            }

            //terrainMeshDraft.

            var floorMeshFilter = floor.GetComponent<MeshFilter>();

            floorMeshFilter.mesh = terrainMeshDraft.ToMesh();
            floorMeshFilter.mesh.RecalculateNormals();
        }

        void HandlePlayerJoined(PlayerJoinedMessage message)
        {
            InstantiateDummy();
        }

        void HandleCommandsRefreshed(CommandRefreshMessage message)
        {
            IssueCommands();

            hasRefreshedCommands = true;

            Time.timeScale = 1.0f;   
        }
    }
}