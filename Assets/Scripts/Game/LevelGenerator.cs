using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Game
{
    /// <summary>
    /// Generate Level
    /// </summary>
    public class LevelGenerator : MonoBehaviour, IOnEventCallback
    {
        private const byte LevelGeneratorEventId = 1;
        
        private enum TileType
        {
            Back, Lower, Upper
        }

        private enum StructureType
        {
            FlatEasy, Gap
        }

        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tile lowerTile, backTile;

        private int _currentX;
        private Camera _mainCamera;

        private void Awake()
        {
            _currentX = 11;
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            var currentWorldPos = grid.CellToWorld(new Vector3Int(_currentX-5, 0, 0));
            var screenPos = _mainCamera.WorldToViewportPoint(currentWorldPos);
            if(screenPos.x < 1f) GenerateNextStructure();
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void GenerateNextStructure()
        {
            var structureType = (StructureType) Random.Range(0, 2);
            int upperY = Random.Range(2,5);
            int lowerY = -Random.Range(2,5);
            int width = 0;
            bool lower = false;
            switch (structureType)
            {
                case StructureType.Gap:
                    width = Random.Range(2, 4);
                    lower = Random.Range(0, 2) == 1;
                    break;
                case StructureType.FlatEasy:
                    width = Random.Range(5, 8);
                    break;
            }

            PhotonNetwork.RaiseEvent(LevelGeneratorEventId, new object[]
                {
                    structureType,
                    upperY,
                    lowerY,
                    _currentX,
                    width,
                    lower
                }, new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.All,
                    CachingOption = EventCaching.DoNotCache
                },
                SendOptions.SendReliable);
            _currentX += width;
        }

        private int Gap(int startX, int width, int upperY, int lowerY, bool lower)
        {
            for (int x = startX; x < startX + width; x++)
            {
                if(lower) FillUpper(x, upperY);
                else FillLower(x, lowerY);
            }
            
            return width;
        }

        private int EasyFlat(int startX, int width, int upperY, int lowerY)
        {
            for (int x = startX; x < startX + width; x++)
            {
                FillUpper(x, upperY);
                FillLower(x, lowerY);
            }

            return width;
        }

        private void FillUpper(int x, int startY)
        {
            SetTile(x, startY, TileType.Upper);
            for (int y = startY + 1; y < tilemap.size.y / 2f; y++)
            {
                SetTile(x, y, TileType.Back);
            }
        }
        private void FillLower(int x, int startY)
        {
            SetTile(x, startY, TileType.Lower);
            for (int y = startY - 1; y >= -tilemap.size.y / 2f; y--)
            {
                SetTile(x, y, TileType.Back);
            }
        }

        private void SetTile(int x, int y, TileType type)
        {
            tilemap.SetTile(new Vector3Int(x,y,0), type == TileType.Back ? backTile : lowerTile);
            Matrix4x4 tileTransform = Matrix4x4.identity;
            switch (type)
            {
                case TileType.Upper:
                    tileTransform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
                        new Vector3(1, -1, 1));
                    break;
                case TileType.Back:
                case TileType.Lower:
                    tileTransform = Matrix4x4.identity;
                    break;
                    
            }
            tilemap.SetTransformMatrix(new Vector3Int(x,y,0), tileTransform);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != LevelGeneratorEventId) return;
            object[] data = (object[])photonEvent.CustomData;
            var structureType = (StructureType) data[0];
            int upperY = (int)data[1];
            int lowerY = (int)data[2];
            int x = (int)data[3];
            int width = (int) data[4];
            bool lower = (bool) data[5];
            switch (structureType)
            {
                case StructureType.Gap:
                    Gap(x, width, upperY, lowerY, lower);
                    break;
                case StructureType.FlatEasy:
                    EasyFlat(x, width, upperY, lowerY);
                    break;
            }
        }
    }
}
