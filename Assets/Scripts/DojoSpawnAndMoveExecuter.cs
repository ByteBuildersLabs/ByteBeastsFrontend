using System;
using System.Collections;
using System.Collections.Generic;
using Dojo;
using Dojo.Starknet;
using UnityEngine;

public class DojoSpawnAndMoveExecuter : MonoBehaviour
{
    [SerializeField] WorldManager worldManager;
    [SerializeField] WorldManagerData dojoConfig;
    [SerializeField] GameManagerData gameManagerData;

    public BurnerManager burnerManager;
    public Actions actions;
    private Dictionary<FieldElement, string> spawnedAccounts = new();

    public JsonRpcClient provider;
    public Account masterAccount;

    private Vector2Int currentPosition;

    public Action<Vector2Int> currentPositionChange;

    void Awake()
    {
        provider = new JsonRpcClient(dojoConfig.rpcUrl);
        masterAccount = new Account(provider, new SigningKey(gameManagerData.masterPrivateKey), new FieldElement(gameManagerData.masterAddress));
        burnerManager = new BurnerManager(provider, masterAccount);

        worldManager.synchronizationMaster.OnEntitySpawned.AddListener(InitEntity);
    }

    public async void SpawnCharacter()
    {
        var burner = await burnerManager.DeployBurner();
        spawnedAccounts[burner.Address] = null;
        var txHash = await actions.spawn(burner);
    }

    public async void Move(Direction direction)
    {
        await actions.move(burnerManager.CurrentBurner ?? masterAccount, direction);
    }

    private void InitEntity(GameObject entity)
    {
        if (!entity.TryGetComponent(out dojo_starter_Position position)) return;
        if (position.player.Hex().Equals(burnerManager.CurrentBurner.Address.Hex()))
        {
            this.currentPosition = new Vector2Int((int)position.vec.x, (int)position.vec.y);
            currentPositionChange.Invoke(currentPosition);
        }
            
    }
}
