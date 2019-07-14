using Assets.SmartContracts.Models;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Assets.Tokenisation.Helpers;
using System.Collections;
using Nethereum.RPC.Eth.DTOs;
using System;
using Random = UnityEngine.Random;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class CollectableManager : MonoBehaviour
{
    public GameObject CollectableTemplate;
    public List<int> Keys;
    // Start is called before the first frame update
    public GameObject enemy;
    public int maxEnemy = 5;
    public int maxCollectible = 4;
    public float minY = .55f, minYCollectibles = 0.987f, minDistanceApart = 25;
    public List<GameObject> environmentObjects;
    CryptoWorldWarService CryptoWorldWarService;
    public Transform right, Top;
    public Contract CryptoWorldWarContract;
    public EthCallUnityRequest EthCallUnityRequest;
    public TransactionSignedUnityRequest TransactionSignedUnityRequest;
    public Exception Exception;
    public TransactionReceiptPollingRequest TransactionReceiptPollingRequest;
    private bool spawned=false;
    public int deadSoFar = 0;
    bool transfered = false;
    public List<Collectable> Collectables { get; private set; }

    void Start()
    {
        Keys = new List<int>();
        CryptoWorldWarService = new CryptoWorldWarService();
        //StartCoroutine(CryptoWorldWarService.RegisterPlayer());
        Collectables = new List<Collectable>();
        StartCoroutine(GetCollectibles());
        Collectables = new List<Collectable>();
        TransactionSignedUnityRequest = new TransactionSignedUnityRequest(Variables.NodeAddress, Variables.PrivateKey);
        EthCallUnityRequest = new EthCallUnityRequest(Variables.NodeAddress);
        TransactionReceiptPollingRequest = new TransactionReceiptPollingRequest(Variables.NodeAddress);
        CryptoWorldWarContract = new Contract(null, Variables.ABI, Variables.ContractAddress);
    }
    private void Update()
    {
        if (deadSoFar == maxEnemy && !transfered)
        {
            foreach(var collectible in Collectables)
            {
                StartCoroutine(TransferCollectible(Convert.ToInt32(collectible.ID), collectible.owner));
            }
            transfered = true;
        }
        if (Keys.Count>0 &&!spawned)
        {
            spawn();
            spawned = true;
        }
    }
    private void spawn()
    {
        var spawned = new List<Vector3>();
        Debug.Log("Key.Count: " + Keys.Count);
        for (int i=0; i < Keys.Count; i++)
        {
            StartCoroutine(getCollectibleDetails(Keys[i]));
            Debug.Log("Key: "+ Keys[i]);
        }
        for(int i=0; i < maxEnemy; )
        {
            var pos = new Vector3(Random.Range(-right.position.x, right.position.x), minY, Random.Range(-Top.position.z, Top.position.z));
            if (checkMinDistanceApart(pos,spawned))
            {
                //collectables.Add(new Collectable { Name="test",ID=Keys[0],CollectableObject=CollectableTemplate });
                Instantiate(enemy,pos, Quaternion.identity);
                spawned.Add(pos);
                i++;
            }
        }
    }

 
    private bool checkMinDistanceApart(Vector3 pos, List<Vector3> spawned)
    {
        if (spawned.Count == 0)
        {
            return true;
        }
        bool ok = false;
        var apart = new List<Vector3>();
        for (int i = 0; i < spawned.Count; i++)
        {
            var dist = Vector3.Distance(pos, spawned[i]);
            Debug.Log(dist);
            if (dist >= minDistanceApart)
            {
                apart.Add(spawned[i]);
            }
        }
        if (apart.Count < spawned.Count)
        {
            for (int i = 0; i < spawned.Count; i++)
            {
                if (!apart.Contains(spawned[i]))
                {
                    spawned.RemoveAt(i);
                }
            }
        }
        return ok;
    }

    public CallInput GetAddTokenFunctionInput(int value, string description)
    {
        return CryptoWorldWarContract.GetFunction("mintNewCollectible").CreateCallInput(new object[] {value, description});
    }
    public CallInput GetAddTokenFunctionInput()
    {
        return CryptoWorldWarContract.GetFunction("mintNewCollectible").CreateCallInput();
    }
    public Function GetAddTokenFunction()
    {
        return CryptoWorldWarContract.GetFunction("mintNewCollectible");
    }
    public CallInput GetAllCollectiblesCountFunctionInput()
    {
        return CryptoWorldWarContract.GetFunction("getAllCollectibleKeys").CreateCallInput();
    }
    public Function GetAllCollectiblesCountFunction()
    {
        return CryptoWorldWarContract.GetFunction("getAllCollectibleKeys");
    }
    public CallInput GetGetCollectibleFunctionInput(int id)
    {
        return CryptoWorldWarContract.GetFunction("getAllCollectibleKeys").CreateCallInput(new object[] {id});
    }
    public Function GetGetCollectibleDetailsFunction()
    {
        return CryptoWorldWarContract.GetFunction("getCollectibelDetails");
    }
    public CallInput GetGetCollectibleDetailsFunctionInput(int id)
    {
        return CryptoWorldWarContract.GetFunction("getCollectibelDetails").CreateCallInput(new object[] { id });
    }
    public Function GetTranferCollectibleFunction()
    {
        return CryptoWorldWarContract.GetFunction("transferCollectible");
    }
    public CallInput GetTranferCollectibleFunctionInput(int id,string owner)
    {
        return CryptoWorldWarContract.GetFunction("transferCollectible").CreateCallInput(new object[] { id,owner });
    }
    public GetCollectibelDetailsOutputDTO DecodeCollectibleDetailsOutput(string results)
    {
        var function = GetGetCollectibleDetailsFunction();
        var output =function.DecodeDTOTypeOutput<GetCollectibelDetailsOutputDTO>(results);
        Debug.Log("Collectible Owner: " + output.Tokenowner);
        Debug.Log("Collectible Details: " + output.Description);
        Debug.Log("Owner: " + output.Tokenowner);
        Collectables.Add(new Collectable { Name = output.Name.ToString(), owner=output.Tokenowner,Description = output.Description.ToString(), CollectableObject = CollectableTemplate, Value = output.Value.ToString(), Level = output.Level.ToString() });
        return output;
        
    }
    public bool DecodeRegistrationOutput(string result)
    {
        var function = GetRegisterUserFunction();
        return function.DecodeSimpleTypeOutput<bool>(result);
    }
    public List<int> DecodeCollectableKeysOutput(string result)
    {
        var function = GetAllCollectiblesCountFunction();
        return function.DecodeDTOTypeOutput<List<int>>(result);
    }
    public bool DecodeTransferResultsOutput(string result)
    {
        var function = GetTranferCollectibleFunction();
        return function.DecodeSimpleTypeOutput<bool>(result);
    }
    public IEnumerator getCollectibleDetails(int id)
    {
        Debug.Log("Log: " + id);
        var getCollectibleDetailsInput = GetGetCollectibleDetailsFunctionInput(id);
        yield return EthCallUnityRequest.SendRequest(getCollectibleDetailsInput, BlockParameter.CreateLatest());
        if(EthCallUnityRequest.Exception == null)
        {
            DecodeCollectibleDetailsOutput(EthCallUnityRequest.Result);
        }
        else
        {
            Debug.Log(EthCallUnityRequest.Exception.ToString());
        }
    }
    public IEnumerator TransferCollectible(int id, string owner)
    {
        TransactionSignedUnityRequest = new TransactionSignedUnityRequest(Variables.NodeAddress, Variables.PrivateKey);
        Debug.Log("Got here");
        var transferCollectibleFunction = new TransferCollectibleFunction();
        transferCollectibleFunction.FromAddress = owner;
        transferCollectibleFunction.Gas = 8000000;
        transferCollectibleFunction.GasPrice = 999000000000;
        transferCollectibleFunction.To = Variables.OtherPlayer;
        transferCollectibleFunction.CollectibleId = id;
        //Call request sends and yield for response	
        yield
        return TransactionSignedUnityRequest.SignAndSendTransaction(transferCollectibleFunction, Variables.ContractAddress);
        //Each request has a exception and a result. The exception is set when an error occurs.
        //Follows a similar patter to the www and unitywebrequest
        if (TransactionSignedUnityRequest.Exception == null)
        {
            //var registered = DecodeRegistrationOutput(TransactionSignedUnityRequest.Result);
            Debug.Log("Results from transfer: " + (TransactionSignedUnityRequest.Result) +" : " + TransactionSignedUnityRequest.Result);
        }
        else
        {
            Debug.Log(TransactionSignedUnityRequest.Exception.ToString());
        }
    }
    //Check if the user top score has changed on the contract chain every 2 seconds
    public IEnumerator RegisterPlayer(string address,string privateKey)
    {
        TransactionSignedUnityRequest = new TransactionSignedUnityRequest(Variables.NodeAddress, privateKey);
        Debug.Log("Got here");
        var regFunc = new RegisterPlayerFunction();
        regFunc.FromAddress =address;
        regFunc.Gas = 8000000;
        regFunc.GasPrice = 9000000000000;
        //Call request sends and yield for response	
        yield
        return TransactionSignedUnityRequest.SignAndSendTransaction(regFunc, Variables.ContractAddress);
        //Each request has a exception and a result. The exception is set when an error occurs.
        //Follows a similar patter to the www and unitywebrequest
        if (TransactionSignedUnityRequest.Exception == null)
        {
            //var registered = DecodeRegistrationOutput(TransactionSignedUnityRequest.Result);
            Debug.Log("Results from registration: " + DecodeRegistrationOutput(TransactionSignedUnityRequest.Result));
        }
        else
        {
            Debug.Log(TransactionSignedUnityRequest.Exception.ToString());
        }
    }
    public IEnumerator GetCollectibles()
    {
        var collectiblesFunction = new GetAllCollectibleKeysFunction();
        collectiblesFunction.Gas = 8000000;
        collectiblesFunction.FromAddress = Variables.FromAddress;
        collectiblesFunction.GasPrice = 9000000000;
        var collectibleQuery = new QueryUnityRequest<GetAllCollectibleKeysFunction, GetAllCollectibleKeysOutputDTO>(Variables.NodeAddress, Variables.FromAddress);
        yield return collectibleQuery.Query(Variables.ContractAddress);
        if(collectibleQuery.Exception == null)
        {
            var keys = collectibleQuery.Result.Keys;
            Keys = keys;
        }
        else
        {
            Debug.Log(collectibleQuery.Exception.ToString());

        }
        yield break;
    }
    public  class GetCollectibelDetailsFunction : GetCollectibelDetailsFunctionBase { }

    [Function("getCollectibelDetails", typeof(GetCollectibelDetailsOutputDTO))]
    public class GetCollectibelDetailsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "collectibleId", 1)]
        public virtual BigInteger CollectibleId { get; set; }
    }

    public  class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public class TransferCollectibleFunction : TransferCollectibleFunctionBase { }

    [Function("transferCollectible", "bool")]
    public class TransferCollectibleFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "collectibleId", 1)]
        public virtual BigInteger CollectibleId { get; set; }
        [Parameter("address", "to", 2)]
        public virtual string To { get; set; }
    }

    public  class UpdateMaxCollectibleLevelFunction : UpdateMaxCollectibleLevelFunctionBase { }

    [Function("updateMaxCollectibleLevel", "bool")]
    public class UpdateMaxCollectibleLevelFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "newlevel", 1)]
        public virtual BigInteger Newlevel { get; set; }
    }

    public  class GetAllCollectibleKeysFunction : GetAllCollectibleKeysFunctionBase { }

    [Function("getAllCollectibleKeys", "uint256[]")]
    public class GetAllCollectibleKeysFunctionBase : FunctionMessage
    {

    }

    public  class RegisterPlayerFunction : RegisterPlayerFunctionBase { }

    [Function("registerPlayer", "bool")]
    public class RegisterPlayerFunctionBase : FunctionMessage
    {

    }
    public  class GetPlayerCollectibleCountFunction : GetPlayerCollectibleCountFunctionBase { }

    [Function("getPlayerCollectibleCount", "uint256")]
    public class GetPlayerCollectibleCountFunctionBase : FunctionMessage
    {

    }
    public CallInput GetRegisterUserFunctionInput()
    {
        return CryptoWorldWarContract.GetFunction("registerPlayer").CreateCallInput();
    }
    public Function GetRegisterUserFunction()
    {
        return CryptoWorldWarContract.GetFunction("registerPlayer");
    }
    public  class GetCollectibelDetailsOutputDTO : GetCollectibelDetailsOutputDTOBase { }

    [FunctionOutput]
    public class GetCollectibelDetailsOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256", "value", 1)]
        public virtual BigInteger Value { get; set; }
        [Parameter("string", "description", 2)]
        public virtual string Description { get; set; }
        [Parameter("string", "name", 3)]
        public virtual string Name { get; set; }
        [Parameter("address", "tokenowner", 4)]
        public virtual string Tokenowner { get; set; }
        [Parameter("uint256", "level", 5)]
        public virtual BigInteger Level { get; set; }
        [Parameter("string", "date", 6)]
        public virtual string Date { get; set; }
        [Parameter("string", "thash", 7)]
        public virtual string Thash { get; set; }
        [Parameter("uint256", "indexMain", 8)]
        public virtual BigInteger IndexMain { get; set; }
    }
    public  class GetAllCollectibleKeysOutputDTO : GetAllCollectibleKeysOutputDTOBase { }

    [FunctionOutput]
    public class GetAllCollectibleKeysOutputDTOBase : IFunctionOutputDTO
    {
        [Parameter("uint256[]", "keys", 1)]
        public virtual List<int> Keys { get; set; }
    }
}
