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


    void Awake()
    {
        provider = new JsonRpcClient(dojoConfig.rpcUrl);
        masterAccount = new Account(provider, new SigningKey(gameManagerData.masterPrivateKey), new FieldElement(gameManagerData.masterAddress));
        burnerManager = new BurnerManager(provider, masterAccount);

        //worldManager.synchronizationMaster.OnEntitySpawned.AddListener(InitEntity);
        //foreach (var entity in worldManager.Entities<dojo_starter_Position>())
        //{
        //    InitEntity(entity);
        //}
    }

    public async void SpawnCharacter()
    {
        var burner = await burnerManager.DeployBurner();
        spawnedAccounts[burner.Address] = null;
        var txHash = await actions.spawn(burner);
    }




}
